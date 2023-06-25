using DNTCaptcha.Core;
using Livemedy.Infrastructure;
using Microsoft.AspNetCore.Authentication.Cookies;

namespace Livemedy.Web
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllersWithViews().AddRazorRuntimeCompilation();

            services.AddDNTCaptcha(options =>
            {
                options.UseCookieStorageProvider().ShowThousandsSeparators(false);
                options.WithEncryptionKey("abdGHKKHGFGGH124578");
            });

            

            services.Configure<CookiePolicyOptions>(options =>
            {
                // This lambda determines whether user consent for non-essential cookies is needed for a given request.
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });
            services.AddMvc(options => options.EnableEndpointRouting = false);
            services.AddAuthentication(options =>
            {
                options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
            }).AddCookie(options =>
            {
                options.LoginPath = "/Login/";
                options.ExpireTimeSpan = DateTime.Now.AddDays(7).TimeOfDay;
                options.Cookie.MaxAge = options.ExpireTimeSpan;
                options.SlidingExpiration = true;
            });
            services.AddHttpContextAccessor();

            services.AddSession();

            services.AddInfrastructure(Configuration);
        }
        public void Configure(IApplicationBuilder app, IHostEnvironment env, IHttpContextAccessor httpAccessor)
        {
            //Api.SetHttpContextAccessor(httpAccessor);
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
            }
            app.UseAuthentication();
            app.UseStaticFiles();
            app.UseCookiePolicy();
            app.UseRouting();
            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
