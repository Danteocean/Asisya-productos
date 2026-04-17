using CoreLibrary.DTOs.Auth.Requests;
using CoreLibrary.DTOs.Auth.Response;
using CoreLibrary.DTOs.Category.Response;
using CoreLibrary.Interface.Repositories;
using CoreLibrary.Interface.Services;
using Domain.Querys;
using Domain.Wrappers;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using BC = BCrypt.Net.BCrypt;

public class AuthService : IAuthService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IConfiguration _config;

    public AuthService(IUnitOfWork unitOfWork, IConfiguration config)
    {
        _unitOfWork = unitOfWork;
        _config = config;
    }

    public async Task<Response<AuthResponse>> Login(LoginRequest request)
    {
        try
        {
            var (user, message) = await _unitOfWork.Queries.QueryFirstOrDefaultAsync<UserEntity>(SqlQueries.SaQuery, new { Username = request.Username });

            if (user == null || !BC.Verify(request.Password, user.PasswordHash))
            {
                return new Response<AuthResponse>(null) { Succeeded = false, Message = "Credenciales incorrectas" };
            }

            var token = GenerateJwtToken((int)user.EmployeeId, (string)user.Username);

            return new Response<AuthResponse>(new AuthResponse
            {
                EmployeeId = user.EmployeeId,
                Username = user.Username,
                FullName = user.FullName,
                Token = token
            })
            { Succeeded = true };
        }
        catch (Exception ex)
        {
            return new Response<AuthResponse>(null)
            { State = "Error", Message = ex.Message, Succeeded = false };
        }
       
    }

    private string GenerateJwtToken(int id, string username)
    {
        var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["JWTSettings:Key"]));
        var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

        var claims = new[] {
            new Claim("id", id.ToString()),
            new Claim(ClaimTypes.Name, username),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
        };

        var token = new JwtSecurityToken(
            issuer: _config["JWTSettings:Issuer"],
            audience: _config["JWTSettings:Audience"],
            claims: claims,
            expires: DateTime.Now.AddMinutes(Convert.ToDouble(_config["JWTSettings:DurationInMinutes"])),
            signingCredentials: credentials);

        return new System.IdentityModel.Tokens.Jwt.JwtSecurityTokenHandler().WriteToken(token);
    }
}