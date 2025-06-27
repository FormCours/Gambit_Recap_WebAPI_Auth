using Microsoft.IdentityModel.Tokens;
using System.Security.Cryptography;

namespace Gambit_WebAPI_Auth.Helpers
{
    public static class SecurityKeyHelper
    {
        public static RSA LoadRsaKey(string filePath)
        {
            string privateKeyPem = File.ReadAllText(filePath);

            var rsa = RSA.Create();
            rsa.ImportFromPem(privateKeyPem.ToCharArray());

            return rsa;
        }
    }
}
