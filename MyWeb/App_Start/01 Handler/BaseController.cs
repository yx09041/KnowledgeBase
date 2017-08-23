using System.Web.Mvc;
using Mysoft.Util;

namespace MyWeb
{
    /// <summary>
    /// 基础控制器，所有controller继承此类
    /// </summary>
    public abstract class BaseController : Controller
    {
  
        protected virtual ActionResult Success(string message)
        {
            return Content(new AjaxResult { state = ResultType.success.ToString(), message = message }.ToJson());
        }
        protected virtual ActionResult Success(string message, object data)
        {
            return Content(new AjaxResult { state = ResultType.success.ToString(), message = message, data = data }.ToJson());
        }
        protected virtual ActionResult Error(string message)
        {
            return Content(new AjaxResult { state = ResultType.error.ToString(), message = message }.ToJson());
        }

        protected virtual ActionResult Default(bool isSuccess, string message)
        {
            if (isSuccess)
            {
                return Content(new AjaxResult { state = ResultType.success.ToString(), message = message }.ToJson());
            }
            else
            {
                return Content(new AjaxResult { state = ResultType.error.ToString(), message = message }.ToJson());
            }
        }
    }
}
