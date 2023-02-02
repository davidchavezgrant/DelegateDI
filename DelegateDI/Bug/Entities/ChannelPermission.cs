namespace DelegateDI.Bug.Entities;

public class ChannelPermission: Entity
{
	private ChannelPermission()
	{
	}
	public ChannelPermission(Permission p)
	{
		this.Permission = p;
	}
	public Permission Permission { get; set; }
}