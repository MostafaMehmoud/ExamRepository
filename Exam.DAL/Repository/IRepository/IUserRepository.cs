using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Exam.DAL.Entities;

namespace Exam.DAL.Repository.IRepository
{
    public interface IUserRepository
    {
        public  Task<ApplicationUser> GetByIdAsync(string id);
        public  Task<IEnumerable<ApplicationUser>> GetAllAsync();
        public Task AddAsync(ApplicationUser user);
        public Task UpdateAsync(ApplicationUser user);


    }
}
