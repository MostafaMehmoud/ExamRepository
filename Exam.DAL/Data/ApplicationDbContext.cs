using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using Exam.DAL.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Exam.DAL.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

        public DbSet<Exam.DAL.Entities.Exam> Exams { get; set; }
        public DbSet<Question> Questions { get; set; }
        public DbSet<Choice> Choices { get; set; }
        public DbSet<UserExam> UserExams { get; set; }
        public DbSet<UserAnswer> UserAnswers { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.Entity<UserAnswer>()
       .HasOne(ua => ua.UserExam)
       .WithMany()
       .HasForeignKey(ua => ua.UserExamId)
       .OnDelete(DeleteBehavior.Cascade); // This is OK

            builder.Entity<UserAnswer>()
                .HasOne(ua => ua.Question)
                .WithMany()
                .HasForeignKey(ua => ua.QuestionId)
                .OnDelete(DeleteBehavior.Restrict); // Change from Cascade to Restrict

            builder.Entity<UserAnswer>()
                .HasOne(ua => ua.SelectedChoice)
                .WithMany()
                .HasForeignKey(ua => ua.SelectedChoiceId)
                .OnDelete(DeleteBehavior.Restrict); // Change from Cascade to Restrict
            // Configure relationships if needed (optional, EF Core is smart)
            builder.Entity<UserExam>()
        .HasIndex(ue => ue.UserId);

            builder.Entity<UserExam>()
                .HasIndex(ue => ue.ExamId);

            builder.Entity<UserAnswer>()
                .HasIndex(ua => ua.UserExamId);

            builder.Entity<Question>()
            .HasIndex(q => q.ExamId);

            builder.Entity<Choice>()
                .HasIndex(c => c.QuestionId);
        }
    }
}
