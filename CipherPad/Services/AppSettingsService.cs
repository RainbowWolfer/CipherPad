using RW.Base.WPF.Interfaces;

namespace CipherPad.Services;

public class AppSettingsService : IAppInitializeAsync
{
	string IAppInitializeAsync.Description { get; } = "";
	int IPriority.Priority { get; } = 0;

	async Task IAppInitializeAsync.AppInitializeAsync(IStatusReport statusReport)
	{
		await Task.CompletedTask;
	}
}
