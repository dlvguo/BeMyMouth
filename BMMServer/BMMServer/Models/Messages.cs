using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace BMMServer.Models
{
    [Table("messages")]
    public class Messages
    {
        [Key]
        public int IdMessages { get; set; }
        public string Message { get; set; }

        public int SendId { get; set; }

        public int ReciveId { get; set; }
        public bool HasRead { get; set; } = false;

    }
}
