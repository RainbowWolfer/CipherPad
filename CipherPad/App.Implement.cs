using CipherPad.Properties;
using RW.Base.WPF.Extensions;
using RW.Base.WPF.ViewModels;

namespace CipherPad;

public partial class App {

	private class _AppManager : AppManager {
		public override string AppName => AppConfig.AppName;
		public override string BuildMode => AppConfig.IsRelease ? "Release" : "Debug";
	}

	private class _DllLoader : DllLoader {

	}

	private class _IoCInitializer(DllLoader dllLoader) : IoCInitializer(dllLoader) {

	}
}
