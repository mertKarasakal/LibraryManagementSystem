using System.Data.Entity;
using LibraryManagementSystem.WebUI.Entity.Concrete;

namespace LibraryManagementSystem.WebUI.Models.EntityFramework
{
    public /*partial*/ class LibraryContext : DbContext
    {
        public LibraryContext() : base("name=LibraryContext") { }

        public virtual DbSet<Category> Categories { get; set; }
        public virtual DbSet<Book> Books { get; set; }
        public virtual DbSet<User> Users { get; set; }

        //protected override void OnModelCreating(DbModelBuilder modelBuilder)
        //{
        //    modelBuilder.Entity<Category>()
        //        .Property(e => e.Name)
        //        .IsUnicode(false);

        //    modelBuilder.Entity<Book>()
        //        .Property(e => e.Name)
        //        .IsUnicode(false);

        //    modelBuilder.Entity<Book>()
        //        .Property(e => e.Isbn)
        //        .IsFixedLength()
        //        .IsUnicode(false);

        //    modelBuilder.Entity<Book>()
        //        .Property(e => e.Author)
        //        .IsUnicode(false);

        //    modelBuilder.Entity<Book>()
        //        .Property(e => e.Description)
        //        .IsUnicode(false);

        //    modelBuilder.Entity<User>()
        //        .Property(e => e.Name)
        //        .IsUnicode(false);

        //    modelBuilder.Entity<User>()
        //        .Property(e => e.Surname)
        //        .IsUnicode(false);

        //    modelBuilder.Entity<User>()
        //        .Property(e => e.Username)
        //        .IsUnicode(false);

        //    modelBuilder.Entity<User>()
        //        .Property(e => e.Password)
        //        .IsUnicode(false);
        //}
    }
}