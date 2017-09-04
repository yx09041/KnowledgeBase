using System;
using System.Web.Mvc;
using Mysoft.Code;
using Mysoft.Util;
using Mysoft.Util.Extension;
using MySoft.Application.Business.BaseManage;
using MySoft.Application.Entity.BaseManage;

namespace MyWeb.Controllers
{
    /// <summary>
    /// 描 述：系统登录
    /// </summary>
    [HandlerLogin(LoginMode.Ignore)]
    public class LoginController : BaseController
    {
        UserApp _app = new UserApp();
        #region 视图功能
        /// <summary>
        /// 默认页面
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult Default()
        {
            return View();
        }
        /// <summary>
        /// 登录页面
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult Index()
        {
            return View();
        }
        /// <summary>
        /// 注册
        /// </summary>
        /// <returns></returns>
         [HttpGet]
        public ActionResult Register()
        {
            return View();
        }
        
        #endregion

        #region 提交数据
        /// <summary>
        /// 生成验证码
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult VerifyCode()
        {
            return File(new VerifyCode().GetVerifyCode(), @"image/Gif");
        }
        /// <summary>
        /// 安全退出
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [AjaxOnly]
        public ActionResult OutLogin()
        {
            Session.Abandon();                                          //清除当前会话
            Session.Clear();                                            //清除当前浏览器所有Session
            WebHelper.RemoveCookie("My_autologin");                    //清除自动登录
            return Success("退出系统");
        }
        /// <summary>
        /// 登录验证
        /// </summary>
        /// <param name="username">用户名</param>
        /// <param name="password">密码</param>
        /// <param name="verifycode">验证码</param>
        /// <param name="autologin">下次自动登录</param>
        /// <returns></returns>
        [HttpPost]
        [AjaxOnly]
        public ActionResult CheckLogin(string username, string password, string verifycode, int autologin)
        {
            try
            {
                #region 验证码验证
                if (autologin == 0)
                {
                    verifycode = Md5Helper.MD5(verifycode.ToLower(), 16);
                    if (Session["session_verifycode"].IsEmpty() || verifycode != Session["session_verifycode"].ToString())
                    {
                        throw new Exception("验证码错误，请重新输入");
                    }
                }
                #endregion

                #region 第三方账户验证
                #endregion

                #region 内部账户验证
              
               UserEntity userEntity = _app.GetEntity(username);
                if (userEntity != null)
                {
                    //验证密码是否正确
                    string dbPassword = Md5Helper.MD5(DESEncrypt.Encrypt(password.ToLower(), userEntity.Secretkey).ToLower(), 32).ToLower();
                    if (dbPassword == userEntity.Password)
                    {
                        DateTime LastVisit = DateTime.Now;
                        int LogOnCount = (userEntity.LogOnCount).ToInt() + 1;
                        if (userEntity.LastVisit != null)
                        {
                            userEntity.PreviousVisit = userEntity.LastVisit.ToDate();
                        }
                        userEntity.LastVisit = LastVisit;
                        userEntity.LogOnCount = LogOnCount;
                        userEntity.UserOnLine = 1;
                        _app.UpdateEntity(userEntity);
                    }
                    else
                    {
                        return Error("密码和账户名不匹配");
                    }
                    Operator operators = new Operator();
                    operators.UserId = userEntity.UserId;
                    operators.Code = userEntity.EnCode;
                    operators.Account = userEntity.Account;
                    operators.UserName = userEntity.RealName;
                    operators.Password = userEntity.Password;
                    operators.Secretkey = userEntity.Secretkey;
                    operators.DepartmentId = userEntity.DepartmentId;
                    operators.IPAddress = Net.Ip;
                    operators.LogTime = DateTime.Now;
                    operators.Token = DESEncrypt.Encrypt(Guid.NewGuid().ToString());
                    //判断是否系统管理员
                    if (userEntity.Account == "System" || userEntity.Account == "admin")
                    {
                        operators.IsSystem = true;
                    }
                    else
                    {
                        operators.IsSystem = false;
                    }
                    OperatorProvider.Provider.AddCurrent(operators);
                }
                return Success("登录成功。");
                #endregion
            }
            catch (Exception ex)
            {
                WebHelper.RemoveCookie("My_autologin");                  //清除自动登录
                return Error(ex.Message);
            }
        }
        #endregion

        #region 注册账户、登录限制
        //private AccountBLL accountBLL = new AccountBLL();
        /// <summary>
        /// 获取验证码(手机)
        /// </summary>
        /// <param name="mobileCode">手机号码</param>
        /// <returns>返回6位数验证码</returns>
        [HttpGet]
        public ActionResult GetSecurityCode(string mobileCode)
        {
            //if (!ValidateUtil.IsValidMobile(mobileCode))
            //{
            //    throw new Exception("手机格式不正确,请输入正确格式的手机号码。");
            //}
            //var data = accountBLL.GetSecurityCode(mobileCode);
            //if (!string.IsNullOrEmpty(data))
            //{
            //    SmsModel smsModel = new SmsModel();
            //    smsModel.account = Config.GetValue("SMSAccount");
            //    smsModel.pswd = Config.GetValue("SMSPswd");
            //    smsModel.url = Config.GetValue("SMSUrl");
            //    smsModel.mobile = mobileCode;
            //    smsModel.msg = "验证码 " + data + "，(请确保是本人操作且为本人手机，否则请忽略此短信)";
            //    SmsHelper.SendSmsByJM(smsModel);
            //}
            return Success("获取成功。");
        }
        /// <summary>
        /// 注册账户
        /// </summary>
        /// <param name="mobileCode">手机号</param>
        /// <param name="securityCode">短信验证码</param>
        /// <param name="fullName">姓名</param>
        /// <param name="password">密码（md5）</param>
        /// <param name="verifycode">图片验证码</param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Register(string Account, string RealName, string Password, string verifycode)
        {
            verifycode = Md5Helper.MD5(verifycode.ToLower(), 16);
            if (Session["session_verifycode"].IsEmpty() || verifycode != Session["session_verifycode"].ToString())
            {
                return Error("验证码错误，请重新输入");
            }
            //验证账号是否存在
            if (_app.ExistAccount(Account))
            {
                return Error("该账号已存在");
            }
            UserEntity userEntity = new UserEntity();
            userEntity.Account = Account;
            userEntity.RealName = RealName;
            userEntity.Password = Password;
            userEntity.Secretkey = Md5Helper.MD5(CommonHelper.CreateNo(), 16).ToLower();
            userEntity.Password = Md5Helper.MD5(DESEncrypt.Encrypt(Md5Helper.MD5(userEntity.Password, 32).ToLower(), userEntity.Secretkey).ToLower(), 32).ToLower();
            _app.CreateForm(userEntity);
            return Success("注册成功。");
        }
        /// <summary>
        /// 登录限制
        /// </summary>
        /// <param name="account">账户</param>
        /// <param name="iPAddress">IP</param>
        /// <param name="iPAddressName">IP所在城市</param>
        public void LoginLimit(string account, string iPAddress, string iPAddressName)
        {
            if (account == "System")
            {
                return;
            }
            string platform = Net.Browser;
            //accountBLL.LoginLimit(platform, account, iPAddress, iPAddressName);
        }
        #endregion
    }
}
