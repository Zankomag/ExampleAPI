using ExampleAPI.Data;
using ExampleAPI.Repository.Abstractions;
using ExampleAPI.Repository;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json;
using AutoMapper;
using ExampleAPI.Web.Services.Abstractions;
using ExampleAPI.Web.Services;
using ExampleAPI.Web.Communication;
using ExampleAPI.Web.Extensions;
using System.Collections.Generic;
using System.Linq;

namespace ExampleAPI.Web {
	public class Startup {
		public Startup(IConfiguration configuration) {
			Configuration = configuration;
		}

		public IConfiguration Configuration { get; }

		// This method gets called by the runtime. Use this method to add services to the container.
		public void ConfigureServices(IServiceCollection services) {

			string connection = Configuration.GetConnectionString("InternetShopConnectionString");
			services.AddDbContext<InternetShopDbContext>(options => 
				options.UseSqlite(connection)
				.UseLoggerFactory(LoggerFactory.Create(config => config.AddConsole()))
			);

			services.AddScoped<IUnitOfWork, UnitOfWork>();
			services.AddScoped<IOrderItemService, OrderItemService>();

			services.AddAutoMapper(typeof(Startup));

			services.AddSwaggerGen(c => {
				c.SwaggerDoc("v1", new OpenApiInfo { Title = "Example API", Version = "1.0" });
				c.CustomSchemaIds(type => type.FullTypeName().Replace("Resource", string.Empty));
				//c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme() {
				//	Description = @"JWT Authorization header using the Bearer scheme.
    			//		Enter 'Bearer' [space] and then your token in the text input below.
				//		Example: 'Bearer 12345abcdef'",
				//	Name = "Authorization",
				//	In = ParameterLocation.Query,
				//	Type = SecuritySchemeType.ApiKey,
				//	Scheme = "Bearer"
				//});
				//c.AddSecurityRequirement(new OpenApiSecurityRequirement() {
				//	{
				//		new OpenApiSecurityScheme() {
				//			Reference = new OpenApiReference() {
				//				Type = ReferenceType.SecurityScheme,
				//				Id = "Bearer"
				//			},
				//			Scheme = "oauth2",
				//			Name = "Bearer",
				//			In = ParameterLocation.Query
				//		},
				//		new List<string>()
				//	}
				//});
			});

			services.AddControllers().AddNewtonsoftJson(options => {
				options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
				options.SerializerSettings.NullValueHandling = NullValueHandling.Ignore;
			});

		}

		// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
		public void Configure(IApplicationBuilder app, IWebHostEnvironment env) {
			if (env.IsDevelopment()) {
				app.UseDeveloperExceptionPage();
			}

			app.UseSwagger();
			app.UseSwaggerUI(c => {
				c.SwaggerEndpoint("/swagger/v1/swagger.json", "APILab");
				c.RoutePrefix = "swagger";
			});

			app.UseHttpsRedirection();
			app.UseHsts();

			app.UseRouting();

			app.UseAuthorization();

			app.UseEndpoints(endpoints => {
				endpoints.MapControllers();
			});
		}
	}
}
