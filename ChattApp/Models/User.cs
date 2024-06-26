using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ChattApp.Models
{
    public class User
    {
        public User()
        {
            Messages = new HashSet<ChattMessage>();
        }

        [Key]
        public int UserId { get; set; }

        [Required]
        [MaxLength(30)]
        public string UserName { get; set; }

        [Required]
        public string PasswordHash { get; set; }

        public ICollection<ChattMessage> Messages { get; set; }  // Navigation property
    }
}