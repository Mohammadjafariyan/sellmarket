using System.Web.Mvc;

namespace sellmarket
{
    public class CentralAreaRegistration : AreaRegistration 
    {
        public override string AreaName 
        {
            get 
            {
                return "Central";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context) 
        {
            context.MapRoute(
                "Central_default",
                "Central/{controller}/{action}/{id}",
                new { action = "Index",controller= "HelloCentral", id = UrlParameter.Optional },
           namespaces: new string[] { "Central.Controllers" }

            );
        }
    }
}