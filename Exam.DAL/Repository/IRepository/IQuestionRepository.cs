using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Exam.DAL.Dtos;
using Exam.DAL.Entities;

namespace Exam.DAL.Repository.IRepository
{
    public interface IQuestionRepository
    {
        Task<IEnumerable<Question>> GetQuestionsByExamIdAsync(int examId, bool includeChoices = true);
        Task<Question> GetByIdAsync(int id, bool includeChoices = true);
        Task<Question> AddAsync(Question question);
        Task UpdateAsync(Question question);
        Task DeleteAsync(int id);
        Task<bool> Exists(int id);
      Task<Question> GetQuestionContainingChoiceAsync(int choiceId);
        
    }
}
