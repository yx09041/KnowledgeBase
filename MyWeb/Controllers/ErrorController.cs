using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MyWeb
{
    /// <summary>
    /// 版 本 1.2
    /// Copyright (c) 2013-2017 
    /// 创建人：yxtic
    /// 日 期：2017-05-15 09:25
    /// 描 述：错误处理
    /// </summary>
    public class ErrorController : Controller
    {
        #region 视图功能
        /// <summary>
        /// 错误页面（异常页面）
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        public ActionResult ErrorMessage(string message)
        {
            //Dictionary<string, string> modulesError = (Dictionary<string, string>)HttpContext.Application["error"];
            ViewBag.Message = message;
            return View();
        }
        /// <summary>
        /// 错误页面（错误路径404）
        /// </summary>
        /// <returns></returns>
        public ActionResult ErrorPath404()
        {
            return View();
        }
        /// <summary>
        /// 错误页面（升级浏览器软件）
        /// </summary>
        /// <returns></returns>
        public ActionResult ErrorBrowser()
        {
            return View();
        }
        #endregion
    }
}
