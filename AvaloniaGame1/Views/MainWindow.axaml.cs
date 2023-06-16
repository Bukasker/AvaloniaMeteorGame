using Avalonia.Controls;
using AvaloniaGame1.ViewModels;

namespace AvaloniaGame1.Views;

public partial class MainWindow : Window
{
	public MainWindow()
	{
		InitializeComponent();
		MyReferences.MainWindowOb = this;
	}

	public void ChangeContent(Control newContent)
	{
		Content = newContent;
	}
}
