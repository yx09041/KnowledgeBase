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
                "knowledge_default",
                "knowledge/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}
