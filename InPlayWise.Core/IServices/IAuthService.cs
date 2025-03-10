using Chat.Common.DTO;
using InPlayWise.Common.DTO;
using InPlayWise.Data.Entities;
using InPlayWiseCommon.DTOs;
using InPlayWiseCommon.Wrappers;
using Microsoft.AspNetCore.Mvc;

namespace InPlayWiseCore.IServices
{
    public interface IAuthService
    {
        Task<Result<string>> SignUp(SignUpDTO signUpDto);
        Task<Result<string>> SignIn(SignInDTO signInDto);
        Task<IActionResult> Logout();
        Task<Result<string>> VerifyEmail(string email, string token);
        Task<Result<bool>> VerifyResetCode(string email, string code);
        Task<Result<bool>> ResetPassword(ResetPasswordDTO resetPasswordDto);
        Task<Result<bool>> RequestReset(RequestResetDto requestResetDto);
        Task<Result<string>> ChangePassword(ChangePasswordDTO changePasswordDto);
        Task<Result<ApplicationUser>> PasswordLesslogin(MagicLinkDto magicLinkDto);
        Task<Result<string>> GoogleLogin(ExternalAuthDto externalAuth);
        Task<Result<bool>> UpdateEmailAsync(string newEmail);
        Task<Result<bool>> VerifyNewEmailAsync(string encodedToken);
    }
}
