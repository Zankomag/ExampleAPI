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
using System.IdentityModel.Tokens.Jwt;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using System.Security.Claims;
using System.Threading.Tasks;
using System.IO;
using Microsoft.AspNetCore.Authorization;
using ExampleAPI.Web.Authorization;

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

			SecurityKey securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration["JWT:Secret"]));

			

			//Generating correct token to get it for testing access
			//This code is here for testing purpose only
			var claims = new List<Claim>() { new Claim("customField", "123456789") };
			var token = new JwtSecurityTokenHandler().WriteToken(
				new JwtSecurityToken(claims: claims,
					signingCredentials: new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256)));
			System.Console.WriteLine(token);

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
				c.CustomSchemaIds(type => type.FullTypeName().Replace("Resource", string.Empty));
				c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme() {
					Description = @"JWT Authorization header using the Bearer scheme.
						Enter 'Bearer <token>' in the text input below",
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

			services.AddAutoMapper(typeof(Startup));

			services.AddCors();

			services.AddControllers().AddNewtonsoftJson(options => {
				options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
				options.SerializerSettings.NullValueHandling = NullValueHandling.Ignore;
			});

		}

		// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
		public void Configure(IApplicationBuilder app, IWebHostEnvironment env) {
			if (env.IsDevelopment()) {
				app.UseDeveloperExceptionPage();
				app.UseCors();
			}

			

			app.UseSwagger();
			app.UseSwaggerUI(c => {
				c.SwaggerEndpoint("/swagger/v1/swagger.json", "APILab");
				c.RoutePrefix = "swagger";
			});

			app.UseHttpsRedirection();
			app.UseHsts();

			app.UseRouting();

			app.UseAuthentication();
			app.UseAuthorization();

			app.UseEndpoints(endpoints => {
				endpoints.MapControllers();
			});
		}
	}
}
