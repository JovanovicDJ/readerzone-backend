using Microsoft.EntityFrameworkCore;
using readerzone_api.Models;

namespace readerzone_api.Data
{
    public class ReaderZoneContext : DbContext
    {
        public ReaderZoneContext(DbContextOptions<ReaderZoneContext> options) : base(options)
        {
            
        }

        public DbSet<User> Users { get; set; }
        public DbSet<UserAccount> UserAccounts { get; set; }
        public DbSet<Author> Authors { get; set; }
        public DbSet<Book> Books { get; set; }
        public DbSet<PurchasedBook> PurchasedBooks { get; set; }
        public DbSet<Customer> Customers { get; set; }
        public DbSet<Employee> Employees { get; set; }
        public DbSet<Publisher> Publishers { get; set; }
        public DbSet<AutomaticPost> AutomaticPosts { get; set; }
        public DbSet<Review> Reviews { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<Post> Posts { get; set; }
        public DbSet<Genre> Genres { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<Notification> Notifications { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Customer>()
            .HasMany(c => c.Friends)
            .WithMany()
            .UsingEntity<Dictionary<string, object>>(
                "CustomerFriend",
                j => j
                    .HasOne<Customer>()
                    .WithMany()
                    .HasForeignKey("CustomerId"),
                j => j
                    .HasOne<Customer>()
                    .WithMany()
                    .HasForeignKey("FriendId")
            );

            modelBuilder.Entity<PurchasedBook>()
                .HasOne(p => p.Customer)
                .WithMany(c => c.PurchasedBooks)
                .HasForeignKey(p => p.CustomerId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Post>()
                .HasOne(p => p.Customer)
                .WithMany(c => c.Posts)
                .HasForeignKey(p => p.CustomerId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Comment>()
                .HasOne(c => c.Customer)
                .WithMany(customer => customer.Comments)
                .HasForeignKey(c => c.CustomerId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Notification>()
                .HasOne(n => n.ToCustomer)
                .WithMany(customer => customer.Notifications)
                .HasForeignKey(n => n.CustomerId)
                .OnDelete(DeleteBehavior.Restrict);            

            base.OnModelCreating(modelBuilder);
        }
    }
}
