using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using MyBlog.IRepository;
using MyBlog.IService;
using MyBlog.Repository;
using MyBlogService;
using SqlSugar.IOC;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Test_2_WebApi
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
            // 配置跨域处理，允许所有来源
            services.AddCors(options =>
            {
                // this defines a CORS policy called "default"
                options.AddPolicy("default", policy =>
                {
                    policy.WithOrigins("*").AllowAnyHeader().AllowAnyMethod();
                });
            });

            services.AddControllers();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Test_2_WebApi", Version = "v1" });
                #region Swagger 使用鉴权组件
                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.ApiKey,
                    Description = "直接在下框中输入Bearer {token}（注意两者之间是一个空格）",
                    Name = "Authorization",
                    BearerFormat = "JWT",
                    Scheme = "Bearer"
                });
                c.AddSecurityRequirement(new OpenApiSecurityRequirement
                                        {
                                          {
                                            new OpenApiSecurityScheme
                                            {
                                              Reference=new OpenApiReference
                                              {
                                                Type=ReferenceType.SecurityScheme,
                                                Id="Bearer"
                                              }
                                            },
                                            new string[] {}
                                          }
                                        });

                #endregion
            });

            #region SqlSugar.IOC
            services.AddSqlSugar(new IocConfig()
            {
                ConnectionString = this.Configuration["SqlConn"], //"server=.;uid=sa;pwd=haosql;database=SQLSUGAR4XTEST",
                DbType = IocDbType.MySql,
                IsAutoCloseConnection = true//自动释放
            });
            #endregion

            #region IOC 依赖注入
            services.AddCustomIOC();
            #endregion

            #region JWT鉴权
            services.AddCustomerJWT();
            #endregion
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            // 允许所有跨域，default是在ConfigureServices方法中配置的跨域策略名称
            app.UseCors("default");

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            app.UseSwagger();
            app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Test_2_WebApi v1"));

            app.UseRouting();

            #region 先鉴权，再授权，顺序不能错
            //添加管道中的 鉴权
            app.UseAuthentication();

            //添加管道中的 授权
            app.UseAuthorization();
            #endregion

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
    static class IOCExtend
    {
        public static IServiceCollection AddCustomIOC(this IServiceCollection services)    //扩展方法 
        {
            services.AddScoped<IBlogNewsRepository, BlogNewsRepository>();               // 接口 对 实现
            services.AddScoped<ITypeInfoRepository, TypeInfoRepository>();               // 接口 对 实现
            services.AddScoped<IWriterInfoRepository, WriterInfoRepository>();           // 接口 对 实现

            services.AddScoped<IBlogNewsService, BlogNewsService>();                     // 接口 对 实现
            services.AddScoped<ITypeInfoService, TypeInfoService>();                     // 接口 对 实现
            services.AddScoped<IWriterInfoService, WriterInfoService>();                 // 接口 对 实现
            return services;
        }

        /* 
         * AddSingleton的生命周期：
           项目启动-项目关闭              相当于静态类  只会有一个
           AddScoped的生命周期：
           请求开始-请求结束              在这次请求中获取的对象都是同一个
           AddTransient的生命周期：
           请求获取-（GC回收-主动释放）   每一次获取的对象都不是同一个  */


        public static IServiceCollection AddCustomerJWT(this IServiceCollection services)
        {
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("SDMC-CJAS1-SAD-DFSFA-SADHJVF-VF")),
                        ValidateIssuer = true,
                        ValidIssuer = "http://localhost:6060",
                        ValidateAudience = true,
                        ValidAudience = "http://localhost:5000",
                        ValidateLifetime = true,
                        ClockSkew = TimeSpan.FromMinutes(60)
                    };
                });

            return services;
        }
    }
}
