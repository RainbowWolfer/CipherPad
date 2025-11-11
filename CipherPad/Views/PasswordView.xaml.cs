using CipherPad.Services;
using CipherPad.ViewModels;
using DevExpress.Mvvm;
using RW.Base.WPF.Extensions;
using RW.Common.Helpers;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Windows.Controls;
using System.Windows.Input;

namespace CipherPad.Views;

public partial class PasswordView : UserControl
{
	public PasswordView()
	{
		InitializeComponent();
	}

}

internal class PasswordViewModel(IAppSettingsService appSettingsService) : TabViewModelBase<PasswordView>
{
	public const byte TypeID = 0x01;

	public const int SaltLength = 16;
	public const int IVLength = 16;

	public IOpenFileDialogService OpenFileDialogService => GetService<IOpenFileDialogService>();
	public ISaveFileDialogService SaveFileDialogService => GetService<ISaveFileDialogService>();

	public IMessageBoxService MessageBoxService => GetService<IMessageBoxService>();


	public string Password
	{
		get => GetProperty(() => Password);
		set => SetProperty(() => Password, value);
	}

	public bool ShouldEnterPassword
	{
		get => GetProperty(() => ShouldEnterPassword);
		set => SetProperty(() => ShouldEnterPassword, value);
	}

	public string ViewText
	{
		get => GetProperty(() => ViewText) ?? string.Empty;
		set => SetProperty(() => ViewText, value);
	}

	public override IEnumerable<ButtonViewModel> GetTabButtons()
	{
		yield return new ButtonViewModel("\uE74E", "Save", SaveCommand);
	}

	protected override void Initialize()
	{
		ShouldEnterPassword = true;
	}

	private DelegateCommand? saveCommand;
	public IDelegateCommand SaveCommand => saveCommand ??= new(Save, CanSave);
	private void Save()
	{
		if (CanSave())
		{
			if (!File.Exists(FilePath))
			{
				if (SaveFileDialogService.ShowDialog())
				{
					string filePath = SaveFileDialogService.GetFullFileName();
					FilePath = filePath;
				}
				else
				{
					return;
				}
			}

			try
			{
				string password = Password;
				string text = ViewText ?? string.Empty;

				byte[] salt = RandomNumberGenerator.GetBytes(SaltLength);
				int iterations = RandomNumberGenerator.GetInt32(10_000, 100_000);
				byte[] iv = RandomNumberGenerator.GetBytes(IVLength);

				using Aes aes = Aes.Create();
				// 从密码派生密钥和IV
				using Rfc2898DeriveBytes key = new(password: password, salt: salt, iterations: iterations, hashAlgorithm: HashAlgorithmName.SHA512);
				aes.Key = key.GetBytes(32); // 256位密钥
				aes.IV = iv;

				using ICryptoTransform encryptor = aes.CreateEncryptor();
				using MemoryStream ms = new();
				using (CryptoStream cs = new(ms, encryptor, CryptoStreamMode.Write))
				using (StreamWriter sw = new(cs, Encoding.UTF8))
				{
					sw.Write(text);
				}

				byte[] encryptedBytes = ms.ToArray();

				byte[] resultBytes = new byte[sizeof(byte) + SaltLength + sizeof(int) + IVLength + encryptedBytes.Length];

				byte fileType = 0x01;

				using FileStream fs = File.Create(FilePath);
				using BinaryWriter bw = new(fs);
				bw.Write(fileType);
				bw.Write(salt.Length);
				bw.Write(salt);
				bw.Write(iterations);
				bw.Write(iv.Length);
				bw.Write(iv);
				bw.Write(encryptedBytes.Length);
				bw.Write(encryptedBytes);
			}
			catch (Exception ex)
			{
				DebugLoggerManager.HandledLogger.Log(ex, "PasswordView_Save");
				MessageBoxService.ShowMessage(ex.ToString(), "Error", MessageButton.OK, MessageIcon.Error);
			}
		}
	}
	private bool CanSave() => Password.IsNotBlank();



	private DelegateCommand? confirmPasswordCommand;
	public IDelegateCommand ConfirmPasswordCommand => confirmPasswordCommand ??= new(ConfirmPassword, CanConfirmPassword);
	private void ConfirmPassword()
	{
		if (CanConfirmPassword())
		{
			if (File.Exists(FilePath))
			{
				try
				{
					string password = Password;

					using FileStream fs = File.OpenRead(FilePath);
					using BinaryReader br = new(fs);

					byte fileType = br.ReadByte();
					int saltLen = br.ReadInt32();
					byte[] salt = br.ReadBytes(saltLen);
					int iterations = br.ReadInt32();
					int ivLen = br.ReadInt32();
					byte[] iv = br.ReadBytes(ivLen);
					int cipherLen = br.ReadInt32();
					byte[] cipherBytes = br.ReadBytes(cipherLen);


					// 派生密钥
					using Rfc2898DeriveBytes keyDerive = new(password, salt, iterations, HashAlgorithmName.SHA512);
					byte[] key = keyDerive.GetBytes(32);

					using Aes aes = Aes.Create();
					aes.Key = key;
					aes.IV = iv;
					aes.Mode = CipherMode.CBC;
					aes.Padding = PaddingMode.PKCS7;

					using ICryptoTransform decryptor = aes.CreateDecryptor();
					using MemoryStream ms = new(cipherBytes);
					using CryptoStream cs = new(ms, decryptor, CryptoStreamMode.Read);
					using StreamReader sr = new(cs, Encoding.UTF8);

					string plainText = sr.ReadToEnd();

					ViewText = plainText;
				}
				catch (Exception ex)
				{
					DebugLoggerManager.HandledLogger.Log(ex, "PasswordView_ConfirmPassword");
					MessageBoxService.ShowMessage(ex.ToString(), "Error", MessageButton.OK, MessageIcon.Error);
					return;
				}
			}

			ShouldEnterPassword = false;
			//todo : focus the textbox
		}
	}
	private bool CanConfirmPassword() => Password.IsNotBlank();


	private DelegateCommand<KeyEventArgs>? passwordBoxKeyDownCommand;
	public ICommand PasswordBoxKeyDownCommand => passwordBoxKeyDownCommand ??= new(PasswordBoxKeyDown);
	private void PasswordBoxKeyDown(KeyEventArgs args)
	{
		if (args.Key is Key.Enter)
		{
			ConfirmPassword();
			args.Handled = true;
		}
	}



}