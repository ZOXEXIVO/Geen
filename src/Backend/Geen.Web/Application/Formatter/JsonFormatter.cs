using System.IO;
using System.Text;
using System.Threading.Tasks;
using Geen.Web.Application.Services.Json;
using Microsoft.AspNetCore.Mvc.Formatters;

namespace Geen.Web.Application.Formatter
{
    public class JsonFormatter : IOutputFormatter, IInputFormatter
    {
        public bool CanRead(InputFormatterContext context)
        {
            return true;
        }

        public async Task<InputFormatterResult> ReadAsync(InputFormatterContext context)
        {
            using var requestStream = new StreamReader(context.HttpContext.Request.Body, Encoding.UTF8);
            
            var stringData = await requestStream.ReadToEndAsync();
            
            return await InputFormatterResult.SuccessAsync(stringData.FromJson(context.ModelType));
        }

        public bool CanWriteResult(OutputFormatterCanWriteContext context)
        {
            return true;
        }

        public async Task WriteAsync(OutputFormatterWriteContext context)
        {
            context.HttpContext.Response.ContentType = "application/json";

            await using var responseStream = new StreamWriter(context.HttpContext.Response.Body, Encoding.UTF8);
            
            await responseStream.WriteAsync(
                context.Object.ToJson()
            );
        }
    }
}
