using System.Threading.Tasks;
using Fitverse.AgreementsService.Commands;
using Fitverse.AgreementsService.Queries;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Fitverse.AgreementsService.Controllers
{
	[Authorize]
	[ApiController]
	[Route("/api/as/agreements")]
	public class InstallmentController : Controller
	{
		private readonly IMediator _mediator;

		public InstallmentController(IMediator mediator)
		{
			_mediator = mediator;
		}

		[HttpGet("{agreementId}/installments")]
		public async Task<IActionResult> GetAllInstallmentsByAgreementId([FromRoute] int agreementId)
		{
			var query = new GetAllInstallmentsByAgreementIdQuery(agreementId);
			var result = await _mediator.Send(query);
			return Ok(result);
		}

		[HttpGet("installments/{installmentId}")]
		public async Task<IActionResult> GetInstallmentById([FromRoute] int installmentId)
		{
			var query = new GetInstallmentByIdQuery(installmentId);
			var result = await _mediator.Send(query);
			return Ok(result);
		}

		[HttpPut("installments/{installmentId}/pay")]
		public async Task<IActionResult> PayForInstallment(int installmentId)
		{
			var command = new PayForInstallmentCommand(installmentId);
			var result = await _mediator.Send(command);
			return Ok(result);
		}

		[HttpGet("installments/overdue")]
		public async Task<IActionResult> GetOverdueInstallments()
		{
			var query = new GetOverdueInstallmentsQuery();
			var result = await _mediator.Send(query);
			return Ok(result);
		}
	}
}