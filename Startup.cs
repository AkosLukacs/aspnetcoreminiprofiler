using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using authsample.Data;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace authsample
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMiniProfiler(options =>
            {
                // All of this is optional. You can simply call .AddMiniProfiler() for all defaults

                // // (Optional) Path to use for profiler URLs, default is /mini-profiler-resources
                // options.RouteBasePath = "/profiler";

                // // (Optional) Control storage
                // // (default is 30 minutes in MemoryCacheStorage)
                // (options.Storage as MemoryCacheStorage).CacheDuration = TimeSpan.FromMinutes(60);

                // // (Optional) Control which SQL formatter to use, InlineFormatter is the default
                // options.SqlFormatter = new StackExchange.Profiling.SqlFormatters.InlineFormatter();

                // // (Optional) To control authorization, you can use the Func<HttpRequest, bool> options:
                // // (default is everyone can access profilers)
                // options.ResultsAuthorize = request => MyGetUserFunction(request).CanSeeMiniProfiler;
                // options.ResultsListAuthorize = request => MyGetUserFunction(request).CanSeeMiniProfiler;

                // // (Optional)  To control which requests are profiled, use the Func<HttpRequest, bool> option:
                // // (default is everything should be profiled)
                // options.ShouldProfile = request => MyShouldThisBeProfiledFunction(request);

                // // (Optional) Profiles are stored under a user ID, function to get it:
                // // (default is null, since above methods don't use it by default)
                // options.UserIdProvider =  request => MyGetUserIdFunction(request);

                // // (Optional) Swap out the entire profiler provider, if you want
                // // (default handles async and works fine for almost all appliations)
                // options.ProfilerProvider = new MyProfilerProvider();

                // (Optional) You can disable "Connection Open()", "Connection Close()" (and async variant) tracking.
                // (defaults to true, and connection opening/closing is tracked)
                options.TrackConnectionOpenClose = true;
            })
            .AddEntityFramework();


            services.Configure<CookiePolicyOptions>(options =>
            {
                // This lambda determines whether user consent for non-essential cookies is needed for a given request.
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });

            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));

            services.AddDefaultIdentity<IdentityUser>()
                .AddEntityFrameworkStores<ApplicationDbContext>();

            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            app.UseMiniProfiler();
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseDatabaseErrorPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseCookiePolicy();

            app.UseAuthentication();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
