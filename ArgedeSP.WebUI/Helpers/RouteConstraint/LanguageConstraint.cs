using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ArgedeSP.WebUI.Helpers.RouteConstraint
{
    public class LanguageConstraint : IRouteConstraint
    {
        private string _language;

        public LanguageConstraint(string language)
        {
            _language = language;
        }

        public bool Match(HttpContext httpContext, IRouter route, string routeKey, RouteValueDictionary values, RouteDirection routeDirection)
        {

            if (values["culture"].ToString() == _language)
            {
                return true;
            }

            return false;
        }
    }
}
