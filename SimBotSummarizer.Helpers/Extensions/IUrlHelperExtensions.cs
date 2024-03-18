using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using System.Collections.Specialized;
using System.Text;


namespace SimBotSummarizer.Helpers.Extensions
{
    public static class IUrlHelperExtensions
    {
        /// <summary>
        /// Generates a fully qualified sort URL by adding sortyn params to the current page url.
        /// </summary>
        /// <param name="helper"></param>
        /// <param name="sortBy"></param>
        /// <param name="sortDesc"></param>
        /// <param name="sortByParamName"></param>
        /// <param name="sortDescParamName"></param>
        /// <param name="preserveQueryStringValues"></param>
        /// <returns></returns>
        public static string SortUrl(this IUrlHelper helper, string sortBy, bool? sortDesc = null, string sortByParamName = "sortBy", string sortDescParamName = "sortDesc", bool preserveQueryStringValues = true)
        {
            var queryString = preserveQueryStringValues ? helper.ActionContext.HttpContext.Request.Query.ToNameValueCollection() : new NameValueCollection();
            string currSortBy = queryString[sortByParamName];
            string currSortDesc = queryString[sortDescParamName];

            if (!sortDesc.HasValue)
            {
                sortDesc = sortBy == currSortBy && (currSortDesc == null || currSortDesc.ToLower() != "true");
            }

            queryString.AddOrSet(sortByParamName, sortBy, ignoreCase: true);
            queryString.RemoveIfExists(sortDescParamName, ignoreCase: true);

            if (sortDesc.Value)
            {
                queryString.AddOrSet(sortDescParamName, sortDesc.Value.ToString().ToLower(), ignoreCase: true);
            }

            return helper.Action(null, null, queryString: queryString);
        }

        /// <summary>
        /// Generates a fully qualified URL to an action method by using the specified action name, controller name, and queryString values.
        /// </summary>
        /// <param name="helper"></param>
        /// <param name="actionName">The name of the action method.</param>
        /// <param name="queryString">The QueryString params</param>
        /// <returns></returns>
        public static string Action(this IUrlHelper helper, string actionName, NameValueCollection queryString)
        {
            return helper.Action(actionName, null, queryString: queryString);
        }

        /// <summary>
        /// Generates a fully qualified URL to an action method by using the specified action name, controller name, and queryString values.
        /// </summary>
        /// <param name="helper"></param>
        /// <param name="actionName">The name of the action method.</param>
        /// <param name="controllerName">The name of the controller.</param>
        /// <param name="queryString">The QueryString params</param>
        /// <returns></returns>
        public static string Action(this IUrlHelper helper, string actionName, string controllerName, NameValueCollection queryString)
        {
            return helper.Action(actionName, controllerName) + queryString.ToQueryString();
        }

        ///// <summary>
        ///// Generates a fully qualified URL to an action method by using the specified action name, controller name, and queryString values.
        ///// </summary>
        ///// <param name="helper"></param>
        ///// <param name="actionName">The name of the action method.</param>
        ///// <param name="controllerName">The name of the controller.</param>
        ///// <param name="queryString">The QueryString params</param>
        ///// <returns></returns>
        //public static string Action(this IUrlHelper helper, string actionName, string controllerName, QueryString? queryString)
        //{
        //    return helper.Action(actionName, controllerName) + queryString?.ToString();
        //}

        ///// <summary>
        ///// Generates a fully qualified URL to an action method by using the specified action name and preserving queryString values.
        ///// </summary>
        ///// <param name="helper"></param>
        ///// <param name="actionName">The name of the action method.</param>        
        ///// <param name="preserveQueryStringValues">Should queryString values be preserved</param>
        ///// <returns></returns>
        //public static string Action(this IUrlHelper helper, string actionName, bool preserveQueryStringValues = false)
        //{
        //    return helper.Action(actionName, null, preserveQueryStringValues: preserveQueryStringValues);
        //}


        ///// <summary>
        ///// Generates a fully qualified URL to an action method by using the specified action name, controller name and preserving queryString values.
        ///// </summary>
        ///// <param name="helper"></param>
        ///// <param name="actionName">The name of the action method.</param>
        ///// <param name="controllerName">The name of the controller.</param>
        ///// <param name="preserveQueryStringValues">Should queryString values be preserved</param>
        ///// <returns></returns>
        //public static string Action(this IUrlHelper helper, string actionName, string controllerName, bool preserveQueryStringValues = false)
        //{
        //    //var queryString = preserveQueryStringValues ? helper.ActionContext?.HttpContext?.Request?.QueryString : null;
        //    var queryString = preserveQueryStringValues ? helper.ActionContext.HttpContext.Request.Query.ToNameValueCollection() : new NameValueCollection();

        //    return helper.Action(actionName, controllerName, queryString: queryString);
        //}

        /// <summary>
        /// Generates a fully qualified URL to an action method by using the specified action name, controller name and preserving queryString values.
        /// </summary>
        /// <param name="helper"></param>
        /// <param name="actionName">The name of the action method.</param>
        /// <param name="preserveQueryStringValues">Should queryString values be preserved</param>
        /// <returns></returns>
        public static string Action(this IUrlHelper helper, string actionName, bool preserveQueryStringValues)
        {
            return helper.Action(actionName, null, preserveQueryStringValues);
        }

        /// <summary>
        /// Generates a fully qualified URL to an action method by using the specified action name, controller name and preserving queryString values.
        /// </summary>
        /// <param name="helper"></param>
        /// <param name="actionName">The name of the action method.</param>
        /// <param name="controllerName">The name of the controller.</param>
        /// <param name="preserveQueryStringValues">Should queryString values be preserved</param>
        /// <returns></returns>
        public static string Action(this IUrlHelper helper, string actionName, string controllerName, bool preserveQueryStringValues)
        {
            var queryString = preserveQueryStringValues ? helper.ActionContext?.HttpContext?.Request?.QueryString : null;

            return helper.Action(actionName, controllerName) + queryString?.ToString();
        }

        /// <summary>
        /// Generates a fully qualified URL to an action method by using the specified action name, controller name, and route values.
        /// </summary>
        /// <param name="helper"></param>
        /// <param name="actionName">The name of the action method.</param>
        /// <param name="controllerName">The name of the controller.</param>
        /// <param name="routeValues">An object that contains the parameters for a route. The parameters are retrieved through reflection by examining the properties of the object. The object is typically created by using object initializer syntax.</param>
        /// <param name="routeArrayValues">A dictionary that contains parameters containing arrays for a route.</param>
        /// <param name="addIndices">If true add the array params in format id[0]=5&id[1]=20, otherwise use format id=5&id=20.</param>
        /// <returns>The fully qualified URL to an action method.</returns>
        public static string Action<T>(this IUrlHelper helper, string actionName, string controllerName, object routeValues, Dictionary<string, IEnumerable<T>> routeArrayValues, bool addIndices = false)
        {
            // add the array params in format id[0]=5&id[1]=20
            if (addIndices)
            {
                var dict = new RouteValueDictionary(routeValues);

                if (routeArrayValues != null)
                {
                    foreach (var key in routeArrayValues.Keys)
                    {
                        var values = routeArrayValues[key];

                        if (!values.HasItems()) { continue; }

                        for (int i = 0; i < values.Count(); i++)
                        {
                            dict.Add($"{key}[{i}]", values.ElementAt(i));
                        }
                    }
                }

                return helper.Action(actionName, controllerName, dict);
            }

            // add the array params in format id=5&id=20
            var parameters = new StringBuilder();

            if (routeArrayValues != null)
            {
                foreach (var key in routeArrayValues.Keys)
                {
                    var values = routeArrayValues[key];

                    if (!values.HasItems()) { continue; }

                    if (parameters.Length > 0) { parameters.Append("&"); }

                    parameters.Append(values.ToUrlParams(key));
                }
            }

            var url = helper.Action(actionName, controllerName, routeValues);

            return url + (url.IndexOf("?") <= -1 ? "?" : "&") + parameters.ToString();
        }


        /// <summary>
        /// Returns current controller name
        /// </summary>
        /// <param name="helper"></param>
        /// <returns></returns>
        public static string CurrentController(this IUrlHelper helper)
        {
            var routeData = helper.ActionContext.RouteData;

            return (routeData.Values.ContainsKey("controller") ? routeData.Values["controller"].ToString() : null);
        }

        /// <summary>
        /// Returns current action name
        /// </summary>
        /// <param name="helper"></param>        
        /// <returns></returns>
        public static string CurrentAction(this IUrlHelper helper)
        {
            var routeData = helper.ActionContext.RouteData;

            return (routeData.Values.ContainsKey("action") ? routeData.Values["action"].ToString() : null);
        }

        /// <summary>
        /// Returns website base url
        /// </summary>
        /// <param name="helper"></param>        
        /// <returns></returns>
        public static string BaseUrl(this IUrlHelper helper, bool addTrailingSlash = true)
        {
            var request = helper.ActionContext.HttpContext.Request;
            var host = request.Host.ToUriComponent();
            var pathBase = request.PathBase.ToUriComponent();
            var url = $"{request.Scheme}://{host}{pathBase}";

            if (addTrailingSlash && !url.EndsWith("/")) { url += "/"; }

            return url;
        }
    }
}
