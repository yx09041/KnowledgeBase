using System;
using System.Collections.Generic;
using Mysoft.Util;
using MySoft.Application.Entity;
using MySoft.Data.Repository;
using Mysoft.Util.Extension;
using Mysoft.Code;
using System.Data;

namespace MySoft.Application.Business
{
    public class knowledgeApp
    {
        IRepository<KnowledgeInfoEntity> _knowledgeRepository = new Repository<KnowledgeInfoEntity>();

        #region 获取数据
        /// <summary>
        /// 查询
        /// </summary>
        /// <param name="doc"></param>
        /// <returns></returns>
        public List<KnowledgeInfoEntity> GetList(Pagination pagination)
        {
            return _knowledgeRepository.FindList(pagination);
        }

        /// <summary>
        /// 获取收藏列表
        /// </summary>
        /// <param name="pagination"></param>
        /// <returns></returns>
        public DataTable GetMyStoreList(Pagination pagination)
        {
            IRepository _knowledgeStoreRepository = new Repository();
            string userId = OperatorProvider.Provider.Current().UserId;
            string sql = string.Format(@"SELECT * FROM V_knowledgeStore 
                         WHERE UserId='{0}'", userId);
            return _knowledgeStoreRepository.FindTable(sql, pagination);
        }


        /// <summary>
        /// 获取所有数据
        /// </summary>
        /// <returns></returns>
        public List<KnowledgeInfoEntity> GetAllList()
        {
            return _knowledgeRepository.FindList(LinqExt.True<KnowledgeInfoEntity>());
        }

        /// <summary>
        /// 获取实体
        /// </summary>
        /// <param name="guid"></param>
        /// <returns></returns>
        public KnowledgeInfoEntity GetEntity(string keyvalue)
        {   
            //更新查看次数
            var db = SugarDbContext.GetInstance();
            db.Updateable<KnowledgeInfoEntity>().UpdateColumns(t => new KnowledgeInfoEntity() { ViewCount = t.ViewCount + 1 }).Where(t => t.knowledgeGUID == keyvalue).ExecuteCommand();
            return _knowledgeRepository.FindEntity(keyvalue);
        }
        #endregion

        #region 提交数据
        /// <summary>
        /// 保存
        /// </summary>
        /// <param name="doc"></param>
        /// <returns></returns>
        public bool SubmitForm(KnowledgeInfoEntity entity, string keyValue)
        {
            if (!string.IsNullOrEmpty(keyValue))
            {
                entity.knowledgeGUID = keyValue;
                entity.UpdateDate = DateTime.Now;
                return _knowledgeRepository.Update(entity);
            }
            else
            {
                entity.CreateById = OperatorProvider.Provider.Current().UserId;
                entity.CreateBy = OperatorProvider.Provider.Current().UserName;
                entity.CreateDate = DateTime.Now;
                entity.UpdateDate = DateTime.Now;
                entity.knowledgeGUID = Guid.NewGuid().ToString();
                entity.ViewCount = 1;
                return _knowledgeRepository.Insert(entity);
            }
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <returns></returns>
        public bool Delete(string keyvalue)
        {
            return _knowledgeRepository.Delete(t => t.knowledgeGUID == keyvalue);
        }
        #endregion


        #region 收藏

        /// <summary>
        /// 收藏
        /// </summary>
        /// <param name="knowledgeGUID"></param>
        /// <returns></returns>
        public bool StoreKnowledge(string knowledgeGUID)
        {
            KnowledgeStoreEntity entity = new KnowledgeStoreEntity();
            entity.F_ID = Guid.NewGuid().ToString();
            entity.UserId = OperatorProvider.Provider.Current().UserId;
            entity.knowledgeGUID = knowledgeGUID;
            entity.F_CreateDate = DateTime.Now;

            var db = SugarDbContext.GetInstance();
            //判断是否已收藏
            var count=db.Queryable<KnowledgeStoreEntity>().Where(t => t.UserId == entity.UserId && t.knowledgeGUID == knowledgeGUID).Count();
            if (count > 0)
            {
                return true;
            }
            else
            {
                return db.Insertable<KnowledgeStoreEntity>(entity).ExecuteCommand() >= 0 ? true : false;
            }
        }


        /// <summary>
        /// 取消收藏
        /// </summary>
        /// <param name="knowledgeGUID"></param>
        /// <returns></returns>
        public bool CancelStoreKnowledge(string knowledgeGUID)
        {
            string userId = OperatorProvider.Provider.Current().UserId;
            var db = SugarDbContext.GetInstance();
            return db.Deleteable<KnowledgeStoreEntity>(t => t.knowledgeGUID == knowledgeGUID && t.UserId == userId).ExecuteCommand() >= 0 ? true : false;
        }
        #endregion
    }
}
