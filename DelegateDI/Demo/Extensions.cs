using DelegateDI.Bug;
using DelegateDI.Bug.DAL;
using DelegateDI.Bug.Entities;
using DelegateDI.Bug.Services;
using DelegateDI.Demo;
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
}