using Gambit_WebAPI_Auth.Helpers;
using Microsoft.IdentityModel.Tokens;
using System.Net;
using System.Security.Cryptography;
using System.Text.Json;

namespace Gambit_WebAPI_Auth.Middlewares
{
    public static class JwksMiddlewareExtensions
    {
        public static IApplicationBuilder UseJwks(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<JwksMiddleware>();
        }
    }

    public class JwksMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly IConfiguration _configuration;
        public JwksMiddleware(RequestDelegate next, IConfiguration configuration)
        {
            _next = next;
            _configuration = configuration;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            switch (context.Request.Path)
            {
                case "/.well-known/openid-configuration":
                    context.Response.StatusCode = (int)HttpStatusCode.OK;
                    context.Response.ContentType = "application/json";
                    await context.Response.WriteAsync(GetOpenidConfig());
                    break;

                case "/.well-known/jwks.json":
                    context.Response.StatusCode = (int)HttpStatusCode.OK;
                    context.Response.ContentType = "application/json";
                    await context.Response.WriteAsync(GetJwksResponse());
                    break;

                default:
                    await _next(context);
                    break;
            }
        }

        private string GetOpenidConfig()
        {
            var config = new
            {
                issuer = _configuration["JwtBearer:Issuer"]!,
                authorization_endpoint = "https://localhost:7250/api/member/login",
                token_endpoint = "https://localhost:7250/api/member/login",
                //userinfo_endpoint = "https://localhost:7250/api/member/profile",
                jwks_uri = "https://localhost:7250/.well-known/jwks.json",
                response_types_supported = new string[] {
                    "code",
                    "token",
                },
                subject_types_supported = new string[] {
                    "public"
                },
                id_token_signing_alg_values_supported = new string[] {
                    "RS256"
                },
                scopes_supported = new string[] {
                    "openid",
                    "profile",
                }
            };

            return JsonSerializer.Serialize(config);
        }
        private string GetJwksResponse()
        {
            RSA rsa = SecurityKeyHelper.LoadRsaKey(_configuration["JwtBearer:PublicKey"]!);
            RSAParameters rsaParameters = rsa.ExportParameters(false);

            var jwks = new
            {
                keys = new object[] {
                    new {
                        kid = _configuration["JwtBearer:KeyId"]!,
                        kty = "RSA",
                        use = "sig",
                        alg = "RS256",
                        n = Base64UrlEncoder.Encode(rsaParameters.Modulus),
                        e = Base64UrlEncoder.Encode(rsaParameters.Exponent)
                    }
                }
            };

            return JsonSerializer.Serialize(jwks);
        }
    }

}
