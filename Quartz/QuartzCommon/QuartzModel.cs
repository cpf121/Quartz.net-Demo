using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuartzCommon
{
    public class QuartzModel
    {
        /// <summary>
        /// SchedName
        /// </summary>
        public string SchedName { get; set; }

        /// <summary>
        /// Job名称
        /// </summary>
        public string JobName { get; set; }

        /// <summary>
        /// JobGroup
        /// </summary>
        public string JobGroup { get; set; }

        /// <summary>
        /// job描述
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// 对应的Job类型名称
        /// </summary>
        public string JobClassName { get; set; }

        /// <summary>
        /// Cron表达式
        /// </summary>
        public string TriggerCron { get; set; }

        public DateTime? StarRunTime { get; set; }

        public DateTime? EndRunTime { get; set; }
    }
}
