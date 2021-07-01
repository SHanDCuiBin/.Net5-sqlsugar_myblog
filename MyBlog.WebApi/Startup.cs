using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SqlSugar.IOC;
using MyBlog.IRepository;
using MyBlog.Repository;
using MyBlog.IService;
using MyBlogService;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using MyBlog.WebApi.Utility._Automapper;

namespace MyBlog.WebApi
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
            // ���ÿ���������������Դ
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
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "MyBlog.WebApi", Version = "v1" });

                #region Swagger ʹ�ü�Ȩ���
                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.ApiKey,
                    Description = "ֱ�����¿�������Bearer {token}��ע������֮����һ���ո�",
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
                IsAutoCloseConnection = true//�Զ��ͷ�
            });
            #endregion

            #region IOC ����ע��
            services.AddCustomIOC();
            #endregion

            #region JWT��Ȩ
            services.AddCustomerJWT();
            #endregion

            #region AutoMapper
            services.AddAutoMapper(typeof(CustomAutoMapperProfile));
            #endregion
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            // �������п���default����ConfigureServices���������õĿ����������
            app.UseCors("default");

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseSwagger();
            app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "MyBlog.WebApi v1"));

            app.UseRouting();

            #region �ȼ�Ȩ������Ȩ��˳���ܴ�
            //��ӹܵ��е� ��Ȩ
            app.UseAuthentication();

            //��ӹܵ��е� ��Ȩ
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
        public static IServiceCollection AddCustomIOC(this IServiceCollection services)    //��չ���� 
        {
            services.AddScoped<IBlogNewsRepository, BlogNewsRepository>();               // �ӿ� �� ʵ��
            services.AddScoped<ITypeInfoRepository, TypeInfoRepository>();               // �ӿ� �� ʵ��
            services.AddScoped<IWriterInfoRepository, WriterInfoRepository>();           // �ӿ� �� ʵ��

            services.AddScoped<IBlogNewsService, BlogNewsService>();                     // �ӿ� �� ʵ��
            services.AddScoped<ITypeInfoService, TypeInfoService>();                     // �ӿ� �� ʵ��
            services.AddScoped<IWriterInfoService, WriterInfoService>();                 // �ӿ� �� ʵ��
            return services;
        }

        /* 
         * AddSingleton���������ڣ�
           ��Ŀ����-��Ŀ�ر�              �൱�ھ�̬��  ֻ����һ��
           AddScoped���������ڣ�
           ����ʼ-�������              ����������л�ȡ�Ķ�����ͬһ��
           AddTransient���������ڣ�
           �����ȡ-��GC����-�����ͷţ�   ÿһ�λ�ȡ�Ķ��󶼲���ͬһ��  */


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
