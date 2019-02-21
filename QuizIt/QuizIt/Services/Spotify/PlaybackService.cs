using Newtonsoft.Json;
using QuizIt.Controllers;
using QuizIt.Models.Spotify.API;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using static QuizIt.Models.Spotify.SpotifyClasses;

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

        public async Task<string> Get(string url)
        {
            using (HttpClient client = new HttpClient())
            {
                client.DefaultRequestHeaders.Clear();
                //string authorizationHeaderValue = "Bearer BQAKW0cCXFEYehNrmhMK8So-cXwX-yhQyrZ4vDVIp90ZVKh--5s33RHGk2EoXFZeaMtCEw3hKTAs-Xz36AwQ4zeSWaKZ4rJjUnEu4MBqYBrrmpZKB6md3yCzutb3FGwA-FPZOga-K_pmOusa5IOS";
                string authorizationHeaderValue = ("Bearer " + SpotifyController._token);
                client.DefaultRequestHeaders.TryAddWithoutValidation("Authorization", authorizationHeaderValue);

                HttpResponseMessage response = await client.GetAsync(url);

                if (!response.IsSuccessStatusCode)
                    throw new Exception(response.ReasonPhrase);

                return await response.Content.ReadAsStringAsync();
            }
        }

        public async Task<Rootobject> GetSpotifyTracks(string title)
        {
            string page = $"https://api.spotify.com/v1/search?q={title}&type=track";

            string result = await Get(page);

            return JsonConvert.DeserializeObject<Rootobject>(result);
        }

        //Här göra Get metod för tracks, kolla på SMHI uppgiften
        //Fråga Oscar hur testa på postman? Lista på det som matchar


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

        public async Task<string> Search(UserAccesstokenModel user, string search)
        {
            //return await Get(UrlSearch, user);
            return await Get(search);
        }

        public async Task<string> Search(string search)
        {
            //q=name:abacab&type=album,track
            //return await Get(UrlSearch, User);
            return await Get(search);
        }

        //public async Task<Rootobject> SearchForTrack(string search)
        //{
        //    //q=name:abacab&type=album,track
        //    //return await Get(UrlSearch, User);
        //    return await GetSpotifyTracks();
        //}

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
