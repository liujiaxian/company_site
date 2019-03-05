using System.Web.Mvc;

namespace WebApp.Areas.GuanWang
{
    public class GuanWangAreaRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get
            {
                return "GuanWang";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
            context.MapRoute(
                "GuanWang_default",
                "GuanWang/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}
