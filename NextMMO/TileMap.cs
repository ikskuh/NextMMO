using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NextMMO
{
	public sealed class TileMap
	{
		public const int LayerCount = 3;

		private readonly Tile[,] tiles;

		public event EventHandler PreRenderMap;
		public event EventHandler<RenderTileEventArgs> RenderTile;
		public event EventHandler<RenderLayerEventArgs> PreRenderLayer;
		public event EventHandler<RenderLayerEventArgs> PostRenderLayer;
		public event EventHandler PostRenderMap;

		public TileMap(int width, int height)
		{
			if (width < 20) throw new ArgumentOutOfRangeException("width", "width must be at least 20.");
			if (height < 15) throw new ArgumentOutOfRangeException("height", "width must be at least 15.");
			this.tiles = new Tile[width, height];
			for (int x = 0; x < this.Width; x++)
			{
				for (int y = 0; y < this.Height; y++)
				{
					this.tiles[x, y] = new Tile();
				}
			}
		}

		public void Draw(Rectangle rect)
		{
			if (this.PreRenderMap != null)
				this.PreRenderMap(this, EventArgs.Empty);
			for (int layer = 0; layer < TileMap.LayerCount; layer++)
			{
				if (this.PreRenderLayer != null)
					this.PreRenderLayer(this, new RenderLayerEventArgs(layer));
				for (int x = rect.Left; x < rect.Right; x++)
				{
					for (int y = rect.Top; y < rect.Bottom; y++)
					{
						if (this[x, y][layer] <= 0)
							continue;
						if (this.RenderTile != null)
							this.RenderTile(this, new RenderTileEventArgs(x - rect.Left, y - rect.Top, this[x, y][layer] - 1));
					}
				}
				if (this.PostRenderLayer != null)
					this.PostRenderLayer(this, new RenderLayerEventArgs(layer));
			}
			if (this.PostRenderMap != null)
				this.PostRenderMap(this, EventArgs.Empty);
		}

		public Tile this[int x, int y]
		{
			get { return this.tiles[x, y]; }
		}

		public int Width { get { return this.tiles.GetLength(0); } }

		public int Height { get { return this.tiles.GetLength(1); } }
	}

	public sealed class Tile
	{
		private readonly int[] sprites = new int[TileMap.LayerCount];

		public int this[int layer]
		{
			get { return this.sprites[layer]; }
			set { this.sprites[layer] = value; }
		}
		public string Script { get; set; }
	}

	public sealed class RenderLayerEventArgs : EventArgs
	{
		public RenderLayerEventArgs(int layer)
		{
			this.Layer = layer;
		}

		public int Layer { get; private set; }
	}

	public sealed class RenderTileEventArgs : EventArgs
	{
		public RenderTileEventArgs(int x, int y, int id)
		{
			this.X = x;
			this.Y = y;
			this.ID = id;
		}

		public int X { get; private set; }

		public int Y { get; private set; }

		public int ID { get; private set; }
	}
}
