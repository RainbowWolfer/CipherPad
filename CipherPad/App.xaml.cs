using CipherPad.Properties;
using RW.Base.WPF;
using RW.Base.WPF.Extensions;
using RW.Base.WPF.ViewModels;
using System.Diagnostics;
using System.Globalization;
using System.Windows;

namespace CipherPad;

public partial class App : ApplicationBase {
	private static App? instance;
	public static App Instance => instance!;

	public override bool IsRelease => AppConfig.IsRelease;

	public App() {
		instance = this;
	}

	public override Window GetMainWindow() => new MainWindow();

	protected override void DebugPrint(string message) {
		Debug.WriteLine(message);
	}

	protected override CultureInfo? GetCultureInfo() {
		// todo
		return null;
	}

	protected override AppManager GetAppManager() => new _AppManager();
	protected override DllLoader GetDllLoader() => new _DllLoader();
	protected override IoCInitializer GetIoCInitializer() => new _IoCInitializer(DllLoader);

	protected override string GetMutexName() => $"{AppConfig.AppName}.{AppConfig.AppName}";
	protected override string GetPipeName() => $"{AppConfig.AppName}.{AppConfig.AppName}";

	protected override void LogException(Exception exception, string flag) {
		Debug.WriteLine($"{flag} - {exception}");
	}

	protected override void ShowFatalDialog(Exception exception) {

	}
}
