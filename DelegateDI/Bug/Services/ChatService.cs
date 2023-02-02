using DelegateDI.Bug.DAL;
using DelegateDI.Bug.Entities;
using Microsoft.EntityFrameworkCore;
using NodaTime;


namespace DelegateDI.Bug.Services;

internal sealed class ChatService: IChatApiContract
{
	private const string SENDER_NOT_FOUND_ERROR      = "Sender profile not found";
	private const string ROOM_MEMBER_NOT_FOUND_ERROR = "Room member profile not found";
	private const string DUPLICATE_DM_ROOM_ERROR     = "Direct message room already exists";

	private const string INVALID_SENDER_ERROR = "first message sender must be a member of the room or a sysadmin account";

	private const    string                                NULL_SENDER_ERROR       = "null first message sender with valid message";
	private const    string                                PROFILE_NOT_FOUND_ERROR = "Profile not found";
	private readonly IChannelService                       _channelService;
	private readonly IDbContextFactory<ChatDbContext>      _dbFactory;
	private readonly GetEnsDomain                          _getEnsDomain;
	private readonly ILogger<ChatService>                  _logger;
	private readonly IClock                                _nodaClock;
	private readonly IProfileService                       _profileService;
	private readonly GetEncryptionPublicKeyByWalletAddress _publicKeyProvider;

	public ChatService(IDbContextFactory<ChatDbContext>      dbFactory,
					   IChannelService                       channelService,
					   IProfileService                       profileService,
					   GetEnsDomain                          getEnsDomain,
					   GetEncryptionPublicKeyByWalletAddress publicKeyProvider,
					   IClock                                nodaClock,
					   ILogger<ChatService>                  logger)

	{
		this._getEnsDomain      = getEnsDomain;
		this._channelService    = channelService;
		this._profileService    = profileService;
		this._nodaClock         = nodaClock;
		this._publicKeyProvider = publicKeyProvider;
		this._dbFactory         = dbFactory;
		this._logger            = logger;
	}

	public async Task<Guid> UpsertProfile(string walletAddress, Guid appId) => await this._profileService.UpsertProfile(walletAddress, appId);

	public async Task<Guid> CreateRoom(NewRoomRequest request)
	{
		if (request.Type == ChannelType.COMMUNITY) return await this._channelService.CreateChannel(request, null);
		if (request.Type == ChannelType.BROADCAST) return await this._channelService.CreateChannel(request, null);

		this._channelService.GuardAgainstDuplicateMembers(request);
		await using var db = await this._dbFactory.CreateDbContextAsync();

		var newRoomMembers = await this.GetRoomMembers(request, db);

		if (request.Type == ChannelType.DIRECTMESSAGE)
		{
			ChatRoom? alreadyExistingRoom = null;
			try

			{
				var query = db.ChatRooms.Where(c => c.ApplicationId == request.ApplicationId
												 && c.Members.Select(m => m.UserProfile.Id).Contains(request.MembersAndPermissions.FirstOrDefault().Item1)
												 && c.Members.Select(m => m.UserProfile.Id).Contains(request.MembersAndPermissions.LastOrDefault().Item1))
							  .AsAsyncEnumerable();

				alreadyExistingRoom = await query.FirstOrDefaultAsync();
			}
			catch (Exception e)
			{
				this._logger.LogError(e.ToString());
				throw new Exception(e.ToString());
			}

			if (alreadyExistingRoom is not null)
			{
				this._logger.LogError(DUPLICATE_DM_ROOM_ERROR);
				this._logger.LogError("{Id}.", alreadyExistingRoom.Id);
				return alreadyExistingRoom.Id;
			}

			var chatRoom = ChatRoom.Create(request.ApplicationId, newRoomMembers);
			db.ChatRooms.Add(chatRoom);

			//send first message
			if (request.FirstMessageSender != Guid.Empty && !string.IsNullOrWhiteSpace(request.FirstMessage))
			{
				var sender        = (Guid)request.FirstMessageSender;
				var senderProfile = await db.Profiles.Where(c => c.Id == sender).FirstOrDefaultAsync();
				if (senderProfile is null)
				{
					this._logger.LogError(SENDER_NOT_FOUND_ERROR);
					this._logger.LogError("Sender not found: {Id}.", sender);
					throw new ArgumentException(SENDER_NOT_FOUND_ERROR);
				}

				if (senderProfile.WalletAddress != "0x0000000000000000000000000000000000000000" && !chatRoom.Members.Select(m => m.UserProfile.Id).Contains(senderProfile.Id))
				{
					this._logger.LogError(INVALID_SENDER_ERROR);
					throw new ArgumentException(INVALID_SENDER_ERROR);
				}

				long currentInstant = this._nodaClock.GetCurrentInstant().ToUnixTimeTicks();
				var messageEntity = ChatMessage.Create(chatRoom,
													   senderProfile,
													   request.FirstMessage,
													   currentInstant);
				db.Add(messageEntity); /// TODO:  is this needed?
				await chatRoom.PushRoom(messageEntity,
									    "",
									    this._getEnsDomain,
									    this._publicKeyProvider);
			}
			else if (request.FirstMessageSender == Guid.Empty && !string.IsNullOrWhiteSpace(request.FirstMessage))
			{
				this._logger.LogError(NULL_SENDER_ERROR);
				throw new ArgumentException(NULL_SENDER_ERROR);
			}
			else
			{
				await chatRoom.PushRoom(null,
									    "",
									    this._getEnsDomain,
									    this._publicKeyProvider);
			}

			this._logger.LogInformation("Created Room between {First} and {Last}", request.MembersAndPermissions.First().Item1, request.MembersAndPermissions.Last().Item1);
			await db.SaveChangesAsync();
			return chatRoom.Id;
		}

		return Guid.Empty;
	}

	public async Task<IEnumerable<ChannelDto>> ListChannelsByProfileId(Guid profileId)
	{
		await using var db = await this._dbFactory.CreateDbContextAsync();

		var userChannelConfigs = await db.UserChannelConfigurations.Include(c => c.UserProfile).Where(p => p.UserProfile.Id == profileId).ToListAsync();

		var channels = await db.Channels.Where(c => c.Members.Any(m => userChannelConfigs.Contains(m))).Include(x => x.Members).ToListAsync();

		List<ChannelDto> results = new();
		foreach (var channel in channels)
		{
			var userConfig = channel.Members.Where(m => m.UserProfile.Id == profileId).FirstOrDefault();
			results.Add(await Channel.MakeDto(channel, userConfig, this._getEnsDomain));
		}

		return results.AsEnumerable();
	}

	public async Task<IEnumerable<DirectMessageDto>> ListRoomsByProfileId(Guid profileId)
	{
		await using var db = await this._dbFactory.CreateDbContextAsync();

		var profile = await db.Profiles.FindAsync(profileId);
		if (profile is null)
		{
			this._logger.LogError(PROFILE_NOT_FOUND_ERROR);
			this._logger.LogError("{@ProfileId}", profile);
			throw new ArgumentException(PROFILE_NOT_FOUND_ERROR);
		}

		var userChannelConfigs = await db.UserChannelConfigurations.Where(p => p.UserProfile.Id == profileId).ToListAsync();
		if (userChannelConfigs.Count == 0) return await Task.FromResult(Enumerable.Empty<DirectMessageDto>());

		var rooms = await db.ChatRooms.Include(r => r.Members).Where(r => r.Members.Any(m => userChannelConfigs.Contains(m))).ToListAsync();

		var results = new List<DirectMessageDto>();

		foreach (var room in rooms)
		{
			var userChannelConfig    = room.Members.FirstOrDefault(m => m.UserProfile.Id == profileId);
			int numberUnreadMessages = userChannelConfig?.NumberUnreadMessages ?? 0;
			results.Add(await ChatRoom.MakeDto((ChatRoom)room,
										       numberUnreadMessages,
										       this._getEnsDomain,
										       this._publicKeyProvider));
		}

		return results;
	}

	public async Task<ProfileDto> GetProfileById(Guid profileId)
	{
		await using var db         = await this._dbFactory.CreateDbContextAsync();
		var             profileDto = await this.GetProfile(db, profileId);
		return profileDto;
	}

	public async Task<IEnumerable<ProfileDto>> GetProfilesById(IEnumerable<Guid> ids)
	{
		await using var db      = await this._dbFactory.CreateDbContextAsync();
		var             members = new List<ProfileDto>();
		foreach (var id in ids)
		{
			var profileDto = await this.GetProfile(db, id);
			members.Add(profileDto);
		}

		if (members.Count() == 0) throw new ArgumentException("No Profiles found");

		return members;
	}

	private async Task<ProfileDto> GetProfile(ChatDbContext dbContext, Guid profileId)
	{
		var profile = await dbContext.Profiles.FindAsync(profileId);
		if (profile is null)
		{
			this._logger.LogError(PROFILE_NOT_FOUND_ERROR);
			this._logger.LogError("{profileId}", profile);
			return null;
		}

		string? ensDomain = await this._getEnsDomain(profile.WalletAddress);
		return profile.ToDto(ensDomain, "");
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
}