using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.Entity;
using System.Net;
using System.Web.Mvc;
using 图书管理系统.Models;
using System.Security.Cryptography;
using System.Text;

namespace 图书管理系统.Controllers
{
    public class HomeController : Controller
    {
        public int inital_flag = 0;
        public CodeFirst1 db = new CodeFirst1();
        public string Sha256(string plainText)
        {
            SHA256Managed _sha256 = new SHA256Managed();
            byte[] _cipherText = _sha256.ComputeHash(Encoding.Default.GetBytes(plainText));
            return Convert.ToBase64String(_cipherText);
        }
      
        public void inital_db()
        {
        
            var book1 = new Book("11111","C#图解教程", "索利斯",5,1);
            var book2 = new Book("2222", "SQL进阶教程", "MICK",10,9);
            var admin = new Admin("admin", Sha256( "admin"));
            var user = new User("user", Sha256("user"));
            db.Books.Add(book1);
            db.Books.Add(book2);
            db.Admins.Add(admin);
            db.Users.Add(user);
            db.SaveChanges();
        }
          public string ToMD5(string a)
        {
            MD5CryptoServiceProvider MD5 = new MD5CryptoServiceProvider();
            string temp = BitConverter.ToString(MD5.ComputeHash(Encoding.GetEncoding("utf-8").GetBytes(a))).Replace("-", "").ToLower();
            return temp;
        }
        public int AdminLogin(string name,string password)
        {
            int flag = 0;
            string Sha256_password = Sha256(password);
            var query = from admin in db.Admins
                        where admin.NameId==name && admin.Password== Sha256_password
                        select admin;
            foreach(var  i in query)
            {
                flag = 1;
                Session["Admin"] = name;
            }
            return flag;
        }
        public int UserLogin(string  id, string password)
        {
            int flag = 0;
            string Sha256_password = Sha256(password);
            var query = from User in db.Users
                        where User.NameId == id && User.password == Sha256_password
                        select User;
            foreach (var i in query)
            {
                flag = 1;
                Session["User"] = id;
            }
            return flag;
        }
        public int JudgeUser()
        {
           
            if (Session["User"] != null)
            {
                return 1;
            }
            return 0;
        }
        public int JudgeAdmin()
        {
            if (Session["Admin"] != null)
            {
                return 1;
            }
            return 0;
        }
        
        public List<Book> Book_Search(string Title,string Author)
        {
            int flag = 0;
            List<Book> books = new List<Book>();
            var query = from Book in db.Books
                        where Book.Title.Contains(Title)  && Book.Author.Contains(Author)
                        select Book;
            foreach (var i in query)
            {
                flag = 1;
                books.Add(i);
            }
            return books;
        }
        public int Book_Return(string BookId)
        {
            string user = Session["User"].ToString();
            if (Session["User"] == null)
            {
                return -4;//未登录，强行post
            }
            int flag = 0;
            var query = from Borrow in db.Borrows
                        where Borrow.BookId == BookId && Borrow.NameId== user
                        select Borrow;
            foreach (var i in query)
            {
                flag = 1;
            }
            if (flag == 0)
            {
                return 0;//没借这本书
            }
            //    var borrow = new Borrow(Session["User"].ToString(), BookId);
            var borrow = db.Borrows.Find( BookId, user);
            db.Borrows.Remove(borrow);
            var book = db.Books.Find(BookId);
            book.Stock += 1;
            db.SaveChanges();
            return 1;
        }
        public int Book_Borrow(string BookId)
        {
            int flag = 0;
            int stock = -1;
            var query = from Book in db.Books
                        where Book.BookId == BookId
                        select Book;
            foreach (var i in query)
            {
                flag = 1;
                stock = i.Stock;
            }
            if (flag == 0)
                return 0;//不存在这本书
            if (stock <= 0)
                return -2;//没库存
            if (Session["User"] == null)
            {
                return -4;//未登录，强行post
            }
            string user = Session["User"].ToString();
            var query2 = from Borrow in db.Borrows
                        where Borrow.BookId == BookId && Borrow.NameId== user
                         select Borrow;
            foreach (var i in query2)
            {
                return -3;//已借过这本书
            }
            var borrow = new Borrow(Session["User"].ToString(), BookId);
            db.Borrows.Add(borrow);
            var book = db.Books.Find(BookId);
            book.Stock -= 1;
            db.SaveChanges();
        return 1;
        }
        public int AddBook(string BookId, string Title, string Author, int Total)
        {
            if (Total <= 0)
            {
                return -3;//书的数量不能小于0
            }
            var book = db.Books.Find(BookId);
            if (book != null)
            {
                return -4;//不能添加已在数据库中的书
            }
            var book1 = new Book(BookId, Title, Author, Total, Total);
            db.Books.Add(book1);
            db.SaveChanges();
            return 1;
        }












        public ActionResult Index()
        {
            if (inital_flag == 0)
            {
                inital_flag = 1;
                inital_db();
            }
            return View();
        }
        public ActionResult Admin()
        {
            var name = Request["name"];
            var password = Request["password"];
            if (name != null && password != null)
            {
                ViewBag.Message = AdminLogin(name,password);
            }
            else
            {
                ViewBag.Message = -1;
            }

            return View();
        }
        public ActionResult Borrow()
        {
            ViewBag.Message = JudgeUser();
            var BookId = Request["BookId"];
            if (BookId != null)
            {
                ViewBag.success = Book_Borrow(BookId);
            }
            else
            {
                ViewBag.success = -1;
            }
            return View();
        }
        public ActionResult Manage()
        {
            ViewBag.Message = JudgeAdmin();
            var BookId = Request["BookId"];
            var Title = Request["Title"];
            var Author = Request["Author"];
            var Total = Request["Total"];
            if (BookId != null && Title != null && Author != null && Total != null)
            {

                ViewBag.success = AddBook(BookId, Title, Author, int.Parse(Total));
            }
            else if (BookId == null && Title == null && Author == null && Total == null)
            {
                ViewBag.success = -1;
            }
            else
            {
                ViewBag.success = -2;
            }
            return View();
        }
        public ActionResult Return()
        {
            ViewBag.Message = JudgeUser();
            var BookId = Request["BookId"];
            if (BookId != null)
            {
                ViewBag.success = Book_Return(BookId);
            }
            else
            {
                ViewBag.success = -1;
            }
            return View();
        }
        public ActionResult Search()
        {
            var Title = Request["Title"];
            var Author = Request["Author"];
            if (Title != null || Author != null)
            {
                ViewBag.result = Book_Search(Title, Author);
                ViewBag.Message = 0;
            }
            else
            {
                List<Book> books = new List<Book>();
                ViewBag.result = books;
                ViewBag.Message = -1;
            }
            return View();
        }
        public ActionResult User()
        {
            var name = Request["name"];
            var password = Request["password"];
            if (name != null && password != null)
            {
                ViewBag.Message = UserLogin(name, password);
            }
            else
            {
                ViewBag.Message = -1;
            }
            return View();
        }
    }
}