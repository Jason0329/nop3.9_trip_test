using System.Web.Mvc;
using System.Web.Routing;
using Nop.Web.Framework.Mvc.Routes;

namespace Nop.Plugin.Payments.ECPay
{
    public partial class RouteProvider : IRouteProvider
    {
        public void RegisterRoutes(RouteCollection routes)
        {
            
          
            //Cancel
            routes.MapRoute("Plugin.Payments.ECPay.CancelOrder",
                 "Plugins/PaymentECPay/CancelOrder",
                 new { controller = "PaymentECPay", action = "CancelOrder" },
                 new[] { "Nop.Plugin.Payments.PayECPay.Controllers" }
            );

            routes.MapRoute("Plugin.Payments.ECPay.RedirectToECPay",
                "ECPay/RedirectToECPay/{Id}",
                new { controller = "PaymentECPay", action = "RedirectToECPay" },
                new[] { "Nop.Plugin.Payments.PayECPay.Controllers" }
           );
        }
        public int Priority
        {
            get
            {
                return 0;
            }
        }
    }
}
