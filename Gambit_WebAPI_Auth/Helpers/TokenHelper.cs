using Gambit_WebAPI_Auth.Domain.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Gambit_WebAPI_Auth.Helpers
{
    public sealed class TokenHelper
    {
        private readonly IConfiguration config;
        public TokenHelper(IConfiguration config)
        {
            this.config = config;
        }

        public string GenerateToken(Member member)
        {
            // Claims 
            Claim[] claims = [
                new Claim(ClaimTypes.NameIdentifier, member.Id.ToString()),
                new Claim(ClaimTypes.Role, member.Role.ToString()),
                new Claim(ClaimTypes.Name, member.Username)
            ];

            // Signing
            string secret = config["JwtBearer:Secret"] ?? throw new Exception("No secret !");
            SymmetricSecurityKey securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secret));
            SigningCredentials signingCredentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha512);

            // Token
            JwtSecurityToken token = new JwtSecurityToken(
                issuer: config["JwtBearer:Issuer"],
                audience: config["JwtBearer:Audience"],
                claims : claims,
                expires: DateTime.Now.AddMinutes(config.GetValue<int>("JwtBearer:Expire")),
                signingCredentials: signingCredentials
            );

            JwtSecurityTokenHandler handler = new JwtSecurityTokenHandler();
            return handler.WriteToken(token);
        }
    }
}
