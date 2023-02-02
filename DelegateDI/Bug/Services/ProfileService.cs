using DelegateDI.Bug.DAL;
using DelegateDI.Bug.Entities;
using Microsoft.EntityFrameworkCore;


namespace DelegateDI.Bug.Services;

internal sealed class ProfileService: IProfileService
{
	private readonly IDbContextFactory<ChatDbContext> _dbFactory;

	public ProfileService(IDbContextFactory<ChatDbContext> dbFactory) { this._dbFactory = dbFactory; }

	public async Task<Guid> UpsertProfile(string walletAddress, Guid appId)
	{
		await using var db = await this._dbFactory.CreateDbContextAsync();

		var profile = await db.Profiles.Where(p => p.WalletAddress.ToLower() == walletAddress.ToLower() && p.ApplicationId == appId).FirstOrDefaultAsync();
		if (profile is not null)
		{
			profile.SetIsBylUserToTrue();
			await db.SaveChangesAsync();
			return profile.Id;
		}

		Console.WriteLine($"Creating new profile for {walletAddress} at {appId}, CreateOrUpdateRegisteredProfile.");
		var newProfile = UserProfile.Create(walletAddress, appId);
		db.Profiles.Add(newProfile);
		await db.SaveChangesAsync();
		return newProfile.Id;
	}
}