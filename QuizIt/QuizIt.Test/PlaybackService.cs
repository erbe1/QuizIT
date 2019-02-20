using QuizIt.Models.Spotify.API;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace QuizIt.Test
{
    public class PlaybackService
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

        //public async Task<Rootobject> GetMeteorologicalForecast(decimal longitude, decimal latitude)
        //{

        //    string sLongitude = Math.Round(longitude, 3).ToString(new CultureInfo("en"));
        //    string sLatitude = Math.Round(latitude, 3).ToString(new CultureInfo("en"));

        //    string page = $"https://opendata-download-metfcst.smhi.se/api/category/pmp3g/version/2/geotype/point/lon/{sLongitude}/lat/{sLatitude}/data.json";

        //    string result = await Get(page);

        //    return JsonConvert.DeserializeObject<Rootobject>(result);
        //}

        public async Task<string> Get(string url, UserAccesstokenModel user)
        {
            using (HttpClient client = new HttpClient())
            using (HttpResponseMessage response = await client.GetAsync(url))
            using (HttpContent content = response.Content)
            {
                client.DefaultRequestHeaders.Clear();

                string authorizationHeaderValue = ("Bearer " + "BQBBApfz2RfLzup7aJ1-TxA5UxAe067bJuXQKhsjF1ZLgKOdS0grtPsPyEYWkVhL0BMHT-1kceZI57yjfJARPu5G9MOGKlz7yuCZuHT34KhZ5FAeLEo5r79dxtoqCJFo2s3rvG4MCHEdc4jWkR2v");
                client.DefaultRequestHeaders.TryAddWithoutValidation("Authorization", authorizationHeaderValue);

                if (!response.IsSuccessStatusCode)
                    throw new Exception(response.ReasonPhrase);

                return await content.ReadAsStringAsync();
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

        public async Task<string> Search(UserAccesstokenModel user, string search)
        {
            //return await Get(UrlSearch, user);
            return await Get(search, user);

        }

        public async Task<string> Search(string search)
        {
            //q=name:abacab&type=album,track
            //return await Get(UrlSearch, User);
            return await Get(search, User);

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
