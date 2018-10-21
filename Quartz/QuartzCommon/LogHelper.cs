using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuartzCommon
{
    /// <summary>
    /// 使用LOG4NET记录日志的功能，在WEB.CONFIG里要配置相应的节点
    /// </summary>
    public class LogHelper
    {
        public static readonly log4net.ILog loginfo = log4net.LogManager.GetLogger("loginfo");
        public static readonly log4net.ILog logerror = log4net.LogManager.GetLogger("logerror");

        public static void SetConfig()
        {
            log4net.Config.XmlConfigurator.Configure();
        }

        public static void SetConfig(FileInfo configFile)
        {
            log4net.Config.XmlConfigurator.Configure(configFile);
        }

        /// <summary>
        /// 普通的文件记录日志
        /// </summary>
        /// <param name="info"></param>
        public static void WriteInfo(string info)
        {
            if (loginfo.IsInfoEnabled)
            {
                loginfo.Info(info);
            }
        }

        /// <summary>
        /// 错误日志
        /// </summary>
        /// <param name="error"></param>
        /// <param name="se"></param>
        public static void WriteError(string error, Exception se)
        {
            if (logerror.IsErrorEnabled)
            {
                logerror.Error(error, se);
            }
        }

        /// <summary>
        /// 职责名称
        /// </summary>
        private string repositoryName;

        /// <summary>
        /// 日志级别
        /// </summary>
        private string level;

        /// <summary>
        /// 初始化
        /// </summary>
        /// <param name="repositoryName">职责名称</param>
        /// <remarks>解决任务和日志对应问题</remarks>
        public LogHelper(string repositoryName, string level)
        {
            this.repositoryName = repositoryName;
            this.level = level;
        }
    } 
}
