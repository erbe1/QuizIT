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

        public async Task UpdateScore(string user)
        {
            int score = QuizController.PlayersScore[user];
            score++;
            QuizController.PlayersScore[user] = score;

            await Clients.All.SendAsync("ReceiveName", QuizController.PlayersScore.Select(x=>new { name=x.Key, score=x.Value }).ToList()); // user,score); //Skicka med score också
        }

        public async Task SendMessage(string user)
        {
            string message = $" tryckte på knappen {DateTime.Now.Hour} : {DateTime.Now.Minute} : {DateTime.Now.Second} : {DateTime.Now.Millisecond}";
            await Clients.All.SendAsync("ReceiveMessage", user, message);
        }

        public async Task SendName(string user)
        {
            int score = 0;

            QuizController.PlayersScore.Add(user, score);

            await Clients.All.SendAsync("ReceiveName", QuizController.PlayersScore.Select(x => new { name = x.Key, score = x.Value }).ToList());
        }

        public async Task DisplayQuestion()
        {
            var questionId = QuizController.QuestionId;
            var currentQuestion = QuizController.CurrentQuestion+1;

            var question = _context.Questions.Single(q => q.Id == questionId);

            await Clients.All.SendAsync("DisplayQuestion", question.TrackQuestion, question.Answer, question.TrackId, currentQuestion);
        }
    }
}
