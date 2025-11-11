using DevExpress.Mvvm;
using RW.Common.Helpers;
using System.IO;
using System.Windows;

namespace CipherPad.ViewModels;

public interface ITabViewModel {
	string Header { get; set; }
	FrameworkElement View { get; }

	string? FilePath { get; }

	IEnumerable<ButtonViewModel> GetTabButtons();

	void Initialize(string? filePath);
}

public abstract class TabViewModelBase<T> : ViewModelBase, ITabViewModel
	where T : FrameworkElement {

	public MainWindowViewModel MainWindowViewModel {
		get => GetProperty(() => MainWindowViewModel);
		set => SetProperty(() => MainWindowViewModel, value);
	}

	public string Header {
		get => GetProperty(() => Header);
		set => SetProperty(() => Header, value);
	}


	public string? FilePath {
		get => GetProperty(() => FilePath);
		protected set {
			SetProperty(() => FilePath, value);
			if (FilePath.IsNotBlank()) {
				Header = Path.GetFileName(FilePath);
			}
		}
	}

	public T View { get; }
	FrameworkElement ITabViewModel.View => View;

	public TabViewModelBase() {
		Header = "Untitled";

		View = Activator.CreateInstance<T>();

	}

	public abstract IEnumerable<ButtonViewModel> GetTabButtons();

	public void Initialize(string? filePath) {
		FilePath = filePath;
		Initialize();
	}

	protected abstract void Initialize();
}
