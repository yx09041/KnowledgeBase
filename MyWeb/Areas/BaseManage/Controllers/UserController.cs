using System;
using System.Web.Mvc;
using Mysoft.Code;
using Mysoft.Util;
using Mysoft.Util.Extension;
using MySoft.Application.Business.BaseManage;
using MySoft.Application.Entity.BaseManage;

namespace MyWeb.Areas.BaseManage.Controllers
{
    public class UserController : BaseController
    {
        UserApp _app = new UserApp();
        #region 视图
        [HandlerLogin(LoginMode.Enforce)]
        public ActionResult BaseInfo()
        {
            return View();
        }

        [HandlerLogin(LoginMode.Enforce)]
        public ActionResult ModifyPassWord()
        {
            return View();
        }
        #endregion


        /// <summary>
        /// 获取当前用户信息
        /// </summary>
        /// <returns></returns>
        public ActionResult GetCurrentUserFormJson()
        {
            string userId = OperatorProvider.Provider.Current().UserId;
            UserEntity userEntity = _app.GetEntityById(userId);
            return Content(userEntity.ToJson());
        }

        public ActionResult SubmitModifyPassWord(string password, string verifycode)
        {
            #region 验证码验证
            verifycode = Md5Helper.MD5(verifycode.ToLower(), 16);
            if (Session["session_verifycode"].IsEmpty() || verifycode != Session["session_verifycode"].ToString())
            {
                return Error("验证码错误，请重新输入");
            }
            #endregion

           string userId = OperatorProvider.Provider.Current().UserId;
           UserEntity userEntity = _app.GetEntityById(userId);
            //密码
           string dbPassword = Md5Helper.MD5(DESEncrypt.Encrypt(Md5Helper.MD5(password, 32).ToLower(), userEntity.Secretkey).ToLower(), 32).ToLower();
            _app.UpdatePassWord(userId, dbPassword);
            //修改密码
            return Success("修改成功");
        }
        /// <summary>
        /// 修改基础信息
        /// </summary>
        /// <param name="NickName"></param>
        /// <returns></returns>
        public ActionResult SubmitModifyBaseInfo(string NickName)
        {
            string userId = OperatorProvider.Provider.Current().UserId;
            UserEntity userEntity = new UserEntity();
            userEntity.UserId = userId;
            userEntity.RealName = NickName;
            _app.UpdateEntity(userEntity);
            return Success("修改成功");
        }

        
    }
}
