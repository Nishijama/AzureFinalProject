using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ChattApp.Models
{
    public class ChattMessage
    {
        public ChattMessage()
        {
            CreatedOn = DateTime.UtcNow;
        }
        
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int MessageId { get; set; }
        public int UserId { get; set; }
        public string Message { get; set; }
        public DateTime CreatedOn { get; private set; }
        [NotMapped]
        public string FormattedCreatedOn => CreatedOn.ToString("yyyy-MM-dd, HH:mm:ss");

        [ForeignKey("UserId")]
        public virtual User User { get; set; }
    }
}