using QuizIt.Models.Spotify.API;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace QuizIt.Services.Spotify
{
    public interface IAuthenticationService
    {
        string GetRequestURI();
        Task<UserAccesstokenModel> RequestRefreshAndAccessTokens(string authorizationCode);
        Task<HttpStatusCode> Pause(UserAccesstokenModel user);
    }
}
