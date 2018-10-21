using Quartz;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace QuartzCommon.Job
{
    //[DisallowConcurrentExecutionAttribute]
    public class HelloJob: IJob
    {
        public Task Execute(IJobExecutionContext context)
        {
            Console.WriteLine("Info From HelloJob");
            Thread.Sleep(30000);
            LogHelper.WriteInfo("Info From HelloJob");
            return Task.FromResult(0);
        }
    }
}
