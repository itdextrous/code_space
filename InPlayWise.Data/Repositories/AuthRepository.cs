using InPlayWise.Data.Entities;
using InPlayWise.Data.IRepositories;
using InPlayWiseCommon.DTOs;
using InPlayWiseData.Data;
using Microsoft.EntityFrameworkCore;

namespace InPlayWise.Data.Repositories
{
    public class AuthRepository : IAuthRepository
    {
        private readonly AppDbContext _db;
        public AuthRepository(AppDbContext appDb) {
            _db = appDb;
        }

        public async Task<bool> AddLoginHistory(LoginHistory loginHistory)
        {
            try
            {
                await _db.LoginHistory.AddAsync(loginHistory);
                await _db.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return false;
            }
        }

        public async Task<bool> SetResetOtp(string email, string code)
        {
            try
            {
                ResetPasswordModel reset = new ResetPasswordModel()
                {
                    Id = new Guid(),
                    UserEmail = email,
                    Code = code,
                    Expires = DateTime.UtcNow.AddMinutes(30)
                };
                var recordExists = await _db.ResetPassword.SingleOrDefaultAsync(rec => rec.UserEmail.Equals(email));
                if(recordExists is null)
                {
                    await _db.ResetPassword.AddAsync(reset);
                    await _db.SaveChangesAsync();
                    return true;
                }
                recordExists.Code = code;
                recordExists.Expires = DateTime.UtcNow.AddMinutes(30);
                await _db.SaveChangesAsync();

                return true;
            }catch (Exception ex)
            {
                Console.WriteLine(ex);
                return false;
            }
        }


        public async Task<bool> VerifyCode(string email, string code)
        {
            try
            {
                var resetOps = await _db.ResetPassword.SingleOrDefaultAsync(rp => rp.UserEmail.Equals(email));
                if(resetOps is null)
                {
                    return false;
                }
                return resetOps is null ? false : resetOps.Code.Equals(code) && DateTime.UtcNow < resetOps.Expires ? true : false;
            }catch(Exception ex)
            {
                Console.WriteLine(ex);
                return false;
            }
        }



    }
}
