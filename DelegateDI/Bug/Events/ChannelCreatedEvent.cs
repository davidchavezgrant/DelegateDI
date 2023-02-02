using DelegateDI.Bug.Dtos;
using DelegateDI.Bug.Entities;


namespace DelegateDI.Bug.Events;

public sealed record ChannelCreatedEvent(ChannelDto ChannelDto): DomainEvent(DateTime.UtcNow);