using RW.Common.Attributes;
using System.Diagnostics;
using System.IO;
using System.Reflection;

namespace CipherPad.Properties;

public static class FolderConfig {
	/// <summary> 软件 exe 路径 </summary>
	public static string AppFolder => AppDomain.CurrentDomain.BaseDirectory;

	/// <summary> AppData/Local </summary>
	public static string LocalFolder => Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);

	/// <summary> AppData/Local/CompanyName/AppName </summary>
	public static string LocalAppFolder => GetFolderPath(Path.Combine(LocalFolder, $"{AppConfig.Author}", $"{AppConfig.AppName}"));

	[Initialize] public static string TargetFolder => LocalAppFolder;

	[Initialize] public static string LoggingFolder => GetFolderPath(Path.Combine(TargetFolder, "Logs"));
	[Initialize] public static string DataFolder => GetFolderPath(Path.Combine(TargetFolder, "Data"));
	[Initialize] public static string DebugFolder => GetFolderPath(Path.Combine(TargetFolder, "Debug"));

	[Initialize] public static string DebugFatalFolder => GetFolderPath(Path.Combine(DebugFolder, "Fatal"));
	[Initialize] public static string DebugHandledFolder => GetFolderPath(Path.Combine(DebugFolder, "Handled"));

	private static string GetFolderPath(string path) {
		try {
			CreateDirectory(path);
		} catch (Exception ex) {
			Debug.WriteLine(ex);
		}
		return path;
	}

	public static void Initialize() {
		IEnumerable<PropertyInfo> initializeProperties = typeof(FolderConfig).GetProperties().Where(static p => p.GetCustomAttribute<InitializeAttribute>() != null);

		foreach (PropertyInfo item in initializeProperties) {
			if (item.GetValue(null) is string str) {
				CreateDirectory(str);
			}
		}

		Debug.WriteLine(AppFolder);
		Debug.WriteLine(TargetFolder);
	}

	private static DirectoryInfo CreateDirectory(string dir) {
		return Directory.CreateDirectory(dir);
	}


}
