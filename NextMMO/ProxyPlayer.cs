using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

namespace NextMMO
{
	public sealed class ProxyPlayer : IEntity
	{
		private PlayerData data = new PlayerData();
		private readonly IGameServices services;


		public ProxyPlayer(IGameServices services)
		{
			this.services = services;
		}

		public void Update()
		{

		}

		public void Draw(IGraphics graphics, float deltaX, float deltaY)
		{
			int cx = (int)(32 * this.X - deltaX);
			int cy = (int)(32 * this.Y - deltaY);

			this.Sprite.Draw(graphics, cx, cy);

			var font = graphics.GetFont(FontSize.Small);
			var size = graphics.MeasureString(this.data.Name, font);

			graphics.DrawString(
				this.data.Name,
				font,
				Color.Black,
				cx - 0.5f * size.Width + TileMap.TileSize / 2,
				cy - 24 - size.Height);
		}

		public double X { get; set; }

		public double Y { get; set; }

		public int Direction
		{
			get { return this.Sprite.Animation; }
			set { this.Sprite.Animation = value; }
		}

		public AnimatedSprite Sprite { get; set; }

		Sprite IEntity.Sprite { get { return this.Sprite; } }

		public PlayerData Data
		{
			get { return data; }
			set 
			{
				if(value == null)
				{
					throw new InvalidOperationException("Cannot assign null to Data.");
				}
				if(this.data.Sprite != value.Sprite)
				{
					this.Sprite = new AnimatedSprite(
						this.Services.Resources.Characters[value.Sprite],
						new Point(16, 42));
				}
				data = value; 
			}
		}

		public IGameServices Services
		{
			get { return services; }
		} 
	}
}
