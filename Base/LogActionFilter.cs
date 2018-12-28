using Microsoft.AspNetCore.Mvc.Filters;
using Newtonsoft.Json;
using System;

namespace Base
{
    public class LogActionFilter : IActionFilter
    {
        public LogActionFilter()
        {
        }
        public void OnActionExecuted(ActionExecutedContext context)
        {

        }

        public void OnActionExecuting(ActionExecutingContext context)
        {
            Console.WriteLine("+++++++++++++++++++++++++++++++++++++++++++++++++++++++");
            Console.WriteLine("+++++++++++++++++++++++++++++++++++++++++++++++++++++++");
            Console.WriteLine("+++++++++++++++++++++++++++++++++++++++++++++++++++++++");
            Console.WriteLine(context.ActionDescriptor.AttributeRouteInfo.Template);
            context.HttpContext.Items.Add("RouteTemplate", context.ActionDescriptor.AttributeRouteInfo.Template);
            Console.WriteLine(JsonConvert.SerializeObject(context.HttpContext.Items.Keys));
        }
    }
}
