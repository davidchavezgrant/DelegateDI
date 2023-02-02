namespace DelegateDI.Bug;

[Flags]
public enum UserPermissions { NONE = 0, NOTIFIER = 1 }

public enum ChannelType { DIRECTMESSAGE, COMMUNITY, BROADCAST }

public enum NotificationFrequency { UNDEFINED = 0, ALWAYS = 1, NEVER = 2 }

public enum Permission
{
	OWNER,
	ADMIN,
	READER,
	WRITER
}