using Bogus;
using DelegateDI.Bug.DAL;
using DelegateDI.Bug.Entities;
using Microsoft.EntityFrameworkCore;


namespace DelegateDI.Bug.Services;

internal sealed class ChannelService: IChannelService
{
	public const string DUPLICATE_MEMBER_ERROR      = "room cannot have duplicate members";
	public const string ROOM_MEMBER_NOT_FOUND_ERROR = "Room member profile not found";

	private readonly IDbContextFactory<ChatDbContext> _dbFactory;
	private readonly ILogger<ChannelService>          _logger;
	private          Faker                            _faker = new("en");

	public ChannelService(IDbContextFactory<ChatDbContext> dbFactory, ILogger<ChannelService> logger)
	{
		this._dbFactory = dbFactory;
		this._logger    = logger;
	}

	public void GuardAgainstDuplicateMembers(NewRoomRequest request)
	{
		var set = new HashSet<Guid>();
		foreach (var s in request.MembersAndPermissions)
		{
			if (!set.Add(s.Item1))
			{
				this._logger.LogError(DUPLICATE_MEMBER_ERROR);
				this._logger.LogError("{Owner}", s);
				throw new ArgumentException(DUPLICATE_MEMBER_ERROR);
			}
		}
	}

	public async Task<Guid> CreateChannel(NewRoomRequest request, string? channelName)
	{
		this.GuardAgainstDuplicateMembers(request);
		await using var db = await this._dbFactory.CreateDbContextAsync();

		string uniqueChannelName = await this.SetName(request, channelName, db);
		var    newRoomMembers    = await this.GetRoomMembers(request, db);

		if (request.Type == ChannelType.COMMUNITY)
		{
			var newCommunityChannel = await CommunityChannel.Create(request.PublicChannel,
																    newRoomMembers,
																    request.ApplicationId,
																    null);
			newCommunityChannel.Name = uniqueChannelName;
			db.CommunityChannels.Add(newCommunityChannel);
			await db.SaveChangesAsync();
			return newCommunityChannel.Id;
		}

		var newBroadcastChannel = await BroadcastChannel.Create(request.PublicChannel,
															    newRoomMembers,
															    request.ApplicationId,
															    null);
		newBroadcastChannel.Name = uniqueChannelName;
		db.BroadcastChannels.Add(newBroadcastChannel);
		await db.SaveChangesAsync();
		return newBroadcastChannel.Id;
	}

	private async Task<List<UserChannelConfiguration>> GetRoomMembers(NewRoomRequest request, ChatDbContext db)
	{
		List<UserChannelConfiguration> newRoomMembers = new();

		foreach (var memberAndPermission in request.MembersAndPermissions)
		{
			var memberUserProfile = await db.Profiles.FindAsync(memberAndPermission.Item1);
			if (memberUserProfile is null)
			{
				this._logger.LogError(ROOM_MEMBER_NOT_FOUND_ERROR);
				this._logger.LogError("{Id}", memberAndPermission.Item1);
				throw new ArgumentException(ROOM_MEMBER_NOT_FOUND_ERROR);
			}

			var channelPermission = new ChannelPermission(memberAndPermission.Item2);
			var newChannelMember  = UserChannelConfiguration.Create(memberUserProfile, channelPermission);
			newRoomMembers.Add(newChannelMember);
		}

		return newRoomMembers;
	}

	private async Task<string> SetName(NewRoomRequest request, string? channelName, ChatDbContext db)
	{
		//generate a random channel name, that is unique to the app 
		while (channelName == null)
		{
			channelName = "Channel#" + this._faker.Random.Number(1000, 9999);
			var channelWithSameName = await db.Channels.Where(c => c.Name == channelName && c.ApplicationId == request.ApplicationId).FirstOrDefaultAsync();
			if (channelWithSameName != null)
				channelName = null;
		}

		return channelName;
	}
}