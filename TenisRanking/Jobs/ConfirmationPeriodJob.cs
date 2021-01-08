using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using Quartz;
using Quartz.Logging;
using TenisRanking.Data;
using TenisRanking.MatchProvider;
using TenisRanking.Models;

namespace TenisRanking.Job
{
    public class ConfirmationPeriodJob : IJob
    {
        private readonly IOptions<ConfirmationPeriodOptions> jobOptions;
        private readonly IDbContextWrapper context;
        private readonly IMatchProvider matchProvider;

        public ConfirmationPeriodJob(IDbContextWrapper context, IMatchProvider matchProvider, IOptions<ConfirmationPeriodOptions> jobOptions)
        {
            this.jobOptions = jobOptions;
            this.context = context;
            this.matchProvider = matchProvider;
        }

        public Task Execute(IJobExecutionContext context)
        {
            if (!this.jobOptions.Value.Enabled)
            {
                return Task.CompletedTask;
            }

            var matches = this.context.GetMatchesWithUnConfirmedResults();

            foreach (var match in matches)
            {
                if (match.DateOfGame < DateTime.Now.AddHours(-this.jobOptions.Value.Hours))
                {
                    this.matchProvider.ConfirmResult(match.Id);
                }
            }

            return Task.CompletedTask;
        }
    }
}
