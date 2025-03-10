using InPlayWise.Data.Entities;
using InPlayWiseCore.IServices;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace InPlayWiseCore.Services.AuthToken
{
    public class TokenServices : ITokenServices
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IConfiguration _config;
        public TokenServices(UserManager<ApplicationUser> userManager, IConfiguration config)
        {
            _userManager = userManager;
            _config = config;
        }
        public async Task<string> GenerateTokenString(ApplicationUser user)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config.GetSection("Jwt:Key").Value));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha512);
            
            string role = (await _userManager.GetRolesAsync(user))[0];
            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id),
                new Claim(ClaimTypes.Role, role),
                new Claim("ProductId", user.ProductId is null ? "free" : user.ProductId)
            };

            var token = new JwtSecurityToken
            (
                issuer: _config.GetSection("Jwt:Issuer").Value,
                audience: _config.GetSection("Jwt:audience").Value,
                claims: claims,
                expires: DateTime.Now.AddMinutes(Double.Parse(_config.GetSection("Jwt:DurationInMinutes").Value)),
                signingCredentials: credentials
            );
            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        public string CreateAppleToken()
        {
            var now = DateTime.UtcNow;
            var ecdsa = ECDsa.Create();
            ecdsa?.ImportPkcs8PrivateKey(Convert.FromBase64String(_config.GetSection("Apple:sub").Value), out _);

            var handler = new JsonWebTokenHandler();
            return handler.CreateToken(new SecurityTokenDescriptor
            {
                Issuer = _config.GetSection("Apple:iis").Value ,
                Audience = _config.GetSection("Apple:aud").Value ,
                Claims = new Dictionary<string, object> { { "sub", _config.GetSection("Apple:sub").Value }},
                Expires = now.AddMinutes(5), // expiry can be a maximum of 6 months - generate one per request or re-use until expiration
                IssuedAt = now,
                NotBefore = now,
                SigningCredentials = new SigningCredentials(new ECDsaSecurityKey(ecdsa), SecurityAlgorithms.EcdsaSha256)

            });
        }
    }
}
