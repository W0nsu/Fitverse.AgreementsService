using System;
using Fitverse.AgreementsService.Interfaces;
using Fitverse.AgreementsService.Models;
using Fitverse.Shared.MessageBus;
using Microsoft.Extensions.Options;

namespace Fitverse.AgreementsService.MessageBus.Senders
{
	public class AddMembershipSender : IAddMembershipSender
	{
		private readonly IOptions<RabbitMqConfiguration> _rabbitMqOptions;

		public AddMembershipSender(IOptions<RabbitMqConfiguration> rabbitMqOptions)
		{
			_rabbitMqOptions = rabbitMqOptions;
		}

		public void SendMembership(Membership membership)
		{
			var exchangeConfig = new Tuple<string, string>("memberships", "addMembership");
			SendEventHandler.SendEvent(membership, _rabbitMqOptions, exchangeConfig);
		}
	}
}