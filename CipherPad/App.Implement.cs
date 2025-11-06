using CipherPad.Properties;
using RW.Base.WPF.Extensions;
using System.Diagnostics;

namespace CipherPad;

public partial class App {

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

	private abstract class _DebugLoggerBase() : DebugLoggerBase(
	   AppConfig.AppStartTime,
	   AppConfig.Version.ToString(),
	   AppConfig.IsRelease ? "Release" : "Debug"
	);

	private class _HandledDebugLogger() : _DebugLoggerBase(), IHandledDebugLogger {
		public override string LogType => "Handled";
		public override string TargetFolderPath => FolderConfig.DebugHandledFolder;
		public override string AppName => AppConfig.AppName;
		protected override void DebugPrint(object message) => Debug.WriteLine(message);
	}

	private class _FatalDebugLogger() : _DebugLoggerBase(), IFatalDebugLogger {
		public override string LogType => "Fatal";
		public override string TargetFolderPath => FolderConfig.DebugFatalFolder;
		public override string AppName => AppConfig.AppName;
		protected override void DebugPrint(object message) => Debug.WriteLine(message);
	}


}
