using Microsoft.AspNetCore.Mvc.Rendering;

namespace TaskManagementPortal
{
    public static class IsSelected
    {

        /// <summary>
        /// highlights menu link
        /// </summary>
        /// <param name="html"></param>
        /// <param name="control">pass controller name</param>
        /// <param name="action">pass action name</param>
        /// <returns></returns>
        public static string IsActive(this IHtmlHelper html, string control, string action)
        {
            var routeData = html.ViewContext.RouteData;

            var routeAction = (string)routeData.Values["action"];
            var routeControl = (string)routeData.Values["controller"];

            // both must match
            var returnActive = control == routeControl &&
                               action == routeAction;

            return returnActive ? "active" : "";
        }
        /// <summary>
        ///  highlights menu header link
        /// </summary>
        /// <param name="html"></param>
        /// <param name="control">pass controller name</param>
        /// <returns></returns>
        public static string IsActiveLi(this IHtmlHelper html, string control)
        {
            var routeData = html.ViewContext.RouteData;

            var routeControl = (string)routeData.Values["controller"];

            // only controller match
            var returnActive = control == routeControl;

            return returnActive ? "menu-is-opening menu-open" : "";
        }
        //use for activating opened tab
        public static string IsActiveTab(this IHtmlHelper html, string control)
        {
            var routeData = html.ViewContext.RouteData;

            var routeControl = (string)routeData.Values["controller"];

            // only controller match
            var returnActive = control == routeControl;

            return returnActive ? "active" : "";
        }

    }
}
