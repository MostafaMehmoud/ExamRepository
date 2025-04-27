using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Exam.DAL.Dtos;
using Exam.DAL.Entities;

namespace Exam.BL.Services.IServices
{
    /// <summary>
    /// واجهة خدمة إدارة الأسئلة والاختيارات
    /// </summary>
    public interface IQuestionService
    {
        /// <summary>
        /// الحصول على جميع أسئلة امتحان معين
        /// </summary>
        /// <param name="examId">معرف الامتحان</param>
        /// <returns>قائمة أسئلة الامتحان</returns>
        Task<IEnumerable<Question>> GetQuestionsByExamIdAsync(int examId);

        /// <summary>
        /// الحصول على سؤال بواسطة معرفه
        /// </summary>
        /// <param name="questionId">معرف السؤال</param>
        /// <returns>السؤال المطلوب أو null إذا لم يوجد</returns>
        Task<Question> GetQuestionByIdAsync(int questionId);

        /// <summary>
        /// إنشاء سؤال جديد
        /// </summary>
        /// <param name="question">نموذج إنشاء السؤال</param>
        /// <returns>السؤال الذي تم إنشاؤه</returns>
        Task<Question> CreateQuestionAsync(QuestionCreateModel question);

        /// <summary>
        /// تحديث بيانات سؤال موجود
        /// </summary>
        /// <param name="questionId">معرف السؤال</param>
        /// <param name="question">نموذج تحديث السؤال</param>
        /// <returns>السؤال الذي تم تحديثه</returns>
        Task<Question> UpdateQuestionAsync(int questionId, QuestionUpdateModel question);

        /// <summary>
        /// حذف سؤال
        /// </summary>
        /// <param name="questionId">معرف السؤال</param>
        /// <returns>True إذا تم الحذف بنجاح</returns>
        Task<bool> DeleteQuestionAsync(int questionId);

        /// <summary>
        /// التحقق من وجود سؤال
        /// </summary>
        /// <param name="questionId">معرف السؤال</param>
        /// <returns>True إذا كان السؤال موجوداً</returns>
        Task<bool> QuestionExistsAsync(int questionId);

        /// <summary>
        /// إضافة اختيار جديد لسؤال
        /// </summary>
        /// <param name="questionId">معرف السؤال</param>
        /// <param name="choice">نموذج إنشاء الاختيار</param>
        /// <returns>الاختيار الذي تم إنشاؤه</returns>
        Task<Choice> AddChoiceToQuestionAsync(int questionId, ChoiceCreateModel choice);

        /// <summary>
        /// تحديث اختيار موجود
        /// </summary>
        /// <param name="choiceId">معرف الاختيار</param>
        /// <param name="choice">نموذج تحديث الاختيار</param>
        /// <returns>الاختيار الذي تم تحديثه</returns>
        Task<Choice> UpdateChoiceAsync(int choiceId, ChoiceUpdateModel choice);

        /// <summary>
        /// حذف اختيار
        /// </summary>
        /// <param name="choiceId">معرف الاختيار</param>
        /// <returns>True إذا تم الحذف بنجاح</returns>
        Task<bool> DeleteChoiceAsync(int choiceId);

        /// <summary>
        /// الحصول على جميع اختيارات سؤال معين
        /// </summary>
        /// <param name="questionId">معرف السؤال</param>
        /// <returns>قائمة اختيارات السؤال</returns>
        Task<IEnumerable<Choice>> GetChoicesForQuestionAsync(int questionId);

        /// <summary>
        /// تغيير الإجابة الصحيحة للسؤال
        /// </summary>
        /// <param name="questionId">معرف السؤال</param>
        /// <param name="correctChoiceId">معرف الاختيار الصحيح</param>
        /// <returns>True إذا تم التغيير بنجاح</returns>
        Task<bool> SetCorrectAnswerAsync(int questionId, int correctChoiceId);
    }
}
