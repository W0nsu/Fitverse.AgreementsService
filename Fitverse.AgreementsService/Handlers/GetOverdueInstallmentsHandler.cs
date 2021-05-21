using System;
using System.Collections.Generic;
using System.Linq;
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
	public class
		GetOverdueInstallmentsHandler : IRequestHandler<GetOverdueInstallmentsQuery, List<OverdueInstallmentDto>>
	{
		private readonly AgreementsContext _dbContext;

		public GetOverdueInstallmentsHandler(AgreementsContext dbContext)
		{
			_dbContext = dbContext;
		}

		public async Task<List<OverdueInstallmentDto>> Handle(GetOverdueInstallmentsQuery request,
			CancellationToken cancellationToken)
		{
			var overdueInstallmentsList = await _dbContext
				.Installments
				.Where(x => x.IsPaid == false && x.DueDate < DateTime.Today)
				.OrderBy(x => x.DueDate)
				.ToListAsync(cancellationToken);

			var overdueInstallmentsDtoList = new List<OverdueInstallmentDto>();

			foreach (var installment in overdueInstallmentsList)
			{
				var agreement = await _dbContext
					.Agreements
					.FirstOrDefaultAsync(x => x.AgreementId == installment.AgreementId,
						cancellationToken);

				var member = await _dbContext
					.Members
					.FirstOrDefaultAsync(x => x.MemberId == agreement.MemberId, cancellationToken);

				var installmentDto = installment.Adapt<InstallmentDto>();

				overdueInstallmentsDtoList.Add(
					new OverdueInstallmentDto
					{
						MemberFirstName = member.Name ?? "",
						MemberSurname = member.SurName ?? "",
						MembershipName = agreement.Name ?? "",
						InstallmentDetails = installmentDto
					}
				);
			}

			return overdueInstallmentsDtoList;
		}
	}
}