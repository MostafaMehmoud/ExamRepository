using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Exam.DAL.Dtos;
using Exam.DAL.Entities;

namespace Exam.DAL.Repository.IRepository
{
    public interface IUserExamRepository
    {
        public Task SaveExamResultAsync(ExamSubmissionDto submission, Exam.DAL.Entities.Exam exam, int correctAnswers, double scorePercentage, bool isPassed);
        Task<IEnumerable<UserExam>> GetUserExamsAsync(string userId);
        Task SaveChangesAsync();
    }
}
