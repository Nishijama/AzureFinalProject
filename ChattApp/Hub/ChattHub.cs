using ChattApp.Models;
using Microsoft.AspNetCore.SignalR;
using System.Threading.Tasks;

namespace ChattApp.Hub
{
    public class ChattHub : Microsoft.AspNetCore.SignalR.Hub
    {
        private readonly ChattAppDb _context;

        public ChattHub(ChattAppDb context)
        {
            _context = context;
        }

        public const string ReceiveMessage = "ReceiveMessage";

        public async Task SendMessage(ChattMessage msg)
        {
            _context.Messages.Add(msg);
            await _context.SaveChangesAsync();

            await Clients.All.SendAsync(method: ReceiveMessage, msg);
        }
    }
}