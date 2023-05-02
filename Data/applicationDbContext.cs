using InvMang.Model;
using Microsoft.EntityFrameworkCore;
using System.Data.SqlClient;

namespace InvMang.Data
{
    public class applicationDbContext : DbContext
    {
        public applicationDbContext(DbContextOptions<applicationDbContext> options) : base(options)
        {
        }

        public DbSet<Invoice> Invoice { get; set; }
        public DbSet<InvoiceDetail> InvoiceDetail { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Invoice>(entity =>
            {
                entity.Property(e => e.InvoiceNumber).IsRequired();
                entity.Property(e => e.InvoiceDate).IsRequired();
                entity.Property(e => e.DueDate).IsRequired();
                entity.Property(e => e.Status).IsRequired();
                entity.Property(e => e.BillTo).IsRequired();
                entity.Property(e => e.SubTotal).IsRequired();
                entity.Property(e => e.Tax).IsRequired();
                entity.Property(e => e.GrandTotal).IsRequired();
            });

            modelBuilder.Entity<InvoiceDetail>(entity =>
            {
                entity.Property(e => e.Product).IsRequired();
                entity.Property(e => e.Description).HasMaxLength(500);
                entity.Property(e => e.Price).IsRequired();
                entity.Property(e => e.Qty).IsRequired();
                entity.Property(e => e.Tax).IsRequired();
                entity.Property(e => e.Total).IsRequired();
            });
        }
    }
}
