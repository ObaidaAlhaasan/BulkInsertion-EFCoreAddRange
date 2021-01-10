using Microsoft.EntityFrameworkCore;
using Proof.Database.BulkInsertion.Persistence;
using Proof.Database.BulkInsertion.Persistence.Models;

namespace Proof.Database.BulkInsertion.Data
{
    public class ProofContext : DbContext
    {
        public ProofContext(DbContextOptions<ProofContext> options) : base(options)
        {
        }

        public DbSet<AuditLog> AuditLogs { get; set; }
        public DbSet<Transaction> Transactions { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<AuditLog>().HasIndex(s => s.Name);
            modelBuilder.Entity<AuditLog>().HasIndex(s => s.Type);
            modelBuilder.Entity<AuditLog>().HasIndex(s => s.TypeDescription);
            modelBuilder.Entity<AuditLog>().HasIndex(s => s.LoanId);

            modelBuilder.Entity<Transaction>().HasKey(t => t.Id);
            modelBuilder.Entity<Transaction>().HasIndex(s => s.LoanId);
            base.OnModelCreating(modelBuilder);
        }
    }
}