using System.Linq;
using Fitverse.AgreementsService.Commands;
using Fitverse.AgreementsService.Data;
using FluentValidation;

namespace Fitverse.AgreementsService.Validators
{
	public class PayForInstallmentCommandValidator : AbstractValidator<PayForInstallmentCommand>
	{
		public PayForInstallmentCommandValidator(AgreementsContext dbContext)
		{
			RuleFor(x => x.InstallmentId)
				.GreaterThan(0);

			RuleFor(x => x.InstallmentId)
				.Must(id => dbContext.Installments.Any(m => m.Id == id))
				.WithMessage(x => $"Installment [InstallmentId: {x.InstallmentId}] not found.");
		}
	}
}