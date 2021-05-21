using System.Threading;
using System.Threading.Tasks;
using Fitverse.AgreementsService.Commands;
using Fitverse.AgreementsService.Data;
using Fitverse.AgreementsService.Dtos;
using Mapster;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Fitverse.AgreementsService.Handlers
{
	public class PayForInstallmentHandler : IRequestHandler<PayForInstallmentCommand, InstallmentDto>
	{
		private readonly AgreementsContext _dbContext;

		public PayForInstallmentHandler(AgreementsContext dbContext)
		{
			_dbContext = dbContext;
		}

		public async Task<InstallmentDto> Handle(PayForInstallmentCommand request, CancellationToken cancellationToken)
		{
			var installmentEntity = await _dbContext
				.Installments
				.SingleOrDefaultAsync(m => m.Id == request.InstallmentId, cancellationToken);

			if (installmentEntity.IsPaid == false)
				installmentEntity.IsPaid = true;
			else
				installmentEntity.IsPaid = false;

			installmentEntity.IsChecked = false;
			_ = await _dbContext.SaveChangesAsync(cancellationToken);

			var installmentDto = installmentEntity.Adapt<InstallmentDto>();

			return installmentDto;
		}
	}
}