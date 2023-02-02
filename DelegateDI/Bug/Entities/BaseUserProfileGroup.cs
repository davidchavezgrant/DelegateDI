namespace DelegateDI.Bug.Entities;

internal class BaseUserProfileGroup: Entity
{
	
	public IReadOnlyCollection<UserProfile> Owners { get; internal init; } = new List<UserProfile>();
	
	
}