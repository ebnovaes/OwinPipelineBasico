using Owin;

namespace SillyLogging
{
    public static class AppBuilderExtensions
    {
        public static void UseSillyLogging(this IAppBuilder app)
        {
            app.Use<SillyLogging>();
        }
    }
}
