using System;

namespace Proof.Database.BulkInsertion.Persistence.Models
{
    public class Transaction
    {
        public Transaction()
        {
            CreatedDate = DateTime.UtcNow;
        }

        public int Id { get; set; }
        public DateTime CreatedDate { get; private set; }
        public Guid LoanId { get; set; }
    }
}