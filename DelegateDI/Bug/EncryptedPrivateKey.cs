using DelegateDI.Bug.Dtos;


namespace DelegateDI.Bug;

internal sealed record EncryptedPrivateKey(string? Version,
										   string? Nonce,
										   string? EphemPublicKey,
										   string? Ciphertext);