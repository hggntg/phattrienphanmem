using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System;
using System.Text;
using System.Threading.Tasks;

namespace Base.Middleware
{
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate Next;

        public ExceptionMiddleware(RequestDelegate Next)
        {
            this.Next = Next;
        }

        public async Task Invoke(HttpContext Context)
        {
            try
            {
                await this.Next(Context);
            }
            catch(Exception ex)
            {
                Context.Response.StatusCode = 404;
                var ResponseBytes = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(ex));
                await Context.Response.Body.WriteAsync(ResponseBytes, 0, ResponseBytes.Length);
            }
        }
    }
}
