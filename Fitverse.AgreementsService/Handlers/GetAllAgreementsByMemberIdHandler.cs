using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Fitverse.AgreementsService.Data;
using Fitverse.AgreementsService.Dtos;
using Fitverse.AgreementsService.Helpers;
using Fitverse.AgreementsService.Queries;
using Mapster;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Fitverse.AgreementsService.Handlers
{
	public class
		GetAllAgreementsByMemberIdHandler : IRequestHandler<GetAllAgreementsByMemberIdQuery, List<AgreementDto>>
	{
		private readonly AgreementsContext _dbContext;

		public GetAllAgreementsByMemberIdHandler(AgreementsContext dbContext)
		{
			_dbContext = dbContext;
		}

		public async Task<List<AgreementDto>> Handle(GetAllAgreementsByMemberIdQuery request,
			CancellationToken cancellationToken)
		{
			var agreementsList = await _dbContext
				.Agreements
				.Where(m => m.MemberId == request.MemberId)
				.ToListAsync(cancellationToken);

			if (agreementsList is null)
				throw new NullReferenceException($"Agreements for member [MemberId: {request.MemberId}] not found");

			agreementsList = await AgreementPaymentValidator
				.IsPaidAsync(_dbContext, agreementsList, cancellationToken);

			return agreementsList.Select(agreement => agreement.Adapt<AgreementDto>()).ToList();
		}
	}
}