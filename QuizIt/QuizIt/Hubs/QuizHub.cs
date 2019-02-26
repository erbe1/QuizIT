using Microsoft.AspNetCore.SignalR;
using System.Threading.Tasks;

namespace QuizIt.Hubs
{
    public class QuizHub : Hub
    {
        public async Task SendMessage(string user, string message)
        {
            await Clients.All.SendAsync("ReceiveMessage", user, message);
        }
    }
}
