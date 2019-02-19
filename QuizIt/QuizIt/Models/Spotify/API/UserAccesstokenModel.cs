using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuizIt.Models.Spotify.API
{
    public class UserAccesstokenModel
    {
        public string access_token { get; set; }
        public string token_type { get; set; }
        public string scope { get; set; }
        public int expires_in { get; set; }
        public string refresh_token { get; set; }

        public override string ToString()
        {
            return "Access_token: " + access_token + " token_type: " + token_type + " scope: " + scope + " expires_in: " + expires_in + " refresh_token:" + refresh_token;
        }
    }
}
