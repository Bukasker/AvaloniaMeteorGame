using Avalonia.Controls;
using Avalonia.Controls.Shapes;
using Avalonia.Media;
using Avalonia.Media.Imaging;

namespace AvaloniaGame1.Models
{
	internal class Player : Rectangle
	{
		public Player()
		{
			this.Width = 50;
			this.Height = 50;

			Bitmap bitmap = new Bitmap("player.png");
			ImageBrush imageBrush = new ImageBrush();
			imageBrush.Source = bitmap;
			this.Fill = imageBrush;

			Canvas.SetLeft(this, 650);
			Canvas.SetTop(this, 250);
		}
		public void MoveRight(double distance)
		{
			double x = Canvas.GetLeft(this);

			x = distance;

			Canvas.SetLeft(this, x);
		}

		public void MoveUp(double distance)
		{
			double y = Canvas.GetLeft(this);

			y = distance;

			Canvas.SetTop(this, y);
		}
	}
}
