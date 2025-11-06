using HandyControl.Tools;
using RW.Base.WPF;
using RW.Base.WPF.Extensions;
using System.Diagnostics;
using System.Globalization;
using System.Net;
using System.Windows;

namespace CipherPad;

public partial class App : ApplicationBase {
	private static App? instance;
	public static App Instance => instance!;

	public override bool IsRelease => AppConfig.IsRelease;

	public App() {
		instance = this;
	}

	protected override void BeforeLoadingModules() {
		base.BeforeLoadingModules();

		ConfigHelper.Instance.SetWindowDefaultStyle();
		ConfigHelper.Instance.SetNavigationWindowDefaultStyle();

		ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
	}

	public override Window GetMainWindow() => new MainWindow();

	protected override void DebugPrint(string message) {
		Debug.WriteLine(message);
	}

	protected override CultureInfo? GetCultureInfo() {
		// todo
		return null;
	}

	protected override DllLoader GetDllLoader() => new _DllLoader();
	protected override IFatalDebugLogger GetFatalDebugLogger() => new _FatalDebugLogger();
	protected override IHandledDebugLogger GetHandledDebugLogger() => new _HandledDebugLogger();
	protected override IoCInitializer GetIoCInitializer() => new _IoCInitializer(DllLoader);

	protected override string GetMutexName() => $"{AppConfig.AppName}.{AppConfig.AppName}";
	protected override string GetPipeName() => $"{AppConfig.AppName}.{AppConfig.AppName}";

	protected override void HandledException(Exception exception, string flag) {
		Debug.WriteLine($"{flag} - {exception}");
	}

	protected override void ShowFatalDialog(Exception exception) {

	}
}
