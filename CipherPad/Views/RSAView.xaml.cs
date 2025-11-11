using CipherPad.ViewModels;
using DevExpress.Mvvm;
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

	public override IEnumerable<ButtonViewModel> GetTabButtons() {
		yield break;
	}

	protected override void Initialize() {
		
	}

}