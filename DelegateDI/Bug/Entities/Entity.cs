using System.ComponentModel.DataAnnotations.Schema;


namespace DelegateDI.Bug.Entities;

public abstract class Entity
{
	private readonly List<DomainEvent> _events = new();

	[NotMapped]
	public IReadOnlyCollection<DomainEvent> Events => this._events.AsReadOnly();

	public Guid Id { get; set; }

	public void ClearEvents() => this._events.Clear();

	protected void RaiseEvent(DomainEvent @event) => this._events.Add(@event);
}