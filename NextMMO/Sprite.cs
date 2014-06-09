using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

namespace NextMMO
{
	public abstract class Sprite
	{
		public void Draw(IGraphics graphics, int x, int y)
		{
			this.OnDraw(
				graphics,
				x + TileMap.TileSize / 2 - this.Offset.X,
				y + TileMap.TileSize / 2 - this.Offset.Y);
		}

		protected abstract void OnDraw(IGraphics g, int x, int y);

		public Point Offset { get; set; }
	}
}
