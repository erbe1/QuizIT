using QuizIt.Models.Spotify.API;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace QuizIt.Services.Spotify
{
    public interface IPlaybackService
    {
        Task<HttpStatusCode> Pause(UserAccesstokenModel user);
        Task<HttpStatusCode> Play(UserAccesstokenModel user);
        Task<HttpStatusCode> Search(UserAccesstokenModel user);


        Task<HttpStatusCode> Pause();
        Task<HttpStatusCode> Play();
        Task<HttpStatusCode> Search();


        void SetUser(UserAccesstokenModel user);

    }
}
