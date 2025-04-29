using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Exam.DAL.Data;
using Exam.DAL.Dtos;
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

        public async Task SaveExamResultAsync(ExamSubmissionDto submission, Exam.DAL.Entities.Exam exam, int correctAnswers, double scorePercentage, bool isPassed)
        {
            var userExam = new UserExam
            {
                UserId = submission.UserId,
                ExamId = exam.Id,
                Score = (int)scorePercentage,
                IsPassed = isPassed,
                TakenAt = DateTime.UtcNow
            };

            _context.UserExams.Add(userExam);
            await _context.SaveChangesAsync();  // تأكد من حفظ UserExam أولاً

            var userAnswers = submission.Answers.Select(answer => new UserAnswer
            {
                UserExamId = userExam.Id,
                QuestionId = answer.QuestionId,
                SelectedChoiceId = answer.ChoiceId,

            }).ToList();

            _context.UserAnswers.AddRange(userAnswers);  // إضافة جميع الأجوبة
            await _context.SaveChangesAsync();  // حفظ الأجوبة
        }


        public async Task<IEnumerable<UserExam>> GetUserExamsAsync(string userId)
        {
            return await _context.UserExams
      .Include(ue => ue.Exam)
          .ThenInclude(e => e.Questions)
              .ThenInclude(q => q.Choices)
      .Include(ue => ue.UserAnswers)
          .ThenInclude(ua => ua.SelectedChoice) // أضف هذا السطر
      .Where(ue => ue.UserId == userId)
      .OrderByDescending(ue => ue.TakenAt)
      .ToListAsync();

        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}
