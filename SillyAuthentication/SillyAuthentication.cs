using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Owin;

namespace SillyAuthentication
{
    using AppFunc = Func<IDictionary<string, object>, Task>;

    public class SillyAuthentication
    {
        AppFunc _next;

        public SillyAuthentication(AppFunc next)
        {
            _next = next;
        }

        public async Task Invoke(IDictionary<string, object> environment)
        {
            IOwinContext context = new OwinContext(environment);

            bool isAuthorized = context.Request.QueryString.Value == "John";
            if (!isAuthorized)
            {
                context.Response.StatusCode = 401;
                context.Response.ReasonPhrase = "Not Authorized!";

                await context.Response.WriteAsync(string.Format("<h1>Error {0}-{1}",
                                                  context.Response.StatusCode,
                                                  context.Response.ReasonPhrase)
                                                 );
            }
            else
            {
                await _next.Invoke(environment);
            }

        }
    }
}
