using Connect4.Frontend.Components;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Rewrite;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Identity.Web;
using Microsoft.Identity.Web.UI;

namespace Connect4.Frontend
{
    public static class FrontendExtensions
    {
        public static IServiceCollection AddFrontend(this IServiceCollection services, IConfigurationSection azureAdConfiguration)
        {
            services.AddAuthentication(OpenIdConnectDefaults.AuthenticationScheme).AddMicrosoftIdentityWebApp(azureAdConfiguration);
            services.AddRazorPages().AddMicrosoftIdentityUI();
            services.AddAuthorization(options =>
            {
                // By default, all incoming requests will be authorized according to the default policy
                options.FallbackPolicy = options.DefaultPolicy;
            });
            services.AddCascadingAuthenticationState();

            services.AddRazorComponents()
                .AddInteractiveServerComponents()
                .AddMicrosoftIdentityConsentHandler();

            return services;
        }

        public static WebApplication UseFrontend(this WebApplication app)
        {
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Error", createScopeForErrors: true);
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseForwardedHeaders();

            app.UseStaticFiles();
            app.UseAuthentication();
            app.UseAuthorization();

            app.UseAntiforgery();

            app.UseRewriter(
                new RewriteOptions().Add(
                    context =>
                    {
                        if (context.HttpContext.Request.Path == "/MicrosoftIdentity/Account/SignOut")
                        {
                            context.HttpContext.Response.Redirect("/");
                        }
                    }
                )
            );

            app.MapRazorComponents<App>()
                .AddInteractiveServerRenderMode();
            app.MapControllers();

            return app;
        }
    }
}
