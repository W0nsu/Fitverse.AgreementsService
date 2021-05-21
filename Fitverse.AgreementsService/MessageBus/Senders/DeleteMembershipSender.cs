using System;
using Fitverse.AgreementsService.Interfaces;
using Fitverse.Shared.MessageBus;
using Microsoft.Extensions.Options;

namespace Fitverse.AgreementsService.MessageBus.Senders
{
	public class DeleteMembershipSender : IDeleteMembershipSender
	{
		private readonly IOptions<RabbitMqConfiguration> _rabbitMqOptions;

		public DeleteMembershipSender(IOptions<RabbitMqConfiguration> rabbitMqOptions)
		{
			_rabbitMqOptions = rabbitMqOptions;
		}

		public void DeleteMembership(int membershipId)
		{
			var exchangeConfig = new Tuple<string, string>("memberships", "deleteMembership");
			SendEventHandler.SendEvent(membershipId, _rabbitMqOptions, exchangeConfig);
		}
	}
}