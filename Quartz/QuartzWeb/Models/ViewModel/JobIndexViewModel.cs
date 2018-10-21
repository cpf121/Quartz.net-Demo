using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace QuartzWeb.Models.ViewModel
{
    public class JobIndexViewModel
    {
        public string Name { get; set; }

        public string GroupName { get; set; }

        public string State { get; set; }

        public string ClassName { get; set; }

        public string LastRunTime { get; set; }

        public string NextRunTime { get; set; }

        public string Description { get; set; }

        public string CronExpression { get; set; }

        public string CronExpressionDescription { get; set; }
    }

    public class JobDeleteViewModel
    {
        public string JobName { get; set; }

        public string GroupName { get; set; }
    }
}