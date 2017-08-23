using System;
using System.Collections.Generic;
using Mysoft.Util;
using MySoft.Application.Entity;
using MySoft.Data.Repository;
using Mysoft.Util.Extension;

namespace MySoft.Application.Business
{
    public class knowledgeApp
    {
        IRepository<KnowledgeInfoEntity> _knowledgeRepository = new Repository<KnowledgeInfoEntity>();

        /// <summary>
        /// 保存
        /// </summary>
        /// <param name="doc"></param>
        /// <returns></returns>
        public bool SubmitForm(KnowledgeInfoEntity entity, string keyValue)
        {
            if (!string.IsNullOrEmpty(keyValue))
            {
                return _knowledgeRepository.Update(entity);
            }
            else
            {
                entity.knowledgeGUID = Guid.NewGuid().ToString();
                return _knowledgeRepository.Insert(entity);
            }
        }

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
            return _knowledgeRepository.FindEntity(keyvalue);
        }


        /// <summary>
        /// 删除
        /// </summary>
        /// <returns></returns>
        public bool Delete(string keyvalue)
        {
            return _knowledgeRepository.Delete(t => t.knowledgeGUID == keyvalue);
        }
    }
}
