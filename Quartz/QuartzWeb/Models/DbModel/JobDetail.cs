using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace QuartzWeb.Models.DbModel
{
    public class JobDetail
    {
        public string SCHED_NAME { get; set; }

        public string JOB_NAME { get; set; }

        public string JOB_GROUP { get; set; }

        public string DESCRIPTION { get; set; }

        public string JOB_CLASS_NAME { get; set; }
    }
}