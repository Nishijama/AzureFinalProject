using System.ComponentModel.DataAnnotations;

namespace ChattApp.Models;

public class SignInVm
{
    [Required]
    public string UserName { get; set; }
}