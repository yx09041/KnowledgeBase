using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySoft.Application.Entity.BaseManage;
using MySoft.Data.Repository;

namespace MySoft.Application.Business.BaseManage
{
    public class UserApp
    {
        IRepository<UserEntity> _knowledgeRepository = new Repository<UserEntity>();
        /// <summary>
        /// 用户实体
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <returns></returns>
        public UserEntity GetEntity(string account)
        {
            return _knowledgeRepository.FindEntity(t => t.Account == account);
        }


        public bool UpdateEntity(UserEntity UpdateEntity)
        {
            var db = SugarDbContext.GetInstance();
            var result = db.Updateable<UserEntity>(UpdateEntity).Where(true).ExecuteCommand();
            return result >= 0 ? true : false;

        }
        
    }
}
