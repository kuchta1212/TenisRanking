using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TenisRanking.Data;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Quartz;
using TenisRanking.Email;
using TenisRanking.Job;
using TenisRanking.MatchProvider;
using TenisRanking.Models;
using TenisRanking.Utils;

namespace TenisRanking
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
            services.Configure<CookiePolicyOptions>(options =>
            {
                // This lambda determines whether user consent for non-essential cookies is needed for a given request.
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });

            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(
                    Configuration.GetConnectionString("DefaultConnection")));
            services.AddDefaultIdentity<IdentityUser>()
                .AddEntityFrameworkStores<ApplicationDbContext>();

            services.Configure<EmailOptions>(opt =>
            {
                opt.Enabled = Configuration.GetSection("Email").GetValue<bool>("Enabled");
                opt.SendGridKeyApi = Configuration.GetSection("Email").GetValue<string>("SendGridKeyApi");
                opt.SenderEmail = Configuration.GetSection("Email").GetValue<string>("SenderEmail");
                opt.SenderName = Configuration.GetSection("Email").GetValue<string>("SenderName");
            });

            services.Configure<MatchDaysLimitOptions>(opt =>
            {
                opt.Enabled = Configuration.GetSection("MaxDaysLimitations").GetValue<bool>("Enabled");
                opt.Days = int.Parse(Configuration.GetSection("MaxDaysLimitations").GetValue<string>("Days"));
                opt.LevelDrop = int.Parse(Configuration.GetSection("MaxDaysLimitations").GetValue<string>("LevelDrop"));
            });

            services.Configure<ConfirmationPeriodOptions>(opt =>
            {
                opt.Enabled = Configuration.GetSection("ConfirmationPeriod").GetValue<bool>("Enabled");
                opt.Hours = int.Parse(Configuration.GetSection("ConfirmationPeriod").GetValue<string>("Hours"));
            });

            services.AddLocalization();
            services.Configure<RequestLocalizationOptions>(options =>
            {
                var supportedCultures = new[]
                {
                    new CultureInfo("en-US"),
                    new CultureInfo("cs-CS"), 
                };
                options.DefaultRequestCulture = new RequestCulture("en-US", "en-US");

                options.SupportedCultures = supportedCultures;

                options.SupportedUICultures = supportedCultures;
            });

            var jobKey = new JobKey("matchLimitJob");
            services.AddTransient<MatchCheckerJob>();
            services.AddQuartz(q =>
            {
                q.SchedulerId = "Scheduler-Core";
                q.SchedulerName = "Scheduler-Core";
                q.UseMicrosoftDependencyInjectionScopedJobFactory();

                q.AddJob<MatchCheckerJob>(j => j
                    .WithIdentity(jobKey)
                );

                q.AddTrigger(t => t
                    .WithIdentity("Cron Trigger")
                    .ForJob(jobKey)
                    .StartAt(DateBuilder.TodayAt(23,55,00))
                    .WithSimpleSchedule(x => x.WithInterval(TimeSpan.FromDays(1)).RepeatForever())
                    .WithDescription("match limit trigger")
                );             
            });

            var jobKey2 = new JobKey("confirmationPeriodJob");
            services.AddTransient<ConfirmationPeriodJob>();
            services.AddQuartz(q =>
            {
                q.SchedulerId = "Scheduler-Core-2";
                q.SchedulerName = "Scheduler-Core-2";
                q.UseMicrosoftDependencyInjectionScopedJobFactory();

                q.AddJob<ConfirmationPeriodJob>(j => j
                    .WithIdentity(jobKey2)
                );

                q.AddTrigger(t => t
                    .WithIdentity("Cron Trigger 2")
                    .ForJob(jobKey2)
                    .StartAt(DateBuilder.TodayAt(23,55,00))
                    .WithSimpleSchedule(x => x.WithInterval(TimeSpan.FromDays(1)).RepeatForever())
                    .WithDescription("confirmation period trigger")
                );
            });

            services.AddQuartzServer(options =>
            {
                // when shutting down we want jobs to complete gracefully
                options.WaitForJobsToComplete = true;
            });


            services.AddTransient<IDbContextWrapper, DbContextWrapper>();
            services.AddTransient<IMatchProvider, MatchProvider.MatchProvider>();
            services.AddTransient<IEmailController, EmailController>();
            services.AddTransient<IViewMessageFactory, ViewMessageFactory>();
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
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
