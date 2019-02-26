using Microsoft.AspNetCore.SignalR;
using System;
using System.Threading.Tasks;

namespace QuizIt.Hubs
{
    public class QuizHub : Hub
    {
        public async Task SendMessage(string user)
        {
            string message = $" tryckte på knappen {DateTime.Now.Hour} : {DateTime.Now.Minute} : {DateTime.Now.Second} : {DateTime.Now.Millisecond}";
            await Clients.All.SendAsync("ReceiveMessage", user, message);
        }
    }
}
