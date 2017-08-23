using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using SqlSugar;
namespace MySoft.Data.Repository
{
    /// <summary>
    ///单库 数据库实例化SqlSugar  
    /// </summary>
    public class SugarDbContext
    {
         //禁止实例化
        private SugarDbContext()
        {

        }
        public static string DefaultConnection = System.Configuration.ConfigurationManager.ConnectionStrings["BaseDb"].ConnectionString;

        /// <summary>
        /// 获取数据库实例
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static SqlSugarClient GetInstance(string ConnectionString = null)
        {
           SqlSugarClient db = new SqlSugarClient(new ConnectionConfig() { 
                     ConnectionString = DefaultConnection, //必填
                     DbType = DbType.SqlServer, //必填
                     IsAutoCloseConnection = true,//默认false
                     InitKeyType=InitKeyType.SystemTable }); //默认SystemTable
            return db;
        }


        /// <summary>
        /// 打印Sql
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="pars"></param>
        private static void PrintSql(string sql, string pars)
        {
            Console.WriteLine("sql:" + sql);
            if (pars != null)
            {
                Console.WriteLine(" pars:" + pars);
            }
            Console.WriteLine("");
        }
    }
}
