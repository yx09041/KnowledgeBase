using System.Web.Mvc;

namespace MyWeb.Areas.BaseManage
{
    public class BaseManageAreaRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get
            {
                return "BaseManage";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
            context.MapRoute(
                "BaseManage_default",
                "BaseManage/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}
