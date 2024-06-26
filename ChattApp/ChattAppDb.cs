using ChattApp.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ChattApp
{
    public class ChattAppDb : DbContext
    {
        public ChattAppDb(DbContextOptions<ChattAppDb> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            ConfigureMessageEntity(modelBuilder.Entity<ChattMessage>());
            ConfigureUserEntity(modelBuilder.Entity<User>());
        }

        private void ConfigureMessageEntity(EntityTypeBuilder<ChattMessage> entity)
        {
            entity.ToTable("Message");
            entity.HasKey(pk => pk.MessageId);
            entity.Property(pk => pk.MessageId).ValueGeneratedOnAdd();
            entity.Property(p => p.Message)
                .IsRequired();  // Assuming Message is required, adjust as needed.
            entity.Property(p => p.CreatedOn)
                .IsRequired()
                .HasDefaultValueSql("getutcdate()");  // Default to current UTC time if not set explicitly.

            entity.HasOne(m => m.User)
                .WithMany(u => u.Messages)
                .HasForeignKey(m => m.UserId)
                .OnDelete(DeleteBehavior.Cascade);  // Adjust delete behavior as needed.
        }

        private void ConfigureUserEntity(EntityTypeBuilder<User> entity)
        {
            entity.ToTable("User");
            entity.HasKey(pk => pk.UserId);
            entity.Property(pk => pk.UserId).ValueGeneratedOnAdd();
            entity.Property(p => p.UserName)
                .HasMaxLength(30)
                .IsRequired();
            entity.Property(p => p.PasswordHash)
                .IsRequired();
        }

        public DbSet<ChattMessage> Messages { get; set; }
        public DbSet<User> Users { get; set; }
    }
}