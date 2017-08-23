using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MyWeb.Areas.SiteManage.Controllers
{
    public class SiteManageController : BaseController
    {
        public ActionResult Index()
        {
            return View();
        }

    }
}
