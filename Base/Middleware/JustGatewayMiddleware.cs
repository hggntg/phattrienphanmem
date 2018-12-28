using Microsoft.AspNetCore.Http;
using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Base.Middleware
{
    class JustGatewayMiddleware
    {
        private readonly RequestDelegate Next;
        private readonly HeaderRequirement HeaderRequirementInstance;

        public JustGatewayMiddleware(RequestDelegate Next, HeaderRequirement HeaderRequirementInstance)
        {
            this.Next = Next;
            this.HeaderRequirementInstance = HeaderRequirementInstance;
        }

        public async Task Invoke(HttpContext Context)
        {
            var XOrigin = Context.Request.Headers.FirstOrDefault(h => h.Key == "X-Origin").Value.ToString();
            var XNote = Context.Request.Headers.FirstOrDefault(h => h.Key == "X-Note").Value.ToString();
            var XId = Context.Request.Headers.FirstOrDefault(h => h.Key == "X-Id").Value.ToString();
            var InputHeaderRequirement = new HeaderRequirementImp(XOrigin, XNote, XId);
            if (InputHeaderRequirement.IsValid(this.HeaderRequirementInstance))
            {
                await Next(Context);
            }
            else
            {
                Context.Response.StatusCode = 404;
                var ResponseBytes = Encoding.UTF8.GetBytes("");
                await Context.Response.Body.WriteAsync(ResponseBytes, 0, ResponseBytes.Length);
            }
        }
    }
}
