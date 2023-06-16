using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using AvaloniaGame1.ViewModels;

namespace AvaloniaGame1.Views;

public partial class MainView : UserControl
{
	public MainView()
	{
		InitializeComponent();
		DataContext = new MainViewModel();
	}

	private void InitializeComponent()
	{
		AvaloniaXamlLoader.Load(this);
	}
	private void CloseApplication(object sender, RoutedEventArgs e)
	{
		Window window = (Window)this.VisualRoot;
		window.Close();
	}
}
