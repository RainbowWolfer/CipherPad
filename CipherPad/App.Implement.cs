using RW.Base.WPF.Extensions;
using RW.Base.WPF.ViewModels;
using System.Diagnostics;

namespace CipherPad;

public partial class App {

	private class _AppManager : AppManager {
		public override string AppName => "";
		public override string BuildMode => "";
	}

	private class _DllLoader : DllLoader {

	}

	private class _IoCInitializer(DllLoader dllLoader) : IoCInitializer(dllLoader) {
		protected override void DebugBreak(string message) {
			Debug.WriteLine(message);
			Debugger.Break();
		}

		protected override void DebugPrint(string message) {
			Debug.WriteLine(message);
		}
	}


}
