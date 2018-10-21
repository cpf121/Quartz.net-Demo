using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace QuartzWeb.Models.DbModel
{
    public class JobState
    {
        public string SCHED_NAME { get; set; }

        public string JOB_NAME { get; set; }

        public string JOB_GROUP { get; set; }

        /// <summary>
        /// 0 已添加 1 运行中 2 已暂停 3 已完成
        /// </summary>
        public int JOB_STATE { get; set; } 
    }
}