using System;
using System.Collections.Generic;
using System.Threading.Tasks;
namespace MyMiddleware
{
    using Microsoft.Owin;
    using AppFunc = Func<IDictionary<string, object>, Task>;

    public class MyMiddleware
    {
        AppFunc _next;
        MyMiddlewareConfigOptions _configOptions;

        public MyMiddleware(AppFunc next, MyMiddlewareConfigOptions configOptions)
        {
            _next = next;
            _configOptions = configOptions;
        }

        public async Task Invoke(IDictionary<string, object> environment)
        {

            IOwinContext context = new OwinContext(environment);
            await context.Response.WriteAsync(string.Format("<h1>{0}</h1>", _configOptions.GetGreeting()));

            context.Response.StatusCode = 200;
            context.Response.ReasonPhrase = "OK";

        }
    }
}
