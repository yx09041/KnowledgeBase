using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Linq.Expressions;
using Mysoft.Util;
using SqlSugar;

namespace MySoft.Data.Repository
{
    /// <summary>
    /// 仓储接口
    /// </summary>
    public interface IRepository 
    {
        #region 新增
        /// <summary>
        /// 新增多条记录
        /// </summary>
        /// <param name="entitys"></param>
        /// <returns></returns>
        bool Insert<TEntity>(TEntity entity) where TEntity : class,new();
        #endregion

    }
}
