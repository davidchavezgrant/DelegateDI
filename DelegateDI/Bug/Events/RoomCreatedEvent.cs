using DelegateDI.Bug.Entities;


namespace DelegateDI.Bug.Events;

public sealed record RoomCreatedEvent(DirectMessageDto DirectMessageDto, ChatMessageDto? FirstMessage): DomainEvent(DateTime.UtcNow);