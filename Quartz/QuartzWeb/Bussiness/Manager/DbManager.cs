using SqlSugar;
using System;

namespace QuartzWeb.Bussiness.Manager
{
    public class DbManager
    {
        private string ConnectionString
        {
            get
            {
                return System.Configuration.ConfigurationManager.ConnectionStrings["connectionString"].ConnectionString;
            }
        }

        private string DbType
        {
            get
            {
                return System.Configuration.ConfigurationManager.AppSettings.Get("DbType");
            }
        }

        public SqlSugarClient db
        {
            get
            {
                SqlSugarClient _db = null;

                if (DbType.ToLower() == "mysql")
                {
                    _db = new SqlSugarClient(new ConnectionConfig() { ConnectionString = ConnectionString, DbType = SqlSugar.DbType.MySql, IsAutoCloseConnection = true });
                }
                else if (DbType.ToLower() == "sqlserver")
                {
                    _db = new SqlSugarClient(new ConnectionConfig() { ConnectionString = ConnectionString, DbType = SqlSugar.DbType.SqlServer, IsAutoCloseConnection = true });
                }
                else
                {
                    throw new Exception("DbType:" + DbType + " 未知");
                }

                _db.Ado.IsEnableLogEvent = false;
                //_db.Ado.LogEventStarting = (sql, pars) =>
                //{
                //    Console.WriteLine(sql + "\r\n" + db.RewritableMethods.SerializeObject(pars));
                //    Console.WriteLine();
                //};
                if (_db != null)
                {
                    string JobStateTable = GetDbTableNameSetting("JobStateMappingDbTable");
                    JobStateTable = string.IsNullOrWhiteSpace(JobStateTable) ? "JobState" : JobStateTable;
                    _db.MappingTables.Add("JobState", JobStateTable);

                    string JobDetailTable = GetDbTableNameSetting("JobDetailMappingDbTable");
                    JobDetailTable = string.IsNullOrWhiteSpace(JobDetailTable) ? "JobDetail" : JobDetailTable;
                    _db.MappingTables.Add("JobDetail", JobDetailTable);

                    string TriggerTable = GetDbTableNameSetting("TriggerMappingDbTable");
                    TriggerTable = string.IsNullOrWhiteSpace(TriggerTable) ? "Trigger" : TriggerTable;
                    _db.MappingTables.Add("Trigger", TriggerTable);

                    string CronTable = GetDbTableNameSetting("CronMappingDbTable");
                    CronTable = string.IsNullOrWhiteSpace(CronTable) ? "Cron" : CronTable;
                    _db.MappingTables.Add("Cron", CronTable);

                }
                return _db;
            }
        }

        private string GetDbTableNameSetting(string dbName)
        {
            return System.Configuration.ConfigurationManager.AppSettings.Get(dbName);
        }
    }
}