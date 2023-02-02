using DelegateDI.Bug.Dtos;
using DelegateDI.Bug.Entities;


namespace DelegateDI.Bug.Events;

public sealed record MessageSentEvent(ChatMessageDto Dto, string?[] RecipientWalletAddresses): DomainEvent(DateTime.UtcNow);