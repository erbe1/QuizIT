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


        public IActionResult Index()
        {
            return View();
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


        public async Task<IActionResult> Search(CreateQuizVM createquizvm)
        {
            var service = new PlaybackService();
            var result = service.GetSpotifyTracks(createquizvm.Question.TrackTitle).Result; //($"https://api.spotify.com/v1/search?q={q.TrackTitle}&type=track").Result;

            Question question = new Question();
            question.TrackQuestion = "Vad heter låten?";
            question.Answer = "Popular";
            question.TrackId = result.tracks.items[0].id; //Dessa värdena ska sparas 
            question.TrackTitle = result.tracks.items[0].name;

            return View("Index", question);
        }

        public IActionResult SearchApi(string term)
        {
            var service = new PlaybackService();
            var result = service.GetSpotifyTracks(term).Result; 

            return Ok(new { result.tracks.items[0].id, result.tracks.items[0].name });
        }

        //public async Task<IActionResult> horror()
        //{
        //    var statuscode = await _playbackService.Horror();
        //    return View("Index");

        //}
    }
}
