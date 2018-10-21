using SqlSugar;

namespace QuartzWeb.Bussiness.Manager
{
    public class BaseManager
    {
        public SqlSugarClient db
        {
            get
            {
                return new DbManager().db;
            }
        }
    }
}