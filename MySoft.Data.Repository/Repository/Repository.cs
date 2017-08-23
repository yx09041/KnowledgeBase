using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Linq.Expressions;
using SqlSugar;
using System.Reflection;
using MySoft.Data.Repository;
using Mysoft.Util;

namespace MySoft.Data.Repository
{
    /// <summary>
    /// 版 本 1.2
    /// Copyright (c) 2017
    /// 创建人：yxtic
    /// 日 期：2017.05.23 13:48
    /// 描 述：定义仓储模型中的数据标准操作
    /// </summary>
    /// <typeparam name="TEntity">动态实体类型</typeparam>
    public class Repository : IRepository 
    {
        SqlSugarClient dbcontext = SugarDbContext.GetInstance();

        #region 新增
        /// <summary>
        /// 新增单条数据
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="entity"></param>
        /// <returns></returns>
        public bool Insert<TEntity>(TEntity entity) where TEntity : class,new()
        {
            var result = dbcontext.Insertable(entity).ExecuteCommand();
            return result > 0 ? true : false;
        }
        #endregion


    }
}
