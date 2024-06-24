namespace ChattApp.Models;

public class ChattMessage
{
    public ChattMessage()
    {
        CreatedOn = DateTime.Now;
    }
    
    public string UserName { get; set; }
    public string Message { get; set; }
    public DateTime CreatedOn { get; }
    public string FormattedCreatedOn => CreatedOn.ToString(format: "yyyy-MM-dd, HH:mm:ss");
}