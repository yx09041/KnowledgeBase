using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Mysoft.Util
{
    /// <summary>
    /// 分页参数
    /// </summary>
    public class Pagination
    {
        /// <summary>
        /// 每页行数
        /// </summary>
        public int rows { get; set; }
        /// <summary>
        /// 当前页
        /// </summary>
        public int page { get; set; }
        /// <summary>
        /// 排序列
        /// </summary>
        public string sidx { get; set; }
        /// <summary>
        /// 排序类型
        /// </summary>
        public string sord { get; set; }
        /// <summary>
        /// 总记录数
        /// </summary>
        public int records { get; set; }
        /// <summary>
        /// 总页数
        /// </summary>
        public int total
        {
            get
            {
                if (records > 0)
                {
                    return records % this.rows == 0 ? records / this.rows : records / this.rows + 1;
                }
                else
                {
                    return 0;
                }
            }
        }
        /// <summary>
        /// 查询条件Json
        /// </summary>
        public string conditionJson { get; set; }
    }


    /// <summary>
    /// 分页帮助类
    /// </summary>
    public class PaginationHelper
    {
        /// <summary>
        /// 获取排序信息
        /// </summary>
        /// <param name="pagination"></param>
        /// <returns></returns>
        public static string GetOrder(Pagination pagination)
        {
            string[] sidxArry = pagination.sidx == null ? new string[] { } : pagination.sidx.Split(',');
            string[] sordArry = pagination.sord == null ? new string[] { "asc" } : pagination.sord.Split(',');
            string[] orderArry = new string[sidxArry.Length];
            int count = sidxArry.Length <= sordArry.Length ? sidxArry.Length : sordArry.Length;
            for (int i = 0; i < count; i++)
            {
                if (sidxArry[i].Split(' ').Length == 1)
                {
                    orderArry[i] = string.Format("{0} {1}", sidxArry[i], sordArry[i]);
                }
                else
                {
                    orderArry[i] = string.Format("{0}", sidxArry[i]);
                }
            }
            for (int i = count; i < sidxArry.Length; i++)
            {
                orderArry[i] = string.Format("{0}", sidxArry[i]);
            }

            string order = "";
            foreach (string item in orderArry)
            {
                order += item + ',';
            }
            return order.TrimEnd(',');
        }
    }
}
