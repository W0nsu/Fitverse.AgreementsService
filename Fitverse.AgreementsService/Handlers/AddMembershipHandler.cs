using System;
using System.Threading;
using System.Threading.Tasks;
using Fitverse.AgreementsService.Commands;
using Fitverse.AgreementsService.Data;
using Fitverse.AgreementsService.Dtos;
using Fitverse.AgreementsService.Interfaces;
using Fitverse.AgreementsService.Models;
using Mapster;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Fitverse.AgreementsService.Handlers
{
	public class AddMembershipHandler : IRequestHandler<AddMembershipCommand, MembershipDto>
	{
		private readonly IAddMembershipSender _addMembershipSender;
		private readonly AgreementsContext _dbContext;

		public AddMembershipHandler(AgreementsContext dbContext, IAddMembershipSender addMembershipSender)
		{
			_dbContext = dbContext;
			_addMembershipSender = addMembershipSender;
		}

		public async Task<MembershipDto> Handle(AddMembershipCommand request, CancellationToken cancellationToken)
		{
			var name = request.Membership.Name;

			var membershipEntity = request.Membership.Adapt<Membership>();
			membershipEntity.IsDeleted = false;

			_ = await _dbContext.AddAsync(membershipEntity, cancellationToken);
			_ = await _dbContext.SaveChangesAsync(cancellationToken);

			var newMembership = await _dbContext
				.Memberships
				.SingleOrDefaultAsync(m => m.Name == name, cancellationToken);

			if (newMembership is null)
				throw new NullReferenceException("Failed to add membership. Try again");

			_addMembershipSender.SendMembership(newMembership);

			var newMembershipDto = newMembership.Adapt<MembershipDto>();
			return newMembershipDto;
		}
	}
}