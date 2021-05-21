using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Fitverse.AgreementsService.Data;
using Fitverse.AgreementsService.Models;
using Microsoft.EntityFrameworkCore;

namespace Fitverse.AgreementsService.Helpers
{
	public static class AgreementPaymentValidator
	{
		public static async Task<List<Agreement>> IsPaidAsync(AgreementsContext dbContext,
			List<Agreement> agreementsList,
			CancellationToken cancellationToken = default)
		{
			foreach (var agreement in agreementsList)
			{
				var installmentsList = await dbContext
					.Installments
					.Where(i => i.AgreementId == agreement.AgreementId).ToListAsync(cancellationToken);

				var installmentsStatuses = new List<bool>();

				foreach (var installment in installmentsList.Where(installment =>
					installment.DueDate.Date < DateTime.Today.Date && !installment.IsChecked))
				{
					installmentsStatuses.Add(installment.IsPaid);
					installment.IsChecked = true;
				}

				agreement.IsPaid = installmentsStatuses.All(x => x);
			}

			return agreementsList;
		}
	}
}