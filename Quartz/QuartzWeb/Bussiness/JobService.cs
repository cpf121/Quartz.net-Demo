using QuartzCommon;
using QuartzWeb.Bussiness.Manager;
using QuartzWeb.Enum;
using QuartzWeb.Models.DbModel;
using QuartzWeb.Models.ViewModel;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace QuartzWeb.Bussiness
{
    public class JobService:BaseManager
    {
        public bool InsertBackgroundJob(QuartzModel model)
        {
            //添加job，添加job状态
            var IsSuccess = false;
            IsSuccess = QuartzHelper.AddJob(model).GetAwaiter().GetResult();
            //var StateModel = new JobState()
            //{
            //    SCHED_NAME=model.SchedName,
            //    JOB_NAME=model.JobName,
            //    JOB_GROUP=model.JobGroup,
            //    JOB_STATE=0
            //};
            //db.Insertable(StateModel).ExecuteCommand();
            return IsSuccess;
        }

        public PagerModel<JobIndexViewModel> GeBackgroundJobInfoPagerList(PageParameter parameter)
        {
            int TotalRecord = 0;
            List<JobIndexViewModel> datalist = null;
            datalist = db.Queryable<JobDetail, Trigger, Cron>((jobdetail, qrttrigger, cron) => new object[] {
                JoinType.Left,jobdetail.SCHED_NAME==qrttrigger.SCHED_NAME&&jobdetail.JOB_NAME==qrttrigger.JOB_NAME&&jobdetail.JOB_GROUP==qrttrigger.JOB_GROUP,
                JoinType.Left,qrttrigger.SCHED_NAME==cron.SCHED_NAME&&qrttrigger.TRIGGER_NAME==cron.TRIGGER_NAME&&qrttrigger.TRIGGER_GROUP==cron.TRIGGER_GROUP,
                //JoinType.Left,jobdetail.SCHED_NAME==qrtstate.SCHED_NAME&&jobdetail.JOB_NAME==qrtstate.JOB_NAME&&jobdetail.JOB_GROUP==qrtstate.JOB_GROUP
            })
            .Select((jobdetail, qrttrigger, cron) => new JobIndexViewModel
            {
                Name = jobdetail.JOB_NAME,
                GroupName = jobdetail.JOB_GROUP,
                State = qrttrigger.TRIGGER_STATE+"", //EnumHelper.GetDescriptionByValue<JobStateEnum>(),
                ClassName = jobdetail.JOB_CLASS_NAME,
                LastRunTime = qrttrigger.PREV_FIRE_TIME.ToString(), //GetDateTime().ToString("yyyy-MM-dd HH:mm:ss"),
                NextRunTime = qrttrigger.NEXT_FIRE_TIME.ToString(), //GetDateTime().ToString("yyyy-MM-dd HH:mm:ss"),
                Description = jobdetail.DESCRIPTION,
                CronExpression = cron.CRON_EXPRESSION
            }).ToPageList(parameter.currentPageIndex, parameter.rows, ref TotalRecord);
            if (datalist != null&&datalist.Count>0)
            {
                foreach (var item in datalist)
                {
                    item.State = EnumHelper.GetDescriptionByName<JobStateEnum>(item.State);
                    item.LastRunTime = item.LastRunTime==null?"": GetDateTime(long.Parse(item.LastRunTime)).ToString("yyyy-MM-dd HH:mm:ss");
                    item.NextRunTime= item.NextRunTime==null?"": GetDateTime(long.Parse(item.NextRunTime)).ToString("yyyy-MM-dd HH:mm:ss");
                }
            }

            var pagerModel = new PagerModel<JobIndexViewModel>();
            pagerModel.dataList = datalist;
            pagerModel.TotalRecord = TotalRecord;
            pagerModel.CurrentPage = parameter.currentPageIndex;
            pagerModel.CalculateTotalPage(parameter.rows, TotalRecord);
            return pagerModel;
        }

        private DateTime GetDateTime(long timeStamp)
        {
            DateTime dtStart = TimeZone.CurrentTimeZone.ToLocalTime(new DateTime());
            //long lTime = timeStamp * 10000000;
            TimeSpan toNow = new TimeSpan(timeStamp);
            DateTime targetDt = dtStart.Add(toNow);
            return targetDt;
        }
    }
}