﻿using System.Web.Mvc;

namespace MyWeb.Areas.SiteManage
{
    public class SiteManageAreaRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get
            {
                return "SiteManage";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
            context.MapRoute(
             this.AreaName + "_Default",
             this.AreaName + "/{controller}/{action}/{id}",
             new { area = this.AreaName, controller = "SiteManage", action = "Index", id = UrlParameter.Optional },
             new string[] { "MyWeb.Areas." + this.AreaName + ".Controllers" }
           );
        }
    }
}
