using System;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Fitverse.AgreementsService.Data;
using Fitverse.AgreementsService.Helpers;
using Fitverse.AgreementsService.Models;
using Fitverse.Shared.MessageBus;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace Fitverse.AgreementsService.MessageBus.Recivers
{
	public class AddAgreementReciver : BackgroundService
	{
		private readonly string _hostName;
		private readonly string _password;

		private readonly IServiceProvider _provider;
		private readonly string _userName;

		private IModel _channel;
		private IConnection _connection;

		public AddAgreementReciver(IOptions<RabbitMqConfiguration> rabbitMqOptions, IServiceProvider serviceProvider)
		{
			_provider = serviceProvider;

			_hostName = rabbitMqOptions.Value.Hostname;
			_userName = rabbitMqOptions.Value.UserName;
			_password = rabbitMqOptions.Value.Password;

			InitializeRabbitMqListener();
		}

		private void InitializeRabbitMqListener()
		{
			var factory = new ConnectionFactory
			{
				HostName = _hostName,
				UserName = _userName,
				Password = _password
			};

			_connection = factory.CreateConnection();
			_channel = _connection.CreateModel();

			_channel.ExchangeDeclare("agreements", "direct");
			_channel.QueueDeclare("AS_AddAgreement", false, false, false, null);
			_channel.QueueBind("AS_AddAgreement", "agreements", "addAgreement", null);
		}

		protected override Task ExecuteAsync(CancellationToken stoppingToken)
		{
			stoppingToken.ThrowIfCancellationRequested();

			var consumer = new EventingBasicConsumer(_channel);
			consumer.Received += (ch, ea) =>
			{
				var content = Encoding.UTF8.GetString(ea.Body.ToArray());
				var newAgreementEntity = JsonConvert.DeserializeObject<Agreement>(content);

				using var scope = _provider.CreateScope();
				var dbContext = scope.ServiceProvider.GetRequiredService<AgreementsContext>();

				EndingDateCalculator.SetEndingDate(newAgreementEntity, dbContext);
				newAgreementEntity.TerminationPeriod = GetTerminationPeriod(newAgreementEntity.MembershipId, dbContext);

				_ = dbContext.Agreements.Add(newAgreementEntity);
				_ = dbContext.SaveChanges();

				var installmentGenerator = new InstallmentGenerator(dbContext);
				installmentGenerator.AddInstallments(newAgreementEntity);

				_channel.BasicAck(ea.DeliveryTag, false);
			};

			_channel.BasicConsume("AS_AddAgreement", false, consumer);

			return Task.CompletedTask;
		}

		private int GetTerminationPeriod(int membershipId, AgreementsContext dbContext)
		{
			var membershipEntity =
				dbContext.Memberships.FirstOrDefault(m => m.MembershipId == membershipId);

			if (membershipEntity is null)
				throw new NullReferenceException($"Membership [MembershipId: {membershipId} not found]");

			return membershipEntity.TerminationPeriod;
		}
	}
}