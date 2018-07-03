using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace 图书管理系统.Models
{
    public class Admin
    {
        public Admin(string name, string Password)
        {
            this.NameId = name;
            this.Password = Password;
        }
        public Admin() { }
        [Key]
        public string NameId { get; set; }
        public string Password { get; set; }
    }
}