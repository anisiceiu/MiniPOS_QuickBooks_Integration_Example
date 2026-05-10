using MiniPOS.Application.DTOs;
using MiniPOS.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiniPOS.Application.Interfaces.Services
{
    public interface IAccountService
    {
        Task<AuthResponseDto> RegisterAsync(RegisterDto registerDto);
        Task<AuthResponseDto> LoginAsync(LoginDto loginDto);
        Task<bool> IsEmailRegisteredAsync(string email);
        Task<User?> GetUserByEmail(string email);
        Task<bool> VerifyPassword(string PasswordHash, string password);
    }
}
