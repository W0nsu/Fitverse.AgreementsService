using System;
using System.Linq;
using Fitverse.AgreementsService.Data;
using Fitverse.AgreementsService.Models;
using Fitverse.Shared.Helpers;

namespace Fitverse.AgreementsService.Helpers
{
	public static class EndingDateCalculator
	{
		private static AgreementsContext _dbContext;
		private static Agreement _agreement;
		private static PeriodType _periodType;
		private static int _duration;
		private static int _terminationPeriod;

		public static void SetEndingDate(Agreement agreement, AgreementsContext dbContext)
		{
			_dbContext = dbContext;
			_agreement = agreement;

			(_periodType, _duration, _terminationPeriod) = GetMembershipDetails();

			agreement.EndingDate = CalculateEndingDate();
			agreement.TerminationPeriod = _terminationPeriod;
		}

		private static DateTime CalculateEndingDate()
		{
			var startingDate = _agreement.StartingDate;

			switch (_periodType)
			{
				case PeriodType.Day:
					return startingDate.AddDays(_duration)
						.AddDays(-1);
				case PeriodType.Month:
					return startingDate.AddMonths(_duration)
						.AddDays(-1);
				case PeriodType.Year:
					return startingDate.AddYears(_duration)
						.AddDays(-1);
				default:
					throw new ArgumentException($"Membership period [period: {_periodType}] do not exists.");
			}
		}

		private static Tuple<PeriodType, int, int> GetMembershipDetails()
		{
			var membershipEntity = _dbContext
				.Memberships
				.SingleOrDefault(m => m.MembershipId == _agreement.MembershipId);

			if (membershipEntity is null)
				throw new NullReferenceException($"Membership [MembershipId: {_agreement.MembershipId} not found]");

			var membershipDetails =
				new Tuple<PeriodType, int, int>((PeriodType) membershipEntity.PeriodType, membershipEntity.Duration,
					membershipEntity.TerminationPeriod);

			return membershipDetails;
		}
	}
}