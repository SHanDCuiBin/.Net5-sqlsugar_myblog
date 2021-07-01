using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using MyBlog.IRepository;
using MyBlog.IService;
using MyBlog.Repository;
using MyBlogService;
using SqlSugar.IOC;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyBlog.JWT
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
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "MyBlog.JWT", Version = "v1" });
            });

            #region SqlSugar.IOC
            services.AddSqlSugar(new IocConfig()
            {
                ConnectionString = this.Configuration["SqlConn"], //"server=.;uid=sa;pwd=haosql;database=SQLSUGAR4XTEST",
                DbType = IocDbType.MySql,
                IsAutoCloseConnection = true//�Զ��ͷ�
            });
            #endregion

            #region IOC����ע��
            services.AddScoped<IWriterInfoRepository, WriterInfoRepository>();
            services.AddScoped<IWriterInfoService, WriterInfoService>();
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
            app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "MyBlog.JWT v1"));

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
