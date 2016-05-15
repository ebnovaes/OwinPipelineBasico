using System;
using System.Collections.Generic;
using Microsoft.Owin.Hosting;
using Owin;
using System.Threading.Tasks;

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
            app.UseMyMiddleware("This is the new greeting for MyMiddleware!");
            app.UserMyOtherMiddleware();
        }
    }

    public static class AppBuildExtensions
    {
        public static void UseMyMiddleware(this IAppBuilder app, string greetingOption)
        {
            app.Use<MyMiddlewareComponent>(greetingOption);
        }

        public static void UserMyOtherMiddleware(this IAppBuilder app)
        {
            app.Use<MyOtherMiddlewareComponent>();
        }
    }

    public class MyMiddlewareComponent
    {
        AppFunc _next;
        string _greeting;

        public MyMiddlewareComponent(AppFunc next, string greeting)
        {
            _next = next;
            _greeting = greeting;
        }

        public async Task Invoke(IDictionary<string, object> environment)
        {

            IOwinContext context = new OwinContext(environment);
            await context.Response.WriteAsync(string.Format("<h1>{0}</h1>", _greeting));
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