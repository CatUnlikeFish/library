using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace 图书管理系统.Models
{
    public class Borrow
    {
        public Borrow(string NameId, string bookId)
        {
            BookId = bookId;
            this.NameId = NameId;
        }
        public Borrow() { }
        [Key]
        [Column(Order = 1)]
        public string BookId { get; set; }
        [Key]
        [Column(Order = 2)]
        public string NameId { get ; set ; }

    }
}