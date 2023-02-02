using DelegateDI.Bug.Dtos;


namespace DelegateDI.Bug;

public interface IChannelService
{
	Task<Guid> CreateChannel(NewRoomRequest request, string? channelName);

	void GuardAgainstDuplicateMembers(NewRoomRequest request);
}