using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Proof.Database.BulkInsertion.Data;
using Proof.Database.BulkInsertion.Persistence.Models;

namespace Proof.Database.BulkInsertion.Services
{
    public class AuditLogService : IAuditLogService
    {
        private readonly ProofContext _proofContext;
        private readonly int maxInsertionCount = 10000;

        public AuditLogService(ProofContext proofContext)
        {
            _proofContext = proofContext;
        }

        public async Task<TimeSpan> Insert()
        {
            var prev = DateTime.Now;
            var audits = new List<AuditLog>();
            var loanId = Guid.NewGuid();
            for (var i = 0; i < maxInsertionCount; i++)
                audits.Add(new AuditLog
                {
                    Id = Guid.NewGuid(),
                    LoanId = loanId,
                    Type = i,
                    Name = $"a-{i}",
                    Order = i,
                    Transaction = new Transaction {LoanId = loanId},
                    TypeDescription = $"des-{i}",
                    FieldId = $"f-{i}",
                    ObjectName = $"obj-{i}"
                });

            await _proofContext.Set<AuditLog>().AddRangeAsync(audits);

            await _proofContext.SaveChangesAsync();

            var after = DateTime.Now;

            var diff = after - prev;

            return diff;
        }

        public async Task<TimeSpan> BulkInsert()
        {
            var prev = DateTime.Now;

            var conn = _proofContext.Database.GetDbConnection();
            var loanId = Guid.NewGuid();
            try
            {
                await using (conn)
                {
                    await using (var cmd = conn.CreateCommand())
                    {
                        var sqlBuilder = new StringBuilder();
                        sqlBuilder.AppendLine("DROP INDEX if exists \"IX_AuditLogs_LoanId\";");
                        sqlBuilder.AppendLine("DROP INDEX if exists \"IX_AuditLogs_Name\";");
                        sqlBuilder.AppendLine("DROP INDEX if exists \"IX_AuditLogs_TransactionId1\";");
                        sqlBuilder.AppendLine("DROP INDEX if exists \"IX_AuditLogs_Type\";");
                        sqlBuilder.AppendLine("DROP INDEX if exists \"IX_AuditLogs_TypeDescription\";");
                        sqlBuilder.AppendLine();

                        sqlBuilder.AppendLine(
                            "INSERT INTO public.\"AuditLogs\" (\"Id\", \"Type\", \"Name\", \"LoanId\", \"TypeDescription\", \"FieldId\", \"ObjectName\", \"Order\", \"CreatedDate\") VALUES ");
                        
                        for (var i = 0; i < maxInsertionCount; i++)
                            sqlBuilder.Append(
                                $"('{Guid.NewGuid()}', '{i}', 'a-{i}', '{loanId}', 'des-{i}', 'f-{i}', 'obj-{i}', '{i}', '{DateTime.UtcNow}'),");

                        sqlBuilder.Replace(',', ';', sqlBuilder.Length - 1, 1);
                        sqlBuilder.AppendLine();
                        sqlBuilder.AppendLine("alter table public.\"AuditLogs\" owner to postgres;");
                        sqlBuilder.AppendLine("create index \"IX_AuditLogs_LoanId\" on \"AuditLogs\" (\"LoanId\");");
                        sqlBuilder.AppendLine("create index \"IX_AuditLogs_Name\" on \"AuditLogs\" (\"Name\");");
                        sqlBuilder.AppendLine("create index \"IX_AuditLogs_TransactionId1\" on \"AuditLogs\" (\"TransactionId1\");");
                        sqlBuilder.AppendLine("create index \"IX_AuditLogs_Type\" on \"AuditLogs\" (\"Type\");");
                        sqlBuilder.AppendLine("create index \"IX_AuditLogs_TypeDescription\" on \"AuditLogs\" (\"TypeDescription\")");

                        var command = sqlBuilder.ToString();

                        cmd.CommandText = command;
                        await conn.OpenAsync();
                        await cmd.ExecuteReaderAsync();
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            finally
            {
                await conn.CloseAsync();
            }

            var after = DateTime.Now;
            var diff = after - prev;
            return diff;
        }
    }
}