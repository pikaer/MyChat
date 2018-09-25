using System.Configuration;
using System.Data;
using System.Data.SqlClient;

namespace Chat.Repository
{
    public abstract class BaseRepository
    {
        protected IDbConnection GetDbConnection()
        {
            var dbEnum = GetDbEnum();
            var dbName = System.Enum.GetName(dbEnum.GetType(), dbEnum);
            var connString = ConfigurationManager.ConnectionStrings[dbName].ConnectionString;
            return new SqlConnection(connString);
        }

        /// <summary>
        /// 数据库连接字符串的名称
        /// </summary>
        protected enum DbEnum
        {
            MyChat,
        }

        /// <summary>
        /// 设定数据库
        /// </summary>
        /// <returns></returns>
        protected abstract DbEnum GetDbEnum();
    }
}
