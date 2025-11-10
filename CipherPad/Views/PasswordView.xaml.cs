using CipherPad.ViewModels;
using DevExpress.Mvvm;
using RW.Common.Helpers;
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

internal class PasswordViewModel : TabViewModelBase<PasswordView>
{
	public IOpenFileDialogService OpenFileDialogService => GetService<IOpenFileDialogService>();
	public ISaveFileDialogService SaveFileDialogService => GetService<ISaveFileDialogService>();


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

	public PasswordViewModel()
	{
		ShouldEnterPassword = true;
	}

	public override IEnumerable<ButtonViewModel> GetTabButtons()
	{
		yield return new ButtonViewModel("\uE74E", "Save", SaveCommand);
	}

	protected override void Initialize()
	{
		if (FilePath.IsBlank())
		{
			return;
		}




	}

	private DelegateCommand? saveCommand;
	public IDelegateCommand SaveCommand => saveCommand ??= new(Save);
	private void Save()
	{

	}



	private DelegateCommand? confirmPasswordCommand;
	public IDelegateCommand ConfirmPasswordCommand => confirmPasswordCommand ??= new(ConfirmPassword, CanConfirmPassword);
	private void ConfirmPassword()
	{
		if (CanConfirmPassword())
		{

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