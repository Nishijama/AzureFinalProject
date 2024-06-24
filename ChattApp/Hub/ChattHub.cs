using ChattApp.Models;
using Microsoft.AspNetCore.SignalR;

namespace ChattApp.Hub;

public class ChattHub : Microsoft.AspNetCore.SignalR.Hub
{
    public const string ReceiveMessage = "ReceiveMessage";
    public async Task SendMessage(ChattMessage msg)
    {
        // TODO: save msg to database
        await Clients.All.SendAsync(method: ReceiveMessage, msg);
    }

}