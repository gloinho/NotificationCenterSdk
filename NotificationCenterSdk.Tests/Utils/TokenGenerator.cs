using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json.Linq;
using NotificationCenterSdk.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace NotificationCenterSdk.Tests.Utils
{
    public static class TokenGenerator
    {
        public static string GenerateToken(UserCredentials usuario)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var tokenDescriptor = new SecurityTokenDescriptor()
            {
                Subject = new ClaimsIdentity(
                    new Claim[]
                    {
                        new Claim(ClaimTypes.NameIdentifier, usuario.Username),
                    }
                ),
                Expires = DateTime.UtcNow.AddHours(1)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }

        public static string GenerateExpiredToken(UserCredentials usuario)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var jwt = tokenHandler.ReadJwtToken(GenerateToken(usuario));
            var invalidDate = DateTime.UtcNow.AddHours(-2);
            jwt.Payload["exp"] = (int)(invalidDate - new DateTime(1970, 1, 1)).TotalSeconds;
            var newToken = tokenHandler.WriteToken(jwt);
            return newToken;
        }
    }
}
