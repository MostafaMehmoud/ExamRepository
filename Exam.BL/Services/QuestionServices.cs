using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Exam.BL.Services.IServices;
using Exam.DAL.Dtos;
using Exam.DAL.Entities;
using Exam.DAL.Exceptions;
using Exam.DAL.Repository.IRepository;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Exam.BL.Services
{
    public class QuestionService : IQuestionService
    {
        private readonly IQuestionRepository _questionRepository;
        private readonly IExamRepository _examRepository;
        private readonly IChoiceRepository _choiceRepository;
        private readonly ILogger<QuestionService> _logger;
        public QuestionService(
            IQuestionRepository questionRepository,
            IExamRepository examRepository,
            IChoiceRepository choiceRepository,
            ILogger<QuestionService> logger)
        {
            _questionRepository = questionRepository;
            _examRepository = examRepository;
            _choiceRepository = choiceRepository;
            _logger = logger;
        }

        public async Task<IEnumerable<Question>> GetQuestionsByExamIdAsync(int examId)
        {
            return await _questionRepository.GetQuestionsByExamIdAsync(examId);
        }

        public async Task<Question> GetQuestionByIdAsync(int questionId)
        {
            return await _questionRepository.GetByIdAsync(questionId);
        }

        public async Task<Question> CreateQuestionAsync(QuestionCreateModel model)
        {
            var examExists = await _examRepository.Exists(model.ExamId);
            if (!examExists)
            {
                throw new ArgumentException("Exam not found");
            }

            var question = new Question
            {
                Title = model.Title,
                ExamId = model.ExamId,
                Points = model.Points,
                Choices = model.Choices.Select(c => new Choice
                {
                    Text = c.Text,
                    IsCorrect = c.IsCorrect
                }).ToList()
            };

            // التحقق من وجود إجابة صحيحة واحدة فقط
            ValidateQuestionChoices(question);

            return await _questionRepository.AddAsync(question);
        }

        public async Task<Question> UpdateQuestionAsync(int questionId, QuestionUpdateModel model)
        {
            var question = await _questionRepository.GetByIdAsync(questionId);
            if (question == null)
            {
                throw new ArgumentException("Question not found");
            }

            question.Title = model.Title;
            question.Points = model.Points;

            await _questionRepository.UpdateAsync(question);
            return question;
        }

        public async Task<bool> DeleteQuestionAsync(int questionId)
        {
            await _questionRepository.DeleteAsync(questionId);
            return true;
        }

        public async Task<bool> QuestionExistsAsync(int questionId)
        {
            return await _questionRepository.Exists(questionId);
        }

        public async Task<Choice> AddChoiceToQuestionAsync(int questionId, ChoiceCreateModel model)
        {
            var question = await _questionRepository.GetByIdAsync(questionId, includeChoices: true);
            if (question == null)
            {
                throw new ArgumentException("Question not found");
            }

            if (question.Choices.Count >= 4)
            {
                throw new InvalidOperationException("Cannot add more than 4 choices to a question");
            }

            var choice = new Choice
            {
                Text = model.Text,
                IsCorrect = model.IsCorrect,
                QuestionId = questionId
            };

            question.Choices.Add(choice);
            await _questionRepository.UpdateAsync(question);
            return choice;
        }

        public async Task<Choice> UpdateChoiceAsync(int choiceId, ChoiceUpdateModel model)
        {
            var choice = await _choiceRepository.GetByIdAsync(choiceId);
            if (choice == null)
            {
                throw new ArgumentException("Choice not found");
            }

            choice.Text = model.Text;
            await _choiceRepository.UpdateAsync(choice);

            return choice;
        }

        public async Task<bool> DeleteChoiceAsync(int choiceId)
        {
            try
            {
                var choice = await _choiceRepository.GetByIdAsync(choiceId);
                if (choice == null)
                {
                    throw new NotFoundException("الاختيار غير موجود");
                }

                await _choiceRepository.DeleteAsync(choice);
                return true;
            }
            catch (Exception ex)
            {
                // تسجيل الخطأ
                _logger.LogError(ex, "خطأ أثناء حذف الاختيار {ChoiceId}", choiceId);
                throw; // إعادة رمي الاستثناء للطبقات الأعلى
            }
        }

        public async Task<IEnumerable<Choice>> GetChoicesForQuestionAsync(int questionId)
        {
            var question = await _questionRepository.GetByIdAsync(questionId, includeChoices: true);
            return question?.Choices ?? Enumerable.Empty<Choice>();
        }

        public async Task<bool> SetCorrectAnswerAsync(int questionId, int correctChoiceId)
        {
            var question = await _questionRepository.GetByIdAsync(questionId, includeChoices: true);
            if (question == null)
            {
                throw new ArgumentException("Question not found");
            }

            var choice = question.Choices.FirstOrDefault(c => c.Id == correctChoiceId);
            if (choice == null)
            {
                throw new ArgumentException("Choice not found for this question");
            }

            // تعيين كل الخيارات كغير صحيحة أولاً
            foreach (var c in question.Choices)
            {
                c.IsCorrect = false;
            }

            // تعيين الخيار المطلوب كإجابة صحيحة
            choice.IsCorrect = true;

            await _questionRepository.UpdateAsync(question);
            return true;
        }

        private void ValidateQuestionChoices(Question question)
        {
            if (question.Choices.Count < 2 || question.Choices.Count > 4)
            {
                throw new InvalidOperationException("Question must have between 2 and 4 choices");
            }

            if (question.Choices.Count(c => c.IsCorrect) != 1)
            {
                throw new InvalidOperationException("Question must have exactly one correct choice");
            }
        }
    }
}
