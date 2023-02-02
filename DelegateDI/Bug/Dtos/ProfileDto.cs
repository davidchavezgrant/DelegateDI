namespace DelegateDI.Bug;

public sealed record ProfileDto(Guid    ProfileId,
								string  WalletAddress,
								string? DisplayName,
								string? EnsDomain,
								string? EncryptionPublicKey,
								string? Email = null,
								Uri?    AvatarUri = null,
								UserPermissions Permissions = UserPermissions.NONE)
{
	public static ProfileDto Empty => new(Guid.Empty,
										  string.Empty,
										  string.Empty,
										  string.Empty,
										  null,
										  null,
										  null,
										  UserPermissions.NONE);
	
}

public static class DtoExtensions
{
	public static string GetDefaultName(this ProfileDto dto)
	{
		if (!string.IsNullOrWhiteSpace(dto.DisplayName))
			return dto.DisplayName;
		if (!string.IsNullOrWhiteSpace(dto.EnsDomain))
			return dto.EnsDomain;
		return dto.WalletAddress;
	}
}

[Flags]
public enum UserPermissions
{
	NONE = 0,
	NOTIFIER = 1
}