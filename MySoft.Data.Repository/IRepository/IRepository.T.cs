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
    public interface IRepository<TEntity> where TEntity : class,new()
    {
        #region 新增
        /// <summary>
        /// 新增单条记录
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        bool Insert(TEntity entity);
        /// <summary>
        /// 新增多条记录
        /// </summary>
        /// <param name="entitys"></param>
        /// <returns></returns>
        bool Insert(List<TEntity> entitys);
        #endregion

        #region 更新
        /// <summary>
        /// 根据表达式条件将对象更新到数据库(部分字段更新)
        /// </summary>
        /// <param name="rowObj">匿名对象rowObj为匿名对象时只更新指定列( 例如:new{ name='abc'}只更新name )，为T类型将更新整个实体(排除主键、自增列和禁止更新列)</param>
        /// <returns>true |false </returns>
        bool Update(object rowObj);
        /// <summary>
        /// 根据实体更新(全字段更新)
        /// </summary>
        /// <param name="entity">实体对象</param>
        /// <returns>true |false </returns>
        bool Update(TEntity entity);
        #endregion

        #region 删除
        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        bool Delete(TEntity entity);

        /// <summary>
        /// 根据表达式删除数据
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="expression">表达式条件</param>
        /// <returns>删除成功返回true</returns>
        bool Delete(Expression<Func<TEntity, bool>> filterExpression);

        /// <summary>
        /// 根据主键删除
        /// </summary>
        /// <param name="keyvalue"></param>
        /// <returns></returns>
        bool Delete(string keyvalue);
        #endregion

        #region 实体
        TEntity FindEntity(object keyValue);
        TEntity FindEntity(Expression<Func<TEntity, bool>> predicate);
        TEntity FindEntity(string tableName, Expression<Func<TEntity, bool>> predicate);
        #endregion

        #region 查询 ISugarQueryable
        ISugarQueryable<TEntity> IQueryable();
        ISugarQueryable<TEntity> IQueryable(string tableName);
        ISugarQueryable<TEntity> IQueryable(Expression<Func<TEntity, bool>> predicate);
        #endregion

        #region 查询 List
        List<TEntity> FindList(string strSql);
        List<TEntity> FindList(string strSql, object dbParameter);
        List<TEntity> FindList(Expression<Func<TEntity, bool>> predicate);
        List<TEntity> FindList(Pagination pagination);
        List<TEntity> FindList(Expression<Func<TEntity, bool>> predicate, Pagination pagination, string extWhere = "");
        List<TEntity> FindList(string tableName, Expression<Func<TEntity, bool>> predicate, Pagination pagination, string extWhere = "");
        #endregion
    }
}
