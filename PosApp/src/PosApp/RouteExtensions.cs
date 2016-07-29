using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Routing;

namespace PosApp
{
    static class RouteExtensions
    {
        public static void MapRoute(
            this HttpRouteCollection routes,
            string template,
            string controller,
            string action,
            HttpMethod method)
        {
            routes.MapHttpRoute(
                $"{controller} {action}",
                template,
                new
                {
                    controller,
                    action
                },
                new
                {
                    httpMethod = new HttpMethodConstraint(method)
                });
        }
    }
}