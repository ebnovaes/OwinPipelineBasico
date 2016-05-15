using System;
using System.Collections.Generic;
using Microsoft.Owin.Hosting;
using Owin;
using System.Threading.Tasks;
using System.IO;

namespace Katana
{
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
            var middleware = new Func<AppFunc, AppFunc>(MyMiddleware);
            app.Use(middleware);
        }

        public AppFunc MyMiddleware(AppFunc next)
        {
            AppFunc appFunc = async (IDictionary<string, object> environment) =>
            {
                Stream response = environment["owin.ResponseBody"] as Stream;
                using (StreamWriter writer = new StreamWriter(response))
                {
                    await writer.WriteAsync("<h1>Hello from my First Middleware</h1>");
                }

                await next.Invoke(environment);
            };
            return appFunc;
        }
    }
}
