using System;
using System.Linq;
using Fitverse.AgreementsService.Commands;
using Fitverse.AgreementsService.Data;
using FluentValidation;

namespace Fitverse.AgreementsService.Validators
{
	public class EndAgreementCommandValidator : AbstractValidator<EndAgreementCommand>
	{
		public EndAgreementCommandValidator(AgreementsContext dbContext)
		{
			RuleFor(x => x.AgreementId)
				.GreaterThan(0);

			RuleFor(x => x.AgreementId)
				.NotEmpty();

			RuleFor(x => x.AgreementId)
				.Must(id => dbContext.Agreements.Any(m => m.AgreementId == id))
				.WithMessage(x => $"Agreement [AgreementId: {x.AgreementId}] not found.");

			RuleFor(x => x.EndDate)
				.Must(endDate => endDate >= DateTime.Now)
				.WithMessage($"Select a date later than {DateTime.Now.ToShortDateString()}");
		}
	}
}