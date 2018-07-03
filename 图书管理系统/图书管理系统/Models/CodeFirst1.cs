namespace 图书管理系统.Models
{
    using System;
    using System.Data.Entity;
    using System.Linq;
    using System.Data.Entity.ModelConfiguration.Conventions;
    using System.Data.Entity.Migrations;
    using System.Data.Entity.Infrastructure;
    using System.Configuration;

    public class CodeFirst1 : DbContext
    {
        //您的上下文已配置为从您的应用程序的配置文件(App.config 或 Web.config)
        //使用“CodeFirst1”连接字符串。默认情况下，此连接字符串针对您的 LocalDb 实例上的
        //“图书管理系统.Models.CodeFirst1”数据库。
        // 
        //如果您想要针对其他数据库和/或数据库提供程序，请在应用程序配置文件中修改“CodeFirst1”
        //连接字符串。
        public CodeFirst1()
            : base("name=CodeFirst1")
        {
   //         Database.SetInitializer<CodeFirst1>(new CreateDatabaseIfNotExists<CodeFirst1>());
            Database.SetInitializer<CodeFirst1>(new DropCreateDatabaseAlways<CodeFirst1>());
      //      Database.SetInitializer(new MigrateDatabaseToLatestVersion<CodeFirst1, Configuration>());
            //       Database.SetInitializer<CodeFirst1>(new MigrateDatabaseToLatestVersion<CodeFirst1, ReportingDbMigrationsConfiguration>());
        }
        
        public DbSet<Book> Books { get; set; }
        public DbSet<Admin> Admins { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Borrow> Borrows { get; set; }
        //为您要在模型中包含的每种实体类型都添加 DbSet。有关配置和使用 Code First  模型
        //的详细信息，请参阅 http://go.microsoft.com/fwlink/?LinkId=390109。

        // public virtual DbSet<MyEntity> MyEntities { get; set; }
    }

    //public class MyEntity
    //{
    //    public int Id { get; set; }
    //    public string Name { get; set; }
    //}
}