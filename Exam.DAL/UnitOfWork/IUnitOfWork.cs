using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Exam.DAL.Repository.IRepository;

namespace Exam.DAL.UnitOfWork
{
    public interface IUnitOfWork : IDisposable
    {
        /// <summary>
        /// مستودع إدارة الامتحانات
        /// </summary>
        IExamRepository Exams { get; }

        /// <summary>
        /// مستودع إدارة الأسئلة
        /// </summary>
        IQuestionRepository Questions { get; }

        /// <summary>
        /// مستودع إدارة المستخدمين
        /// </summary>
        IUserRepository Users { get; }

        /// <summary>
        /// مستودع إدارة امتحانات المستخدمين
        /// </summary>
        IUserExamRepository UserExams { get; }

        /// <summary>
        /// مستودع إدارة الاختيارات
        /// </summary>
        IChoiceRepository Choices { get; }

        /// <summary>
        /// حفظ جميع التغييرات في قاعدة البيانات
        /// </summary>
        /// <returns>عدد السجلات المتأثرة</returns>
        Task<int> CompleteAsync();

        /// <summary>
        /// بدء معاملة جديدة
        /// </summary>
        void BeginTransaction();

        /// <summary>
        /// إتمام المعاملة الحالية
        /// </summary>
        void CommitTransaction();

        /// <summary>
        /// التراجع عن المعاملة الحالية
        /// </summary>
        void RollbackTransaction();
    }
}
