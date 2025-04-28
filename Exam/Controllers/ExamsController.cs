using System.Security.Claims;
using Exam.BL.Services.IServices;
using Exam.DAL.Dtos;
using Exam.DAL.Exceptions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Exam.Core.Controllers
{
    [Authorize]
    public class ExamsController : Controller
    {
        private readonly IExamService _examService;
        private readonly ILogger<ExamsController> _logger;

        public ExamsController(
            IExamService examService,
            ILogger<ExamsController> logger)
        {
            _examService = examService;
            _logger = logger;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            try
            {
                var exams = await _examService.GetAllExamsAsync();
                return View(exams);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting exams");
                return View("Error");
            }
        }

        [HttpGet("Take/{id}")]
        public async Task<IActionResult> TakeExam(int id)
        {
            try
            {
                var exam = await _examService.GetExamWithQuestionsAsync(id);
                return View(exam);
            }
            catch (NotFoundException ex)
            {
                _logger.LogWarning(ex, "Exam not found");
                return NotFound();
            }
        }

        [HttpPost("Submit")]
        public async Task<IActionResult> SubmitExam(ExamSubmissionDto submission)
        {
            try
            {
               
                var result = await _examService.EvaluateExamAsync(submission,User);
                return View("ExamResult", result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error submitting exam");
                return View("Error");
            }
        }
        [HttpGet("UserResults")]
        public async Task<IActionResult> UserExamResults()
        {
            try
            {
                var userId = User.Identity.Name; // الحصول على userId من identity الخاص بالمستخدم الحالي
                if (string.IsNullOrEmpty(userId))
                {
                    return RedirectToAction("Login", "Account"); // إعادة توجيه المستخدم إلى صفحة تسجيل الدخول إذا كان الـ userId فارغ
                }

                var results = await _examService.GetUserExamResultsAsync(User);
                if (results == null || !results.Any())
                {
                    return View("NoResults"); // تعرض رسالة للمستخدم لو لا يوجد نتائج
                }

                return View("UserExamResults", results);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error retrieving exam results for user {User.Identity.Name}");
                return View("Error");
            }
        }
        public IActionResult Error()
        {
            return View("Error"); // Ensure this matches the actual location of your Error view
        }


    }
}
