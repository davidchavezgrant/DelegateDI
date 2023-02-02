#pragma warning disable CS8618
namespace DelegateDI.Bug.Entities;

internal sealed class UserChannelConfiguration: Entity
{
	public UserChannelConfiguration(UserProfile profile, ChannelPermission permission)
	{
		this.UserProfile    = profile;
		this.Permissions    = permission;
	}
	
	public static UserChannelConfiguration Create(UserProfile profile, ChannelPermission permission) 
		=> new (profile, permission);

	private UserChannelConfiguration() {}
	public UserProfile UserProfile { get; private set;}
	public int         NumberUnreadMessages { get; set;  }

	public ChannelPermission Permissions { get; private set; }

	public NotificationFrequency NotificationFrequency { get; set; } = NotificationFrequency.UNDEFINED;
	
	public Guid AbstractChannelId { get; set; }
}