using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Exam.DAL.Dtos;

namespace Exam.BL.Services.IServices
{
    public interface IAuthService
    {
        Task<bool> RegisterAsync(RegisterDto model);
        Task<bool> LoginAsync(LoginDto model);
        Task LogoutAsync();
    }
}
