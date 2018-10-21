using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace QuartzWeb.Models.DbModel
{
    public class Trigger
    {
        public string SCHED_NAME { get; set; }

        public string TRIGGER_NAME { get; set; }

        public string TRIGGER_GROUP { get; set; }

        public string JOB_NAME { get; set; }

        public string JOB_GROUP { get; set; }

        public string TRIGGER_STATE { get; set; }

        public string DESCRIPTION { get; set; }

        public int NEXT_FIRE_TIME { get; set; }

        public int PREV_FIRE_TIME { get; set; }
    }
}