using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Exam.DAL.Entities;

namespace Exam.DAL.Repository.IRepository
{
    public interface IUserAnswerRepository
    {
        Task AddAsync(UserAnswer userAnswer);
        Task SaveChangesAsync();
        Task<List<UserExam>> GetUserExamsAsync(string userId); // يكون فيه Include(UserAnswers, Exam, Exam.Questions, Exam.Questions.Choices)
    }
}
