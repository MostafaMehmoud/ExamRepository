using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Exam.DAL.Data;
using Exam.DAL.Entities;
using Exam.DAL.Repository.IRepository;
using Microsoft.EntityFrameworkCore;

namespace Exam.DAL.Repository
{
    public class UserExamRepository : IUserExamRepository
    {
        private readonly ApplicationDbContext _context;

        public UserExamRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<UserExam> GetByIdAsync(int id)
        {
            return await _context.UserExams
                .Include(ue => ue.Exam)
                
                .FirstOrDefaultAsync(ue => ue.Id == id);
        }

        public async Task AddAsync(UserExam userExam)
        {
            await _context.UserExams.AddAsync(userExam);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(UserExam userExam)
        {
            _context.UserExams.Update(userExam);
            await _context.SaveChangesAsync();
        }
    }
}
