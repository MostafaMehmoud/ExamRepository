using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Exam.DAL.Repository.IRepository
{
    public interface IExamRepository
    {
        Task<IEnumerable<Exam.DAL.Entities.Exam>> GetAllAsync(bool includeQuestions = false, bool includeChoices = false);
        Task<Exam.DAL.Entities.Exam> GetByIdAsync(int id, bool includeQuestions = true, bool includeChoices = true);
        Task<Exam.DAL.Entities.Exam> AddAsync(Exam.DAL.Entities.Exam exam);
        Task UpdateAsync(Exam.DAL.Entities.Exam exam);
        Task DeleteAsync(int id);
        Task<bool> Exists(int id);
        Task<int> GetQuestionsCount(int examId);
    }
}
