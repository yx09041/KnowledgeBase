using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Mysoft.Util.Extension;
using MySoft.Application.Entity.BaseManage;
using MySoft.Data.Repository;

namespace MySoft.Application.Business.BaseManage
{
    public class UserApp
    {
        IRepository<UserEntity> _knowledgeRepository = new Repository<UserEntity>();

        #region 获取数据
        /// <summary>
        /// 用户实体
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <returns></returns>
        public UserEntity GetEntity(string account)
        {
            return _knowledgeRepository.FindEntity(t => t.Account == account);
        }


        public UserEntity GetEntityById(string keyValue)
        {
            return _knowledgeRepository.FindEntity(t => t.UserId == keyValue);
        }
        #endregion


        #region 验证
        /// <summary>
        /// 账户不能重复
        /// </summary>
        /// <param name="account">账户值</param>
        /// <param name="keyValue">主键</param>
        /// <returns></returns>
        public bool ExistAccount(string account)
        {
            var expression = LinqExt.True<UserEntity>();
            expression = expression.And(t => t.Account == account);
            var db = SugarDbContext.GetInstance();
            return _knowledgeRepository.IQueryable(expression).Count() > 0 ? true : false;
        }
        #endregion


        #region 提交
        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="UpdateEntity"></param>
        /// <returns></returns>
        public bool UpdateEntity(UserEntity UpdateEntity)
        {
            var db = SugarDbContext.GetInstance();
            var result = db.Updateable<UserEntity>(UpdateEntity).Where(true).ExecuteCommand();
            return result >= 0 ? true : false;
        }

        /// <summary>
        /// 修改密码
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="passWord"></param>
        /// <returns></returns>
        public bool UpdatePassWord(string userId, string passWord)
        {
            var db = SugarDbContext.GetInstance();
            var result = db.Updateable<UserEntity>(new { Password = passWord }).Where(t => t.UserId == userId).ExecuteCommand();
            return result >= 0 ? true : false;
        }

        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="UpdateEntity"></param>
        /// <returns></returns>
        public bool CreateForm(UserEntity entity)
        {
            entity.UserId = Guid.NewGuid().ToString();
            entity.CreateDate = DateTime.Now;
            return _knowledgeRepository.Insert(entity);
        }
        #endregion

    }
}
