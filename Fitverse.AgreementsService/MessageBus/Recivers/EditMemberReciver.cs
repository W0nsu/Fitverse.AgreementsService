using System;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Fitverse.AgreementsService.Data;
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
	public class EditMemberReciver : BackgroundService
	{
		private readonly string _hostName;
		private readonly string _password;
		private readonly IServiceProvider _provider;
		private readonly string _userName;

		private IModel _channel;
		private IConnection _connection;

		public EditMemberReciver(IOptions<RabbitMqConfiguration> rabbitMqOptions, IServiceProvider serviceProvider)
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
			_channel.QueueDeclare("AS_EditMember", false, false, false, null);
			_channel.QueueBind("AS_EditMember", "members", "editMember", null);
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
				var editedMemberEntity = JsonConvert.DeserializeObject<Member>(content);

				var memberEntity = dbContext
					.Members
					.First(x => x.MemberId == editedMemberEntity.MemberId);

				memberEntity.Name = editedMemberEntity.Name;
				memberEntity.SurName = editedMemberEntity.SurName;

				_ = dbContext.SaveChanges();

				_channel.BasicAck(ea.DeliveryTag, false);
			};

			_channel.BasicConsume("AS_EditMember", false, consumer);

			return Task.CompletedTask;
		}
	}
}