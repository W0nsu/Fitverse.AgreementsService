using System.Threading.Tasks;
using Fitverse.AgreementsService.Commands;
using Fitverse.AgreementsService.Dtos;
using Fitverse.AgreementsService.Models;
using Fitverse.AgreementsService.Queries;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;

namespace Fitverse.AgreementsService.Controllers
{
	[Authorize]
	[Route("api/as/agreements/settings")]
	[ApiController]
	public class SettingsController : Controller
	{
		private readonly IMediator _mediator;

		public SettingsController(IMediator mediator)
		{
			_mediator = mediator;
		}

		[HttpGet]
		public async Task<IActionResult> GetAllMemberships()
		{
			var query = new GetAllMembershipsQuery();
			var result = await _mediator.Send(query);
			return Ok(result);
		}

		[HttpPost]
		public async Task<IActionResult> AddMembership([FromBody] MembershipDto membershipDto)
		{
			var command = new AddMembershipCommand(membershipDto);
			var result = await _mediator.Send(command);
			return Ok(result);
		}

		[HttpGet("{id}")]
		public async Task<IActionResult> GetMembershipById([FromRoute] int id)
		{
			var query = new GetMembershipByIdQuery(id);
			var result = await _mediator.Send(query);
			return Ok(result);
		}

		[HttpPatch("{id}")]
		public async Task<IActionResult> EditMembership([FromRoute] int id,
			[FromBody] JsonPatchDocument<Membership> membershipEntity)
		{
			var command = new EditMembershipCommand(id, membershipEntity);
			var result = await _mediator.Send(command);
			return Ok(result);
		}

		[HttpDelete("{id}")]
		public async Task<IActionResult> DeleteMembership([FromRoute] int id)
		{
			var command = new DeleteMembershipCommand(id);
			var result = await _mediator.Send(command);
			return Ok($"Membership [MembershipId: {result.MembershipId}] has been deleted");
		}
	}
}