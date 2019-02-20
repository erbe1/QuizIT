using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace QuizIt.Test
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void get_spotify_tracks()
        {
            var service = new PlaybackService();
            string result = service.Search("https://api.spotify.com/v1/search?q=popular&type=track").Result;

        }


    }
}
