using System;
using Fitverse.AgreementsService.Interfaces;
using Fitverse.AgreementsService.Models;
using Fitverse.Shared.MessageBus;
using Microsoft.Extensions.Options;

namespace Fitverse.AgreementsService.MessageBus.Senders
{
	public class EditMembershipSender : IEditMembershipSender
	{
		private readonly IOptions<RabbitMqConfiguration> _rabbitMqOptions;

		public EditMembershipSender(IOptions<RabbitMqConfiguration> rabbitMqOptions)
		{
			_rabbitMqOptions = rabbitMqOptions;
		}

		public void EditMembership(Membership membership)
		{
			var exchangeConfig = new Tuple<string, string>("memberships", "editMembership");
			SendEventHandler.SendEvent(membership, _rabbitMqOptions, exchangeConfig);
		}
	}
}