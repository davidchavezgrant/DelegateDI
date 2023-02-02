using DelegateDI.Bug;
using DelegateDI.Bug.Dtos;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using NodaTime;


namespace DelegateDI;

internal static class Extensions
{
	public static IServiceCollection AddDemoServices(this IServiceCollection services)
	{
		services.AddSingleton<MySingletonService>();
		services.AddScoped<MyScopedService>();
		services.AddTransient<MyTransientService>();
		services.AddSingleton<MySingletonDelegate>(serviceProvider =>
												   {
													   var outerId = Guid.NewGuid();
													   Console.WriteLine($"Construct {nameof(MySingletonDelegate)} with outerId: {outerId}");
													   return () =>
														      {
															      var innerId = Guid.NewGuid();
															      Console.WriteLine($"Invoke {nameof(MySingletonDelegate)} with outerId: {outerId} and innerId: {innerId}");
															      return innerId;
														      };
												   });

		services.AddScoped<MyScopedDelegate>(serviceProvider =>
											 {
												 var outerId = Guid.NewGuid();
												 Console.WriteLine($"Construct {nameof(MyScopedDelegate)} with outerId: {outerId}");
												 return () =>
													    {
														    var innerId = Guid.NewGuid();
														    Console.WriteLine($"Invoke {nameof(MyScopedDelegate)} with outerId: {outerId} and innerId: {innerId}");
														    return innerId;
													    };
											 });

		services.AddTransient<MyTransientDelegate>(serviceProvider =>
												   {
													   var outerId          = Guid.NewGuid();
													   var outerTransient   = serviceProvider.GetRequiredService<MyTransientService>();
													   var outerTransientId = outerTransient.Id;
													   Console.WriteLine($"Located {nameof(MyTransientService)}: {outerTransientId} while CONSTRUCTING {nameof(MyTransientDelegate)}: {outerId}");
													   return () =>
														      {
															      var innerTransient   = serviceProvider.GetRequiredService<MyTransientService>();
															      var innerTransientId = innerTransient.Id;
															      var innerId          = Guid.NewGuid();
															      Console.WriteLine($"Located {nameof(MyTransientService)}: {innerTransientId} while INVOKING {nameof(MyTransientDelegate)}: {innerId}");
															      return innerId;
														      };
												   });
		return services;
	}

	public static IServiceCollection AddBylinesServices(this IServiceCollection services)
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
		services.AddDbContext<BylinesIdentityDbContext>(x => x.UseSqlite("Data Source=identity.db;"), ServiceLifetime.Transient);
		services.AddScoped<IChannelService, ChannelService>();
		services.AddScoped<IChatApiContract, ChatService>();
		services.AddDbContextFactory<ChatDbContext>(options => options.UseSqlite("Data Source=chat.db;"));
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
		services.AddSingleton<IClock>(SystemClock.Instance);

		return services;
	}
}