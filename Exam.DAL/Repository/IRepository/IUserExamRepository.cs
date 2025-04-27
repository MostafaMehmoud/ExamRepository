using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Exam.DAL.Entities;

namespace Exam.DAL.Repository.IRepository
{
    public interface IUserExamRepository
    {
        public Task<UserExam> GetByIdAsync(int id);
        public Task AddAsync(UserExam userExam);
        public Task UpdateAsync(UserExam userExam);
    }
}
