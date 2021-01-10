using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Proof.Database.BulkInsertion.Services;

namespace Proof.Database.BulkInsertion.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        private readonly IAuditLogService _auditLogService;

        private static readonly string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        private readonly ILogger<WeatherForecastController> _logger;

        public WeatherForecastController(ILogger<WeatherForecastController> logger, IAuditLogService auditLogService)
        {
            _logger = logger;
            _auditLogService = auditLogService;
        }

        [HttpPost("insert")]
        public async Task<TimeSpan> Insert()
        {
            return await _auditLogService.Insert();
        }
        
        [HttpPost("insert/bulk")]
        public async Task<TimeSpan> BulkInsert()
        {
            return await _auditLogService.BulkInsert();
        }

    }
}