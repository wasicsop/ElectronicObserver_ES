using System.IO;
using CommunityToolkit.Mvvm.DependencyInjection;
using ElectronicObserver.Database;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace ElectronicObserverCoreTests;

public class Startup
{
	public async void ConfigureServices(IServiceCollection services)
	{
		Ioc.Default.ConfigureServices(new ServiceCollection().BuildServiceProvider());

		Directory.CreateDirectory("Record");

		await using ElectronicObserverContext db = new();
		await db.Database.MigrateAsync();
	}
}
