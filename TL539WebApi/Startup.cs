using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GitWebApi.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using TL539WebApi.DbContexts;

namespace TL539WebApi
{
	public class Startup
	{
		public Startup(IConfiguration configuration)
		{
			Configuration = configuration;
		}

		public IConfiguration Configuration { get; }
		readonly string MyAllowSpecificOrigins = "_myAllowSpecificOrigins";

		// This method gets called by the runtime. Use this method to add services to the container.
		public void ConfigureServices(IServiceCollection services)
		{
			services.AddCors(options =>
			{
				options.AddPolicy(MyAllowSpecificOrigins,
				builder =>
				{
					builder.WithOrigins("*",
						"http://localhost:3003"
										);
				});
			});

			services.AddHttpClient();

			services.AddControllers(setupAction =>
			{
				setupAction.ReturnHttpNotAcceptable = true;//如果為false則使用預設資料格式

			});
			//services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());//透過套件轉換Models

			services.AddScoped<IWinNumberRepository, WinNumberRepository>();//使用EF Repos
			

			services.AddDbContext<TL539WebApiContext>(options =>
			{
				options.UseSqlServer(Configuration.GetConnectionString("TL539WebApiContext"));//使用EF Dbcontext連接字串
																							//Data Source = (localdb)\MSSQLLocalDB; Initial Catalog = master; Integrated Security = True; Connect Timeout = 30; Encrypt = False; TrustServerCertificate = False; ApplicationIntent = ReadWrite; MultiSubnetFailover = False
			});
		}

		// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
		public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
		{
			if (env.IsDevelopment())
			{
				app.UseDeveloperExceptionPage();
			}

			app.UseRouting();

			app.UseCors(MyAllowSpecificOrigins);

			app.UseAuthorization();

			app.UseEndpoints(endpoints =>
			{
				endpoints.MapControllers();
			});
		}
	}
}
