using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Mysoft.Util;
using MySoft.Application.Business;
using MySoft.Application.Entity;

namespace MyWeb.Areas.knowledge.Controllers
{
    public class KnowledgeController : Controller
    {
        knowledgeApp _app = new knowledgeApp();
        #region 视图
        public ActionResult Index()
        {
            return View();
        }
        #endregion


        /// <summary>
        /// 获取所有数据
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult GetListJson(Pagination pagination)
        {
            List<KnowledgeInfoEntity> data = _app.GetList(pagination);
            var JsonData = new
            {
                data = data,
                total = pagination.records,
                page = pagination.page,
            };
            return Content(JsonData.ToJson());
        }
    }
}
