using Exam.BL.Services.IServices;
using Exam.DAL.Dtos;
using Exam.DAL.Exceptions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Exam.Core.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
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
            IEnumerable<Exam.DAL.Entities.Exam> exams = await _examService.GetAllExamsAsync(true);
            return View(exams);
        }

        [HttpGet("Create")]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost("Create")]
        public async Task<IActionResult> Create(ExamCreateDto model)
        {
            try
            {
                ModelState.Remove("Questions.Choices.IsCorrect");

                // Custom validation
                if (!ValidateExam(model)) // Validate method checks correct answers
                {
                    ModelState.AddModelError("Questions.Description", "Please ensure there is exactly one correct answer per question.");
                    return View(model);
                }

                //if (!ModelState.IsValid)
                //{
                //    return View(model);
                //}

                await _examService.CreateExamAsync(model);
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating exam");
                ModelState.AddModelError("", "Error creating exam");
                return View(model);
            }
        }
        private bool ValidateExam(ExamCreateDto model)
        {
            foreach (var question in model.Questions)
            {
                var correctChoiceCount = question.Choices.Count(c => c.IsCorrect);
                if (correctChoiceCount != 1)
                {
                    // Add custom validation error
                    ModelState.AddModelError("Questions", "Each question must have exactly one correct answer.");
                    return false;
                }
            }
            return true;
        }

        [HttpGet("Edit/{id}")]
        public async Task<IActionResult> Edit(int id)
        {
            try
            {
                var exam = await _examService.GetExamByIdAsync(id);
                var model = new ExamUpdateDto
                {
                    Title = exam.Title,
                    Description = exam.Description,
                    DurationMinutes = exam.DurationMinutes
                };
                return View(model);
            }
            catch (NotFoundException)
            {
                return NotFound();
            }
        }

        [HttpPost("Edit/{id}")]
        public async Task<IActionResult> Edit(int id, ExamUpdateDto model)
        {
            try
            {
                if (!ModelState.IsValid)
                    return View(model);

                await _examService.UpdateExamWithIdAsync(id, model);
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error updating exam {id}");
                ModelState.AddModelError("", "Error updating exam");
                return View(model);
            }
        }

        [HttpPost("Delete/{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                await _examService.DeleteExamAsync(id);
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error deleting exam {id}");
                return RedirectToAction(nameof(Index));
            }
        }
    }
}
