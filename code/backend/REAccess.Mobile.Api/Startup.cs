#region
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using REAccess.Mobile.Api.Controllers.Attributes;
using REAccess.Mobile.Api.Extensions;
using REAccess.Mobile.Common.Utils;
using REAccess.Mobile.Database.Models;
using REAccess.Mobile.Database.Utils;
#endregion

namespace REAccess.Mobile.Api
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
            services.AddDbContext<DatabaseContext>();

            services.AddControllers();

            NativeInjectorBootStrapper.RegisterServices(services);

            #region CORS
            services.AddCors(c =>
            {
                c.AddPolicy("cors", policy =>
                {
                    policy
                    .AllowAnyOrigin()
                    .AllowAnyHeader()
                    .AllowAnyMethod();
                });
            });
            #endregion

            #region ���Swagger
            services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("v1", new OpenApiInfo
                {
                    Version = "v1.0.0",
                    Title = "REAccess Mobile API",
                    Description = "���˵���ĵ�",
                    Contact = new OpenApiContact
                    {
                        Name = "John Jiang",
                        Email = "1033596188@qq.com",
                    }
                });
                var basePath = Path.GetDirectoryName(typeof(Program).Assembly.Location);//��ȡӦ�ó�������Ŀ¼
                var xmlPath = Path.Combine(basePath, "Swagger.xml");
                options.IncludeXmlComments(xmlPath);
            });
            #endregion
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IHostApplicationLifetime appLife)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseHsts();
            }

            app.UseForwardedHeaders(new ForwardedHeadersOptions { ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto });

            app.UseSwagger();

            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "ApiHelp V1");
            });

            //������ӵ����ŷ���
            //���÷����ļ�
            app.UseStaticFiles(new StaticFileOptions
            {
                //���ó���Ĭ�ϵ�wwwroot�ļ��еľ�̬�ļ�������ļ����ṩ Web ��Ŀ¼����ļ� ,
                //�����������Ժ󣬾Ϳ��Է��ʷ�wwwroot�ļ��µ��ļ�
                FileProvider = new PhysicalFileProvider(
                  Path.Combine(Directory.GetCurrentDirectory(), "RealTimeInfoImgs")),
                RequestPath = "/RealTimeInfoImgs",
            });

            app.UseCors();

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            appLife.ApplicationStarted.Register(() =>
            {
                //app���������еĳ���
                //����̬���ݼ�������̬����Ϊ����ʹ��
                StaticCache.RefreshCache(null);
                //app���������еĳ���
                //������500kb����ѶͼƬ ѹ������
                ImageHelper.MagickImage();

                Console.Write("ApplicationStarted");
            });
        }
    }
}
