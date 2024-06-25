using ChattApp.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;


namespace ChattApp.Controllers;

[ApiController]
[Route("[controller]")]
public class MessageController : ControllerBase
{
    private readonly ChattAppDb _db;
    private readonly ILogger<MessageController> _logger;

    public MessageController(ChattAppDb db,
        ILogger<MessageController> logger)
    {
        _db = db;
        _logger = logger;
    }

    [HttpGet(Name = "GetMessages")]
    public async Task<IEnumerable<ChattMessage>> Get()
    {
        var messages = await _db.Messages
            .ToListAsync();

        foreach (var m in messages)
        {
            Console.WriteLine(m.MessageId);
        }
        
        return messages;
    }

    [HttpPost(Name = "AddMessage")]
    public async Task<ChattMessage> AddPerson([FromBody] AddPersonCommand command)
    {
        var chattMessage = new ChattMessage()
        {
            UserName = command.UserName,
            Message = command.Message
        };

        _db.Messages.Add(chattMessage);
        await _db.SaveChangesAsync();

        return chattMessage;
    }
}

public class AddPersonCommand
{
    public string UserName { get; set; }
    public string Message { get; set; }
}