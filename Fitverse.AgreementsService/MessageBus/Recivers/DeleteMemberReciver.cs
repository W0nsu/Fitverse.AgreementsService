using System;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Fitverse.AgreementsService.Data;
using Fitverse.AgreementsService.Helpers;
using Fitverse.Shared.MessageBus;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace Fitverse.AgreementsService.MessageBus.Recivers
{
	public class DeleteMemberReciver : BackgroundService
	{
		private readonly string _hostName;
		private readonly string _password;
		private readonly IServiceProvider _provider;
		private readonly string _userName;

		private IModel _channel;
		private IConnection _connection;

		public DeleteMemberReciver(IOptions<RabbitMqConfiguration> rabbitMqOptions,
			IServiceProvider serviceProvider)
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

			_channel.ExchangeDeclare("members", "direct");
			_channel.QueueDeclare("AS_DeleteMember", false, false, false, null);
			_channel.QueueBind("AS_DeleteMember", "members", "deleteMember", null);
		}

		protected override Task ExecuteAsync(CancellationToken stoppingToken)
		{
			stoppingToken.ThrowIfCancellationRequested();

			var consumer = new EventingBasicConsumer(_channel);
			consumer.Received += (ch, ea) =>
			{
				using var scope = _provider.CreateScope();
				var dbContext = scope.ServiceProvider.GetRequiredService<AgreementsContext>();

				var content = Encoding.UTF8.GetString(ea.Body.ToArray());
				var deletedMemberId = JsonConvert.DeserializeObject<int>(content);

				var deletedMemberEntity = dbContext
					.Members
					.First(x => x.MemberId == deletedMemberId);

				_ = dbContext.Members.Remove(deletedMemberEntity);
				_ = dbContext.SaveChanges();

				var deletedMemberAgreements = dbContext.Agreements
					.Where(x => x.MemberId == deletedMemberEntity.MemberId)
					.ToList();

				var installmentGenerator = new InstallmentGenerator(dbContext);

				foreach (var agreement in deletedMemberAgreements)
				{
					installmentGenerator.DeleteInstallments(agreement);
					_ = dbContext.Remove(agreement);
				}

				_ = dbContext.SaveChanges();

				_channel.BasicAck(ea.DeliveryTag, false);
			};

			_channel.BasicConsume("AS_DeleteMember", false, consumer);

			return Task.CompletedTask;
		}
	}
}