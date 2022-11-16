using Bemobi.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Bemobi.Infra.Infra.Context
{
    public class BemobiContext : DbContext
    {
        public BemobiContext(DbContextOptions<BemobiContext> options) : base(options) {}

        public DbSet<Files> Files { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Files>(entity =>
            {
                entity.HasKey(e => e.FileName);
                entity.Property(e => e.FileName).HasColumnName("filename").IsUnicode(false);
                entity.Property(e => e.FileSize).IsRequired().HasColumnName("filesize");
                entity.Property(e => e.LastModified).HasColumnName("lastmodified").HasColumnType("datetime").HasDefaultValueSql("(now())");
            });
        }
    }
}
