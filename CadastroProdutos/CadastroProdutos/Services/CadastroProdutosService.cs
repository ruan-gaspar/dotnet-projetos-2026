// Services/TokenService.csusing Microsoft.IdentityModel.Tokens;using System.IdentityModel.Tokens.Jwt;using System.Security.Claims;using System.Text;

using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;

namespace CadastroProdutos.Services;
public interface ITokenService{
    string GerarToken(string usuario, string papel);
}
public class TokenService : ITokenService{
    private readonly IConfiguration _cfg;
    public TokenService(IConfiguration cfg) => _cfg = cfg;

    public string GerarToken(string usuario, string papel)
    {
        var chave = Encoding.UTF8.GetBytes(_cfg["Jwt:Key"]!);
        var creds = new SigningCredentials(
            new SymmetricSecurityKey(chave),
            SecurityAlgorithms.HmacSha256);

        var claims = new[]
        {
            new Claim(ClaimTypes.Name, usuario),
            new Claim(ClaimTypes.Role, papel),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
        };

        var token = new JwtSecurityToken(
            issuer: _cfg["Jwt:Issuer"],
            audience: _cfg["Jwt:Audience"],
            claims: claims,
            expires: DateTime.UtcNow.AddMinutes(int.Parse(_cfg["Jwt:ExpiresMinutes"]!)),
            signingCredentials: creds);

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}