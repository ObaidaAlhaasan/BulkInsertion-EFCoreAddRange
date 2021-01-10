using System;
using System.Threading.Tasks;

namespace Proof.Database.BulkInsertion.Services
{
    public interface IAuditLogService
    {
        Task<TimeSpan> Insert();
        Task<TimeSpan> BulkInsert();
    }
}