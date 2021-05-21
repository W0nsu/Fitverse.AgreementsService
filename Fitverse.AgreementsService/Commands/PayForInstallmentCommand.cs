using Fitverse.AgreementsService.Dtos;
using MediatR;

namespace Fitverse.AgreementsService.Commands
{
	public class PayForInstallmentCommand : IRequest<InstallmentDto>
	{
		public PayForInstallmentCommand(int installmentId)
		{
			InstallmentId = installmentId;
		}

		public int InstallmentId { get; }
	}
}