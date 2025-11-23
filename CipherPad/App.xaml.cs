using CipherPad.Properties;
using RW.Base.WPF;
using RW.Base.WPF.Configs;
using RW.Base.WPF.Extensions;
using RW.Base.WPF.ViewModels;
using System.Diagnostics;
using System.Globalization;
using System.Windows;

namespace CipherPad;

public partial class App : ApplicationBase {
	private static App? instance;
	public static App Instance => instance!;

	static App() {
		DebugConfig.Print = o => Debug.WriteLine(o);
		DebugConfig.DebuggerBreak = Debugger.Break;
	}

	public override bool IsRelease => AppConfig.IsRelease;

	public App() {
		instance = this;
	}

	protected override Window GetMainWindow() => new MainWindow();

	protected override CultureInfo? GetCultureInfo() {
		// todo
		return null;
	}

	protected override AppManager GetAppManager() => new _AppManager();
	protected override DllLoader GetDllLoader() => new _DllLoader();
	protected override IoCInitializer GetIoCInitializer() => new _IoCInitializer(DllLoader);

	protected override void ShowFatalDialog(Exception exception) {
		MessageBox.Show(exception.ToString(), "Fatal Error");
	}
}
