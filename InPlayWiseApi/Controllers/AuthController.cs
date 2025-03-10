using Chat.Common.DTO;
using InPlayWise.Common.DTO;
using InPlayWiseCommon.DTOs;
using InPlayWiseCore.IServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using Stripe;

namespace InPlayWiseApi.Controllers
{
    [Route("[Controller]")]
    [EnableCors("AllowAllOrigins")]
    public class AuthController : ControllerBase
    {

        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        //[HttpPost("AddRole")]
        //[Authorize(Roles = "Admin")]
        //public async Task<IActionResult> AddRole(string roleName)
        //{
        //    return Ok(await _authService.AddRole(roleName));
        //}

        /// <summary>
        /// Registers a new user with the provided information.
        /// </summary>
        /// <param name="signUpDto">The SignUpDTO containing user registration details.</param>
        /// <returns>An IActionResult representing the result of the registration.</returns>
        [HttpPost("Signup")]
        public async Task<IActionResult> SignUp([FromBody]SignUpDTO signUpDto)
        {
            try
            {
                var result = await _authService.SignUp(signUpDto);
                switch (result.StatusCode)
                {
                    case 401:
                        return Unauthorized(result);
                    case 400:
                        return BadRequest(result);
                    case 404:
                        return NotFound(result);
                    case 403:
                        return BadRequest(result);
                    case 409:
                        return Conflict(result);
                    case 200:
                        return Ok(result);
                    default:
                        return StatusCode(StatusCodes.Status500InternalServerError, "An unexpected error occurred.");
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, "An error occurred while processing your request.");
            }
        }


        [HttpGet("VerifyEmail")]
        public async Task<IActionResult> VerifyEmail(string email, string token)
        {
            try
            {
                var result = await _authService.VerifyEmail(email, token);
                switch (result.StatusCode)
                {
                    case 401:
                        return Unauthorized(result);
                    case 400:
                        return BadRequest(result);
                    case 404:
                        return NotFound(result);
                    case 200:
                        return Ok(result);
                    default:
                        return StatusCode(StatusCodes.Status500InternalServerError, "An unexpected error occurred.");
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, "An error occurred while processing your request.");
            }
        }

        /// <summary>
        /// Logs in a user with the provided credentials.
        /// </summary>
        /// <param name="signInDto">The SignInDTO containing user login details.</param>
        /// <returns>An IActionResult representing the result of the login.</returns>
        /// 

        [HttpPost("Signin")]
        public async Task<IActionResult> SignIn([FromBody]SignInDTO signInDto)
        {
            try
            {
                var result = await _authService.SignIn(signInDto);
                switch (result.StatusCode)
                {
                    case 401:
                        return Unauthorized(result);
                    case 400:
                        return BadRequest(result);
                    case 404:
                        return NotFound(result);
                    case 403:
                        return BadRequest(result);
                    case 200:
                        return Ok(result);
                    default:
                        return StatusCode(StatusCodes.Status500InternalServerError, "An unexpected error occurred.");
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, "An error occurred while processing your request.");
            }
        }

        [HttpPost("Logout")]
        public async Task <IActionResult> LogOut()
        {
            return await _authService.Logout();
        }

        [HttpPost("ChangeEmail")]
        public async Task<IActionResult> UpdateEmailAsync(string newMail)
        {
            try
            {
                var result = await _authService.UpdateEmailAsync(newMail);
                switch (result.StatusCode)
                {
                    case 401:
                        return Unauthorized(result);
                    case 400:
                        return BadRequest(result);
                    case 404:
                        return NotFound(result);
                    case 200:
                        return Ok(result);
                    default:
                        return StatusCode(StatusCodes.Status500InternalServerError, "An unexpected error occurred.");
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, "An error occurred while processing your request.");
            }
        }

        [HttpPut("VerifyNewEmail")]
        public async Task<IActionResult> VerifyNewEmail(string encodedToken)
        {
            try
            {
                var result = await _authService.VerifyNewEmailAsync(encodedToken);
                switch (result.StatusCode)
                {
                    case 401:
                        return Unauthorized(result);
                    case 400:
                        return BadRequest(result);
                    case 404:
                        return NotFound(result);
                    case 200:
                        return Ok(result);
                    default:
                        return StatusCode(StatusCodes.Status500InternalServerError, "An unexpected error occurred.");
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, "An error occurred while processing your request.");
            }
        }
        

        /// <summary>
        /// Resets a user's password with a new one.
        /// </summary>
        /// <param name="resetPasswordDto">The ResetPasswordDTO containing the new password.</param>
        /// <returns>An IActionResult representing the result of the password reset.</returns>
        [HttpPost("ResetPassword")]
        public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordDTO resetPasswordDto)
        {
            try
            {
                var result = await _authService.ResetPassword(resetPasswordDto);
                switch (result.StatusCode)
                {
                    case 401:
                        return Unauthorized(result);
                    case 400:
                        return BadRequest(result);
                    case 404:
                        return NotFound(result);
                    case 200:
                        return Ok(result);
                    default:
                        return StatusCode(StatusCodes.Status500InternalServerError, "An unexpected error occurred.");
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, "An error occurred while processing your request.");
            }
        }

        [HttpGet("VerifyResetCode")]
        public async Task<IActionResult> VerifyResetCode(string email, string code)
        {
            try
            {
                var result = await _authService.VerifyResetCode(email, code);
                switch (result.StatusCode)
                {
                    case 401:
                        return Unauthorized(result);
                    case 400:
                        return BadRequest(result);
                    case 404:
                        return NotFound(result);
                    case 200:
                        return Ok(result);
                    default:
                        return StatusCode(StatusCodes.Status500InternalServerError, "An unexpected error occurred.");
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, "An error occurred while processing your request.");
            }
        }

        [HttpPost("RequestReset")]
        public async Task<IActionResult> RequestReset([FromBody] RequestResetDto requestResetDto) {
            try
            {
                var result = await _authService.RequestReset(requestResetDto);
                switch (result.StatusCode)
                {
                    case 401:
                        return Unauthorized(result);
                    case 400:
                        return BadRequest(result);
                    case 404:
                        return NotFound(result);
                    case 200:
                        return Ok(result);
                    default:
                        return StatusCode(StatusCodes.Status500InternalServerError, "An unexpected error occurred.");
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, "An error occurred while processing your request.");
            }
        }


        /// <summary>
        /// Changes a user's password with a new one.
        /// </summary>
        /// <param name="changePasswordDto">The ChangePasswordDTO containing old and new password details.</param>
        /// <returns>An IActionResult representing the result of the password change.</returns>
        [HttpPost("ChangePassword")]
        public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordDTO changePasswordDto)
        {
            try
            {
                var result = await _authService.ChangePassword(changePasswordDto);
                switch (result.StatusCode)
                {
                    case 401:
                        return Unauthorized(result);
                    case 400:
                        return BadRequest(result);
                    case 404:
                        return NotFound(result);
                    case 200:
                        return Ok(result);
                    default:
                        return StatusCode(StatusCodes.Status500InternalServerError, "An unexpected error occurred.");
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, "An error occurred while processing your request.");
            }
        }


        /// <summary>
        /// Implements a passwordless login using a magic link sent via email.
        /// </summary>
        /// <param name="email">The email address of the user.</param>
        /// <returns>An IActionResult representing the result of the passwordless login.</returns>
        [HttpPost("MagicLink")]
        public async Task<IActionResult> LoginWithLink([FromBody] MagicLinkDto magicLinkDto)
        {
            return Ok(await _authService.PasswordLesslogin(magicLinkDto));
        }


        /// <summary>
        /// Initiates Google authentication and performs user login or registration based on the authentication result.
        /// </summary>
        /// <param name="externalAuth">The ExternalAuthDto containing authentication details.</param>
        /// <returns>An IActionResult representing the result of Google authentication and user login or registration.</returns>
        [HttpPost("GoogleLogin")]
        public async Task<IActionResult> GoogleLogin([FromBody] ExternalAuthDto externalAuth)
        {
            return Ok(await _authService.GoogleLogin(externalAuth));
        }


    }
}
