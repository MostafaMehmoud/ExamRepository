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
    public class QuestionRepository : IQuestionRepository
    {
        private readonly ApplicationDbContext _context;

        public QuestionRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Question>> GetQuestionsByExamIdAsync(int examId, bool includeChoices = true)
        {
            var query = _context.Questions.Where(q => q.ExamId == examId);

            if (includeChoices)
            {
                query = query.Include(q => q.Choices);
            }

            return await query.ToListAsync();
        }

        public async Task<Question> GetByIdAsync(int id, bool includeChoices = true)
        {
            var query = _context.Questions.AsQueryable();

            if (includeChoices)
            {
                query = query.Include(q => q.Choices);
            }

            return await query.FirstOrDefaultAsync(q => q.Id == id);
        }

        public async Task<Question> AddAsync(Question question)
        {
            await _context.Questions.AddAsync(question);
            await _context.SaveChangesAsync();
            return question;
        }

        public async Task UpdateAsync(Question question)
        {
            _context.Entry(question).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var question = await _context.Questions
                .Include(q => q.Choices)
                .FirstOrDefaultAsync(q => q.Id == id);

            if (question != null)
            {
                _context.Choices.RemoveRange(question.Choices);
                _context.Questions.Remove(question);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<bool> Exists(int id)
        {
            return await _context.Questions.AnyAsync(q => q.Id == id);
        }
        public async Task<Question> GetQuestionContainingChoiceAsync(int choiceId)
        {
            return await _context.Questions
                .Include(q => q.Choices)
                .FirstOrDefaultAsync(q => q.Choices.Any(c => c.Id == choiceId));
        }
    }
}
