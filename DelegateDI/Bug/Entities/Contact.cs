#pragma warning disable CS8618
namespace DelegateDI.Bug.Entities;

internal sealed class Contact: Entity
{
	private Contact() {}

	private Contact(Guid        id,
					UserProfile owner,
					Guid        contactProfileId,
					string      contactProfileWalletAddress,
					string      nickname)
	{
		this.Id               = id;
		this.Owner            = owner;
		this.ContactProfileId = contactProfileId;
		this.WalletAddress    = contactProfileWalletAddress;
		this.Nickname         = nickname;
	}

	public Guid        ContactProfileId { get; set; }
	public UserProfile Owner            { get; set; }
	public string      Nickname         { get; set; }
	public string      WalletAddress    { get; set; }

	public static Contact Create(UserProfile owner,
								 Guid        contactProfileId,
								 string      contactProfileWalletAddress,
								 string      nickname) => new(Guid.NewGuid(),
															  owner,
															  contactProfileId,
															  contactProfileWalletAddress,
															  nickname);

	public ContactDto ToDto() => new(this.Owner.Id,
									 this.ContactProfileId,
									 this.WalletAddress,
									 this.Nickname);
}