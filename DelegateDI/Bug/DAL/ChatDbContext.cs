using DelegateDI.Bug.Entities;
using Microsoft.EntityFrameworkCore;


namespace DelegateDI.Bug.DAL;

internal sealed class ChatDbContext: DbContext
{
	public ChatDbContext(DbContextOptions<ChatDbContext> options): base(options) {}

	public DbSet<UserProfile>              Profiles                  => this.Set<UserProfile>();
	public DbSet<Channel>                  Channels                  => this.Set<Channel>();
	public DbSet<BroadcastChannel>         BroadcastChannels         => this.Set<BroadcastChannel>();
	public DbSet<ChatRoom>                 ChatRooms                 => this.Set<ChatRoom>();
	public DbSet<CommunityChannel>         CommunityChannels         => this.Set<CommunityChannel>();
	public DbSet<UserChannelConfiguration> UserChannelConfigurations => this.Set<UserChannelConfiguration>();

	protected override void OnModelCreating(ModelBuilder modelBuilder)
	{
		modelBuilder.HasDefaultSchema("chat");

		modelBuilder.Entity<UserChannelConfiguration>().HasKey(x => x.Id);

		modelBuilder.Entity<UserProfile>().HasKey(x => x.Id);

		modelBuilder.Entity<UserProfile>().Property(x => x.Id).ValueGeneratedNever();

		modelBuilder.Entity<Contact>().Property(x => x.Id).ValueGeneratedNever();

		modelBuilder.Entity<ChatRoom>().Property(x => x.Id).ValueGeneratedNever();

		modelBuilder.Entity<UserProfile>().Navigation(x => x.Contacts).AutoInclude();

		modelBuilder.Entity<UserProfile>()
					.HasIndex(e => new
					{
						e.WalletAddress,
						e.ApplicationId
					})
					.IsUnique();

		// In general, we will want the UserProfile and Permissions for a given 
		// UserChannelConfiguration when we query it. In the future, we should 
		// optimize these queries to not rely on AutoInclude() for the sake of performance.
		modelBuilder.Entity<UserChannelConfiguration>().Navigation(x => x.UserProfile).AutoInclude();

		modelBuilder.Entity<Channel>().HasMany(x => x.Members).WithOne().OnDelete(DeleteBehavior.ClientCascade);

		modelBuilder.Entity<UserChannelConfiguration>().Navigation(x => x.Permissions).AutoInclude();

		modelBuilder.Entity<UserProfile>();

		base.OnModelCreating(modelBuilder);
	}
}