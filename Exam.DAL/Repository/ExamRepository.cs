using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Exam.DAL.Data;
using Exam.DAL.Repository.IRepository;
using Microsoft.EntityFrameworkCore;

namespace Exam.DAL.Repository
{
    public class ExamRepository : IExamRepository
    {
        private readonly ApplicationDbContext _context;

        public ExamRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Exam.DAL.Entities.Exam>> GetAllAsync(bool includeQuestions = false, bool includeChoices = false)
        {
            var query = _context.Exams.AsQueryable();

            if (includeQuestions)
            {
                query = query.Include(e => e.Questions);

                if (includeChoices)
                {
                    query = query.Include(e => e.Questions)
                                 .ThenInclude(q => q.Choices);
                }
            }

            return await query.ToListAsync();
        }

        public async Task<Exam.DAL.Entities.Exam> GetByIdAsync(int id, bool includeQuestions = true, bool includeChoices = true)
        {
            var query = _context.Exams.AsQueryable();

            if (includeQuestions)
            {
                query = query.Include(e => e.Questions);

                if (includeChoices)
                {
                    query = query.Include(e => e.Questions)
                                 .ThenInclude(q => q.Choices);
                }
            }

            return await query.FirstOrDefaultAsync(e => e.Id == id);
        }

        public async Task<Exam.DAL.Entities.Exam> AddAsync(Exam.DAL.Entities.Exam exam)
        {
            await _context.Exams.AddAsync(exam);
            await _context.SaveChangesAsync();
            return exam;
        }

        public async Task UpdateAsync(Exam.DAL.Entities.Exam exam)
        {
            _context.Entry(exam).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var exam = await _context.Exams
                .Include(e => e.Questions)
                .ThenInclude(q => q.Choices)
                .FirstOrDefaultAsync(e => e.Id == id);

            if (exam != null)
            {
                // حذف جميع الخيارات أولاً
                foreach (var question in exam.Questions)
                {
                    _context.Choices.RemoveRange(question.Choices);
                }

                // ثم حذف الأسئلة
                _context.Questions.RemoveRange(exam.Questions);

                // وأخيراً حذف الامتحان
                _context.Exams.Remove(exam);

                await _context.SaveChangesAsync();
            }
        }

        public async Task<bool> Exists(int id)
        {
            return await _context.Exams.AnyAsync(e => e.Id == id);
        }

        public async Task<int> GetQuestionsCount(int examId)
        {
            return await _context.Questions
                .Where(q => q.ExamId == examId)
                .CountAsync();
        }
    }
}
