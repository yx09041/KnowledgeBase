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
    public class Repository<TEntity> : IRepository<TEntity> where TEntity : class,new()
    {
        SqlSugarClient dbcontext = SugarDbContext.GetInstance();

        #region 新增
        /// <summary>
        /// 新增单条数据
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="entity"></param>
        /// <returns></returns>
        public bool Insert(TEntity entity)
        {
            var result = dbcontext.Insertable(entity).ExecuteCommand();
            return result > 0 ? true : false;
        }

        /// <summary>
        /// 新增多条记录（批量插入 适合海量数据插入）
        /// </summary>
        /// <param name="entitys"></param>
        /// <returns></returns>
        public bool Insert(List<TEntity> entitys)
        {
            var result = dbcontext.Insertable(entitys.ToArray()).ExecuteCommand();
            return result > 0 ? true : false;
        }
        #endregion

        #region 更新
        /// <summary>
        /// 根据表达式条件将对象更新到数据库(部分字段更新)
        /// </summary>
        /// <param name="rowObj">匿名对象rowObj为匿名对象时只更新指定列( 例如:new{ name='abc'}只更新name )，为T类型将更新整个实体(排除主键、自增列和禁止更新列)</param>
        /// <param name="filterExpression">表达式条件</param>
        /// <returns>true |false </returns>
        public bool Update(object rowObj)
        {
            var result = dbcontext.Updateable<TEntity>(rowObj).ExecuteCommand();
            return result >= 0 ? true : false;
        }

        /// <summary>
        /// 根据实体更新,全字段更新
        /// </summary>
        /// <param name="entity">实体对象</param>
        /// <returns>true |false </returns>
        public bool Update(TEntity entity)
        {
            var result = dbcontext.Updateable(entity).Where(true).ExecuteCommand();
            return result >= 0 ? true : false;
        }
        #endregion

        #region 删除
        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public bool Delete(TEntity entity)
        {
           var result= dbcontext.Deleteable<TEntity>(entity).ExecuteCommand();
            return result >= 0 ? true : false;
        }

        /// <summary>
        /// 根据表达式删除数据
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="expression">表达式条件</param>
        /// <returns>删除成功返回true</returns>
        public bool Delete(Expression<Func<TEntity, bool>> filterExpression)
        {
            var result = dbcontext.Deleteable<TEntity>(filterExpression).ExecuteCommand();
            return result >= 0 ? true : false;
        }

        /// <summary>
        /// 根据主键删除
        /// </summary>
        /// <param name="keyvalue"></param>
        /// <returns></returns>
        public bool Delete(string keyvalue)
        {
            var result = dbcontext.Deleteable<TEntity>().In(keyvalue).ExecuteCommand();
            return result >= 0 ? true : false;
        }
        #endregion

        #region 实体

        /// <summary>
        /// 根据主键查询
        /// </summary>
        /// <param name="keyValue">主键</param>
        /// <returns></returns>
        public TEntity FindEntity(object keyValue)
        {
            return dbcontext.Queryable<TEntity>().InSingle(keyValue);
        }

        /// <summary>
        /// 返回序列中的第一条记录,如果序列为NULL返回default(T)
        /// </summary>
        /// <param name="predicate">表达式条件</param>
        /// <returns></returns>
        public TEntity FindEntity(Expression<Func<TEntity, bool>> predicate)
        {
            return dbcontext.Queryable<TEntity>().Where(predicate).First();
        }
      
        #endregion

        #region 查询 ISugarQueryable
        public ISugarQueryable<TEntity> IQueryable()
        {
            return dbcontext.Queryable<TEntity>();
        }
        public ISugarQueryable<TEntity> IQueryable(Expression<Func<TEntity, bool>> predicate)
        {
            return dbcontext.Queryable<TEntity>().Where(predicate);
        }
        #endregion

        #region 查询 List
        public List<TEntity> FindList(string strSql)
        {
            return dbcontext.Ado.SqlQuery<TEntity>(strSql).ToList<TEntity>();
        }
        public List<TEntity> FindList(string strSql, object dbParameter)
        {
            return dbcontext.Ado.SqlQuery<TEntity>(strSql, dbParameter).ToList<TEntity>();
        }
        public List<TEntity> FindList(Expression<Func<TEntity, bool>> predicate)
        {
            return dbcontext.Queryable<TEntity>().Where(predicate).ToList();
        }
        public List<TEntity> FindList(Pagination pagination)
        {
            //获取排序字段
            string order = PaginationHelper.GetOrder(pagination);
            pagination.records = dbcontext.Queryable<TEntity>().Count();
            return dbcontext.Queryable<TEntity>().OrderBy(order).ToPageList(pagination.page, pagination.rows);
        }
        public List<TEntity> FindList(Expression<Func<TEntity, bool>> predicate, Pagination pagination, string extWhere = "")
        {
            //获取排序字段
            string order = PaginationHelper.GetOrder(pagination);

            var query = dbcontext.Queryable<TEntity>().Where(predicate);
            if (!string.IsNullOrEmpty(extWhere))
            {
                query.Where(extWhere);
            }

            //总数
            pagination.records = query.Count();
            return query.OrderBy(order).ToPageList(pagination.page, pagination.rows);
        }
        #endregion

    }
}
