using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using Exam.BL.Services.IServices;

using Exam.DAL.Dtos;
using Exam.DAL.Entities;
using Exam.DAL.Exceptions;
using Exam.DAL.Repository;
using Exam.DAL.Repository.IRepository;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Exam.BL.Services
{
    public class ExamService : IExamService
    {
        private readonly IExamRepository _examRepository;
        private readonly IQuestionRepository _questionRepository;
        private readonly IUserExamRepository _userExamRepository;
        private readonly IUserAnswerRepository _userAnswerRepository;
        private readonly IUserAuthRepository _userAuthRepository;
        private readonly ILogger<ExamService> _logger;

        public ExamService(
            IExamRepository examRepository,
            IQuestionRepository questionRepository,
            ILogger<ExamService> logger,
            IUserExamRepository userExamRepository,
            IUserAnswerRepository userAnswerRepository,
            IUserAuthRepository userAuthRepository)
        {
            _examRepository = examRepository;
            _questionRepository = questionRepository;
            _logger = logger;
            _userExamRepository = userExamRepository;
            _userAnswerRepository = userAnswerRepository;
            _userAuthRepository = userAuthRepository;
        }
        public async Task<List<ExamResultDto>> GetUserExamResultsAsync(ClaimsPrincipal user)
        {
            try
            {
                var currentUser = await _userAuthRepository.GetCurrentUserAsync(user);
                if (currentUser == null)
                {
                    throw new InvalidOperationException("المستخدم غير موجود");
                }

                // التحقق من وجود الامتحانات للمستخدم
                var userExams = await _userExamRepository.GetUserExamsAsync(currentUser.Id);

                if (userExams == null || !userExams.Any())
                {
                    // لا توجد امتحانات للمستخدم
                    return new List<ExamResultDto>();
                }

                var results = userExams.Select(ue =>
                {
                    // التحقق من وجود الامتحان
                    var exam = ue.Exam;
                    if (exam == null || exam.Questions == null)
                    {
                        // إذا لم يكن هناك امتحان أو أسئلة، يمكن تجاهل هذا العنصر أو تسجيل خطأ
                        return null;
                    }

                    int totalQuestions = exam.Questions.Count;

                    // التأكد من أن UserAnswers ليست فارغة
                    var correctAnswers = ue.UserAnswers?.Count(ua => ua.SelectedChoice != null && ua.SelectedChoice.IsCorrect) ?? 0;

                    return new ExamResultDto
                    {
                        ExamId = ue.ExamId,
                        ExamTitle = exam.Title,
                        Score = ue.Score,
                        IsPassed = ue.IsPassed,
                        CorrectAnswers = correctAnswers,
                        TotalQuestions = totalQuestions,
                        PassingPercentage = 60 // الحد الأدنى للنجاح
                    };
                }).Where(result => result != null).ToList();  // تصفية العناصر null

                return results;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while retrieving user exam results");
                throw;
            }
        }


        public async Task<IEnumerable<Exam.DAL.Entities.Exam>> GetAllExamsAsync(bool includeQuestions = false)
        {
            try
            {
                return await _examRepository.GetAllAsync(includeQuestions);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while getting all exams");
                throw;
            }
        }

        public async Task<Exam.DAL.Entities.Exam> GetExamByIdAsync(int id, bool includeQuestions = true)
        {
            try
            {
                var exam = await _examRepository.GetByIdAsync(id, includeQuestions);
                if (exam == null)
                {
                    throw new NotFoundException(nameof(Exam.DAL.Entities.Exam), id);
                }
                return exam;
            }
            catch (NotFoundException)
            {
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error while getting exam with ID {id}");
                throw;
            }
        }

        public async Task<Exam.DAL.Entities.Exam> CreateExamAsync(ExamCreateDto examDto)
        {
            try
            {
                var exam = new Exam.DAL.Entities.Exam
                {
                    Title = examDto.Title,
                    Description = examDto.Description,
                    DurationMinutes = examDto.DurationMinutes,
                    CreatedDate = DateTime.UtcNow,
                    Questions = examDto.Questions?.Select(q => new Question
                    {
                        Title = q.Title,
                        Points = q.Points,
                        Choices = q.Choices?.Select(c => new Choice
                        {
                            Text = c.Text,
                            IsCorrect = c.IsCorrect
                        }).ToList()
                    }).ToList()
                };

                await _examRepository.AddAsync(exam);

                return exam;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while creating new exam");
                throw;
            }
        }

        public async Task UpdateExamAsync(int id, ExamUpdateDto examDto)
        {
            try
            {
                var exam = await _examRepository.GetByIdAsync(id);
                if (exam == null)
                {
                    throw new NotFoundException(nameof(Exam), id);
                }

                exam.Title = examDto.Title;
                exam.Description = examDto.Description;
                exam.DurationMinutes = examDto.DurationMinutes;

               await _examRepository.UpdateAsync(exam);
               
            }
            catch (NotFoundException)
            {
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error while updating exam with ID {id}");
                throw;
            }
        }

        public async Task DeleteExamAsync(int id)
        {
            try
            {
                var exam = await _examRepository.GetByIdAsync(id, includeQuestions: true);
                if (exam == null)
                {
                    throw new NotFoundException(nameof(Exam), id);
                }

                // حذف جميع الأسئلة المرتبطة والاختيارات أولاً
                foreach (var question in exam.Questions)
                {
                    await _questionRepository.DeleteAsync(question.Id);
                }

                await _examRepository.DeleteAsync(id);
              
            }
            catch (NotFoundException)
            {
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error while deleting exam with ID {id}");
                throw;
            }
        }

        public async Task<ExamWithQuestionsDto> GetExamWithQuestionsAsync(int id)
        {
            try
            {
                var exam = await _examRepository.GetByIdAsync(id, includeQuestions: true);
                if (exam == null)
                {
                    throw new NotFoundException(nameof(Exam), id);
                }

                return new ExamWithQuestionsDto
                {
                    Id = exam.Id,
                    Title = exam.Title,
                    Description = exam.Description,
                    DurationMinutes = exam.DurationMinutes,
                    Questions = exam.Questions.Select(q => new QuestionDto
                    {
                        Id = q.Id,
                        Title = q.Title,
                        Choices = q.Choices.Select(c => new ChoiceDto
                        {
                            Id = c.Id,
                            Text = c.Text
                        }).ToList()
                    }).ToList()
                };
            }
            catch (NotFoundException)
            {
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error while getting exam with questions (ID: {id})");
                throw;
            }
        }

        public async Task<ExamResultDto> EvaluateExamAsync(ExamSubmissionDto submission, ClaimsPrincipal User)
        {
            try
            {
                // التأكد من وجود المستخدم
                var user =await _userAuthRepository.GetCurrentUserAsync(User);
                
                if (user == null)
                {
                    throw new InvalidOperationException($"المستخدم {submission.UserId} غير موجود في قاعدة البيانات.");
                }
                else
                {
                    submission.UserId = user.Id;
                }

                var exam = await _examRepository.GetByIdAsync(submission.ExamId, includeQuestions: true);
                if (exam == null)
                {
                    throw new NotFoundException(nameof(Exam), submission.ExamId);
                }

                int correctAnswers = 0;
                foreach (var answer in submission.Answers)
                {
                    var question = exam.Questions.FirstOrDefault(q => q.Id == answer.QuestionId);
                    if (question != null && question.Choices.Any(c => c.Id == answer.ChoiceId && c.IsCorrect))
                    {
                        correctAnswers++;
                    }
                }

                double scorePercentage = (correctAnswers / (double)exam.Questions.Count) * 100;
                bool isPassed = scorePercentage >= 60;

                // حفظ النتيجة والأجوبة
                await _userExamRepository.SaveExamResultAsync(submission, exam, correctAnswers, scorePercentage, isPassed);

                return new ExamResultDto
                {
                    ExamId = exam.Id,
                    ExamTitle = exam.Title,
                    Score = scorePercentage,
                    IsPassed = isPassed,
                    CorrectAnswers = correctAnswers,
                    TotalQuestions = exam.Questions.Count,
                    PassingPercentage = 60
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while evaluating exam");
                throw;
            }
        }


        public async Task UpdateExamAsync(Exam.DAL.Entities.Exam exam)
        {
            try
            {
                await _examRepository.UpdateAsync(exam);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error while updating exam with ID {exam.Id}");
                throw;
            }
        }

        // الطريقة الجديدة التي تضيفها
        public async Task UpdateExamWithIdAsync(int id, ExamUpdateDto examDto)
        {
            try
            {
                var exam = await _examRepository.GetByIdAsync(id);
                if (exam == null)
                {
                    throw new NotFoundException(nameof(Exam), id);
                }

                exam.Title = examDto.Title;
                exam.Description = examDto.Description;
                exam.DurationMinutes = examDto.DurationMinutes;

                await _examRepository.UpdateAsync(exam);
            }
            catch (NotFoundException)
            {
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error while updating exam with ID {id}");
                throw;
            }
        }
    }
}