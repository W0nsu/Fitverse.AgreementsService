using System;
using System.Linq;
using Fitverse.AgreementsService.Commands;
using Fitverse.AgreementsService.Data;
using Fitverse.Shared.Helpers;
using FluentValidation;

namespace Fitverse.AgreementsService.Validators
{
	public class AddMembershipCommandValidator : AbstractValidator<AddMembershipCommand>
	{
		public AddMembershipCommandValidator(AgreementsContext dbContext)
		{
			RuleFor(x => x.Membership.Name)
				.NotEmpty()
				.MinimumLength(3)
				.MaximumLength(30);

			RuleFor(x => x.Membership.Name)
				.Must(name => !dbContext.Memberships.Any(m => m.Name == name))
				.WithMessage(x => $"Name [name: {x.Membership.Name}] already in use");

			RuleFor(x => x.Membership.PeriodType)
				.Must(periodType => Enum.IsDefined(typeof(PeriodType), periodType))
				.WithMessage(
					x =>
						$"PeriodType [periodType: {x.Membership.PeriodType}] not valid. Possible values: [Day = 0, Month = 1, Year = 2]");

			RuleFor(x => x.Membership.Duration)
				.GreaterThan(0);

			RuleFor(x => x.Membership.TerminationPeriod)
				.GreaterThan(0);

			RuleFor(x => x.Membership.InstallmentPrice)
				.GreaterThan(0);
		}
	}
}