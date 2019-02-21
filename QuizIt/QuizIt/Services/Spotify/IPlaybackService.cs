using QuizIt.Models.Spotify.API;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using static QuizIt.Models.Spotify.SpotifyClasses;

namespace QuizIt.Services.Spotify
{
    public interface IPlaybackService
    {
        Task<HttpStatusCode> Pause(UserAccesstokenModel user);
        Task<HttpStatusCode> Play(UserAccesstokenModel user);
        Task<string> Search(UserAccesstokenModel user, string search);


        Task<HttpStatusCode> Pause();
        Task<HttpStatusCode> Play();
        Task<string> Search(string search);
        Task<Rootobject> SearchForTrack(string search);

        void SetUser(UserAccesstokenModel user);

    }
}
