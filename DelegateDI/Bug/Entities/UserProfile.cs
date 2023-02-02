namespace DelegateDI.Bug.Entities;

internal sealed class UserProfile: Entity
{
	private UserProfile() {}

	private UserProfile(Guid id, string? walletAddress, string? displayName)
	{
		this.Id            = id;
		this.WalletAddress = walletAddress;
		this.DisplayName   = displayName;
		this.IsBylinesUser = true;
	}
	
	private UserProfile(Guid id, string? walletAddress, Guid? appId, string? displayName, bool isBylinesUser)
	{
		this.Id            = id;
		this.WalletAddress = walletAddress;
		this.ApplicationId = appId;
		this.DisplayName   = displayName;
		this.IsBylinesUser = isBylinesUser;
	}
	
	private UserProfile(Guid id, string? walletAddress, Guid? appId, string? displayName, bool isBylinesUser, UserPermissions permissions)
	{
		this.Id            = id;
		this.WalletAddress = walletAddress;
		this.ApplicationId = appId;
		this.DisplayName   = displayName;
		this.IsBylinesUser = isBylinesUser;
		this.Permissions   = permissions;
	}

	public void SetIsBylUserToTrue()
	{
		this.IsBylinesUser = true;
	}

	public bool   IsBylinesUser { get; private set; } = false;
	public string WalletAddress { get; set; }
	
	public string?       EmailAddress  { get; set; }

	public UserPermissions Permissions   { get; set; } = 0;
	public Guid?            ApplicationId { get; private set; }
	public string?          DisplayName   { get; private set; }
	public List<Contact>    Contacts      { get; private set; } = new();
	
	public static UserProfile Create(string? walletAddress, Guid? appId, string? displayName = null) =>
		new(Guid.NewGuid(), walletAddress, appId, displayName, true);
	
	public static UserProfile CreateAnonymousUserProfile(string walletAddress, Guid? appId, string? displayName = null) =>
		new(Guid.NewGuid(), walletAddress, appId, displayName, false);

	public ProfileDto ToDto(string? ensDomain, string? encryptionPublicKey) => new(this.Id, this.WalletAddress, this.DisplayName, ensDomain, encryptionPublicKey, this.EmailAddress, null, this.Permissions);
}

