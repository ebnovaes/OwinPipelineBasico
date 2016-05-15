using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Owin;

namespace MyMiddleware
{
    using AppFunc = Func<IDictionary<string, object>, Task>;

    public class MyOtherMiddleware
    {
        AppFunc _next;

        public MyOtherMiddleware(AppFunc next)
        {
            _next = next;
        }

        public async Task Invoke(IDictionary<string, object> environment)
        {
            IOwinContext context = new OwinContext(environment);
            await context.Response.WriteAsync("<h1>Hello from my second middleware</h1>");
            await _next.Invoke(environment);
        }
    }
}
