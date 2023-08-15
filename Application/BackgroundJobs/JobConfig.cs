using Application.Interfaces;
using Microsoft.Extensions.Hosting;
using Quartz;
using Quartz.Spi;

namespace Application.BackgroundJobs
{
    public class JobConfig: IHostedService
    {
        private readonly ISchedulerFactory _schedulerFactory;
        private readonly ISymbolDataAccess _symbolDataAccess;
        private readonly IJobFactory _jobFactory;
        private IScheduler Scheduler { get; set; }

        public JobConfig(ISchedulerFactory schedulerFactory, IJobFactory jobFactory, ISymbolDataAccess symbolDataAccess)
        {
            _schedulerFactory = schedulerFactory;
            _jobFactory = jobFactory;
            _symbolDataAccess = symbolDataAccess;
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            await _symbolDataAccess.DeleteAllSymbols();
            Scheduler = _schedulerFactory.GetScheduler(cancellationToken).Result;
            Scheduler.JobFactory = _jobFactory;

            var job = JobBuilder.Create<TseRequestCrawlJob>()
                .WithIdentity("localJob", "default")
                .Build();

            var trigger = TriggerBuilder.Create()
                .WithIdentity("trigger1", "default")
                .ForJob(job)
                .StartNow()
                .WithCronSchedule("0/3 * * * * ?")//Every 3 Seconds 
                .Build();

            await Scheduler.ScheduleJob(job, trigger, cancellationToken);

            await Scheduler.Start(cancellationToken);
        }

        public async Task StopAsync(CancellationToken cancellationToken)
        {
            await Scheduler?.Shutdown(cancellationToken)!;
        }
    }
}