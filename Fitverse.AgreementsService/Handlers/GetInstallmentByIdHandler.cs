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
	public class GetInstallmentByIdHandler : IRequestHandler<GetInstallmentByIdQuery, InstallmentDto>
	{
		private readonly AgreementsContext _dbContext;

		public GetInstallmentByIdHandler(AgreementsContext dbContext)
		{
			_dbContext = dbContext;
		}

		public async Task<InstallmentDto> Handle(GetInstallmentByIdQuery request, CancellationToken cancellationToken)
		{
			var installmentEntity = await _dbContext
				.Installments
				.SingleOrDefaultAsync(m => m.Id == request.InstallmentId, cancellationToken);

			if (installmentEntity is null)
				throw new NullReferenceException($"Installment [InstallmentId: {request.InstallmentId} not found]");

			var installmentDto = installmentEntity.Adapt<InstallmentDto>();

			return installmentDto;
		}
	}
}