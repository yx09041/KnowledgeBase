using System.Web.Mvc;

namespace MyWeb.Areas.knowledge
{
    public class knowledgeAreaRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get
            {
                return "knowledge";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
            context.MapRoute(
             this.AreaName + "_Default",
             this.AreaName + "/{controller}/{action}/{id}",
             new { area = this.AreaName, controller = "knowledge", action = "Index", id = UrlParameter.Optional },
             new string[] { "MyWeb.Areas." + this.AreaName + ".Controllers" }
            );
        }
    }
}
