using InPlayWiseCommon.Wrappers;

namespace InPlayWiseCore.IServices
{
    public interface IEmailServices
    {
        Task<bool> SendEmail(string receiverEmail, string header, string body);
        Task<bool> SendMagicLink(string email);
        Task<Result<bool>> SendResetPasswordOtp(string email, string otp);
        Task<Result<bool>> SendEmailConfirmationMail(string email, string token);
        Task<Result<bool>> SendNewEmailConfirmationMail(string email, string token);
    }
}
