using System;
using System.Threading;
using System.Threading.Tasks;
using Fitverse.AgreementsService.Data;
using Fitverse.AgreementsService.Dtos;
using Fitverse.AgreementsService.Queries;
using Mapster;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Fitverse.AgreementsService.Handlers
{
	public class GetAgreementByIdHandler : IRequestHandler<GetAgreementByIdQuery, AgreementDto>
	{
		private readonly AgreementsContext _dbContext;

		public GetAgreementByIdHandler(AgreementsContext dbContext)
		{
			_dbContext = dbContext;
		}

		public async Task<AgreementDto> Handle(GetAgreementByIdQuery request, CancellationToken cancellationToken)
		{
			var agreementEntity = await _dbContext
				.Agreements
				.SingleOrDefaultAsync(m => m.AgreementId == request.AgreementId, cancellationToken);

			if (agreementEntity is null)
				throw new NullReferenceException($"Agreement [AgreementId: {request.AgreementId} not found]");

			var agreementDto = agreementEntity.Adapt<AgreementDto>();

			return agreementDto;
		}
	}
}