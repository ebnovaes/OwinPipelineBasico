using System;
using System.Collections.Generic;
using Microsoft.Owin.Hosting;
using Owin;
using System.Threading.Tasks;
using Microsoft.Owin;

namespace Katana
{
    using MyMiddleware;
    using SillyAuthentication;
    using SillyLogging;
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

}