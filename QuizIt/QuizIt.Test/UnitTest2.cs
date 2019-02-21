using Microsoft.VisualStudio.TestTools.UnitTesting;
using QuizIt.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuizIt.Test
{
    public class UnitTest2
    {
        [TestMethod]
        public void get_spotify_tracks()
        {
            var service = new PlaybackService();
            string result = service.Search("https://api.spotify.com/v1/search?q=popular&type=track").Result;

        }

        [TestMethod]
        public void get_spotify_tracks_as_root_object()
        {
            var service = new PlaybackService();
            var result = service.SearchForTrack("https://api.spotify.com/v1/search?q=popular&type=track").Result;

        }

        [TestMethod]
        public void add_spotify_track_name_and_id_to_question()
        {
            var service = new PlaybackService();
            var result = service.SearchForTrack("https://api.spotify.com/v1/search?q=popular&type=track").Result;

            Question question = new Question();
            question.TrackQuestion = "Vad heter låten?";
            question.Answer = "Popular";
            question.TrackId = result.tracks.items[0].id;
            question.TrackTitle = result.tracks.items[0].name;

        }
    }
}
