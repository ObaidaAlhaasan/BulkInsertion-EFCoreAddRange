using System;

namespace Proof.Database.BulkInsertion.Persistence.Models
{
    public class AuditLog
    {
        public AuditLog()
        {
            CreatedDate = DateTime.UtcNow;
        }

        public Guid Id { get; set; }
        public int Type { get; set; }
        public string Name { get; set; }

        public Guid LoanId { get; set; }

        public int TransactionId { get; }
        public Transaction Transaction { get; set; }

        public string TypeDescription { get; set; }
        public DateTime CreatedDate { get; set; }
        public string? FieldId { get; set; }
        public string? ObjectName { get; set; }
        public int? Order { get; set; }
    }
}