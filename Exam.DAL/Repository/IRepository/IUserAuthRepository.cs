using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Exam.DAL.Entities;

namespace Exam.DAL.Repository.IRepository
{
    public interface IUserAuthRepository
    {
        Task<ApplicationUser> FindByEmailAsync(string email);
        Task<bool> CreateUserAsync(ApplicationUser user, string password);
        Task<bool> SignInUserAsync(string email, string password, bool rememberMe);
        Task SignOutAsync();
    }
}
