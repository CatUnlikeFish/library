using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace 图书管理系统.Models
{
    public class Book
    {
  

        [Key]
        public string BookId { get; set; }
        public string Title { get; set; }
        public string Author { get; set; }
        public int Total { get; set; }
        public int Stock { get; set; }

        public Book(string BookId, string Title, string Author, int Total, int Stock)
        {
            this.BookId = BookId;
            this.Title = Title;
            this.Author = Author;
            this.Total = Total;
            this.Stock = Stock;
        }

        //定义无参数的构造函数主要是因为在通过DbSet获取对象进行linq查询时会报错
        //The class 'EFCodeFirstModels.Student' has no parameterless constructor.
        public Book() { }
    }
}