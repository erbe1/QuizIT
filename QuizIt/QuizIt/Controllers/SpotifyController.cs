﻿using Microsoft.AspNetCore.Mvc;
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

        public async Task<IActionResult> Search(CreateQuizVM createquizvm, int quizId)
        {
            if (string.IsNullOrWhiteSpace(createquizvm.Question.TrackTitle))
            {
                return RedirectToAction("Index", "Quiz");
            }
            var service = new PlaybackService();
            var result = await service.GetSpotifyTracks(createquizvm.Question.TrackTitle);
            QuizQuestionsVm vm = new QuizQuestionsVm
            {
                Suggestions = result.tracks.items.Select(x => x.id).ToList(),
                Quiz = new Quiz
                {
                    Id = quizId
                },
                Question = new Question
                {
                    TrackTitle = createquizvm.Question.TrackTitle
                }
            };

            return View("Index", vm);
        }
    }
}
