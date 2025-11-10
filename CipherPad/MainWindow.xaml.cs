using CipherPad.Properties;
using CipherPad.ViewModels;
using CipherPad.Views;
using DevExpress.Mvvm;
using HandyControl.Themes;
using HandyControl.Tools.Extension;
using RW.Base.WPF.Extensions;
using RW.Common.Helpers;
using System.Collections.ObjectModel;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace CipherPad;

public partial class MainWindow : HandyControl.Controls.Window
{
	public MainWindow()
	{
		InitializeComponent();
	}

}

public class MainWindowViewModel : ViewModelBase
{
	public IOpenFileDialogService OpenFileDialogService => GetService<IOpenFileDialogService>();
	public ISaveFileDialogService SaveFileDialogService => GetService<ISaveFileDialogService>();


	public string Hello { get; } = AppStrings.Hello;

	public ObservableCollection<ITabViewModel> TabItems { get; } = [];
	public ObservableCollection<ButtonViewModel> AdditionalButtons { get; } = [];

	public ITabViewModel SelectedTabItem
	{
		get => GetProperty(() => SelectedTabItem);
		set
		{
			SetProperty(() => SelectedTabItem, value);
			AdditionalButtons.Clear();
			if (value != null)
			{
				AdditionalButtons.AddRange(value.GetTabButtons());
			}
		}
	}

	public MainWindowViewModel()
	{

	}

	private DelegateCommand? switchThemeCommand;
	public IDelegateCommand SwitchThemeCommand => switchThemeCommand ??= new(SwitchTheme);
	private void SwitchTheme()
	{
		if (ThemeManager.Current.ApplicationTheme == ApplicationTheme.Light)
		{
			ThemeManager.Current.ApplicationTheme = ApplicationTheme.Dark;
		}
		else
		{
			ThemeManager.Current.ApplicationTheme = ApplicationTheme.Light;
		}
	}

	private ITabViewModel CreateTabViewModel<T>() where T : notnull
	{
		return (ServicePool.Resolve<T>() as ITabViewModel) ?? throw new Exception();
	}


	private DelegateCommand? newPasswordFileCommand;
	public IDelegateCommand NewPasswordFileCommand => newPasswordFileCommand ??= new(NewPasswordFile);
	private void NewPasswordFile()
	{
		ITabViewModel viewModel = CreateTabViewModel<PasswordViewModel>();
		TabItems.Add(viewModel);
		SelectedTabItem = viewModel;
	}



	private DelegateCommand? openCommand;
	public IDelegateCommand OpenCommand => openCommand ??= new(Open);
	private void Open()
	{
		if (OpenFileDialogService.ShowDialog())
		{
			string filePath = OpenFileDialogService.GetFullFileName();

			if (TabItems.FirstOrDefault(x => FileHelper.AreFilePathsEqualSafe(x.FilePath, filePath) is true) is { } found)
			{
				SelectedTabItem = found;
				return;
			}

			if (File.Exists(filePath))
			{
				// todo : judge the type here 
				ITabViewModel viewModel = CreateTabViewModel<PasswordViewModel>();
				viewModel.Initialize(filePath);
				TabItems.Add(viewModel);
				SelectedTabItem = viewModel;
			}

		}
	}



	public static string Decrypt(
		string cipherText,
		string password,
		byte[] salt,
		int iterations,
		byte[] iv
	)
	{
		// 1. 从密码+盐+迭代次数派生密钥
		using Rfc2898DeriveBytes keyDerive = new(password, salt, iterations, HashAlgorithmName.SHA512);
		byte[] key = keyDerive.GetBytes(32); // AES-256

		// 2. 配置 AES
		using Aes aes = Aes.Create();
		aes.Key = key;
		aes.IV = iv;
		aes.Mode = CipherMode.CBC;
		aes.Padding = PaddingMode.PKCS7;

		// 3. 解密
		byte[] cipherBytes = Convert.FromBase64String(cipherText);
		using ICryptoTransform decryptor = aes.CreateDecryptor();
		using MemoryStream ms = new(cipherBytes);
		using CryptoStream cs = new(ms, decryptor, CryptoStreamMode.Read);
		using StreamReader sr = new(cs, Encoding.UTF8);

		return sr.ReadToEnd();
	}


	private DelegateCommand? openFileCommand;
	public IDelegateCommand OpenFileCommand => openFileCommand ??= new(OpenFile);
	private void OpenFile()
	{

		string password = "123456";
		string text = "dwj qi hf io q等我何求i等会我后i和哦ih哦i回复v给我互容儿保护柔儿很弱iu";

		byte[] salt = Encoding.UTF8.GetBytes("固定盐值");
		int iterations = 10000;

		using Aes aes = Aes.Create();
		// 从密码派生密钥和IV
		using Rfc2898DeriveBytes key = new(password: password, salt: salt, iterations: iterations, hashAlgorithm: HashAlgorithmName.SHA512);
		aes.Key = key.GetBytes(32); // 256位密钥
		aes.IV = RandomNumberGenerator.GetBytes(16); // 128位IV 推荐随机 IV

		using ICryptoTransform encryptor = aes.CreateEncryptor();
		using MemoryStream ms = new();
		using (CryptoStream cs = new(ms, encryptor, CryptoStreamMode.Write))
		using (StreamWriter sw = new(cs))
		{
			sw.Write(text);
		}
		string v = Convert.ToBase64String(ms.ToArray());


		var r = Decrypt(v, password, salt, iterations, aes.IV);

	}



}