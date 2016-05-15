using System;
using System.Collections.Generic;
using Microsoft.Owin.Hosting;
using Owin;
using System.Threading.Tasks;
using System.IO;

namespace Katana
{
    using Microsoft.Owin;
    using AppFunc = Func<IDictionary<string, object>, Task>;
    class Program
    {
        static void Main(string[] args)
        {
            WebApp.Start<Startup>("http://localhost:85");
            Console.WriteLine("Server Started; Press enter to Quit");
            Console.ReadLine();
        }
    }

    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            app.Use<MyMiddlewareComponent>();
            app.Use<MyOtherMiddlewareComponent>();
        }
    }

    public class MyMiddlewareComponent
    {
        AppFunc _next;

        public MyMiddlewareComponent(AppFunc next)
        {
            _next = next;
        }

        public async Task Invoke(IDictionary<string, object> environment)
        {

            IOwinContext context = new OwinContext(environment);
            await context.Response.WriteAsync("<h1>Hello from my First Middleware</h1>");
            await _next.Invoke(environment);
        }

    }

    public class MyOtherMiddlewareComponent
    {
        AppFunc _next;

        public MyOtherMiddlewareComponent(AppFunc next)
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