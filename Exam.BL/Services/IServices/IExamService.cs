using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Exam.DAL.Dtos;
using Exam.DAL.Entities;
namespace Exam.BL.Services.IServices
{
    public interface IExamService
    {
        Task<IEnumerable<Exam.DAL.Entities.Exam>> GetAllExamsAsync(bool includeQuestions = false);
        Task<Exam.DAL.Entities.Exam> GetExamByIdAsync(int id, bool includeQuestions = true);
        Task<Exam.DAL.Entities.Exam> CreateExamAsync(ExamCreateDto examDto);
        Task UpdateExamAsync(Exam.DAL.Entities.Exam exam); // الطريقة الأصلية
        Task UpdateExamWithIdAsync(int id, ExamUpdateDto examDto); // الطريقة الجديدة
        Task DeleteExamAsync(int id);
        Task<ExamWithQuestionsDto> GetExamWithQuestionsAsync(int id);
        Task<ExamResultDto> EvaluateExamAsync(ExamSubmissionDto submission);
    }
}
