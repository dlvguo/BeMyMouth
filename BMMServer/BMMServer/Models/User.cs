using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace BMMServer.Models
{
    [Table("user")]
    public class User
    {
        [Key]
        public int Id { get; set; }
        public string UserName { get; set; }

        public string Password { get; set; }

        public string NickName { get; set; }
        public bool IsLoginFirst { get; set; }//是否第一次登录

        //public User(int id, string username, string password, string nickName = "null", bool ilf = true)
        //{
        //    this.Id = id;
        //    this.UserName = username;
        //    this.Password = password;
        //    this.NickName = nickName;
        //    this.IsLoginFirst = ilf;
        //}
    }
}
