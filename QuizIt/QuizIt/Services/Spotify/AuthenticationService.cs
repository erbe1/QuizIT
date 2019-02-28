using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using QuizIt.Models;
using QuizIt.Models.Spotify;
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
        const string ClientID = "3477b483f7e1404ea1c8d6061ff5d815";

        const string scope_ModifyPlayback = "user-modify-playback-state";
        private readonly IOptions<SpotifyApiConfig> _spotifyapiconfig;
        private readonly IOptions<SiteConfig> _siteconfig;

        //public string AccessToken { get; private set; }
        //public DateTime TokenValidTo { get; private set; }
        public AuthenticationService(IOptions<SpotifyApiConfig> spotifyapiconfig, IOptions<SiteConfig> siteconfig)
        {
            _spotifyapiconfig = spotifyapiconfig;
            _siteconfig = siteconfig;
        }

        public string GetRequestURI()
        {
            return $"{BaseURL}?client_id={ClientID}&response_type=code&redirect_uri={_siteconfig.Value.SpotifyApiRedirectURI}&scope={scope_ModifyPlayback}&show_dialog=true";
        }

        public async Task<UserAccesstokenModel> RequestRefreshAndAccessTokens(string authorizationCode)
        {
            HttpClient client = new HttpClient();

            client.DefaultRequestHeaders.Clear();
            client.DefaultRequestHeaders.TryAddWithoutValidation("Content-Type", "application/x-www-form-urlencoded");


            var keyAndSecret = Encoding.UTF8.GetBytes(ClientID + ":" + _spotifyapiconfig.Value.ClientSecret);
            string authorizationHeaderValue = ("Basic " + Convert.ToBase64String(keyAndSecret));
            client.DefaultRequestHeaders.TryAddWithoutValidation("Authorization", authorizationHeaderValue);

            var bodyParameters = new Dictionary<string, string>
            {
                { "grant_type", "authorization_code" },
                { "code", authorizationCode },
                { "redirect_uri", _siteconfig.Value.SpotifyApiRedirectURI }
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
