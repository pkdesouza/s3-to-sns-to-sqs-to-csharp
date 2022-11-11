using Microsoft.EntityFrameworkCore;

namespace Bemobi.Infra.Infra.Context
{
    public class BemobiContext : DbContext
    {
        public BemobiContext(DbContextOptions<BemobiContext> options)
            : base(options)
        {
        }

        public DbSet<Domain.Entities.File> Files { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                //optionsBuilder.UseMySql(_configuration.GetSection("ConnectionString").Value););
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Domain.Entities.File>(entity =>
            {
                entity.Property(e => e.FileName).IsRequired().HasColumnName("filename").HasMaxLength(255).IsUnicode(false);
                entity.Property(e => e.FileSize).IsRequired().HasColumnName("filesize");
                entity.Property(e => e.LastModified).HasColumnName("lastmodified").HasColumnType("datetime").HasDefaultValueSql("(getdate())");
            });
        }
    }
}
