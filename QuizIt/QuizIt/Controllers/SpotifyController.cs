using Microsoft.AspNetCore.Mvc;
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
        private Services.Spotify.IAuthenticationService _authenticationService;
        private IPlaybackService _playbackService;


        public SpotifyController(Services.Spotify.IAuthenticationService authenticationService, IPlaybackService playbackService)
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
            return Ok(user.access_token);
            //return RedirectToAction("Index");

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

        public async Task<IActionResult> Search()
        {
            string search = "clutch";
            var result = await _playbackService.Search(search);
            return View("Index");
        }

        //public async Task<IActionResult> horror()
        //{
        //    var statuscode = await _playbackService.Horror();
        //    return View("Index");

        //}
    }
}
