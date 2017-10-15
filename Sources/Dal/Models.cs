using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Dal
{
    public abstract class BaseModel
    {
        protected BaseModel()
        {
            Id = Guid.NewGuid();
        }

        [Key]
        public Guid Id { get; set; }

        [Timestamp]
        public byte[] RowVersion { get; set; }
    }

    [Table("Messages")]
    public class MessageModel : BaseModel
    {
        public MessageModel()
        {
            UserName=string.Empty;
            Content = string.Empty;
            TimeMessage = DateTime.UtcNow;
        }

        public void Init(string userName, string content)
        {
            UserName = userName;
            Content = !string.IsNullOrEmpty(content) ? content : string.Empty;
            TimeMessage = DateTime.UtcNow;
        }

        public string UserName { get; set; }
        public string Content { get; set; }
        [Column(TypeName = "datetime2")]
        public DateTime TimeMessage { get; set; }
    }
}
