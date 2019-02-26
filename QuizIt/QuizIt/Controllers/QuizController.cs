using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using QuizIt.Data;
using QuizIt.Hubs;
using QuizIt.Models;
using QuizIt.Models.ViewModels;

namespace QuizIt.Controllers
{

    public class QuizController : Controller
    {
        public static int QuestionId;
        public static int CurrentQuestion;
        public static int QuizId;

        private readonly ApplicationDbContext _context;
        private readonly IHubContext<QuizHub> _quizHub;
        public static CreateQuizVM _createquizvm;

        public QuizController(ApplicationDbContext context, IHubContext<QuizHub> quizHub)
        {
            _context = context;
            _quizHub = quizHub;
        }

        // GET: Quiz
        public async Task<IActionResult> Index()
        {
            QuizQuestionsVm vm = new QuizQuestionsVm();
            vm.Quizzez = await _context.Quizzes.ToListAsync();
            return View("Index", vm);
        }

        //Testa denna metod i testprojekt!!
        public IActionResult NextQuestion() //Endast quizledare anropar denna metod
        {
            CurrentQuestion++;

            var allQuestions = _context.Quizzes
                                .Include(q => q.QuizQuestions)
                                .ThenInclude(q => q.Question)
                                .Single(m => m.Id == QuizId)
                                .QuizQuestions.Select(x => x.Question).ToList();

            if (CurrentQuestion >= allQuestions.Count()) //Fråga Oscar
            {
                //_quizHub.Clients.All.SendAsync("DisplayQuestion", "Quizet är slut!").Wait();
                return View("QuizCompleted"); //Kommer ej till vyn
            }

            var question = allQuestions[CurrentQuestion]; //outOfRange exception

            QuestionId = question.Id;

            _quizHub.Clients.All.SendAsync("DisplayQuestion", question.TrackQuestion, question.Answer, question.TrackId).Wait();

            return Ok();
        }

        public IActionResult PlayQuiz(int id)
        {
            //Göra om nedanstående till metod
            var quiz = _context.Quizzes
            .FirstOrDefault(q => q.Id == id);

            if (quiz == null)
            {
                return NotFound();
            }

            var questions = _context.Quizzes
                .Include(q => q.QuizQuestions)
                .ThenInclude(q => q.Question)
                .Single(m => m.Id == id)
                .QuizQuestions.Select(x => x.Question);

            //if (questions == null)
            //{
            //    return NotFound();
            //}

            QuestionId = questions.First().Id;
            QuizId = id;

            return View();
        }

        // GET: Quiz/Details/5
        public IActionResult Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var quiz = _context.Quizzes
                        .FirstOrDefault(q => q.Id == id);

            if (quiz == null)
            {
                return NotFound();
            }

            var questions = _context.Quizzes
                .Include(q => q.QuizQuestions)
                .ThenInclude(q => q.Question)
                .Single(m => m.Id == id)
                .QuizQuestions.Select(x => x.Question);

            if (questions == null)
            {
                return NotFound();
            }

            var vm = new QuizQuestionsVm
            {
                Quiz = quiz,
                Questions = questions.ToList()
            };

            return View(vm);
        }

        // GET: Quiz/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Quiz/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name")] Quiz quiz)
        {
            if (ModelState.IsValid)
            {
                _context.Add(quiz);
                await _context.SaveChangesAsync();

                return RedirectToAction("Create", "Questions", new { quizId = quiz.Id, quizName = quiz.Name });
            }
            return View(quiz);
        }

        // GET: Quiz/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var quiz = await _context.Quizzes.FindAsync(id);
            if (quiz == null)
            {
                return NotFound();
            }

            var questions = _context.Quizzes
                            .Include(q => q.QuizQuestions)
                            .ThenInclude(q => q.Question)
                            .Single(m => m.Id == id)
                            .QuizQuestions.Select(x => x.Question);

            var vm = new QuizQuestionsVm //hämta frågorna också
            {
                Quiz = quiz,
                Questions = questions.ToList()
            };

            return View(vm);
        }

        // POST: Quiz/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name")] Quiz quiz)
        {
            if (id != quiz.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(quiz);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!QuizExists(quiz.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(quiz);
        }

        // GET: Quiz/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var quiz = await _context.Quizzes
                .FirstOrDefaultAsync(m => m.Id == id);
            if (quiz == null)
            {
                return NotFound();
            }

            return View(quiz);
        }

        // POST: Quiz/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var quiz = await _context.Quizzes.FindAsync(id);
            _context.Quizzes.Remove(quiz);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool QuizExists(int id)
        {
            return _context.Quizzes.Any(e => e.Id == id);
        }
    }
}
