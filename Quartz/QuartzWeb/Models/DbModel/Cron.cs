using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace QuartzWeb.Models.DbModel
{
    public class Cron
    {
        public string SCHED_NAME { get; set;}

        public string TRIGGER_NAME { get; set; }

        public string TRIGGER_GROUP { get; set; }

        public string CRON_EXPRESSION { get; set; }

        public string TIME_ZONE_ID { get; set; }
    }
}