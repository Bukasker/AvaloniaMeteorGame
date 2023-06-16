using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Primitives;
using Avalonia.Controls.Primitives.PopupPositioning;
using Avalonia.Input;
using Avalonia.Media;
using Avalonia.Media.Imaging;
using Avalonia.Threading;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;


namespace AvaloniaGame1.Models
{
	internal class GameScene : UserControl
	{
		private Player player;
		private const double playerSpeed = 14;
		private double playerPositionY;
		private double playerPositionX;
		private double meteorTimeToSpawn = 1500;

		private List<Bolt> bolt = new List<Bolt>();
		private List<Meteor> meteor = new List<Meteor>();

		private DispatcherTimer gameTimer;
		private DispatcherTimer keyboardTimer;
		private DispatcherTimer spawnMetheorTimer;
		private DispatcherTimer shootTimer;

		private bool isLeftKeyDown;
		private bool isRightKeyDown;
		private bool isDownKeyDown;
		private bool isUpKeyDown;

		private bool ableToShoot;

		private Popup popup;
		private Popup popupScore;
		private Popup popupButton;
		private Popup ScoreDisplayer;

		public bool GameIsPlayed;
		public Canvas canvas;
		public int playerScore;

		public GameScene()
		{
			CreateView();
			player = new Player();
			playerPositionY = 250;
			playerPositionX = 650;
			canvas.Children.Add(player);

			gameTimer = new DispatcherTimer();
			gameTimer.Interval = TimeSpan.FromMilliseconds(16);
			gameTimer.Tick += OnGameTimerTick;
			gameTimer.Start();

			keyboardTimer = new DispatcherTimer();
			keyboardTimer.Interval = TimeSpan.FromMilliseconds(16);
			keyboardTimer.Tick += OnKeyboardTimerTick;

			shootTimer = new DispatcherTimer();
			shootTimer.Interval = TimeSpan.FromSeconds(0.8);
			ableToShoot = true;
			shootTimer.Tick += OnShootTimerTick;
			shootTimer.Start();

			spawnMetheorTimer = new DispatcherTimer();
			spawnMetheorTimer.Interval = TimeSpan.FromMilliseconds(1500);
			spawnMetheorTimer.Tick += OnMetheorSpawnTick;
			spawnMetheorTimer.Start();
			GameIsPlayed = true;
		}


		private void CreateView()
		{

			var backgroundImage = new Bitmap("2.jpg"); // Ścieżka do pliku obrazu

			var backgroundBrush = new ImageBrush();
			backgroundBrush.Stretch = Stretch.Fill;
			backgroundBrush.Source = backgroundImage;

			canvas = new Canvas();
			canvas.Background = backgroundBrush;

			Content = canvas;

			this.KeyDown += HandleKeyDown;
			this.PointerPressed += OnPointerPressed;

			this.Focusable = true;
			this.Focus();

			popup = new Popup
			{
				PlacementMode = PlacementMode.AnchorAndGravity,
				PlacementGravity = PopupGravity.Top,
				PlacementTarget = this,
				HorizontalAlignment = Avalonia.Layout.HorizontalAlignment.Center,
				VerticalAlignment = Avalonia.Layout.VerticalAlignment.Center,
				IsOpen = false,
				Child = new TextBlock
				{

					Text = $"GAME OVER!",
					FontFamily = "Impact",
					FontSize = 60,
					Foreground = Brushes.Black,
					Background = Brushes.Red,
				}
			};
			ScoreDisplayer = new Popup
			{
				PlacementMode = PlacementMode.Top,
				HorizontalOffset = canvas.Bounds.Width + 650, 
				VerticalOffset = 50, 
				IsOpen = false,
				Child = new TextBlock
				{
					Text = $"Score : " + playerScore.ToString(),
					FontFamily = "Impact",
					FontSize = 16,
					Foreground = Brushes.Black,
					Background = Brushes.Red,
					Padding = new Thickness(5),
				}
			};

			popupScore = new Popup
			{
				PlacementMode = PlacementMode.AnchorAndGravity,
				PlacementGravity = PopupGravity.Bottom,
				PlacementTarget = this,
				HorizontalAlignment = Avalonia.Layout.HorizontalAlignment.Center,
				VerticalAlignment = Avalonia.Layout.VerticalAlignment.Center,
				IsOpen = false,
				Child = new TextBlock
				{
					Text = $"Score : " + playerScore.ToString(),
					FontFamily = "Impact",
					FontSize = 25,
					Foreground = Brushes.Black,
					Background = Brushes.Red,
				}
			};
			popupButton = new Popup
			{
				PlacementMode = PlacementMode.AnchorAndGravity,
				PlacementGravity = PopupGravity.Bottom,
				PlacementTarget = this,
				Height = 150,
				HorizontalAlignment = Avalonia.Layout.HorizontalAlignment.Center,
				VerticalAlignment = Avalonia.Layout.VerticalAlignment.Center,
				IsOpen = false,
				Child = new Button
				{

					Content = $"Reset",
					FontFamily = "Impact",
					FontSize = 20,
					Foreground = Brushes.Black,
					Background = Brushes.Red,
					Command = new RelayCommand(Reset),
				}
			};
			ScoreDisplayer.IsOpen = true;
			canvas.Children.Add(ScoreDisplayer); 
			canvas.Children.Add(popup);
			canvas.Children.Add(popupScore);
			canvas.Children.Add(popupButton);
		}
		private void HandleKeyDown(object sender, KeyEventArgs e)
		{
			if (player != null)
			{
				switch (e.Key)
				{
					case Key.W:
						playerPositionY -= playerSpeed;
						break;
					case Key.A:
						playerPositionX -= playerSpeed;
						break;
					case Key.S:
						playerPositionY += playerSpeed;
						break;
					case Key.D:
						playerPositionX += playerSpeed;
						break;
				}
				
				if (playerPositionX < 0)
				{
					playerPositionX = 0;
				}
				else if (playerPositionX > canvas.Bounds.Width - 50)
				{
					playerPositionX = canvas.Bounds.Width - 50;
				}

				if (playerPositionY > canvas.Bounds.Height - 50)
				{
					playerPositionY = canvas.Bounds.Height - 50;
				}
				else if (playerPositionY < 0)
				{
					playerPositionY = 0;
				}
				player.MoveRight(playerPositionX);
				player.MoveUp(playerPositionY);


				if (e.Key == Key.A)
				{
					isLeftKeyDown = true;
				}
				else if (e.Key == Key.D)
				{
					isRightKeyDown = true;
				}
				else if (e.Key == Key.S)
				{
					isDownKeyDown = true;
				}
				else if (e.Key == Key.W)
				{
					isUpKeyDown = true;
				}


				if (!keyboardTimer.IsEnabled)
				{
					keyboardTimer.Start();
				}

			}
		}
		private void OnKeyUp(object sender, KeyEventArgs e)
		{
			if (e.Key == Key.A)
			{
				isLeftKeyDown = false;
			}
			else if (e.Key == Key.D)
			{
				isRightKeyDown = false;
			}
			else if (e.Key == Key.S)
			{
				isDownKeyDown = false;
			}
			else if (e.Key == Key.W)
			{
				isUpKeyDown = false;
			}


			if (!isLeftKeyDown && !isRightKeyDown)
			{
				keyboardTimer.Stop();
			}
		}
		private void OnPointerPressed(object sender, PointerPressedEventArgs e)
		{
			if (e.GetCurrentPoint(canvas).Properties.IsLeftButtonPressed && ableToShoot)
			{
				ableToShoot = false;
				double playerX = playerPositionX;
				double playerY = playerPositionY;

				double mouseX = e.GetCurrentPoint(canvas).Position.X; 
				double mouseY = e.GetCurrentPoint(canvas).Position.Y; 

				bolt.Add(new Bolt(playerX, playerY, mouseX, mouseY));

				canvas.Children.Add(bolt[bolt.Count - 1]);
			}
		}
		private void OnGameTimerTick(object sender, EventArgs e)
		{
			if (bolt != null)
			{
				for (int i = 0; i < bolt.Count; i++)
				{
					if (bolt[i] != null && GameIsPlayed)
						bolt[i].Update();
				}
			}
			if (meteor != null)
			{
				for (int i = 0; i < meteor.Count; i++)
				{
					if (meteor[i] != null && GameIsPlayed)
						meteor[i].Update();
				}
			}
			CheckBoltCollision();
			CheckPlayerCollision();
		}
		private void OnMetheorSpawnTick(object sender, EventArgs e)
		{
            if (GameIsPlayed)
            {
				meteor.Add(new Meteor());
				canvas.Children.Add(meteor[meteor.Count - 1]);
			
				if (meteorTimeToSpawn > 460)
				{
					meteorTimeToSpawn -= 50;
					spawnMetheorTimer.Stop();
					spawnMetheorTimer.Interval = TimeSpan.FromMilliseconds(meteorTimeToSpawn); 
					spawnMetheorTimer.Start();
				}
			}
		}
		private void OnShootTimerTick(object sender, EventArgs e)
		{
			ableToShoot = true;
		}
		private void OnKeyboardTimerTick(object sender, EventArgs e)
		{
			if (player != null)
			{
				
				double x = Canvas.GetLeft(player);

				if (isLeftKeyDown)
				{
					x -= playerSpeed * 16 / 1000;
				}
				if (isRightKeyDown)
				{
					x += playerSpeed * 16 / 1000;
				}
				if (isDownKeyDown)
				{
					x -= playerSpeed * 16 / 1000;
				}
				if (isUpKeyDown)
				{
					x += playerSpeed * 16 / 1000;
				}
				// Ustawiamy maksymalne pole ruchu paletki
				x = Math.Max(0, x);
				x = Math.Min(1423 - player.Width, x);

				//Ustawiamy nowa pozycja paletki
				Canvas.SetLeft(player, x);

				if (!isLeftKeyDown && !isRightKeyDown && isDownKeyDown && isUpKeyDown)
				{
					keyboardTimer.Stop();
				}
			}
		}
		private void CheckBoltCollision()
		{
			for (int i = 0; i < bolt.Count; i++)
			{
				for (int j = 0; j < meteor.Count; j++)
				{
					if (bolt[i] != null && meteor[j] != null)
					{
						var boltRect = new Rect(Canvas.GetLeft(bolt[i]), Canvas.GetTop(bolt[i]), bolt[i].Width, bolt[i].Height);
						var meteorRect = new Rect(Canvas.GetLeft(meteor[j]), Canvas.GetTop(meteor[j]), meteor[j].Width, meteor[j].Height);

						double boltX = Canvas.GetLeft(bolt[i]);
						double boltY = Canvas.GetTop(bolt[i]);
						if ((boltY < -2050 || boltY > 3050) || (boltX < -2050 || boltX > 3050))
						{
							this.canvas.Children.Remove(bolt[i]);
							bolt[i].VelocityY = 0;
							bolt[i].VelocityX = 0;
							bolt[i] = null;
						}

						double meteorX = Canvas.GetLeft(meteor[j]);
						double meteorY = Canvas.GetTop(meteor[j]);
						if ((meteorY < -2050 || meteorY > 3050) || (meteorX < -2050 || meteorX > 3050))
						{
							this.canvas.Children.Remove(meteor[j]);
							meteor[j].VelocityY = 0;
							meteor[j].VelocityX = 0;
							meteor[j] = null;
						}

						if (meteorRect.Intersects(boltRect))
						{
							this.canvas.Children.Remove(bolt[i]);
							this.canvas.Children.Remove(meteor[j]);
							meteor[j].VelocityY = 0;
							meteor[j].VelocityX = 0;
							bolt[i].VelocityY = 0;
							bolt[i].VelocityX = 0;
							meteor[j] = null;
							bolt[i] = null;
							playerScore+= 10;
							UpdatePopupScore();
							return;
						}
					}
				}
			}
		}
		private void CheckPlayerCollision()
		{
			for (int j = 0; j < meteor.Count; j++)
			{
				if (player != null && meteor[j] != null && GameIsPlayed)
				{
					var playerRect = new Rect(Canvas.GetLeft(player), Canvas.GetTop(player), player.Width, player.Height); ;
					var meteorRect = new Rect(Canvas.GetLeft(meteor[j]), Canvas.GetTop(meteor[j]), meteor[j].Width, meteor[j].Height);
					if (meteorRect.Intersects(playerRect))
					{
						this.canvas.Children.Remove(player);
						this.canvas.Children.Remove(meteor[j]);
						GameIsPlayed = false;
						meteor[j].VelocityY = 0;
						meteor[j].VelocityX = 0;
						meteor[j] = null;
						player = null;
						ScoreDisplayer.IsOpen = false;
						popup.IsOpen = true;
						popupScore.IsOpen = true;
						popupButton.IsOpen = true;
						return;
					}
				}
			}
		}
		public void Reset()
		{
			popup.IsOpen = false;
			popupScore.IsOpen = false;
			popupButton.IsOpen = false;
			ScoreDisplayer.IsOpen = true;
			playerScore = 0;
			UpdatePopupScore();
			GameIsPlayed = true;
			meteorTimeToSpawn = 1500;
			player = new Player();
			playerPositionY = 250;
			playerPositionX = 650;
			canvas.Children.Add(player);

			new GameScene();
		}
		private void UpdatePopupScore()
		{
			var textBlock = popupScore.Child as TextBlock;
			if (textBlock != null)
			{
				textBlock.Text = $"Score: {playerScore}";
			}
			var textBlock2 = ScoreDisplayer.Child as TextBlock;
			if (textBlock2 != null)
			{
				textBlock2.Text = $"Score: {playerScore}";
			}
		}
	}	
}
