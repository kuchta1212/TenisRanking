using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using Quartz;
using Quartz.Logging;
using TenisRanking.Data;
using TenisRanking.MatchProvider;

namespace TenisRanking.Job
{
    public class MatchCheckerJob : IJob
    {
        private readonly IMatchProvider matchProvider;
        private readonly IOptions<MatchDaysLimitOptions> jobOptions;

        public MatchCheckerJob(IMatchProvider matchProvider, IOptions<MatchDaysLimitOptions> jobOptions)
        {
            this.matchProvider = matchProvider;
            this.jobOptions = jobOptions;
        }

        public Task Execute(IJobExecutionContext context)
        {
            if (!this.jobOptions.Value.Enabled)
            {
                return Task.CompletedTask;
            }

            this.matchProvider.CheckDeadlines();

            return Task.CompletedTask;
        }
    }
}
