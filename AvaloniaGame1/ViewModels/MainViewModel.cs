using Avalonia.Controls;
using AvaloniaGame1.Models;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace AvaloniaGame1.ViewModels;

public partial class MainViewModel : ObservableObject
{
	[RelayCommand]
	public void NewGameCreateButton()
	{

		var gameScene = new GameScene();
		var control = new ContentControl();
		control.Content = gameScene;

		MyReferences.MainWindowOb.Content = control;
	}
}
