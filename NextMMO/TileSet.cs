using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

namespace NextMMO
{
	public class TileSet
	{
		IGameServices services;
		private readonly int sizeX, sizeY;
		private readonly TileSide[] tilePassages;

		public TileSet(IGameServices services, int sizeX, int sizeY)
		{
			this.services = services;
			this.sizeX = sizeX;
			this.sizeY = sizeY;
			this.tilePassages = new TileSide[sizeX * sizeY + 1];
			this.tilePassages[0] = TileSide.All;
		}

		internal void DrawTile(int x, int y, int id)
		{
			var texmap = this.Source;
			this.services.Graphics.DrawImage(
				texmap,
				new Rectangle(32 * x, 32 * y, 32, 32),
				new Rectangle(
					32 * (id % this.sizeX),
					32 * (id / this.sizeX),
					32, 32),
				GraphicsUnit.Pixel);
		}

		public bool IsWalkable(int id)
		{
			return this.tilePassages[id].HasFlag(TileSide.Center);
		}

		public bool IsPassable(int id, TileSide side)
		{
			switch(side)
			{
				case TileSide.Left:
				case TileSide.Right:
				case TileSide.Top:
				case TileSide.Bottom:
					return this.tilePassages[id].HasFlag(side);
				default:
					throw new ArgumentException("side can only be Left, Right, Top or Bottom.", "side");
			}
		}

		public Bitmap Source { get; set; }

		public TileSide[] TilePassages
		{
			get { return tilePassages; }
		}

		public int SizeX
		{
			get { return sizeX; }
		}

		public int SizeY
		{
			get { return sizeY; }
		}
	}

	public enum TileSide
	{
		None,
		Left = (1 << 0),
		Right = (1 << 1),
		Top = (1 << 2),
		Bottom = (1 << 3),
		Center = (1 << 4),
		All = Left | Right | Top | Bottom | Center
	}
}
