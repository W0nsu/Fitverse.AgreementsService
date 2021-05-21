using System.Collections.Generic;
using Fitverse.AgreementsService.Dtos;
using MediatR;

namespace Fitverse.AgreementsService.Queries
{
	public class GetOverdueInstallmentsQuery : IRequest<List<OverdueInstallmentDto>>
	{
	}
}