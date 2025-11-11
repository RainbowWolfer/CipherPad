using CipherPad.ViewModels;
using DevExpress.Mvvm;
using RW.Base.WPF.Extensions;
using System.IO;
using System.Windows.Controls;

namespace CipherPad.Views;

public partial class RSAView : UserControl {
	public RSAView() {
		InitializeComponent();
	}
}

internal class RSAViewModel : TabViewModelBase<RSAView> {
	public const byte TypeID = 0x02;

	public IOpenFileDialogService OpenFileDialogService => GetService<IOpenFileDialogService>();
	public ISaveFileDialogService SaveFileDialogService => GetService<ISaveFileDialogService>();

	public IMessageBoxService MessageBoxService => GetService<IMessageBoxService>();


	public bool ShouldSelectPrivateKey {
		get => GetProperty(() => ShouldSelectPrivateKey);
		set => SetProperty(() => ShouldSelectPrivateKey, value);
	}

	public string PrivateKeyFilePath {
		get => GetProperty(() => PrivateKeyFilePath);
		set => SetProperty(() => PrivateKeyFilePath, value);
	}

	public string ViewText {
		get => GetProperty(() => ViewText) ?? string.Empty;
		set => SetProperty(() => ViewText, value);
	}

	public override IEnumerable<ButtonViewModel> GetTabButtons() {
		yield return new ButtonViewModel("\uE74E", "Save", SaveCommand);
	}

	protected override void Initialize() {
		ShouldSelectPrivateKey = true;
	}


	private DelegateCommand? saveCommand;
	public IDelegateCommand SaveCommand => saveCommand ??= new(Save, CanSave);
	private void Save() {
		if (CanSave()) {
			if (!File.Exists(FilePath)) {
				if (SaveFileDialogService.ShowDialog()) {
					string filePath = SaveFileDialogService.GetFullFileName();
					FilePath = filePath;
				} else {
					return;
				}
			}

			try {



			} catch (Exception ex) {
				DebugLoggerManager.HandledLogger.Log(ex, "RSAView_Save");
				MessageBoxService.ShowMessage(ex.ToString(), "Error", MessageButton.OK, MessageIcon.Error);
			}
		}
	}
	private bool CanSave() => /*Password.IsNotBlank()*/true;


	private DelegateCommand? openPrivateKeyFileCommand;
	public IDelegateCommand OpenPrivateKeyFileCommand => openPrivateKeyFileCommand ??= new(OpenPrivateKeyFile);
	private void OpenPrivateKeyFile() {
		if (OpenFileDialogService.ShowDialog()) {
			string filePath = OpenFileDialogService.GetFullFileName();


		}
	}



	private DelegateCommand? confirmCommand;
	public IDelegateCommand ConfirmCommand => confirmCommand ??= new(Confirm);
	private void Confirm() {
		ShouldSelectPrivateKey = false;
	}


}