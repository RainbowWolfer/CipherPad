using DevExpress.Mvvm;
using System.Windows.Input;

namespace CipherPad.ViewModels;

public class ButtonViewModel : BindableBase
{

	public string Icon
	{
		get => GetProperty(() => Icon);
		set => SetProperty(() => Icon, value);
	}


	public string ToolTip
	{
		get => GetProperty(() => ToolTip);
		set => SetProperty(() => ToolTip, value);
	}

	public ICommand Command
	{
		get => GetProperty(() => Command);
		set => SetProperty(() => Command, value);
	}

	public ButtonViewModel(string icon, string toolTip, ICommand command)
	{
		Icon = icon;
		ToolTip = toolTip;
		Command = command;
	}

	public ButtonViewModel()
	{

	}
}
