using HandyControl.Themes;
using System.Windows;
using System.Windows.Controls;

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
		Panel.Children.Add(new Button() { Content = "??" });
	}
}