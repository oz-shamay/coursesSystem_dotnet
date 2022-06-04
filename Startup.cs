using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.EntityFrameworkCore;
using coursesSystem.Models;
using Microsoft.AspNetCore.ResponseCompression;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using coursesSystem.repository;

namespace coursesSystem
{
    public class Startup
    {
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public Startup(IConfiguration config)
        {
            Configuration = config;
        }

        public IConfiguration Configuration { get; set; }
        private static readonly log4net.ILog logger = 
            log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddAutoMapper(typeof(Startup));
            services.AddControllers();

            services.AddCors(opts => opts.AddDefaultPolicy(builder =>
            {
                builder.AllowAnyHeader().AllowAnyMethod().AllowAnyOrigin();
            }));


            services.AddDbContext<Context>(opts => {
                opts.UseSqlServer(Configuration["ConnectionStrings:CourseConnection"]);
                opts.EnableSensitiveDataLogging(true);
            });

            
            services.AddScoped<IAttendancesRepository, AttendancesRepository>();
            services.AddScoped<IClassesRepository, ClassesRepository>();
            services.AddScoped<ICoursesRepository, CoursesRepository>();
            services.AddScoped<IAccountRepository, AccountRepository>();

            // identity
            services.AddIdentity<AppUser, IdentityRole>()
            .AddEntityFrameworkStores<Context>()
            .AddDefaultTokenProviders();


            //password config

            //services.Configure<IdentityOptions>(opts => {
            //    opts.Password.RequiredLength = 8;
            //    opts.Password.RequireNonAlphanumeric = true;
            //    opts.Password.RequireLowercase = false;
            //    opts.Password.RequireUppercase = true;
            //    opts.Password.RequireDigit = true;
            //    opts.User.RequireUniqueEmail = false;
            //});



            // Authentication
            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                
                options.SaveToken = true;
                options.RequireHttpsMetadata = false;
                options.TokenValidationParameters = new TokenValidationParameters()
                {

                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidAudience = Configuration["JWT:Validaudience"],
                    ValidIssuer = Configuration["JWT:ValidIssuer"],
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration["JWT:Secret"]))
                };

                options.Events = new JwtBearerEvents
                {
                    OnTokenValidated = async ctx =>
                    {
                        var userMnager = ctx.HttpContext.RequestServices.GetRequiredService<UserManager<AppUser>>();
                        var signinManager = ctx.HttpContext.RequestServices.GetRequiredService<SignInManager<AppUser>>();
                        string userName = ctx.Principal.FindFirst(ClaimTypes.Name)?.Value;
                        AppUser User = await userMnager.FindByNameAsync(userName);
                        ctx.Principal = await signinManager.CreateUserPrincipalAsync(User);
                    }
                };

            });
            
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env ,Context context)
        {
            //app.UseExceptionHandler("/Error");
            //app.UseDeveloperExceptionPage();

            
            app.UseRouting();
            app.UseCors();
            app.UseAuthentication();
            app.UseAuthorization();
            

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapGet("/", async context =>
                {
                    await context.Response.WriteAsync("Courses Server Is ON!");
                    logger.Info("the courses server is on");
                });
                endpoints.MapControllers();
            });

            SeedData.SeedDatabase(context);
        }
    }
}
