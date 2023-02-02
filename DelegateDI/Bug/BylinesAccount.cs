using Microsoft.AspNetCore.Identity;


namespace DelegateDI.Bug;

internal class BylinesAccount: IdentityUser
{
	public EncryptedPrivateKey EncryptedPrivateKey { get; set; } = null!;
	public string?              PublicKey           { get; set; } = null!;
	public string?             EnsDomain           { get; set; } = null;
}