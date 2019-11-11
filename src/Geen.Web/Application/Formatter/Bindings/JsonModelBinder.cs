using System.Collections.Concurrent;
using System.Threading.Tasks;
using Geen.Web.Application.Services.Json;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace Geen.Web.Application.Formatter.Bindings
{
    public class JsonModelBinder : IModelBinder
    {
        private static readonly ConcurrentDictionary<string, object> ModelCache 
            = new ConcurrentDictionary<string, object>();

        public Task BindModelAsync(ModelBindingContext bindingContext)
        {
            var modelStringValue = bindingContext.HttpContext.Request.Query[bindingContext.ModelName];

            if(string.IsNullOrWhiteSpace(modelStringValue))
                return Task.CompletedTask;

            var cacheKey = GetCacheKey(bindingContext, modelStringValue);
            
            var result = ModelCache.GetOrAdd(cacheKey, 
                key => ((string)modelStringValue).FromJson(bindingContext.ModelType));

            bindingContext.Result = ModelBindingResult.Success(result);

            return Task.CompletedTask;
        }

        private string GetCacheKey(ModelBindingContext context, string value)
        {
            return context.ModelType.Name + "_" + value;
        }
    }
}
