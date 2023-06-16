using Avalonia.Animation.Easings;
using Avalonia.Animation;
using Avalonia.Controls;
using Avalonia.Controls.Shapes;
using Avalonia.Media;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Avalonia.Media.Imaging;

namespace AvaloniaGame1.Models
{
	internal class Bolt : Ellipse
	{
		public double VelocityX { get; set; }
		public double VelocityY { get; set; }

		public Bolt(double playerX, double playerY, double mouseX, double mouseY)
		{
			Bitmap bitmap = new Bitmap("bolt.png");
			ImageBrush imageBrush = new ImageBrush();
			imageBrush.Source = bitmap;

			this.Fill = imageBrush;

			double deltaX = mouseX - playerX;
			double deltaY = mouseY - playerY;
			double distance = Math.Sqrt(deltaX * deltaX + deltaY * deltaY);

			double speed = 450;

			VelocityX = speed * (deltaX / distance);
			VelocityY = speed * (deltaY / distance);

			this.Width = 20;
			this.Height = 20;

			Canvas.SetLeft(this, playerX+15);
			Canvas.SetTop(this, playerY+15);

		}

		public void Update()
		{
			double x = Canvas.GetLeft(this);
			double y = Canvas.GetTop(this);

			x += VelocityX * 16 / 1000;
			y += VelocityY * 16 / 1000;

			Canvas.SetLeft(this, x);
			Canvas.SetTop(this, y);
		}
	}
}
