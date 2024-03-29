using Serilog;
using AutoMapper;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.OpenApi.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Linq;
using System.Text;
using System.Security.Claims;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using ExampleAPI.Data;
using ExampleAPI.Repository;
using ExampleAPI.Repository.Abstractions;
using ExampleAPI.Web.Services;
using ExampleAPI.Web.Extensions;
using ExampleAPI.Web.Communication;
using ExampleAPI.Web.Authorization;
using ExampleAPI.Web.Mapping;
using ExampleAPI.Web.Services.Abstractions;

namespace ExampleAPI.Web {
	public class Startup {
		public Startup(IConfiguration configuration) {
			Configuration = configuration;
		}

		public IConfiguration Configuration { get; }

		// This method gets called by the runtime. Use this method to add services to the container.
		public void ConfigureServices(IServiceCollection services) {

			services.AddRazorPages();

			string connection = Configuration.GetConnectionString("InternetShopConnectionString");
			services.AddDbContext<InternetShopDbContext>(options => {
				options.UseSqlite(connection);
				////Adding "Microsoft.EntityFrameworkCore": "Information" 
				////to Serilog MinimumLevel in config  allows to get more convenient output
				//options.LogTo(Log.Information, minimumLevel: LogLevel.Information);
			});
			SecurityKey securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration["JWT:Secret"]));

			

			//Generating correct token to get it for testing access
			//This code is here for testing purpose only
			var claims = new List<Claim>() { new Claim("customField", "123456789") };
			var token = new JwtSecurityTokenHandler().WriteToken(
				new JwtSecurityToken(claims: claims,
					signingCredentials: new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256)));
			Log.Information("Testing JWT token:\n{0}", token);
			services.AddAuthentication(options => {
				options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
				options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
				options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
			})
			.AddJwtBearer(options => {
				options.TokenValidationParameters = new TokenValidationParameters() {
					IssuerSigningKey = securityKey,
					RequireExpirationTime = false,
					ValidateIssuer = false,
					ValidateAudience = false,
					RequireSignedTokens = true,
					ValidateIssuerSigningKey = true,
					//To prevent abusing other algorithms
					ValidAlgorithms = new List<string>() { SecurityAlgorithms.HmacSha256 },
					
				};
				options.IncludeErrorDetails = false;
			});

			//Authorization Middleware handles Unauthorized response
			services.AddTransient<IAuthorizationMiddlewareResultHandler, ResponseAuthorizationMiddlewareResultHandler>();

			//Authorization Policy that is used in [Authorize] Attribute
			//Never add "sub" field!!! - it somewhy doesn't compare claim value
			services.AddAuthorization(options => {
				options.AddPolicy("IsSub", 
					policyBuilder => policyBuilder.RequireAssertion(handler => {
						var subClaim = handler.User.FindFirst("customField");
						if (subClaim?.Value == "123456789")
							return true;
						return false;
					}).Build());
			});

			


			services.AddSwaggerGen(c => {
				c.SwaggerDoc("v1", new OpenApiInfo { Title = "Example API", Version = "1.0" });
				c.CustomSchemaIds(type => type.FullTypeName());
				c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme() {
					Description = @"JWT Authorization header using the Bearer scheme.
						Enter 'Bearer [token]' in the text input below",
					Name = "Authorization",
					In = ParameterLocation.Header,
					Type = SecuritySchemeType.ApiKey,
					Scheme = "Bearer"
				});
				c.AddSecurityRequirement(new OpenApiSecurityRequirement() {
					{
						new OpenApiSecurityScheme() {
							Reference = new OpenApiReference() {
								Type = ReferenceType.SecurityScheme,
								Id = "Bearer"
							},
							Scheme = "oauth2",
							Name = "Bearer",
							In = ParameterLocation.Header
						},
						new List<string>()
					}
				});
			});

			services.AddScoped<IUnitOfWork, UnitOfWork>();
			services.AddScoped<IOrderItemService, OrderItemService>();

			//Here we need to pass any type of assembly WHERE AutomapperProfile class is
			// (better way - pass AutomapperProfile class itself)
			services.AddAutoMapper(typeof(ModelToResourceProfile)); //(typeof(Startup)

			services.AddCors();

			services.AddControllers()
				.AddNewtonsoftJson(options => {
					options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
					options.SerializerSettings.NullValueHandling = NullValueHandling.Ignore;})
				.ConfigureApiBehaviorOptions(options => {
					//Override default model state error response
					options.InvalidModelStateResponseFactory = context => {
						if (context.ModelState.ErrorCount > 0) {
							string messages = string.Join("; ", context.ModelState.Values
								.SelectMany(x => x.Errors).Select(x => x.ErrorMessage));
							return (ObjectResult)new Response<object>(messages);
						}
						return (ObjectResult)Response<object>.BadRequestResponse;
					};
				});

		}

		// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
		public void Configure(IApplicationBuilder app, IWebHostEnvironment env) {

			app.UseSwagger();
			app.UseSwaggerUI(c => {
				c.SwaggerEndpoint("/swagger/v1/swagger.json", "APILab");
				c.RoutePrefix = "swagger";
			});

			app.UseHttpsRedirection();
			app.UseHsts();

			//To log only Warning or greater requests
			//Set "Serilog.AspNetCore": "Warning" in Serilog MinimumLevel Config
			app.UseSerilogRequestLogging();

			app.UseRouting();

			app.UseAuthentication();
			app.UseAuthorization();

			app.UseEndpoints(endpoints => {
				endpoints.MapControllers();
				endpoints.MapRazorPages();
			});
		}
	}
}
