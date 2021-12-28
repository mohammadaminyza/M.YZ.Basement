using M.YZ.Basement.Core.ApplicationServices.Events;
using M.YZ.Basement.Utilities;
using Microsoft.AspNetCore.Http;

namespace M.YZ.Basement.EndPoints.Web.Extensions
{
    public static class HttpContextExtensions
    {
        public static ICommandDispatcher CommandDispatcher(this HttpContext httpContext) =>
            (ICommandDispatcher)httpContext.RequestServices.GetService(typeof(ICommandDispatcher));

        public static IQueryDispatcher QueryDispatcher(this HttpContext httpContext) =>
            (IQueryDispatcher)httpContext.RequestServices.GetService(typeof(IQueryDispatcher));
        public static IEventDispatcher EventDispatcher(this HttpContext httpContext) =>
            (IEventDispatcher)httpContext.RequestServices.GetService(typeof(IEventDispatcher));
        public static BasementServices BasementApplicationContext(this HttpContext httpContext) =>
            (BasementServices)httpContext.RequestServices.GetService(typeof(BasementServices));
    }
}
