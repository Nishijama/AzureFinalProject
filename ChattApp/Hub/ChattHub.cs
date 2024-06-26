using ChattApp.Models;
using Microsoft.AspNetCore.SignalR;
using System.Security.Claims;
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
            var userId = Context.User.FindFirstValue(ClaimTypes.NameIdentifier);
            var userName = Context.User.FindFirstValue(ClaimTypes.Name);

            msg.UserId = int.Parse(userId);
            msg.User = null;

            _context.Messages.Add(msg);
            await _context.SaveChangesAsync();

            var messageToSend = new
            {
                UserId = msg.UserId,
                UserName = userName,
                Message = msg.Message,
                FormattedCreatedOn = msg.FormattedCreatedOn,
                MessageId = msg.MessageId
            };

            await Clients.All.SendAsync(ReceiveMessage, messageToSend);
        }
    }
}