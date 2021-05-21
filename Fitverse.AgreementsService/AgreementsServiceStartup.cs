using System;
using System.Text;
using Fitverse.AgreementsService.Data;
using Fitverse.AgreementsService.Interfaces;
using Fitverse.AgreementsService.MessageBus.Recivers;
using Fitverse.AgreementsService.MessageBus.Senders;
using Fitverse.Shared;
using Fitverse.Shared.MessageBus;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;

namespace Fitverse.AgreementsService
{
	public class AgreementsServiceStartup : SharedStartup
	{
		public AgreementsServiceStartup(IConfiguration configuration, IWebHostEnvironment environment) : base(
			configuration,
			environment)
		{
		}

		public override void ConfigureServices(IServiceCollection services)
		{
			services.AddSwaggerGen(c =>
			{
				c.SwaggerDoc("v1", new OpenApiInfo
				{
					Title = "Fitverse.AgreementService",
					Version = "v1",
					Description = "ASP.NET Core Web API for Fitverse, complex fitness management solution",
					Contact = new OpenApiContact
					{
						Name = "Paweł Wąsowski",
						Email = "pwasowski@edu.cdv.pl",
						Url = new Uri("https://www.linkedin.com/in/pawelwasowski/")
					}
				});
			});
			
			services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
				.AddJwtBearer(options =>
				{
					options.TokenValidationParameters = new TokenValidationParameters
					{
						ClockSkew = TimeSpan.Zero,
						ValidateIssuer = true,
						ValidateAudience = true,
						ValidateLifetime = true,
						ValidateIssuerSigningKey = true,
						ValidIssuer = Configuration["Jwt:Issuer"],
						ValidAudience = Configuration["Jwt:Issuer"],
						IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration["Jwt:Key"]))
					};
				});

			base.ConfigureServices(services);
			services.AddDbContext<AgreementsContext>(options =>
				options.UseSqlServer(Configuration.GetConnectionString("AzureDb")));

			services.AddMediatR(typeof(AgreementsServiceStartup));

			services.AddValidatorsFromAssembly(typeof(AgreementsServiceStartup).Assembly);

			services.Configure<RabbitMqConfiguration>(Configuration.GetSection("RabbitMq"));

			services.AddTransient<IAddMembershipSender, AddMembershipSender>();
			services.AddTransient<IDeleteMembershipSender, DeleteMembershipSender>();
			services.AddTransient<IEditMembershipSender, EditMembershipSender>();

			services.AddHostedService<AddMemberReciver>();
			services.AddHostedService<AddAgreementReciver>();
			services.AddHostedService<EditMemberReciver>();
			services.AddHostedService<DeleteMemberReciver>();
		}
	}
}