using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using TL539WebApi.DbContexts;

namespace TL539WebApi
{
	public class Program
	{
		public static void Main(string[] args)
		{
			//	CreateHostBuilder(args).Build().Run();


			var host = CreateHostBuilder(args).Build();

			// migrate the database.  Best practice = in Main, using service scope
			using (var scope = host.Services.CreateScope())
			{
				//�ϥ�EF���m��Ʈw
				try
				{
					var context = scope.ServiceProvider.GetService<TL539WebApiContext>();
					//�R����Ʈw
					//context.Database.EnsureDeleted();
					context.Database.Migrate();
				}
				catch (Exception ex)
				{
					var logger = scope.ServiceProvider.GetRequiredService<ILogger<Program>>();
					logger.LogError(ex, "An error occurred while migrating the database.");
				}
			}

			// run the web app
			host.Run();
		}

		public static IHostBuilder CreateHostBuilder(string[] args) =>
			Host.CreateDefaultBuilder(args)
				.ConfigureWebHostDefaults(webBuilder =>
				{
					webBuilder.UseStartup<Startup>();
				});
	}
}
