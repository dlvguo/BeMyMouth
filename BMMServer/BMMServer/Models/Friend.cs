using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace BMMServer.Models
{
    [Table("friends")]
    public class Friend
    {
        [Key]
        public int Id { get; set; }

        public int LeftId { get; set; }
        public int RightId { get; set; }

    }
}
