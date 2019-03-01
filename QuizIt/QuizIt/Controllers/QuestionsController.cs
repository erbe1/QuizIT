using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using QuizIt.Data;
using QuizIt.Models;
using QuizIt.Models.ViewModels;
using QuizIt.Services.Spotify;
using System.Linq;
using System.Threading.Tasks;

namespace QuizIt.Controllers
{
    public class QuestionsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public QuestionsController(ApplicationDbContext context)
        {
            _context = context;
        }


        // GET: Questions
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.Question;
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: Questions/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var question = await _context.Question
                .FirstOrDefaultAsync(m => m.Id == id);
            if (question == null)
            {
                return NotFound();
            }

            return View(question);
        }

        // GET: Questions/Create
        public IActionResult Create(int quizId, string quizName, int trackIndex, string trackTitle)
        {

            QuizQuestionsVm vm = new QuizQuestionsVm
            {
                Quiz = new Quiz
                {
                    Id = quizId,
                    Name = quizName
                },
                TrackIndex = trackIndex,
                Question = new Question
                {
                    TrackTitle = trackTitle
                }

            };

            return View(vm);
        }

        // POST: Questions/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(QuizQuestionsVm vm, int trackIndex, string trackTitle)
        {
            if (ModelState.IsValid)
            {
                var question = vm.Question;
                var quiz = await _context.Quizzes.FindAsync(vm.Quiz.Id);

                var service = new PlaybackService();
                var result = service.GetSpotifyTracks(vm.Question.TrackTitle).Result;

                question.TrackId = result.tracks.items[trackIndex].id;
                question.TrackTitle = result.tracks.items[trackIndex].name;

                question.QuizQuestions.Add(new QuizQuestion { Quiz = quiz});

                _context.Add(question);

                await _context.SaveChangesAsync();
                return RedirectToAction("Create", new { quizId = quiz.Id, quizName = quiz.Name });
            }
            return View();
        }

        // GET: Questions/Edit/5
        public async Task<IActionResult> Edit(int? id, int quizId)
        {
            if (id == null)
            {
                return NotFound();
            }

            var question = await _context.Question.FindAsync(id);
            if (question == null)
            {
                return NotFound();
            }

            var vm = new QuizQuestionsVm
            {
                Question = question
            };

            var quiz = await _context.Quizzes.FindAsync(quizId);
            vm.Quiz = quiz;

            return View(vm);
        }

        // POST: Questions/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, QuizQuestionsVm vm, int quizId)
        {
            if (id != vm.Question.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var service = new PlaybackService();
                    var result = service.GetSpotifyTracks(vm.Question.TrackTitle).Result;

                    vm.Question.TrackId = result.tracks.items[0].id;
                    vm.Question.TrackTitle = result.tracks.items[0].name;

                    _context.Update(vm.Question);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!QuestionExists(vm.Question.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction("Edit", "Quiz", new { vm.Quiz.Id});
            }
            return View(vm.Question.Id);
        }

        // GET: Questions/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var question = await _context.Question
                .FirstOrDefaultAsync(m => m.Id == id);
            if (question == null)
            {
                return NotFound();
            }

            return View(question);
        }

        // POST: Questions/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var question = await _context.Question.FindAsync(id);
            _context.Question.Remove(question);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool QuestionExists(int id)
        {
            return _context.Question.Any(e => e.Id == id);
        }
    }
}
