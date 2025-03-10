using InPlayWise.Data.Entities;
using Microsoft.AspNetCore.Identity;

namespace InPlayWiseCore.IServices
{
    public interface ITokenServices
    {
        Task<string> GenerateTokenString(ApplicationUser user);
        string CreateAppleToken();
    }
}
