// source: https://ef.readthedocs.io/en/staging/platforms/aspnetcore/new-db.html
// then used this one as previous was a bit outdated http://www.learnentityframeworkcore.com/walkthroughs/aspnetcore-application

/*
dotnet ef migrations list
Unable to create an object of type 'BloggingContext'. Add an implementation of 'IDesignTimeDbContextFactory<BloggingContext>' to the project, 
or see https://go.microsoft.com/fwlink/?linkid=851728 for additional patterns supported at design time.
---------------
add some test data 
https://docs.microsoft.com/en-us/aspnet/core/data/ef-mvc/intro#add-code-to-initialize-the-database-with-test-data

 */

using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Design;
using System.Data.SqlClient;
using System.Linq;

namespace EFGetStarted.AspNetCore.NewDb.Models
{

     public class BloggingContextFactory : IDesignTimeDbContextFactory<BloggingContext>
    {
        public BloggingContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<BloggingContext>();
            
            SqlConnectionStringBuilder builder = new SqlConnectionStringBuilder();            
            builder.DataSource = "sqlexpress"; 
            builder.UserID = "sa";            
            builder.Password = "tota11y5ecret";     
            builder.InitialCatalog = "excercise";                        
            optionsBuilder.UseSqlServer(builder.ConnectionString);

            return new BloggingContext(optionsBuilder.Options);
        }
    }
    



    public class BloggingContext : DbContext
    {
        public BloggingContext(DbContextOptions<BloggingContext> options)
            : base(options)
        { }

        public BloggingContext(DbContextOptions options) : base(options)
{
}


        public DbSet<Blog> Blogs { get; set; }
        public DbSet<Post> Posts { get; set; }


          protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Blog>().ToTable("Blogs");
            modelBuilder.Entity<Post>().ToTable("Posts");         
        }
    }

    public class Blog
    {
        public int BlogId { get; set; }
        public string Url { get; set; }

        public List<Post> Posts { get; set; }
    }

    public class Post
    {
        public int PostId { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }

        public int BlogId { get; set; }
        public Blog Blog { get; set; }
    }

    public static class DbInitializer
    {
        public static void Initialize(BloggingContext context)
        {
            context.Database.EnsureCreated();

            if (context.Blogs.Any())
            {
                return;   // DB has been seeded
            }
            var blogs = new Blog[]{
            new Blog{BlogId=1},
            new Blog{BlogId=2},
            new Blog{BlogId=3},
            };


            if (context.Posts.Any())
            {
                return;   // DB has been seeded
            }

            var posts = new Post[]{
                new Post{PostId= 1, Title="Greg meets Docker", Content = "123", BlogId=1},
                new Post{PostId= 2, Title="Joe meets Docker", Content = "123", BlogId=1},
                new Post{PostId= 3, Title="Dave meets apple", Content = "123", BlogId=1},
                new Post{PostId= 4, Title="Who wants banana", Content = "124", BlogId=1},
            };

            context.SaveChanges();
            




            context.SaveChanges();
        }
    }
}



