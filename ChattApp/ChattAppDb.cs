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
        }

        private void ConfigureMessageEntity(EntityTypeBuilder<ChattMessage> entity)
        {
            entity.ToTable("Message");
            entity.HasKey(pk => pk.MessageId);
            entity.Property(pk => pk.MessageId).ValueGeneratedOnAdd();
            entity.Property(p => p.UserName)
                .HasMaxLength(30)
                .IsRequired();
            entity.Property(p => p.Message)
                .IsRequired();  // Assuming Message is required, adjust as needed.
            entity.Property(p => p.CreatedOn)
                .IsRequired()
                .HasDefaultValueSql("getutcdate()");  // Default to current UTC time if not set explicitly.
        }
    }
}