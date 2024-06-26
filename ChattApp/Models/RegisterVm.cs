using System.ComponentModel.DataAnnotations;

namespace ChattApp.Models
{
    public class RegisterVm
    {
        [Required]
        public string UserName { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
}