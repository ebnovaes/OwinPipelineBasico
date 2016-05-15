using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Katana
{
    using Microsoft.Owin;
    using AppFunc = Func<IDictionary<string, object>, Task>;
    public class SillyLoggingComponent
    {
        AppFunc _next;

        public SillyLoggingComponent(AppFunc next)
        {
            _next = next;
        }

        public async Task Invoke(IDictionary<string, object> environment)
        {
            await _next.Invoke(environment);

            IOwinContext context = new OwinContext(environment);
            Console.WriteLine("URI: {0} Status Code: {1}", 
                               context.Request.Uri, context.Response.StatusCode);
        }
    }
}
