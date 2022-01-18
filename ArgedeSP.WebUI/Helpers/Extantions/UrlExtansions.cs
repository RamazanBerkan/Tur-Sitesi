using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ArgedeSP.WebUI.Helpers.Extantions
{
    public static class UrlExtansions
    {
        public static string GetUrlFromAbsolutePath(string absolutePath)
        {
            return absolutePath.Replace(Startup.wwwRootFolder, "").Replace(@"\", "/");
        }

        public static string GetBaseUrl(HttpContext currentContext)
        {
            var request = currentContext.Request;

            var host = request.Host.ToUriComponent();

            var pathBase = request.PathBase.ToUriComponent();

            return $"{request.Scheme}://{host}{pathBase}";
        }

    }
}
