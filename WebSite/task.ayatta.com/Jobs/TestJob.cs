using Hangfire;
using Hangfire.Console;
using Hangfire.RecurringJobExtensions;
using Hangfire.Server;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Ayatta.Web.Jobs
{
    public class TestJob : IRecurringJob
    {
        public void Execute(PerformContext context)
        {
            context.SetJobData("NewIntVal", 99);

            var newIntVal = context.GetJobData<int>("NewIntVal");

            context.WriteLine($"NewIntVal:{newIntVal}");

            context.WriteLine($"{DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss")} MyJob1 Running ...");
        }
    }

    public class RecurringJobService
    {
        [RecurringJob("*/1 * * * *")]
        [Queue("jobs")]
        public void TestJob1(PerformContext context)
        {
            context.WriteLine($"{DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss")} TestJob1 Running ...");
        }
        [RecurringJob("*/2 * * * *", RecurringJobId = "TestJob2")]
        [Queue("jobs")]
        public void TestJob2(PerformContext context)
        {
            context.WriteLine($"{DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss")} TestJob2 Running ...");
        }
        [RecurringJob("*/2 * * * *", "China Standard Time", "jobs")]
        public void TestJob3(PerformContext context)
        {
            context.WriteLine($"{DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss")} TestJob3 Running ...");
        }
        [RecurringJob("*/5 * * * *", "jobs")]
        public void InstanceTestJob(PerformContext context)
        {
            context.WriteLine($"{DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss")} InstanceTestJob Running ...");
        }

        [RecurringJob("*/6 * * * *", "UTC", "jobs")]
        public static void StaticTestJob(PerformContext context)
        {
            context.WriteLine($"{DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss")} StaticTestJob Running ...");
        }
    }
}
