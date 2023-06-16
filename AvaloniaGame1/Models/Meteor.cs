using Avalonia.Controls.Shapes;
using Avalonia.Controls;
using Avalonia.Media;
using System;
using Avalonia.Media.Imaging;

namespace AvaloniaGame1.Models
{
	internal class Meteor : Rectangle
	{
		private static Random random = new Random();
		public double VelocityX { get; set; }
		public double VelocityY { get; set; }

		public Meteor()
		{
			var size = random.Next(50, 101);
			this.Width = size;
			this.Height = size;
			Bitmap bitmap = new Bitmap("meteor.png");
			ImageBrush imageBrush = new ImageBrush();
			imageBrush.Source = bitmap;

			this.Fill = imageBrush;


			double screenWidth = 1400;
			double screenHeight = 800; 

			double spawnX, spawnY; 

			double randomSide = random.NextDouble();

			if (randomSide < 0.25) 
			{
				spawnX = 0;
				spawnY = screenHeight;
			}
			else if (randomSide < 0.5) 
			{
				spawnX = screenWidth;
				spawnY = 0;
			}
			else if (randomSide < 0.75) 
			{
				spawnX = screenWidth;
				spawnY = screenHeight;
			}
			else 
			{
				spawnX = 0;
				spawnY = 0;
			}

			double centerX = random.Next(500/2, (1400 + 500)/2);
			double centerY = random.Next(200/2, (800 + 500)/2); 

			double deltaX = centerX - spawnX;
			double deltaY = centerY - spawnY;

			double distance = Math.Sqrt(deltaX * deltaX + deltaY * deltaY);

			double speed = random.Next(300, 700);

			VelocityX = speed * (deltaX / distance);
			VelocityY = speed * (deltaY / distance);

			Canvas.SetLeft(this, spawnX);
			Canvas.SetTop(this, spawnY);
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