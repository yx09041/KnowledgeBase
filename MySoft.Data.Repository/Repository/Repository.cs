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
using System.Text;

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


        #region 事务
        public IRepository BeginTran()
        {
            dbcontext.Ado.BeginTran();
            return this;
        }

        public void Commit()
        {
            try
            {
                //提交事务
                dbcontext.Ado.CommitTran();
            }
            catch (Exception ex)
            {
                if (dbcontext != null)
                {
                    dbcontext.Ado.RollbackTran();//回滚事务
                }
                throw ex;
            }
        }

        public void Rollback()
        {
            dbcontext.Ado.RollbackTran();//回滚事务
        }

        public void Dispose()
        {
            if (dbcontext != null)
            {
                dbcontext.Dispose();
            }
        }
        #endregion

        #region 新增
        /// <summary>
        /// 新增单条记录
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="entity"></param>
        /// <returns></returns>
        public bool Insert<TEntity>(TEntity entity) where TEntity : class,new()
        {
            var result = dbcontext.Insertable<TEntity>(entity).ExecuteCommand();
            return result>=0 ? false : true;
        }
        /// <summary>
        /// 批量新增
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="entitys"></param>
        /// <returns></returns>
        public bool Insert<TEntity>(List<TEntity> entitys) where TEntity : class,new()
        {
            var result = dbcontext.Insertable<TEntity>(entitys.ToArray()).ExecuteCommand(); ;
            return true;
        }
        #endregion

        #region 更新
        public bool Update<TEntity>(TEntity entity) where TEntity : class,new()
        {
            var result = dbcontext.Updateable<TEntity>(entity).Where(true).ExecuteCommand();
            return result >= 0 ? true : false;
        }
        #endregion

        #region 删除
        public bool Delete<TEntity>(TEntity entity) where TEntity : class,new()
        {
            var result = dbcontext.Deleteable<TEntity>(entity).ExecuteCommand();
            return result >= 0 ? true : false;
        }
        public bool Delete<TEntity>(Expression<Func<TEntity, bool>> filterExpression) where TEntity : class,new()
        {
            var result = dbcontext.Deleteable<TEntity>(filterExpression).ExecuteCommand();
            return result >= 0 ? true : false;
        }
        public bool Delete<TEntity>(string keyvalue) where TEntity : class,new()
        {
            var result = dbcontext.Deleteable<TEntity>().In(keyvalue).ExecuteCommand();
            return result >= 0 ? true : false;
        }
        #endregion

        #region 实体
        public TEntity FindEntity<TEntity>(object keyValue) where TEntity : class,new()
        {
            return dbcontext.Queryable<TEntity>().InSingle(keyValue);
        }
        public TEntity FindEntity<TEntity>(Expression<Func<TEntity, bool>> predicate) where TEntity : class,new()
        {
            return dbcontext.Queryable<TEntity>().Where(predicate).First();
        }
        #endregion

        #region 查询 ISugarQueryable
        public ISugarQueryable<TEntity> IQueryable<TEntity>() where TEntity : class,new()
        {
            return dbcontext.Queryable<TEntity>();
        }
        public ISugarQueryable<TEntity> IQueryable<TEntity>(Expression<Func<TEntity, bool>> predicate) where TEntity : class,new()
        {
            return dbcontext.Queryable<TEntity>().Where(predicate);
        }
        #endregion

        #region 查询 List
        public List<TEntity> FindList<TEntity>(string strSql) where TEntity : class
        {
            return dbcontext.Ado.SqlQuery<TEntity>(strSql).ToList<TEntity>();
        }
        public List<TEntity> FindList<TEntity>(string strSql, object dbParameter) where TEntity : class
        {
            return dbcontext.Ado.SqlQuery<TEntity>(strSql, dbParameter).ToList<TEntity>();
        }
        public List<TEntity> FindList<TEntity>(Expression<Func<TEntity, bool>> predicate) where TEntity : class,new()
        {
            return dbcontext.Queryable<TEntity>().Where(predicate).ToList();
        }
        public List<TEntity> FindList<TEntity>(Pagination pagination) where TEntity : class,new()
        {
            //获取排序字段
            string order = PaginationHelper.GetOrder(pagination);
            pagination.records = dbcontext.Queryable<TEntity>().Count();
            return dbcontext.Queryable<TEntity>().OrderBy(order).ToPageList(pagination.page, pagination.rows);
        }
        public List<TEntity> FindList<TEntity>(Expression<Func<TEntity, bool>> predicate, Pagination pagination) where TEntity : class,new()
        {
            //获取排序字段
            string order = PaginationHelper.GetOrder(pagination);
            var query = dbcontext.Queryable<TEntity>().Where(predicate);
            //总数
            pagination.records = query.Count();
            return query.OrderBy(order).ToPageList(pagination.page, pagination.rows);
        }

        public List<TEntity> FindList<TEntity>(string strSql, DbParameter[] dbParameter, Pagination pagination) where TEntity : class,new()
        {
            int num = (pagination.page - 1) * pagination.rows;
            int num1 = (pagination.page) * pagination.rows;
            //获取排序字段
            string OrderBy = PaginationHelper.GetOrder(pagination);
            StringBuilder sb = new StringBuilder();
            sb.Append("Select * From (Select ROW_NUMBER() Over (" + OrderBy + ")");
            sb.Append(" As rowNum, * From (" + strSql + ") As T ) As N Where rowNum > " + num + " And rowNum <= " + num1 + "");
            //总数
            pagination.records = dbcontext.Ado.GetInt("Select Count(1) From (" + strSql + ") As t", dbParameter);
            return dbcontext.Ado.SqlQuery<TEntity>(sb.ToString(), dbParameter);
        }
        #endregion

        #region 查询 DataTable
        public DataTable FindTable(string strSql)
        {
            strSql = strSql.ToLower();
            return dbcontext.Ado.GetDataTable(strSql);
        }
        public DataTable FindTable(string strSql, DbParameter[] dbParameter)
        {
            strSql = strSql.ToLower();
            return dbcontext.Ado.GetDataTable(strSql, dbParameter);
        }
        /// <summary>
        /// 通过sql获取datatable（带分页，无参数）
        /// </summary>
        /// <param name="strSql"></param>
        /// <param name="pagination"></param>
        /// <returns></returns>
        public DataTable FindTable(string strSql, Pagination pagination)
        {
            strSql = strSql.ToLower();
            var data = FindTable(strSql, null, pagination);
            return data;
        }
        public DataTable FindTable(string strSql, DbParameter[] dbParameter, Pagination pagination)
        {
            StringBuilder sb = new StringBuilder();
            int num = (pagination.page - 1) * pagination.rows;
            int num1 = (pagination.page) * pagination.rows;
            //获取排序字段
            string OrderBy = PaginationHelper.GetOrder(pagination);
            sb.Append("Select * From (Select ROW_NUMBER() Over (" + OrderBy + ")");
            sb.Append(" As rowNum, * From (" + strSql + ") As T ) As N Where rowNum > " + num + " And rowNum <= " + num1 + "");
            pagination.records = dbcontext.Ado.GetInt("Select Count(1) From (" + strSql + ") As t", dbParameter);
            DataTable resultTable = dbcontext.Ado.GetDataTable(sb.ToString(), dbParameter);
            resultTable.Columns.Remove("rowNum");
            return resultTable;
        }
        #endregion
    }
}
