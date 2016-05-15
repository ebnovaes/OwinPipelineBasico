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
            app.UseSillyLogging();
            app.UseSillyAuthentication();

            MyMiddlewareConfigOptions options = new MyMiddlewareConfigOptions("Greetings!", "John");
            options.IncludeDate = true;
            app.UseMyMiddleware(options);            
        }
    }

    public static class AppBuildExtensions
    {
        public static void UseMyMiddleware(this IAppBuilder app, MyMiddlewareConfigOptions configOptions)
        {
            app.Use<MyMiddlewareComponent>(configOptions);
        }

        public static void UserMyOtherMiddleware(this IAppBuilder app)
        {
            app.Use<MyOtherMiddlewareComponent>();
        }

        public static void UseSillyAuthentication(this IAppBuilder app)
        {
            app.Use<SillyAuthenticationComponent>();
        }

        public static void UseSillyLogging(this IAppBuilder app)
        {
            app.Use<SillyLoggingComponent>();
        }
    }

    public class MyMiddlewareConfigOptions {
        string _greetingTextFormat = "{0} from {1}{2}";

        public MyMiddlewareConfigOptions(string greeting, string greeter)
        {
            GreetingText = greeting;
            Greeter = greeter;
            Date = DateTime.Now;
        }

        public string GreetingText { get; set; }
        public string Greeter { get; set; }
        public DateTime Date { get; set; }

        public bool IncludeDate { get; set; }

        public string GetGreeting()
        {
            string DateText = "";
            if (IncludeDate)
            {
                DateText = string.Format(" on {0}", Date.ToShortDateString());
            }
            return string.Format(_greetingTextFormat, GreetingText, Greeter, DateText);
        }
    }


    public class MyMiddlewareComponent
    {
        AppFunc _next;
        MyMiddlewareConfigOptions _configOptions;

        public MyMiddlewareComponent(AppFunc next, MyMiddlewareConfigOptions configOptions)
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