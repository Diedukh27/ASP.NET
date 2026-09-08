using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using WebGenQRCode.Data.Entities.Identity;
using WebGenQRCode.Interfaces;

namespace WebGenQRCode.Services;

public class JwtTokenService(UserManager<UserEntity> userManager,
    IConfiguration configuration) : IJwtTokenService
{
    public async Task<string> CreateTokenAsync(UserEntity user)
    {
        var key = configuration["Jwt:Key"];
        //claims - це спеціальні дані, які будуть записані у jwt token
        //jwt - не оборотка hash фунція. 
        var claims = new List<Claim>
        {
            new Claim("email", user.Email)
        };
        //В токен можемо вписати список ролей,які має користувача
        foreach(var role in await userManager.GetRolesAsync(user))
        {
            claims.Add(new Claim("roles", role));
        }
        //Ключ для шифрування токена перетворили в байти
        var keyBytes = Encoding.UTF8.GetBytes(key);
        //Створюємо ключ авторизації
        var symmetricSecurityKey = new SymmetricSecurityKey(keyBytes);
        //Вказуємо ключ і алгоримт шифрування токена
        var signinCredentials = new SigningCredentials(symmetricSecurityKey,
            SecurityAlgorithms.HmacSha256);
        //Готуємо усі налаштування для токена
        var jwtSecurityToken = new JwtSecurityToken(
            claims: claims,
            expires: DateTime.UtcNow.AddDays(7),
            signingCredentials: signinCredentials);
        string token = new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken);
        return token;
    }
}
