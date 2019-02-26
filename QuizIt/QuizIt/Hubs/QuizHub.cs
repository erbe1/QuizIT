using Microsoft.AspNetCore.SignalR;
using QuizIt.Controllers;
using QuizIt.Data;
using QuizIt.Models;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace QuizIt.Hubs
{
    public class QuizHub : Hub
    {
        private readonly ApplicationDbContext _context;

        public QuizHub(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task SendMessage(string user, string result) //result borde vara resultList2
        {


            string message = $" tryckte på knappen {DateTime.Now.Hour} : {DateTime.Now.Minute} : {DateTime.Now.Second} : {DateTime.Now.Millisecond}";
            await Clients.All.SendAsync("ReceiveMessage", user, message, result);
        }

        public async Task DisplayQuestion()
        {
            // hämta frågan
            // skicka till klienterna

            //hämta nuvarande fråga från quizet
            var questionId = QuizController.QuestionId;

            //int questionId = 23; //OBS hårdkodat frågeId

            var question = _context.Questions.Single(q => q.Id == questionId);

            await Clients.All.SendAsync("DisplayQuestion", question.TrackQuestion);
        }
    }
}
