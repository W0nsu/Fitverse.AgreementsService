using Fitverse.AgreementsService.Models;

namespace Fitverse.AgreementsService.Interfaces
{
	public interface IAddMembershipSender
	{
		public void SendMembership(Membership membership);
	}
}