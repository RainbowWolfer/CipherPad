using CipherPad.Properties;
using DevExpress.Mvvm;
using HandyControl.Themes;
using System.Windows;

namespace CipherPad;

public partial class MainWindow : HandyControl.Controls.Window {
	public MainWindow() {
		InitializeComponent();
	}

	private void Button_Click(object sender, RoutedEventArgs e) {
		ThemeManager.Current.ApplicationTheme = ApplicationTheme.Dark;
	}

	private void Button_Click_1(object sender, RoutedEventArgs e) {
		ThemeManager.Current.ApplicationTheme = ApplicationTheme.Light;
	}

	private void Button_Click_2(object sender, RoutedEventArgs e) {
	}

	private void Button_Click_3(object sender, RoutedEventArgs e) {
		new MainWindow().Show();
	}

	private void Button_Click_4(object sender, RoutedEventArgs e) {
		//Panel.Children.Add(new Button() { Content = "??" });
	}
}



internal class MainWindowViewModel : ViewModelBase {

	public string Hello { get; } = AppStrings.Hello;

	public MainWindowViewModel() {

	}

	private DelegateCommand? switchThemeCommand;
	public IDelegateCommand SwitchThemeCommand => switchThemeCommand ??= new(SwitchTheme);
	private void SwitchTheme() {
		if (ThemeManager.Current.ApplicationTheme == ApplicationTheme.Light) {
			ThemeManager.Current.ApplicationTheme = ApplicationTheme.Dark;
		} else {
			ThemeManager.Current.ApplicationTheme = ApplicationTheme.Light;
		}
	}


	private DelegateCommand? newFileCommand;
	public IDelegateCommand NewFileCommand => newFileCommand ??= new(NewFile);
	private void NewFile() {

	}


	private DelegateCommand? openFileCommand;
	public IDelegateCommand OpenFileCommand => openFileCommand ??= new(OpenFile);
	private void OpenFile() {

	}



}