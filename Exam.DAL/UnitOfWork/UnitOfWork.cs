using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Exam.DAL.Data;
using Exam.DAL.Repository.IRepository;
using Exam.DAL.Repository;
using Microsoft.EntityFrameworkCore.Storage;

namespace Exam.DAL.UnitOfWork
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationDbContext _context;
        private IDbContextTransaction _transaction;

        public UnitOfWork(ApplicationDbContext context)
        {
            _context = context;
            Exams = new ExamRepository(_context);
            Questions = new QuestionRepository(_context);
            Users = new UserRepository(_context);
            UserExams = new UserExamRepository(_context);
            Choices = new ChoiceRepository(_context);
        }

        public IExamRepository Exams { get; private set; }
        public IQuestionRepository Questions { get; private set; }
        public IUserRepository Users { get; private set; }
        public IUserExamRepository UserExams { get; private set; }
        public IChoiceRepository Choices { get; private set; }

        public async Task<int> CompleteAsync()
        {
            try
            {
                return await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                // يمكنك إضافة تسجيل الأخطاء هنا
                throw new Exception("فشل في حفظ التغييرات", ex);
            }
        }

        public void BeginTransaction()
        {
            if (_transaction != null)
            {
                throw new InvalidOperationException("يوجد معاملة نشطة بالفعل");
            }
            _transaction = _context.Database.BeginTransaction();
        }

        public void CommitTransaction()
        {
            try
            {
                _transaction?.Commit();
            }
            catch
            {
                RollbackTransaction();
                throw;
            }
            finally
            {
                if (_transaction != null)
                {
                    _transaction.Dispose();
                    _transaction = null;
                }
            }
        }

        public void RollbackTransaction()
        {
            try
            {
                _transaction?.Rollback();
            }
            finally
            {
                if (_transaction != null)
                {
                    _transaction.Dispose();
                    _transaction = null;
                }
            }
        }

        private bool _disposed = false;

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    _transaction?.Dispose();
                    _context.Dispose();
                }
                _disposed = true;
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
