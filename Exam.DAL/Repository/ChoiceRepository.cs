using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Exam.DAL.Data;
using Exam.DAL.Entities;
using Exam.DAL.Repository.IRepository;

namespace Exam.DAL.Repository
{
    public class ChoiceRepository : IChoiceRepository
    {
        private readonly ApplicationDbContext _context;

        public ChoiceRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Choice> GetByIdAsync(int id)
        {
            return await _context.Choices.FindAsync(id);
        }

        public async Task UpdateAsync(Choice choice)
        {
            _context.Choices.Update(choice);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(Choice choice)
        {
            _context.Choices.Remove(choice);
            await _context.SaveChangesAsync();
        }
    }
}
