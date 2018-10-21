using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;

namespace QuartzWeb.Enum
{
    public enum JobStateEnum
    {
        /// <summary>
        /// 等待
        /// </summary>
        [Description("等待")]
        WAITING = 0,

        /// <summary>
        /// 暂停 
        /// </summary>
        [Description("暂停")]
        PAUSED = 1,

        /// <summary>
        /// 正常执行
        /// </summary>
        [Description("正常执行")]
        EXECUTING = 2,

        /// <summary>
        /// 阻塞 
        /// </summary>
        [Description("阻塞")]
        BLOCKED = 3,

        /// <summary>
        /// 错误
        /// </summary>
        [Description("错误")]
        ERROR = 4,

        /// <summary>
        /// 停止_阻塞
        /// </summary>
        [Description("停止_阻塞")]
        PAUSED_BLOCKED = 5,

        /// <summary>
        /// 已获得
        /// </summary>
        [Description("已获得")]
        ACQUIRED = 6,

        /// <summary>
        /// 完成
        /// </summary>
        [Description("完成")]
        COMPLETE = 7,

        /// <summary>
        /// 已删除
        /// </summary>
        [Description("已删除")]
        DELETED = 8,
    }
}