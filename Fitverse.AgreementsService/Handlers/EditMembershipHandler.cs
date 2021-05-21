using System;
using System.Threading;
using System.Threading.Tasks;
using Fitverse.AgreementsService.Commands;
using Fitverse.AgreementsService.Data;
using Fitverse.AgreementsService.Dtos;
using Fitverse.AgreementsService.Interfaces;
using Mapster;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Fitverse.AgreementsService.Handlers
{
	public class EditMembershipHandler : ControllerBase, IRequestHandler<EditMembershipCommand, MembershipDto>
	{
		private readonly AgreementsContext _dbContext;
		private readonly IEditMembershipSender _deleteMembershipSender;

		public EditMembershipHandler(AgreementsContext dbContext, IEditMembershipSender deleteMembershipSender)
		{
			_deleteMembershipSender = deleteMembershipSender;
			_dbContext = dbContext;
		}

		public async Task<MembershipDto> Handle(EditMembershipCommand request, CancellationToken cancellationToken)
		{
			var membershipEntity = await _dbContext
				.Memberships
				.SingleOrDefaultAsync(m => m.MembershipId == request.Id && !m.IsDeleted, cancellationToken);

			if (membershipEntity is null)
				throw new NullReferenceException($"Membership [MembershipId: {request.Id}] not found");

			var editedMembership = request.Membership;

			editedMembership.ApplyTo(membershipEntity, ModelState);
			_ = await _dbContext.SaveChangesAsync(cancellationToken);

			var patchedMembershipEntity = await _dbContext
				.Memberships
				.SingleOrDefaultAsync(m => m.MembershipId == request.Id && !m.IsDeleted, cancellationToken);

			if (patchedMembershipEntity is null)
				throw new NullReferenceException($"Failed to fetch patched membership [MembershipId: {request.Id}]");

			_deleteMembershipSender.EditMembership(patchedMembershipEntity);

			var patchedMembershipDto = patchedMembershipEntity.Adapt<MembershipDto>();

			return patchedMembershipDto;
		}
	}
}