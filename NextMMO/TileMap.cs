using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NextMMO
{
	public sealed class TileMap
	{
		public const int LayerCount = 3;
		public const int TileSize = 32;

		private readonly Tile[,] tiles;

		public event EventHandler<RenderMapEventArgs> PreRenderMap;
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

		#region Save/Load

		public void Save(Stream stream)
		{
			BinaryWriter writer = new BinaryWriter(stream, Encoding.UTF8);

			writer.Write("TILEMAP"); // Identifier
			writer.Write(1); // Version Major
			writer.Write(1); // Version Minor

			writer.Write(this.Width);
			writer.Write(this.Height);

			for (int x = 0; x < this.Width; x++)
			{
				for (int y = 0; y < this.Height; y++)
				{
					var tile = this.tiles[x, y];
					for (int layer = 0; layer < TileMap.LayerCount; layer++)
					{
						writer.Write(tile[layer]);
					}
				}
			}
		}

		public static TileMap Load(Stream stream)
		{
			BinaryReader reader = new BinaryReader(stream, Encoding.UTF8);

			if (reader.ReadString() != "TILEMAP")
				throw new InvalidDataException();
			int major = reader.ReadInt32();
			int minor = reader.ReadInt32();
			switch (major)
			{
				case 1:
					int width = reader.ReadInt32();
					int height = reader.ReadInt32();

					var map = new TileMap(width, height);
					for (int x = 0; x < map.Width; x++)
					{
						for (int y = 0; y < map.Height; y++)
						{
							var tile = map.tiles[x, y];
							for (int layer = 0; layer < TileMap.LayerCount; layer++)
							{
								tile[layer] = reader.ReadInt32();
							}
							if(minor == 0)
							{
								// Read deprecated "Script" string
								reader.ReadString();
							}
							else if(minor == 1)
							{
								// Don't read any string here
							}
						}
					}
					return map;
				default:
					throw new InvalidDataException();
			}
		}

		#endregion

		public void Draw(IGraphics graphics)
		{
			if (this.PreRenderMap != null)
				this.PreRenderMap(this, new RenderMapEventArgs(graphics));
			for (int layer = 0; layer < TileMap.LayerCount; layer++)
			{
				if (this.PreRenderLayer != null)
					this.PreRenderLayer(this, new RenderLayerEventArgs(graphics, layer));
				for (int x = 0; x < this.Width; x++)
				{
					for (int y = 0; y < this.Height; y++)
					{
						if (this[x, y][layer] <= 0)
							continue;
						if (this.RenderTile != null)
							this.RenderTile(this, new RenderTileEventArgs(graphics, x, y, this[x, y][layer]));
					}
				}
				if (this.PostRenderLayer != null)
					this.PostRenderLayer(this, new RenderLayerEventArgs(graphics, layer));
			}
			if (this.PostRenderMap != null)
				this.PostRenderMap(this, new RenderMapEventArgs(graphics));
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

		public Tile()
		{

		}

		public int this[int layer]
		{
			get { return this.sprites[layer]; }
			set { this.sprites[layer] = value; }
		}
	}

	public sealed class RenderMapEventArgs : EventArgs
	{
		public RenderMapEventArgs(IGraphics graphics)
		{
			this.Graphics = graphics;
		}

		public IGraphics Graphics { get; private set; }
	}

	public sealed class RenderLayerEventArgs : EventArgs
	{
		public RenderLayerEventArgs(IGraphics graphics, int layer)
		{
			this.Graphics = graphics;
			this.Layer = layer;
		}

		public IGraphics Graphics { get; private set; }

		public int Layer { get; private set; }
	}

	public sealed class RenderTileEventArgs : EventArgs
	{
		public RenderTileEventArgs(IGraphics graphics, int x, int y, int id)
		{
			this.Graphics = graphics;
			this.X = x;
			this.Y = y;
			this.ID = id;
		}

		public IGraphics Graphics { get; private set; }

		public int X { get; private set; }

		public int Y { get; private set; }

		public int ID { get; private set; }
	}
}
