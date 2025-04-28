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
    public class UserAnswerRepository : IUserAnswerRepository
    {
        private readonly ApplicationDbContext _context;

        public UserAnswerRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task AddAsync(UserAnswer userAnswer)
        {
            await _context.UserAnswers.AddAsync(userAnswer);
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }
        public async Task<List<UserExam>> GetUserExamsAsync(string userId)
        {
            return await _context.UserExams
                .Include(ue => ue.Exam)
                    .ThenInclude(e => e.Questions)
                        .ThenInclude(q => q.Choices)
                .Include(ue => ue.UserAnswers)
                    .ThenInclude(ua => ua.SelectedChoice)
                .Where(ue => ue.UserId == userId)
                .ToListAsync();
        }

    }
}
