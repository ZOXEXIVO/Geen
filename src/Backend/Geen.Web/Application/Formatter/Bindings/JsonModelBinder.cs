using System.Threading.Tasks;
using Geen.Web.Application.Services.Json;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace Geen.Web.Application.Formatter.Bindings
{
    public class JsonModelBinder : IModelBinder
    {
        public Task BindModelAsync(ModelBindingContext bindingContext)
        {
            var modelStringValue = bindingContext.HttpContext.Request.Query[bindingContext.ModelName];

            if(string.IsNullOrWhiteSpace(modelStringValue))
                return Task.CompletedTask;

            var result = ((string) modelStringValue).FromJson(bindingContext.ModelType);

            bindingContext.Result = ModelBindingResult.Success(result);

            return Task.CompletedTask;
        }
    }
}
