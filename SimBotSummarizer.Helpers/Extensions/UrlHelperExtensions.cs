
using Microsoft.AspNetCore.Mvc;

namespace SimBotSummarizer.Helpers.Extensions
{
    public static class UrlHelperExtensions
    {
        public static string Action(this IUrlHelper helper, string action, string controller)
        {
            var routeValues = helper.ActionContext.HttpContext.Request.QueryString;

            return helper.Action(action, controller, routeValues);
        }
    }
}
