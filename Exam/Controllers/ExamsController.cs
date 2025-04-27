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
                submission.UserId = User.Identity.Name;
                var result = await _examService.EvaluateExamAsync(submission);
                return View("ExamResult", result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error submitting exam");
                return View("Error");
            }
        }
    }
}
