using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Exam.DAL.Entities;

namespace Exam.DAL.Repository.IRepository
{
    public interface IChoiceRepository
    {
        Task<Choice> GetByIdAsync(int id);
        Task UpdateAsync(Choice choice);
        Task DeleteAsync(Choice choice);
    }
}
