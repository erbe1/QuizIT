using Newtonsoft.Json;
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
    public class AuthenticationService : IAuthenticationService
    {
        //AUTHENTICATION 
        const string BaseURL = "https://accounts.spotify.com/authorize";
        const string APITokenURL = "https://accounts.spotify.com/api/token";
        const string ClientID = "d0c8f0103d91489d8f6ec4c18e003a6d";
        const string ClientSecret = "b1afdacbb6e74065b586689be77528b0";

        const string RedirectURI = "https://localhost:44353/spotify/callback/";
        const string scope_ModifyPlayback = "user-modify-playback-state";
        //public string AccessToken { get; private set; }
        //public DateTime TokenValidTo { get; private set; }

        public string GetRequestURI()
        {
            return $"{BaseURL}?client_id={ClientID}&response_type=code&redirect_uri={RedirectURI}&scope={scope_ModifyPlayback}&show_dialog=true";
        }

        public async Task<UserAccesstokenModel> RequestRefreshAndAccessTokens(string authorizationCode)
        {
            HttpClient client = new HttpClient();

            client.DefaultRequestHeaders.Clear();
            client.DefaultRequestHeaders.TryAddWithoutValidation("Content-Type", "application/x-www-form-urlencoded");


            var keyAndSecret = Encoding.UTF8.GetBytes(ClientID + ":" + ClientSecret);
            string authorizationHeaderValue = ("Basic " + Convert.ToBase64String(keyAndSecret));
            client.DefaultRequestHeaders.TryAddWithoutValidation("Authorization", authorizationHeaderValue);

            var bodyParameters = new Dictionary<string, string>
            {
                { "grant_type", "authorization_code" },
                { "code", authorizationCode },
                { "redirect_uri", RedirectURI }
            };

            var content = new FormUrlEncodedContent(bodyParameters);

            Uri url = new Uri(APITokenURL);

            using (HttpResponseMessage response = await client.PostAsync(url, content))
            {
                return await GetModel<UserAccesstokenModel>(response.Content);
            }
        }

        public async Task<HttpStatusCode> Pause(UserAccesstokenModel user)
        {
            HttpClient client = new HttpClient();
            client.DefaultRequestHeaders.Clear();

            string authorizationHeaderValue = ("Bearer " + user.access_token);
            client.DefaultRequestHeaders.TryAddWithoutValidation("Authorization", authorizationHeaderValue);

            Uri url = new Uri("https://api.spotify.com/v1/me/player/pause");
            using (HttpResponseMessage response = await client.PutAsync(url, null))
            {
                return response.StatusCode;
            }
        }

        private async Task<T> GetModel<T>(HttpContent content)
        {
            var stringContet = Encoding.UTF8.GetString(await content.ReadAsByteArrayAsync());
            return JsonConvert.DeserializeObject<T>(stringContet);
        }
    }
}
