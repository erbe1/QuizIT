using QuizIt.Models.Spotify.API;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace QuizIt.Services.Spotify
{
    public class PlaybackService : IPlaybackService
    {
        const string UrlPause = "https://api.spotify.com/v1/me/player/pause";
        const string UrlPlay = "https://api.spotify.com/v1/me/player/play";
        const string UrlSearch = "https://api.spotify.com/v1/search";
        public UserAccesstokenModel User { get; set; }

        public async Task<HttpStatusCode> Put(string url, UserAccesstokenModel user, FormUrlEncodedContent body = null)
        {
            Uri uri = new Uri(url);

            using (HttpClient client = new HttpClient())
            {
                client.DefaultRequestHeaders.Clear();

                string authorizationHeaderValue = ("Bearer " + user.access_token);
                client.DefaultRequestHeaders.TryAddWithoutValidation("Authorization", authorizationHeaderValue);
                HttpResponseMessage response = await client.PutAsync(uri, body);
                return response.StatusCode;
            }
        }

        public async Task<HttpStatusCode> Pause(UserAccesstokenModel user)
        {
            return await Put(UrlPause, user);
        }

        public async Task<HttpStatusCode> Pause()
        {
            return await Put(UrlPause, User);
        }

        public async Task<HttpStatusCode> Play(UserAccesstokenModel user)
        {
            return await Put(UrlPlay, user);
        }

        public async Task<HttpStatusCode> Play()
        {
            return await Put(UrlPlay, User);
        }

        public async Task<HttpStatusCode> Search(UserAccesstokenModel user)
        {
            return await Put(UrlSearch, user);
        }

        public async Task<HttpStatusCode> Search()
        {
            return await Put(UrlSearch, User);
        }

        public void SetUser(UserAccesstokenModel user)
        {
            User = user;
        }

        public async Task<HttpStatusCode> Horror()
        {
            var bodyParameters = new Dictionary<string, string>
            {
                { "context_uri", "spotify:album:5ht7ItJgpBH7W6vJ5BqpPr" }
            };

            FormUrlEncodedContent body = new FormUrlEncodedContent(bodyParameters);

            return await Put(UrlPlay, User, body);

        }
    }
}
