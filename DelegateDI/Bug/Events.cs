namespace DelegateDI.Bug;

internal sealed record ChannelCreatedEvent(ChannelDto ChannelDto): DomainEvent(DateTime.UtcNow);

internal sealed record MessageSentEvent(ChatMessageDto Dto, string?[] RecipientWalletAddresses): DomainEvent(DateTime.UtcNow);

internal sealed record RoomCreatedEvent(DirectMessageDto DirectMessageDto, ChatMessageDto? FirstMessage): DomainEvent(DateTime.UtcNow);

public abstract record DomainEvent(DateTime DateOccurred);