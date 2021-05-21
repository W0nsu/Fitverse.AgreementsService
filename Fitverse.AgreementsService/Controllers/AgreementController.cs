using System;
using System.Threading.Tasks;
using Fitverse.AgreementsService.Commands;
using Fitverse.AgreementsService.Queries;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Fitverse.AgreementsService.Controllers
{
	[Authorize]
	[Route("/api/as/agreements")]
	[ApiController]
	public class AgreementController : Controller
	{
		private readonly IMediator _mediator;

		public AgreementController(IMediator mediator)
		{
			_mediator = mediator;
		}

		[HttpGet("member/{memberId}")]
		public async Task<IActionResult> GetAllAgreementsByMemberId([FromRoute] int memberId)
		{
			var query = new GetAllAgreementsByMemberIdQuery(memberId);
			var result = await _mediator.Send(query);
			return Ok(result);
		}

		[HttpGet("{agreementId}")]
		public async Task<IActionResult> GetAgreementById([FromRoute] int agreementId)
		{
			var query = new GetAgreementByIdQuery(agreementId);
			var result = await _mediator.Send(query);
			return Ok(result);
		}

		[HttpPost("terminate/{agreementId}")]
		public async Task<IActionResult> TerminateAgreement([FromRoute] int agreementId,
			[FromForm] DateTime terminationDate)
		{
			var command = new TerminateAgreementCommand(agreementId, terminationDate);
			var result = await _mediator.Send(command);
			return Ok(result);
		}

		[HttpPost("end/{agreementId}")]
		public async Task<IActionResult> EndAgreement([FromRoute] int agreementId, [FromForm] DateTime endDate)
		{
			var command = new EndAgreementCommand(agreementId, endDate);
			var result = await _mediator.Send(command);
			return Ok(result);
		}
	}
}