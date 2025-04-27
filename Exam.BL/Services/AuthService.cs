using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Exam.BL.Services.IServices;
using Exam.DAL.Dtos;
using Exam.DAL.Entities;
using Exam.DAL.Repository.IRepository;
using Microsoft.AspNetCore.Identity;

namespace Exam.BL.Services
{
    public class AuthService : IAuthService
    {
        private readonly IUserAuthRepository _userRepository;

        public AuthService(IUserAuthRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<bool> RegisterAsync(RegisterDto model)
        {
            var user = new ApplicationUser
            {
                UserName = model.Email,
                Email = model.Email,
                FullName = model.FullName
            };

            var result = await _userRepository.CreateUserAsync(user, model.Password);

            if (result)
            {
                await _userRepository.SignInUserAsync(model.Email, model.Password, rememberMe: false);
                return true;
            }

            return false;
        }

        public async Task<bool> LoginAsync(LoginDto model)
        {
            var result = await _userRepository.SignInUserAsync(model.Email, model.Password, model.RememberMe);
            return result;
        }

        public async Task LogoutAsync()
        {
            await _userRepository.SignOutAsync();
        }
    }
}
