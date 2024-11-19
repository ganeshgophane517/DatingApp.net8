using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using API.Entites;
using API.Interfaces;
using Microsoft.IdentityModel.Tokens;

namespace API.Services;

public class TokenService(IConfiguration config) : ITokenService
{
    public string CreateToken(AppUser user)
    {
        var tokenkey=config["TokenKey"] ?? throw new Exception("Cannot access tokenkey from appsetting");
        if(tokenkey.Length<64) throw new Exception ("Your tokenkey needs top be longer");

         var key= new SymmetricSecurityKey(Encoding.UTF8.GetBytes(tokenkey));

          var claims= new  List<Claim>
          {
            new(ClaimTypes.NameIdentifier, user.UserName)
          };
           var creds= new SigningCredentials(key,SecurityAlgorithms.HmacSha512Signature);
           
          var tokenDescripter = new SecurityTokenDescriptor
          {
            Subject = new ClaimsIdentity(claims),
            Expires= DateTime.UtcNow.AddDays(7),
            SigningCredentials= creds
          };
         
         var tokenHandler=new JwtSecurityTokenHandler();
         var token=tokenHandler.CreateToken(tokenDescripter);

        return tokenHandler.WriteToken(token);
    }
}
