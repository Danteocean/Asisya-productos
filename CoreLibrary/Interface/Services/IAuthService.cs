using CoreLibrary.DTOs.Auth.Requests;
using CoreLibrary.DTOs.Auth.Response;
using Domain.Wrappers;

namespace CoreLibrary.Interface.Services;

public interface IAuthService
{
    Task<Response<AuthResponse>> Login(LoginRequest request);
}