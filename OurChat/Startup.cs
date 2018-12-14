using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Net.Http.Headers;
using OurChat.Entities;
using OurChat.Hubs;
using OurChat.Repositories;
using OurChat.Repositories.Interfaces;
using OurChat.Services;
using OurChat.Services.Interfaces;

namespace OurChat
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

            services.AddSession(options =>
            {
                options.IdleTimeout = TimeSpan.FromMinutes(26);//You can set Time
            });

            // Dependency Injection 
            services.AddSingleton(Configuration);
            services.AddTransient<IMemberRepository, MemberRepository>();
            services.AddTransient<IMemberService, MemberService>();
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

            services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                         .AddCookie(options =>
                         {
                             options.Cookie.Expiration = TimeSpan.FromDays(1);
                             options.ExpireTimeSpan = TimeSpan.FromDays(1);
                             options.LoginPath = "/Auth/Login";
                             options.LogoutPath = "/Auth/LogOff";
                             options.AccessDeniedPath = "/Auth/AccessDenied";
                         });



            services.AddMvc();

            services.AddAuthorization(options =>
            {
                options.AddPolicy("AdminOnly", policy => policy.RequireClaim("IsAdministrator"));
            });
            //services.AddAntiforgery(opts => opts.Cookie.Name = "jagbiscuit");

            string connection = Configuration.GetConnectionString("OurChatConnection");
            services.AddDbContext<OurChatContext>
                (options => options.UseSqlServer(connection));

            //services.Configure<CookiePolicyOptions>(options =>
            //    {
            //        // This lambda determines whether user consent for non-essential cookies is needed for a given request.
            //        options.CheckConsentNeeded = context => true;
            //        options.MinimumSameSitePolicy = SameSiteMode.None;
            //    });


            //   // services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);
            services.AddSignalR();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, OurChatContext db)
        {
            app.Use((context, next) =>
            {
                context.Response.Headers.Remove("Server");
                context.Response.Headers.Remove("X-Powered-By");
                return next();
            });

            app.UseAuthentication();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }

            app.UseStaticFiles(new StaticFileOptions
            {
                OnPrepareResponse = ctx =>
                {
                    const int durationInSeconds = 60 * 60 * 24;
                    ctx.Context.Response.Headers[HeaderNames.CacheControl] =
                    "public,max-age=" + durationInSeconds;
                }
            });

            app.UseSession();

            app.UseSignalR(routes =>
            {
                routes.MapHub<ChatHub>("/chatHub");
            });

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });

            db.EnsureSeedData();

        }
    }
}
