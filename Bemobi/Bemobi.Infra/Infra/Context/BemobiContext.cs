using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace Bemobi.Infra.Infra.Context
{
    public class BemobiContext : DbContext
    {
        public BemobiContext(DbContextOptions<BemobiContext> options)
            : base(options)
        {
        }
        public DbSet<Domain.Entities.Files> Files { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Domain.Entities.Files>(entity =>
            {
                entity.HasKey(e => e.FileName);
                entity.Property(e => e.FileName).HasColumnName("filename").HasMaxLength(255).IsUnicode(false);
                entity.Property(e => e.FileSize).IsRequired().HasColumnName("filesize");
                entity.Property(e => e.LastModified).HasColumnName("lastmodified").HasColumnType("datetime").HasDefaultValueSql("(now())");
            });
        }
    }
}
