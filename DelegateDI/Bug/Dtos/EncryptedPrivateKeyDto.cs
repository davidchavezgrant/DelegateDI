namespace DelegateDI.Bug.Dtos;

public sealed record EncryptedPrivateKeyDto(string? Version,
											string? Nonce,
											string? EphemPublicKey,
											string? Ciphertext)
{
	public static EncryptedPrivateKeyDto Empty => new(string.Empty,
													  string.Empty,
													  string.Empty,
													  string.Empty);
}