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
        IRepository BeginTran();

        #region 事物
        void Commit();
        void Rollback();
        #endregion

        #region 新增
        /// <summary>
        /// 新增
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="entity"></param>
        /// <returns></returns>
        bool Insert<TEntity>(TEntity entity) where TEntity : class,new();
        /// <summary>
        /// 批量插入
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="entitys"></param>
        /// <returns></returns>
        bool Insert<TEntity>(List<TEntity> entitys) where TEntity : class,new();
        #endregion

        #region 更新
        bool Update<TEntity>(TEntity entity) where TEntity : class,new();
        #endregion

        #region 删除
        bool Delete<TEntity>(TEntity entity) where TEntity : class,new();
        bool Delete<TEntity>(Expression<Func<TEntity, bool>> predicate) where TEntity : class,new();

        bool Delete<TEntity>(string keyvalue) where TEntity : class,new();
        #endregion

        #region 实体
        TEntity FindEntity<TEntity>(object keyValue) where TEntity : class,new();
        TEntity FindEntity<TEntity>(Expression<Func<TEntity, bool>> predicate) where TEntity : class,new();
        #endregion

        #region 查询 ISugarQueryable
        ISugarQueryable<TEntity> IQueryable<TEntity>() where TEntity : class,new();
        ISugarQueryable<TEntity> IQueryable<TEntity>(Expression<Func<TEntity, bool>> predicate) where TEntity : class,new();
        #endregion

        #region 查询 List
        List<TEntity> FindList<TEntity>(string strSql) where TEntity : class;
        List<TEntity> FindList<TEntity>(string strSql, object dbParameter) where TEntity : class;
        List<TEntity> FindList<TEntity>(Expression<Func<TEntity, bool>> predicate) where TEntity : class,new();
        List<TEntity> FindList<TEntity>(Pagination pagination) where TEntity : class,new();
        List<TEntity> FindList<TEntity>(Expression<Func<TEntity, bool>> predicate, Pagination pagination) where TEntity : class,new();
        List<TEntity> FindList<TEntity>(string strSql, DbParameter[] dbParameter, Pagination pagination) where TEntity : class,new();
        #endregion

        #region 查询 DataTable
        DataTable FindTable(string strSql);
        DataTable FindTable(string strSql, DbParameter[] dbParameter);
        DataTable FindTable(string strSql, Pagination pagination);
        DataTable FindTable(string strSql, DbParameter[] dbParameter, Pagination pagination);
        #endregion


    }
}
