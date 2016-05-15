using Owin;

namespace SillyAuthentication
{
    public static class AppBuilderExtensions
    {
        public static void UseSillyAuthentication(this IAppBuilder app)
        {
            app.Use<SillyAuthentication>();
        }
    }
}
