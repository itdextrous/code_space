using System.Text;
using Chat.Common.DTO;
using Google.Apis.Auth;
using InPlayWise.Common.DTO;
using InPlayWise.Core.IServices;
using InPlayWise.Data.Entities;
using InPlayWise.Data.IRepositories;
using InPlayWiseCommon.DTOs;
using InPlayWiseCommon.Wrappers;
using InPlayWiseCore.IServices;
//using Microsoft.ApplicationInsights.Extensibility.Implementation;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
//using static Microsoft.ApplicationInsights.MetricDimensionNames.TelemetryContext;

namespace InPlayWiseCore.Services
{
    public class AuthService : IAuthService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IMembershipService _membershipService;
        private readonly IHttpContextService _context;
        private readonly IConfiguration _config;
        private readonly ITokenServices _token;
        private readonly IEmailServices _emailServices;
        private readonly IAuthRepository _authRepo;
        private readonly ILogger<AuthService> _logger;


        public AuthService(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, IConfiguration config, RoleManager<IdentityRole> roleManager, ITokenServices token, IEmailServices emailServices, IAuthRepository auth, IMembershipService membershipService, ILogger<AuthService> logger, IHttpContextService context)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _config = config;
            _roleManager = roleManager;
            _token = token;
            _emailServices = emailServices;
            _authRepo = auth;
            _membershipService = membershipService;
            _logger = logger;
            _context = context;
        }

        /// <summary>
        /// Changes the password for the currently authenticated user.
        /// </summary>
        /// <param name="changePasswordDto">The ChangePasswordDTO containing old and new password details.</param>
        /// <returns>An IActionResult representing the result of the password change operation.</returns>
        public async Task<Result<String>> ChangePassword(ChangePasswordDTO changePasswordDto)
        {

            try
            {
                //var userIdClaim = _httpContextAccessor.HttpContext.User.Claims.FirstOrDefault(c => c.Type.Equals(ClaimTypes.NameIdentifier));
                string userId = _context.GetUserId();

                if (userId is null)
                    return Result<string>.Unauthorized();
                
                //var userId = userIdClaim.Value;
                var user = await _userManager.FindByIdAsync(userId);

                if (!changePasswordDto.NewPassword.Equals(changePasswordDto.ConfirmedPassword))
                    return Result<string>.BadRequest("Password and Confirmed Password do not match");
                


                if (!await _userManager.CheckPasswordAsync(user, changePasswordDto.CurrentPassword))
                    return Result<string>.Unauthorized("Invalid Current Password");
                
                var result = await _userManager.ChangePasswordAsync(user, changePasswordDto.CurrentPassword, changePasswordDto.NewPassword);
                if (result.Succeeded)
                    return Result<string>.Success("Password Changed Successfully");
             

                string errors = "";
                foreach(var error in result.Errors)
                    errors += error.Description + "  ";

                return Result<string>.BadRequest(errors);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());
                return Result<string>.InternalServerError();
            }
        }


        public async Task<IActionResult> Logout()
        {
            try
            {
                await _signInManager.SignOutAsync();
                return new JsonResult("Logout successful");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());
                return new JsonResult("Failed to logout") { StatusCode = 500 };
            }
        }

       
        /// <summary>
        /// Resets the password for the currently authenticated user.
        /// </summary>
        /// <param name="resetPasswordDto">The ResetPasswordDTO containing the new password.</param>
        /// <returns>An IActionResult representing the result of the password reset operation.</returns>
        public async Task<Result<bool>> ResetPassword(ResetPasswordDTO resetPasswordDto)
        {
            try
            {
                var user = await _userManager.FindByEmailAsync(resetPasswordDto.Email);
                var role = await _userManager.GetRolesAsync(user);
                if(user is null)
                    return Result<bool>.NotFound("User not found");

                if(!( await _authRepo.VerifyCode(resetPasswordDto.Email, resetPasswordDto.Code)))
                    return Result<bool>.Unauthorized();
                
                var removePassword = await _userManager.RemovePasswordAsync(user);
                if (removePassword.Succeeded)
                {
                    var addNewPassword = await _userManager.AddPasswordAsync(user, resetPasswordDto.NewPassword);
                    if (addNewPassword.Succeeded)
                    {
                        Logout();
                        return Result<bool>.Success(role[0]);
                    }                       
                }
                return Result<bool>.InternalServerError();

            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());
                return Result<bool>.InternalServerError();
            }
        }



        public async Task<Result<bool>> VerifyResetCode(string email, string code)
        {
            try
            {
                var user = await _userManager.FindByEmailAsync(email);
                if (user is null)
                    return Result<bool>.NotFound("User not found");

                if (!(await _authRepo.VerifyCode(email, code)))
                    return Result<bool>.Unauthorized();

                return Result<bool>.Success();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());
                return Result<bool>.InternalServerError();

            }
        }




        public async Task<Result<bool>> RequestReset(RequestResetDto requestResetDto)
        {
            try
            {
                string email = requestResetDto.Email; 
                Result<bool> res = new Result<bool>();
                var user = await _userManager.FindByEmailAsync(email);
                if (user is null)
                    return Result<bool>.NotFound("User not found");

                var roles = await _userManager.GetRolesAsync(user);

                if (roles.Count == 0 ||(requestResetDto.isAdmin && roles[0] != "admin") || (!requestResetDto.isAdmin && roles[0] == "admin"))
                    return Result<bool>.BadRequest("Invalid Role");


                if (!user.EmailConfirmed)
                    return Result<bool>.BadRequest("Email not confirmed, Please confirm your email before resetting the password");

                Random random = new Random();
                int otp = random.Next(100000, 999999);
                var otpSaved = await _authRepo.SetResetOtp(email, otp.ToString());
                if (!otpSaved)
                    return Result<bool>.InternalServerError();
 
                var sent = await _emailServices.SendResetPasswordOtp(email, otp.ToString());
                return sent;
            }
            catch(Exception ex)
            {
                _logger.LogError(ex.ToString());
                return Result<bool>.InternalServerError();
            }
        }


        /// <summary>
        /// Attempts to sign in a user with the provided credentials.
        /// </summary>
        /// <param name="signInDto">The SignInDTO containing user login details.</param>
        /// <returns>An IActionResult representing the result of the sign-in operation.</returns>
        public async Task<Result<string>> SignIn(SignInDTO signInDto)
        {
            try
            {
                var user = await _userManager.FindByEmailAsync(signInDto.Email);
                if (user is null)
                    return Result<string>.NotFound("User not found");

                if (user.GoogleOauth)
                    return Result<string>.Unauthorized("You have done sign up using a different method");

                bool isEmailConfirmed = await _userManager.IsEmailConfirmedAsync(user);


                if (!isEmailConfirmed)
                    return Result<string>.Forbidden("Email is not confirmed yet");
                

                var result = await _signInManager.PasswordSignInAsync(user.UserName, signInDto.Password, isPersistent: signInDto.RememberMe, false);

                if (result.Succeeded)
                {

                    LoginHistory loginHistory = new LoginHistory()
                    {
                        Id = Guid.NewGuid(),
                        UserId = user.Id,
                        LoginTime = DateTime.UtcNow
                    };
                    await _authRepo.AddLoginHistory(loginHistory);
                    string token = await _token.GenerateTokenString(user);
                    return Result<string>.Success(msg:"Log in Successful", item:token);
                }


                return Result<string>.Unauthorized("Invalid Credentials");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());
                return Result<string>.InternalServerError();
            }
        }


        /// <summary>
        /// Registers a new user with the provided information.
        /// </summary>
        /// <param name="signUpDto">The SignUpDTO containing user registration details.</param>
        /// <returns>An IActionResult representing the result of the registration.</returns>
        public async Task<Result<string>> SignUp(SignUpDTO signUpDto)        
        {
            try
            {
                string password = signUpDto.Password;
                if (!(password.Length >= 3 && password.Substring(password.Length - 3).Equals("dev", StringComparison.OrdinalIgnoreCase)))
                {
                    return Result<string>.BadRequest("Sign Up prohibited");
                }


                if (!IsValidEmail(signUpDto.Email))
                    return Result<string>.BadRequest("Invalid Email Address");

                if (!signUpDto.Password.Equals(signUpDto.RepeatPassword))
                    return Result<string>.BadRequest("Password and Repeat Password do not match");

                if (signUpDto.Role.Equals("Admin", StringComparison.OrdinalIgnoreCase))
                    return Result<string>.Forbidden("This role can't be requested");

                var user = await _userManager.FindByEmailAsync(signUpDto.Email);
                if (user is not null)
                    return Result<string>.Conflict("Email already exists");

                user = await _userManager.FindByNameAsync(signUpDto.UserName);
                if (user is not null)
                    return Result<string>.Conflict("Username already taken");
 
                var role = await _roleManager.FindByNameAsync(signUpDto.Role);
                if (role is null)
                    return Result<string>.NotFound("Role not found");

                user = new ApplicationUser
                {
                    UserName = signUpDto.UserName,
                    Email = signUpDto.Email
                };
                var result = await _userManager.CreateAsync(user, signUpDto.Password);
                if (!result.Succeeded)
                {
                    string errors = "";

                    foreach (var error in result.Errors)
                        errors += (error.Description + "  ");
                    
                    return Result<string>.BadRequest(errors);
                }

                await _userManager.AddToRoleAsync(user, signUpDto.Role);
                string tokenEmail = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                var sendConfirmationLink = await _emailServices.SendEmailConfirmationMail(user.Email, tokenEmail);
                if (!sendConfirmationLink.IsSuccess)
                {
                    await _userManager.DeleteAsync(user);
                    return Result<string>.InternalServerError("Sending confirmation email failed");
                    
                }
                await _membershipService.SetFreeSubscription(await _userManager.GetUserIdAsync(await _userManager.FindByEmailAsync(user.Email)));
                string freePlanId = await _membershipService.GetFreePlanId();
                user.ProductId = freePlanId;
                await _userManager.UpdateAsync(user);
                return Result<string>.Success("There will an email verification link on your email");
            }

            catch(Exception ex)
            {
                _logger.LogError(ex.ToString());
                return Result<string>.InternalServerError();
            }
        }


        /// <summary>
        /// Attempts passwordless login for a user using their email.
        /// </summary>
        /// <param name="email">The email address of the user.</param>
        /// <returns>A <see cref="Result{IdentityUser}"/> representing the login result.</returns>
        public async Task<Result<ApplicationUser>> PasswordLesslogin(MagicLinkDto magicLinkDto)
        {
            string email = magicLinkDto.Email;
            var user = await _userManager.FindByEmailAsync(email);

            if (user is not null)
            {
                var token = await _token.GenerateTokenString(user);
                return new Result<ApplicationUser>
                {
                    Items = user,
                    IsSuccess = true, // Change this to true
                    StatusCode = StatusCodes.Status200OK,
                    Message = token.ToString()
                };
            }
            SignUpDTO createuser = new SignUpDTO
            {
                Email = email,
                Role = "User",
                Password = "User@123"
            };

            await SignUp(createuser);
            bool result = await _emailServices.SendMagicLink(email);

            if (result)
            {
                return new Result<ApplicationUser>
                {
                    Items = null,
                    IsSuccess = true,
                    StatusCode = StatusCodes.Status200OK,
                    Message = "Check email link will be sent"
                };
            }
            return new Result<ApplicationUser>
            {
                IsSuccess = false,
                StatusCode = StatusCodes.Status500InternalServerError,
                Message = "Failed to send email"
            };
        }

        public async Task<Result<string>> GoogleLogin(ExternalAuthDto externalAuth)
        {
            try
            {
				string clientId = _config.GetSection("Authentication:Google:ClientId").Value;
				var settings = new GoogleJsonWebSignature.ValidationSettings()
				{
					Audience = new List<string>() { clientId }
				};
				var payload = await GoogleJsonWebSignature.ValidateAsync(externalAuth.IdToken, settings);

				if (payload is null)
					return Result<string>.Unauthorized();

				ApplicationUser user = await _userManager.FindByEmailAsync(payload.Email);

				if (user is not null)
				{
					if (!user.GoogleOauth)
						return Result<string>.Unauthorized("You have signed up using different sign up method");
					string token = await _token.GenerateTokenString(user);
					return Result<string>.Success("Log in Successful", token);
				}

				user = new ApplicationUser
				{
					UserName = payload.Name + Guid.NewGuid().ToString(),
					Email = payload.Email
				};
				var signUp = await _userManager.CreateAsync(user, $"GooglePassword{Guid.NewGuid().ToString()}");
				if (!signUp.Succeeded)
					return Result<string>.InternalServerError("Sign up failed");

				var roleAssigned = await _userManager.AddToRoleAsync(user, "user");
				if (!roleAssigned.Succeeded)
					return Result<string>.InternalServerError("Role assignment failed");
				string token2 = await _token.GenerateTokenString(user);
                LoginHistory loginHistory = new LoginHistory()
                {
                    UserId = user.Id,
                    LoginTime = DateTime.UtcNow
                };
                await _authRepo.AddLoginHistory(loginHistory);
                return Result<string>.Success("Sign Up Successful", token2);
			}
            catch(Exception ex)
            {
                _logger.LogError(ex.ToString());
                return Result<string>.InternalServerError();
            }

		}

        public async Task<Result<string>> VerifyEmail(string email, string token)
        {
            try
            {
                var user = await _userManager.FindByEmailAsync(email);
                if (user is null)
                    return Result<string>.NotFound("User not found");

                StringBuilder sb = new StringBuilder();
                foreach(char c in token)
                {
                    if (c.Equals(' ')) {
                        sb.Append('+');
                        continue;
                    }
                    sb.Append(c);
                }

                token = sb.ToString();

                var result = await _userManager.ConfirmEmailAsync(user, token);

                if (!result.Succeeded)
                    return Result<string>.Unauthorized("Email Verification failed");

                var loginToken = await _token.GenerateTokenString(user);
                LoginHistory loginHistory = new LoginHistory()
                {
                    UserId = user.Id,
                    LoginTime = DateTime.UtcNow
                };
                await _authRepo.AddLoginHistory(loginHistory);
                return Result<string>.Success("Email Verified Succcessfully", loginToken);

            }
            catch(Exception ex)
            {
                Console.WriteLine(ex);
                return Result<string>.InternalServerError();
            }
        }



        private bool IsValidEmail(string email)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(email))
                    return false;

                string[] parts = email.Split('@');
                if (parts.Length != 2)
                    return false;

                if (string.IsNullOrWhiteSpace(parts[0]))
                    return false;
                if (!parts[1].Contains('.') || parts[1].EndsWith("."))
                    return false;

                string[] part2 = parts[1].Split('.');

                if (string.IsNullOrWhiteSpace(part2[0]))
                    return false;

                if (!(part2[1].Length >= 2))
                    return false;

                return true;
            }
            catch
            {
                return false;
            }
        }

        public async Task<Result<bool>> UpdateEmailAsync(string newEmail)
        {
            try
            {
                // Get the current user's ID
                var userId = _context.GetUserId();
                if (userId == null)
                {
                    _logger.LogWarning("User not logged in");
                    return Result<bool>.Unauthorized("User not logged in");
                }

                var existingUser = await _userManager.FindByEmailAsync(newEmail);
                if (existingUser != null)
                {
                    _logger.LogWarning("Email {NewEmail} is already in use", newEmail);
                    return Result<bool>.Conflict("This Email is already registred");
                }

                // Retrieve the user
                var user = await _userManager.FindByIdAsync(userId);
                if (user == null)
                {
                    _logger.LogWarning("User not found with ID: {UserId}", userId);
                    return Result<bool>.NotFound("User not found");
                }

                // Generate email confirmation token
                string tokenEmail = await _userManager.GenerateChangeEmailTokenAsync(user, newEmail);

                // Encode the token with old and new email
                string tokenWithEmails = $"{user.Email}|{newEmail}|{tokenEmail}";
                string encodedToken = Convert.ToBase64String(Encoding.UTF8.GetBytes(tokenWithEmails));

                // Send confirmation email
                var sendConfirmationResult = await _emailServices.SendNewEmailConfirmationMail(newEmail, encodedToken);
                if (!sendConfirmationResult.IsSuccess)
                {
                    _logger.LogError("Sending confirmation email failed for user: {UserId} to {NewEmail}", user.Id, newEmail);
                    return Result<bool>.InternalServerError("Sending confirmation email failed");
                }

                _logger.LogInformation("Confirmation email sent successfully to {NewEmail} for user: {UserId}", newEmail, user.Id);
                return Result<bool>.Success("Confirmation email sent successfully");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating email for user: {UserId}");
                return Result<bool>.InternalServerError($"Error updating email: {ex.Message}");
            }
        }



        public async Task<Result<bool>> VerifyNewEmailAsync(string encodedToken)
        {
            try
            {
                // Decode the token
                string decodedToken = Encoding.UTF8.GetString(Convert.FromBase64String(encodedToken));
                var parts = decodedToken.Split('|');

                if (parts.Length != 3)
                {
                    _logger.LogWarning("Invalid token format: {EncodedToken}", encodedToken);
                    return Result<bool>.BadRequest("Invalid token format");
                }

                string oldEmail = parts[0];
                string newEmail = parts[1];
                string token = parts[2];

                // Find the user by the old email
                var user = await _userManager.FindByEmailAsync(oldEmail);
                if (user == null)
                {
                    _logger.LogWarning("User not found with email: {OldEmail}", oldEmail);
                    return Result<bool>.NotFound("User not found");
                }

                // Confirm the email change token
                var changeEmailResult = await _userManager.ChangeEmailAsync(user, newEmail, token);
                if (!changeEmailResult.Succeeded)
                {
                    _logger.LogError("Failed to change email for user: {UserId}. Errors: {Errors}", user.Id, changeEmailResult.Errors);
                    return Result<bool>.InternalServerError("Failed to change email");
                }

                // Update the username if it is tied to the email
                user.Email = newEmail;
                user.NormalizedEmail = newEmail.ToUpperInvariant();

                // Update the user record in the application user table
                var updateResult = await _userManager.UpdateAsync(user);
                if (!updateResult.Succeeded)
                {
                    _logger.LogError("Failed to update user record for user: {UserId}. Errors: {Errors}", user.Id, updateResult.Errors);
                    return Result<bool>.InternalServerError("Failed to update user record");
                }

                _logger.LogInformation("Successfully changed email for user: {UserId} to {NewEmail}", user.Id, newEmail);
                return Result<bool>.Success();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error processing token: {EncodedToken}", encodedToken);
                return Result<bool>.InternalServerError($"Error processing token: {ex.Message}");
            }
        }
    }
}
