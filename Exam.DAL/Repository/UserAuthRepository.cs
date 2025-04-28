using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Exam.DAL.Entities;
using Exam.DAL.Repository.IRepository;
using Microsoft.AspNetCore.Identity;

namespace Exam.DAL.Repository
{
    public class UserAuthRepository:IUserAuthRepository
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;

        public UserAuthRepository(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        public async Task<ApplicationUser> FindByEmailAsync(string email)
        {
            return await _userManager.FindByEmailAsync(email);
        }

        public async Task<bool> CreateUserAsync(ApplicationUser user, string password)
        {
            var result = await _userManager.CreateAsync(user, password);
            return result.Succeeded;
        }

        public async Task<bool> SignInUserAsync(string email, string password, bool rememberMe)
        {
            var result = await _signInManager.PasswordSignInAsync(email, password, rememberMe, lockoutOnFailure: false);
            return result.Succeeded;
        }

        public async Task SignOutAsync()
        {
            await _signInManager.SignOutAsync();
        }// Method to fetch a user by their ID
        public async Task<ApplicationUser> GetUserByIdAsync(string userId)
        {
            // This will use UserManager to find the user by ID
            var user = await _userManager.FindByIdAsync(userId);
            return user;
        }

        // Method to fetch the current user (you could call this in your controller or service)
        public async Task<ApplicationUser> GetCurrentUserAsync(ClaimsPrincipal currentUser)
        {
            return await _userManager.GetUserAsync(currentUser);
        }
    }
}
