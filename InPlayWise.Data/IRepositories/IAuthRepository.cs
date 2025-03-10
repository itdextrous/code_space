using InPlayWise.Data.Entities;

namespace InPlayWise.Data.IRepositories
{
    public interface IAuthRepository
    {
        Task<bool> VerifyCode(string email, string code);
        Task<bool> SetResetOtp(string email, string code);
        Task<bool> AddLoginHistory(LoginHistory loginHistory);
    }
}
