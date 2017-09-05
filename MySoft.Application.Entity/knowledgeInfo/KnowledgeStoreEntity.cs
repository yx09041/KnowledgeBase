using System;
using System.Linq;
using System.Text;
using SqlSugar;

namespace MySoft.Application.Entity
{
    ///<summary>
    ///知识收藏实体
    ///</summary>
    [SugarTable("knowledgeStore")]
    public class KnowledgeStoreEntity
    {
        /// <summary>
        /// Desc:
        /// Default:
        /// Nullable:False
        /// </summary>           
        public string F_ID { get; set; }

        /// <summary>
        /// Desc:
        /// Default:
        /// Nullable:True
        /// </summary>           
        public string UserId { get; set; }

        /// <summary>
        /// Desc:
        /// Default:
        /// Nullable:True
        /// </summary>           
        public string knowledgeGUID { get; set; }

        /// <summary>
        /// 创建日期
        /// </summary>
        public DateTime? F_CreateDate { get; set; }
    }
}
