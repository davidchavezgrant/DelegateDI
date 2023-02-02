using DelegateDI.Bug.DAL;
using DelegateDI.Bug.Entities;
using DelegateDI.Bug.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using NodaTime;


namespace DelegateDI.Bug;

internal static class Extensions
{
	public static IServiceCollection AddBylinesServices(this IServiceCollection services, bool useSqlite = false)
	{
		services.AddIdentity<BylinesAccount, IdentityRole>(options =>
														   {
															   options.Password.RequiredLength         = 8;
															   options.Password.RequireLowercase       = true;
															   options.Password.RequireUppercase       = true;
															   options.Lockout.MaxFailedAccessAttempts = 5;
															   options.Lockout.DefaultLockoutTimeSpan  = TimeSpan.FromMinutes(15);
															   options.User.RequireUniqueEmail         = true;
														   })
				.AddEntityFrameworkStores<BylinesIdentityDbContext>();
		if (useSqlite)
		{
			services.AddDbContext<BylinesIdentityDbContext>(x => x.UseSqlite("Data Source=identity.db;"), ServiceLifetime.Transient);
			services.AddDbContextFactory<ChatDbContext>(options => options.UseSqlite("Data Source=chat.db;"));
		}
		else
		{
			services.AddDbContext<BylinesIdentityDbContext>(x => x.UseNpgsql("User ID = bylines_admin; Password = app_password; Host = localhost; Port = 5432; Database = identity_db"), ServiceLifetime.Transient);
			services.AddDbContextFactory<ChatDbContext>(options => options.UseNpgsql("User ID = bylines_admin; Password = app_password; Host = localhost; Port = 5432; Database = chat_db"));
		}

		services.AddSingleton<IClock>(SystemClock.Instance);
		services.AddScoped<IChannelService, ChannelService>();
		services.AddScoped<IProfileService, ProfileService>();
		services.AddScoped<IChatApiContract, ChatService>();

		services.AddTransient<GetEncryptionPublicKeyByWalletAddress>(sp =>
																	 {
																		 var dbContext = sp.GetRequiredService<BylinesIdentityDbContext>();

																		 async Task<string?> getEncryptionPublicKey(string walletAddress)
																		 {
																			 var account = await dbContext.Users.FirstOrDefaultAsync(x => x.UserName.ToLower() == walletAddress.ToLower());
																			 return account is null? null : account.PublicKey;
																		 }

																		 return getEncryptionPublicKey;
																	 });
		services.AddTransient<GetEnsDomain>(sp =>
										    {
											    var userManager = sp.GetRequiredService<UserManager<BylinesAccount>>();
											    return async (walletAddress) =>
												       {
													       var account = await userManager.FindByNameAsync(walletAddress);
													       if (account is null) return await EnsLookup.ResolveAddress(walletAddress);

													       return account.EnsDomain;
												       };
										    });

		return services;
	}
}