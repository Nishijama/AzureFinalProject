using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ChattApp.Models
{
    public class ChattMessage
    {
        // Default constructor required by EF
        protected ChattMessage()
        {
            CreatedOn = DateTime.UtcNow;
        }

        // Public constructor to initialize with UserName
        public ChattMessage(string userName) : this()
        {
            UserName = userName;
        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int MessageId { get; set; }

        public string UserName { get; set; }
        
        public string Message { get; set; }

        // Public getter and private setter for EF mapping
        public DateTime CreatedOn { get; private set; }

        // Read-only property, not mapped by EF
        [NotMapped]
        public string FormattedCreatedOn => CreatedOn.ToString("yyyy-MM-dd, HH:mm:ss");
    }
}