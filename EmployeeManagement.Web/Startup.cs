using EmployeeManagement.BusinessModel.AuthModel;
using EmployeeManagement.Configuration;
using EmployeeManagement.DataAccess;
using EmployeeManagement.Models;
using EmployeeManagement.Services.IService;
using EmployeeManagement.Services.Service;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Collections.Generic;
using System.Text;


namespace EmployeeManagement
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
            // Setup CORS
            // var corsBuilder = new CorsPolicyBuilder();
            // corsBuilder.AllowAnyOrigin(); // For anyone access.
            // corsBuilder.AllowAnyMethod();
            // corsBuilder.AllowAnyHeader();
            // corsBuilder.AllowCredentials();

            // services.AddCors(options =>
            // {
            //options.AddPolicy("NttDataEmployeeOrigin", corsBuilder.Build());
            // });

            // // use Cors globally.
            // services.Configure<MvcOptions>(options =>
            // {
            //options.Filters.Add(new CorsAuthorizationFilterFactory("NttDataEmployeeOrigin"));
            // });
            services.AddCors(c =>
            {
                c.AddPolicy("AllowOrigin", options =>
                {
                    options.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod().Build();
                });
            });
            // services.Configure<Microsoft.AspNetCore.Mvc.MvcOptions>(options =>
            // {
            //options.Filters.Add(new CorsAuthorizationFilterFactory("MircomOrigin"));
            // });
            services.AddControllers();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc(
            name: "v1",
            new OpenApiInfo
                {
                    Title = "NTT Employee Data Api",
                    Version = "v1"
                });
                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Description = "JWT Authorization header using the Bearer scheme." +
                            " \r\n\r\n Enter 'Bearer' [space] and then your token in the text input below." +
                            "\r\n\r\nExample: \"Bearer 12345abcdef\"",
                    Name = "Authorization",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "Bearer"
                });

                c.AddSecurityRequirement(new OpenApiSecurityRequirement()
                {
              {
                  new OpenApiSecurityScheme
                  {
                      Reference = new OpenApiReference
                      {
                          Type = ReferenceType.SecurityScheme,
                          Id = "Bearer"
                      },
                      Scheme = "oauth2",
                      Name = "Bearer",
                      In = ParameterLocation.Header,

                  },
                  new List<string>()
              }
                });
            });

            // get the connection string from appsettings.json
            var connectionString = Configuration.GetConnectionString("Ntt_DB");
            services.AddDbContext<NTT_DBContext>(options =>
            options.UseSqlServer(
            connectionString, b => b.MigrationsAssembly("NttDataEmployee")));

            var JwtIssuerOptionsSection = Configuration.GetSection("JwtIssuerOptions");
            services.Configure<AppSettings>(JwtIssuerOptionsSection);

            //JWT Authentication
            var JwtIssuerOptions = JwtIssuerOptionsSection.Get<AppSettings>();
            var key = Encoding.ASCII.GetBytes(JwtIssuerOptions.SecretKey);

            services.AddAuthentication(au =>
            {
                au.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                au.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(jwt =>
            {
                jwt.RequireHttpsMetadata = false;
                jwt.SaveToken = true;
                jwt.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = false,
                    ValidateAudience = false
                };
            });
            //services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            //  .AddJwtBearer(options =>
            //  {
            // options.TokenValidationParameters = new TokenValidationParameters
            // {
            //   ValidateIssuer = true,
            //   ValidateAudience = true,
            //   ValidateLifetime = true,
            //   ValidateIssuerSigningKey = true,

            //   ValidIssuer = "http://localhost:44323",
            //   ValidAudience = "http://localhost:44323",
            //   IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("KeyForSignInSecret@1234"))
            // };
            //  });

            services.AddMvc();


            //Register Services through Dependency injection.
            services.AddScoped<IEmployeeRecordsService, EmployeeRecordsService>();
            services.AddScoped<IEmployeeEntryRecordsService, EmployeeEntryRecordsService>();
            services.AddScoped<GenericUnitOfWork<NTT_DBContext>>();
            services.AddScoped<IAuthenticateService, AuthenticateService>();
            services.AddScoped<IUserService, UserService>();

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            // app.UseCors(options => options.WithOrigins());
            //app.UseCors("NttDataEmployee");
            // app.UseCors("AllowOrigin");
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint(url: "/swagger/v1/swagger.json", name: "NTT Api Version: 1.0");
            });

            app.UseHttpsRedirection();

            app.UseRouting();



            // Setting middleware for global level exception handling.
            app.ConfigureExceptionMiddleware();
            app.UseAuthorization();
            // global CORS policy
            app.UseCors("AllowOrigin");
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}

