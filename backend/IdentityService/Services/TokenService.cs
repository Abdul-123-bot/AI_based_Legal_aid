using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;

namespace IdentityService.Services;

public interface ITokenService{
    string CreateToken(string userPrincipleName, string objectId);
}

public class TokenService: ITokenService{
    private readonly IConfiguration _config;

    public TokenService(IConfiguration config){
        _config = config;
    }

    public string CreateToken(string userPrincipleName, string objectId){
        var securitykey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
        var credentials = new SigningCredentials(securitykey, SecurityAlgorithms.HmacSha256);

        var claims = new [] {
            new Claim(JwtRegisteredClaimNames.Sub, objectId),
            new Claim("upn", userPrincipleName),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
        };

        var token = new JwtSecurityToken(
            issuer: _config["Jwt:Issuer"],
            audience: _config["Jwt:Audience"],
            claims: claims,
            expires: DateTime.Now.AddMinutes(15),
            signingCredentials: credentials
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}