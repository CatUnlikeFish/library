using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace 图书管理系统.Models
{
    public class User
    {
        public User(string NameId, string password)
        {
            this.NameId = NameId;
            this.password = password;
        }
        public User() { }
        [Key]
        public string NameId { get; set; }
        public string password { get; set; }
    }
}