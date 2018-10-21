using Quartz;
using Quartz.Impl;
using Quartz.Impl.Matchers;
using System;
using System.Collections.Specialized;
using System.Configuration;
using System.Net.Http;
using System.Reflection;
using System.Threading.Tasks;

namespace QuartzCommon
{
    public class QuartzHelper
    {
        private static object obj = new object();
        private static IScheduler scheduler = null;
        private static readonly string assemblyName= "QuartzCommon";
        #region 初始化任务调度对象
        public async static Task<IScheduler> GetScheduler()
        {
            try
            {
                if (scheduler == null)
                {
                    #region quartz 实例配置 还包括集群，线程池等配置，可单独放到config文件中配置
                    var properties = new NameValueCollection();
                    //存储类型
                    properties["quartz.jobStore.type"] = "Quartz.Impl.AdoJobStore.JobStoreTX, Quartz";
                    properties["quartz.serializer.type"] = "binary";
                    //表明前缀
                    properties["quartz.jobStore.tablePrefix"] = "QRTZ_";
                    //驱动类型
                    properties["quartz.jobStore.driverDelegateType"] = "Quartz.Impl.AdoJobStore.SqlServerDelegate, Quartz";
                    //数据源名称
                    properties["quartz.jobStore.dataSource"] = "Quartz";
                    //连接字符串
                    properties["quartz.dataSource.Quartz.connectionString"] = ConfigurationManager.AppSettings["quartzConnect"];
                    //sqlserver版本
                    properties["quartz.dataSource.Quartz.provider"] = "SqlServer";
                    //最大链接数
                    properties["quartz.dataSource.Quartz.maxConnections"] = "5";
                    #endregion

                    ISchedulerFactory sf = new StdSchedulerFactory(properties);
                    scheduler =await sf.GetScheduler();

                    LogHelper.WriteInfo(ConfigurationManager.AppSettings["quartzConnect"].ToString());
                    LogHelper.WriteInfo("任务调度初始化成功");
                }
            }
            catch (Exception ex)
            {
                LogHelper.WriteError("任务调度初始化失败！", ex);
            }
            return scheduler;
        }
        #endregion

        #region 添加job到调度器
        public static async Task<bool> AddJob(QuartzModel model)
        {
            var IsSuccess = false;
            var sche = await GetScheduler();
            model.SchedName = sche.SchedulerName;
            //验证是否是正确的Cron表达式
            if (CronExpression.IsValidExpression(model.TriggerCron))
            {
                JobKey jk = new JobKey(model.JobName,model.JobGroup);
                //var scheduRes = sche.Result;
                var IsExist = sche.CheckExists(jk).GetAwaiter().GetResult();
                if (IsExist)
                {
                    LogHelper.WriteError($"Job:{model.JobName}已存在", new Exception($"Job:{model.JobName}已存在"));
                    //throw new Exception($"Job:{model.JobName}已存在");
                    return false;
                }
                var type = GetClassInfo(model.JobClassName);
                if (type != null)
                {
                    if (model.StarRunTime == null)
                    {
                        model.StarRunTime = DateTime.Now;
                    }
                    DateTimeOffset starRunTime = DateBuilder.NextGivenSecondDate(model.StarRunTime, 1);
                    if (model.EndRunTime == null)
                    {
                        model.EndRunTime = DateTime.MaxValue.AddDays(-1);
                    }
                    DateTimeOffset endRunTime = DateBuilder.NextGivenSecondDate(model.EndRunTime, 1);
                    IJobDetail job = new JobDetailImpl(model.JobName, model.JobGroup, type);

                    ICronTrigger trigger = (ICronTrigger)TriggerBuilder.Create()
                                                 .StartAt(starRunTime)
                                                 .EndAt(endRunTime)
                                                 .WithDescription(model.Description)
                                                 .WithIdentity(model.JobName, model.JobGroup)
                                                 .WithCronSchedule(model.TriggerCron)
                                                 .Build();

                    
                    sche.ScheduleJob(job, trigger).GetAwaiter().GetResult();
                    LogHelper.WriteInfo($"Job:{model.JobName}添加完成");
                    IsSuccess = true;
                }
            }
            return IsSuccess;
        }
        #endregion

        #region 暂停指定的job
        public static async Task PauseJob(string jobName,string jobgroup)
        {
            try
            {
                var schedu = GetScheduler();
                JobKey jk = new JobKey(jobName, jobgroup);
                var scheduRes = schedu.Result;
                var IsExist = scheduRes.CheckExists(jk).GetAwaiter().GetResult();
                if (IsExist)
                {
                    //任务存在则暂停任务
                    await scheduRes.PauseJob(jk);
                    LogHelper.WriteInfo($"任务{jobName}已经暂停");
                }
            }
            catch (Exception ex)
            {
                LogHelper.WriteError($"任务{jobName}暂停出错", ex);
                throw ex;
            }
        }
        #endregion

        #region 恢复停止的job
        public static async Task ResumeJob(string jobName, string jobgroup)
        {
            try
            {
                var schedu = GetScheduler();
                JobKey jk = new JobKey(jobName, jobgroup);
                var scheduRes = schedu.Result;
                var IsExist = scheduRes.CheckExists(jk).GetAwaiter().GetResult();
                if (IsExist)
                {
                    await scheduRes.ResumeJob(jk);
                    LogHelper.WriteInfo($"任务{jobName}已经恢复");
                }
            }
            catch (Exception ex)
            {
                LogHelper.WriteError($"任务{jobName}恢复出错", ex);
                throw ex;
            }
        }
        #endregion

        #region 开始指定的job(立即开始，即使没到预定的时间)
        public static async Task StartJob(string jobName, string jobgroup)
        {
            try
            {
                var schedu = GetScheduler();
                JobKey jk = new JobKey(jobName, jobgroup);
                var scheduRes = schedu.Result;
                var IsExist = scheduRes.CheckExists(jk).GetAwaiter().GetResult();
                if (IsExist)
                {
                    await scheduRes.TriggerJob(jk);
                    LogHelper.WriteInfo($"任务{jobName}已经开始");
                }
            }
            catch (Exception ex)
            {
                LogHelper.WriteError($"任务{jobName}开始失败", ex);
                throw ex;
            }
        }
        #endregion

        #region 删除指定job
        public static async Task DeleteJob(string jobName, string jobgroup)
        {
            try
            {
                var schedu = GetScheduler();
                JobKey jk = new JobKey(jobName, jobgroup);
                var scheduRes = schedu.Result;
                var IsExist = scheduRes.CheckExists(jk).GetAwaiter().GetResult();
                if (IsExist)
                {
                    await scheduRes.DeleteJob(jk);
                    LogHelper.WriteInfo($"任务{jobName}已经删除");
                }
            }
            catch (Exception ex)
            {
                LogHelper.WriteError($"任务{jobName}删除失败", ex);
                throw ex;
            }
        }
        #endregion

        #region 私有方法 
        /// <summary>
        /// 通过反射获取job的Type
        /// </summary>
        /// <param name="jobName">job的名称</param>
        /// <returns></returns>
        private static Type GetClassInfo(string jobName)
        {
            try
            {
                var abName = new AssemblyName(assemblyName);
                var assembly = Assembly.Load(abName);
                Type type = assembly.GetType("QuartzCommon.Job."+jobName, true, true);
                if (typeof(IJob).IsAssignableFrom(type))
                {
                    return type;
                }
                else
                {
                    LogHelper.WriteError(string.Format("{0}没继承IJob接口", jobName), new Exception());
                    return null;
                }
            }
            catch (Exception ex)
            {
                LogHelper.WriteError("反射出错", ex);
                throw ex;
            }
        }
        #endregion

        public void start()
        {
            try
            {
                var sch =  GetScheduler().GetAwaiter().GetResult();
                if (!sch.IsStarted)
                {
                    sch.Start();
                    LogHelper.WriteInfo("任务调度开始");
                }
                var groupNames=sch.GetJobGroupNames().GetAwaiter().GetResult();
                foreach (var item in groupNames)
                {
                    LogHelper.WriteInfo(item);
                }
            }
            catch (Exception ex)
            {
                LogHelper.WriteError("任务调度开始失败", ex);
            }
        }

        public void StopSchedule()
        {
            try
            {
                if (!scheduler.IsShutdown)
                {
                    scheduler.Shutdown(true);
                    LogHelper.WriteInfo("任务调度停止！");
                }
            }
            catch (Exception ex)
            {
                LogHelper.WriteError("任务调度停止失败", ex);
            }
        }
    }
}
