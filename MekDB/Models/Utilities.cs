using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace MekDB.Models
{
    public static class Utilities
    {
        //used to mark a tab in the websides main nav bar as active
        public static string IsActive(this HtmlHelper html, string control, string action)
        {
            //out current location on the site
            RouteData routeData = html.ViewContext.RouteData;

            //get the current method in controller
            string routeAction = (string)routeData.Values["action"];

            //get the current controller
            string routeControl = (string)routeData.Values["controller"];

            // match agenst input
            bool returnActive = control == routeControl &&
                               action == routeAction;

            return returnActive ? "active" : "";

            //if (control == routeControl && action == routeAction)
            //{
            //    return "active";
            //}
            //else
            //{
            //    return "";
            //}
        }
    }
}