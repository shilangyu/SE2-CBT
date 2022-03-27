using System.Text;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

using Microsoft.IdentityModel.Tokens;

using CbtBackend.Entities;

namespace CbtBackend.Services;

public interface IJwtTokenService {
    string GenerateToken(User user, DateTime expiration);
}

public class JwtTokenService : IJwtTokenService {
    private readonly IConfiguration configuration;

    public JwtTokenService(IConfiguration configuration) {
        this.configuration = configuration;
    }

    public string GenerateToken(User user, DateTime expiration) {
        var claims = new List<Claim>() {
            new Claim("id", user.Id.ToString()),
            new Claim(ClaimTypes.Name, user.Email)
        };

        // TODO: verify if user can have this role
        foreach (var role in user.Roles) {
            claims.Add(new Claim(ClaimTypes.Role, role));
        }

        var securityKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(configuration.GetValue<string>("Jwt:Key")));
        var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256Signature);

        var tokenDescriptor = new SecurityTokenDescriptor() {
            Subject = new ClaimsIdentity(claims),
            Expires = expiration,
            SigningCredentials = credentials,
            Issuer = configuration.GetValue<string>("Jwt:Issuer"),
            Audience = configuration.GetValue<string>("Jwt:Audience")
        };

        var tokenHandler = new JwtSecurityTokenHandler();
        var token = tokenHandler.CreateToken(tokenDescriptor);

        return tokenHandler.WriteToken(token);
    }
}
