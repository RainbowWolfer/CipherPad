using RW.Base.WPF.Interfaces;

namespace CipherPad.Services;

public interface IAppSettingsService
{

}

public class AppSettingsService() : IAppSettingsService, IAppInitializeAsync
{
	string IAppInitializeAsync.Description { get; } = "";
	int IPriority.Priority { get; } = 0;

	async Task IAppInitializeAsync.AppInitializeAsync(IStatusReport statusReport)
	{
		await Task.CompletedTask;
	}
}
