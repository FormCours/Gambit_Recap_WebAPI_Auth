using Gambit_WebAPI_Auth.Domain.Models;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;

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
            RSA rsa = SecurityKeyHelper.LoadRsaKey(config["JwtBearer:PrivateKey"]!);
            SecurityKey securityKey = new RsaSecurityKey(rsa);
            SigningCredentials signingCredentials = new SigningCredentials(securityKey, SecurityAlgorithms.RsaSha256);

            // Token
            JwtSecurityToken token = new JwtSecurityToken(
                issuer: config["JwtBearer:Issuer"],
                audience: config["JwtBearer:Audience"],
                claims : claims,
                expires: DateTime.Now.AddMinutes(config.GetValue<int>("JwtBearer:Expire")),
                signingCredentials: signingCredentials
            );
            token.Header.Add("kid", config["JwtBearer:KeyId"]!);

            JwtSecurityTokenHandler handler = new JwtSecurityTokenHandler();
            return handler.WriteToken(token);
        }
    }
}
