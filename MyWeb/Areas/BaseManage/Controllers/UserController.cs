using System;
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

        public ActionResult ModifyPassWord(string password, string verifycode)
        {
            #region 验证码验证
            verifycode = Md5Helper.MD5(verifycode.ToLower(), 16);
            if (Session["session_verifycode"].IsEmpty() || verifycode != Session["session_verifycode"].ToString())
            {
                return Error("验证码错误，请重新输入");
            }
            #endregion

            //修改密码
            return Success("修改成功");
        }
    }
}
