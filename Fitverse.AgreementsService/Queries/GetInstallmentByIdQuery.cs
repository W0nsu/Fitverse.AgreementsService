using Fitverse.AgreementsService.Dtos;
using MediatR;

namespace Fitverse.AgreementsService.Queries
{
	public class GetInstallmentByIdQuery : IRequest<InstallmentDto>
	{
		public GetInstallmentByIdQuery(int installmentId)
		{
			InstallmentId = installmentId;
		}

		public int InstallmentId { get; }
	}
}