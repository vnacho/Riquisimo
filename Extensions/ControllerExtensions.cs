using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewEngines;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Ferpuser
{
    public static class ControllerExtensions
    {
        public static void AddValidationErrors(this Controller controller, IEnumerable<ValidationResult> errors)
        {
            foreach (ValidationResult result in errors)
            {
                if (result.MemberNames.Any())
                    controller.ModelState.AddModelError(result.MemberNames.First(), result.ErrorMessage);
                else
                    controller.ModelState.AddModelError(string.Empty, result.ErrorMessage);
            }
        }

        public static async Task<string> RenderViewAsync<TModel>(this Controller controller, string viewName, TModel model, bool partial = false)
        {
            if (string.IsNullOrEmpty(viewName))
            {
                viewName = controller.ControllerContext.ActionDescriptor.ActionName;
            }

            controller.ViewData.Model = model;

            using (var writer = new StringWriter())
            {
                IViewEngine viewEngine = controller.HttpContext.RequestServices.GetService(typeof(ICompositeViewEngine)) as ICompositeViewEngine;
                ViewEngineResult viewResult = viewEngine.FindView(controller.ControllerContext, viewName, !partial);

                if (viewResult.Success == false)
                {
                    return $"A view with the name {viewName} could not be found";
                }

                ViewContext viewContext = new ViewContext(
                    controller.ControllerContext,
                    viewResult.View,
                    controller.ViewData,
                    controller.TempData,
                    writer,
                    new HtmlHelperOptions()
                );

                await viewResult.View.RenderAsync(viewContext);

                return writer.GetStringBuilder().ToString();
            }
        }

        public static async Task<string> RenderPartialViewToString(this Controller controller, string viewName, object model)
        {
            ICompositeViewEngine _viewEngine = (ICompositeViewEngine)controller.HttpContext.RequestServices.GetService(typeof(ICompositeViewEngine));
            if (string.IsNullOrEmpty(viewName))
                viewName = controller.ControllerContext.ActionDescriptor.ActionName;

            controller.ViewData.Model = model;

            using (var writer = new StringWriter())
            {
                ViewEngineResult viewResult =
                    _viewEngine.FindView(controller.ControllerContext, viewName, false);

                ViewContext viewContext = new ViewContext(
                    controller.ControllerContext,
                    viewResult.View,
                    controller.ViewData,
                    controller.TempData,
                    writer,
                    new HtmlHelperOptions()
                     );

                await viewResult.View.RenderAsync(viewContext);

                return writer.GetStringBuilder().ToString();
            }
        }
    }
}
