using Microsoft.AspNetCore.Mvc;
using QuizIt.Models;
using QuizIt.Models.ViewModels;
using QuizIt.Services.Spotify;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace QuizIt.Controllers
{
    public class SpotifyController : Controller
    {
        private IAuthenticationService _authenticationService;
        private IPlaybackService _playbackService;
        public static string _token;

        public SpotifyController(IAuthenticationService authenticationService, IPlaybackService playbackService)
        {
            _authenticationService = authenticationService;
            _playbackService = playbackService;
        }


        public IActionResult Index(QuizQuestionsVm vm)
        {
            return View(vm);
        }

        public IActionResult Authorize()
        {
            return Redirect(_authenticationService.GetRequestURI());
        }

        public async Task<IActionResult> Callback(string code)
        {
            var user = await _authenticationService.RequestRefreshAndAccessTokens(code);
            _playbackService.SetUser(user);
            _token = user.access_token;
            return RedirectToAction("Index", "Quiz");

        }

        public async Task<IActionResult> Play()
        {
            var statuscode = await _playbackService.Play();
            return View("Index");
        }
        public async Task<IActionResult> Paus()
        {
            var statuscode = await _playbackService.Pause();
            return View("Index");
        }


        //public async Task<IActionResult> Search(CreateQuizVM createquizvm)
        //{
        //    var service = new PlaybackService();
        //    var result = service.GetSpotifyTracks(createquizvm.Question.TrackTitle).Result;

        //    Question question = new Question
        //    {
        //        TrackId = result.tracks.items[0].id,
        //        TrackTitle = result.tracks.items[0].name
        //    };

        //    return View("Index", question);
        //}


        public IActionResult SearchApi(CreateQuizVM createquizvm, int quizId)
        {
            var service = new PlaybackService();
            var result = service.GetSpotifyTracks(createquizvm.Question.TrackTitle).Result;
            QuizQuestionsVm vm = new QuizQuestionsVm();
            vm.Suggestions = result.tracks.items.Select(x => x.id).ToList();
            vm.Quiz = new Quiz
            {
                Id = quizId
            };

            //Question suggetstion = new Question
            //{
            //    Suggestions = result.tracks.items.Select(x => x.id).ToList()
            //};

            return View("Index", vm);

        }
    }
}
