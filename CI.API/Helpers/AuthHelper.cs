using System;
using System.Text;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.Linq;

namespace CI.API.Helpers
{
  public static class AuthHelper
  {
    /// <summary>
    /// Verify the JWT Authorization token sent to the API from the client app.
    /// Returns NULL if verification failed; user id if verification passed.
    /// </summary>
    public static (string userId, string username) VerifyAuthToken(IConfiguration config, string token, string claimType)
    {
      var tokenHandler = new JwtSecurityTokenHandler();
      var key = Encoding.ASCII.GetBytes(config.GetSection("AppSettings:Key").Value);
      tokenHandler.ValidateToken(token, new TokenValidationParameters
      {
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(key),
        ValidateIssuer = false,
        ValidateAudience = false,
        ClockSkew = TimeSpan.Zero
      }, out SecurityToken validatedToken);

      var jwtToken = (JwtSecurityToken)validatedToken;
      var accountId = jwtToken.Claims.First(x => x.Type == claimType).Value;
      var username = jwtToken.Claims.First(x => x.Type == "Name").Value;

      return (userId: accountId, username: username);
    }
  }
}