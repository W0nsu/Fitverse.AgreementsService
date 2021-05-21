using System;
using System.Collections.Generic;
using System.Linq;
using Fitverse.AgreementsService.Data;
using Fitverse.AgreementsService.Models;
using Fitverse.Shared.Helpers;

namespace Fitverse.AgreementsService.Helpers
{
	public class InstallmentGenerator
	{
		private readonly AgreementsContext _dbContext;
		private readonly List<Installment> _installmentsList = new();

		private Membership _membershipEntity;

		public InstallmentGenerator(AgreementsContext dbContext)
		{
			_dbContext = dbContext;
		}

		public void AddInstallments(Agreement agreement)
		{
			GenerateInstallments(agreement);
			foreach (var installment in _installmentsList)
				_dbContext.Installments.Add(installment);

			_ = _dbContext.SaveChanges();
		}

		public void DeleteInstallments(Agreement agreement)
		{
			var installmentsList = _dbContext
				.Installments
				.Where(x => x.AgreementId == agreement.AgreementId).ToList();

			foreach (var installment in installmentsList)
				_dbContext.Remove(installment);

			_dbContext.SaveChanges();
		}

		private void GenerateInstallments(Agreement agreement)
		{
			_membershipEntity = GetMembership(agreement);

			for (var i = 0; i < _membershipEntity.Duration; i++)
			{
				if (i == 0)
				{
					_installmentsList.Add(new Installment
					{
						AgreementId = agreement.AgreementId,
						Price = _membershipEntity.InstallmentPrice,
						StartingDate = agreement.StartingDate,
						EndingDate = CalculateInstallmentEndingDate(agreement.StartingDate),
						DueDate = agreement.StartingDate,
						IsPaid = false,
						IsChecked = false
					});
				}
				else
				{
					var installmentStartingDate = _installmentsList[i - 1].EndingDate.AddDays(1);
					_installmentsList.Add(new Installment
					{
						AgreementId = agreement.AgreementId,
						Price = _membershipEntity.InstallmentPrice,
						StartingDate = installmentStartingDate,
						EndingDate = CalculateInstallmentEndingDate(installmentStartingDate),
						DueDate = installmentStartingDate,
						IsPaid = false,
						IsChecked = false
					});
				}
			}
		}

		private DateTime CalculateInstallmentEndingDate(DateTime installmentStartingDate)
		{
			switch ((PeriodType) _membershipEntity.PeriodType)
			{
				case PeriodType.Day:
					return installmentStartingDate;
				case PeriodType.Month:
					return installmentStartingDate.AddMonths(1)
						.AddDays(-1);
				case PeriodType.Year:
					return installmentStartingDate.AddYears(1)
						.AddDays(-1);
				default:
					throw new ArgumentException(
						$"Membership period [period: {_membershipEntity.PeriodType}] do not exists.");
			}
		}

		private Membership GetMembership(Agreement agreement)
		{
			_membershipEntity = _dbContext
				.Memberships
				.SingleOrDefault(m => m.MembershipId == agreement.MembershipId);

			if (_membershipEntity is null)
				throw new NullReferenceException($"Membership [MembershipId: {agreement.MembershipId} not found]");

			return _membershipEntity;
		}
	}
}