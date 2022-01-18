using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Abstractions;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewEngines;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Routing;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace ArgedeSP.WebUI.Helpers
{
    public class RazorPartialToStringRenderer : IRazorPartialToStringRenderer
    {
        private readonly IHttpContextAccessor _accessor;
        private IRazorViewEngine _viewEngine;
        private ITempDataProvider _tempDataProvider;
        private IServiceProvider _serviceProvider;
        private IActionContextAccessor _actionContextAccessor;
        public RazorPartialToStringRenderer(
            IRazorViewEngine viewEngine,
            ITempDataProvider tempDataProvider,
            IServiceProvider serviceProvider,
            IHttpContextAccessor accessor,
            IActionContextAccessor actionContextAccessor)
        {
            _viewEngine = viewEngine;
            _tempDataProvider = tempDataProvider;
            _serviceProvider = serviceProvider;
            _accessor = accessor;
            _actionContextAccessor = actionContextAccessor;
        }
        public async Task<string> RenderPartialToStringAsync<TModel>(string partialName, TModel model)
        {
            try
            {
                var actionContext = GetActionContext();
                var partial = FindView(actionContext, partialName);
                using (var output = new StringWriter())
                {
                    var viewContext = new ViewContext(
                        actionContext,
                        partial,
                        new ViewDataDictionary<TModel>(
                            metadataProvider: new EmptyModelMetadataProvider(),
                            modelState: _actionContextAccessor.ActionContext.ModelState)
                        {
                          Model=model
                        },
                        new TempDataDictionary(
                            actionContext.HttpContext,
                            _tempDataProvider),
                        output,
                        new HtmlHelperOptions()
                    );
                    viewContext.RouteData = _accessor.HttpContext.GetRouteData();


                    await partial.RenderAsync(viewContext);
                    return output.ToString();
                }
            }
            catch (Exception ex)
            {

                throw;
            }

        }
        private IView FindView(ActionContext actionContext, string partialName)
        {
            var getPartialResult = _viewEngine.GetView(null, partialName, false);
            if (getPartialResult.Success)
            {
                return getPartialResult.View;
            }
            var findPartialResult = _viewEngine.FindView(actionContext, partialName, false);
            if (findPartialResult.Success)
            {
                return findPartialResult.View;
            }
            var searchedLocations = getPartialResult.SearchedLocations.Concat(findPartialResult.SearchedLocations);
            var errorMessage = string.Join(
                Environment.NewLine,
                new[] { $"Unable to find partial '{partialName}'. The following locations were searched:" }.Concat(searchedLocations)); ;
            throw new InvalidOperationException(errorMessage);
        }
        private ActionContext GetActionContext()
        {
            HttpContext httpContext = _accessor.HttpContext;
            httpContext.RequestServices = _serviceProvider;

            return new ActionContext(httpContext, new RouteData(), new ActionDescriptor());
        }
    }
    public interface IRazorPartialToStringRenderer
    {
        Task<string> RenderPartialToStringAsync<TModel>(string partialName, TModel model);
    }
}
