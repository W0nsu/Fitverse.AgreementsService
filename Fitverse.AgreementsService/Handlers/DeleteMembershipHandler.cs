using System.Threading;
using System.Threading.Tasks;
using Fitverse.AgreementsService.Commands;
using Fitverse.AgreementsService.Data;
using Fitverse.AgreementsService.Dtos;
using Fitverse.AgreementsService.Interfaces;
using Mapster;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Fitverse.AgreementsService.Handlers
{
	public class DeleteMembershipHandler : IRequestHandler<DeleteMembershipCommand, MembershipDto>
	{
		private readonly AgreementsContext _dbContext;
		private readonly IDeleteMembershipSender _deleteMembershipSender;

		public DeleteMembershipHandler(AgreementsContext dbContext, IDeleteMembershipSender deleteMembershipSender)
		{
			_deleteMembershipSender = deleteMembershipSender;
			_dbContext = dbContext;
		}

		public async Task<MembershipDto> Handle(DeleteMembershipCommand request, CancellationToken cancellationToken)
		{
			var membershipEntity = await _dbContext
				.Memberships
				.SingleOrDefaultAsync(m => m.MembershipId == request.MembershipId && !m.IsDeleted, cancellationToken);

			membershipEntity.IsDeleted = true;
			_ = await _dbContext.SaveChangesAsync(cancellationToken);

			_deleteMembershipSender.DeleteMembership(membershipEntity.MembershipId);

			var membershipDto = membershipEntity.Adapt<MembershipDto>();

			return membershipDto;
		}
	}
}