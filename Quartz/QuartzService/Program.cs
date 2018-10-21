using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Topshelf;
using Quartz;
using QuartzCommon;
using System.IO;
using log4net.Config;

namespace QuartzService
{
    class Program
    {
        static void Main(string[] args)
        {
            FileInfo fi = new FileInfo(AppDomain.CurrentDomain.BaseDirectory + "Web.config");
            XmlConfigurator.ConfigureAndWatch(fi);
            LogHelper.SetConfig(fi);
            HostFactory.Run(config => 
            {
                config.Service<QuartzHelper>(setting =>
                {
                    setting.ConstructUsing(name => new QuartzHelper());
                    setting.WhenStarted( tc =>  tc.start());
                    setting.WhenStopped( tc =>  tc.StopSchedule());
                });
                config.RunAsLocalSystem();

                config.SetDescription("Quartz初使用");
                config.SetDisplayName("QuartzService");
                config.SetServiceName("QuartzService");
            });
        }
    }
}
