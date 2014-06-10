using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;

namespace NextMMO
{
	public class TileSet
	{
		readonly IGameServices services;

		private readonly int sizeX, sizeY;
		private readonly TileDefinition[] tiles;

		TileDefinition emptyTile;

		public TileSet(IGameServices services, int sizeX, int sizeY)
		{
			this.services = services;
			this.sizeX = sizeX;
			this.sizeY = sizeY;
			this.tiles = new TileDefinition[this.sizeX * this.sizeY];
			for (int i = 0; i < this.tiles.Length; i++)
			{
				this.tiles[i] = new TileDefinition(this);
				this.tiles[i].Source = new Rectangle(
					TileMap.TileSize * (i % this.sizeX),
					TileMap.TileSize * (i / this.sizeX),
					TileMap.TileSize,
					TileMap.TileSize);
			}

			this.emptyTile = new TileDefinition(this);
			this.emptyTile.Source = new Rectangle(0, 0, 0, 0);
			//this.emptyTile.Colliders.Add(new TileCollider()
			//	{
			//		Rectangle = new Rectangle(0, 0, 32, 32)
			//	});
		}

		#region Save/Load

		public void Save(Stream stream)
		{
			BinaryWriter writer = new BinaryWriter(stream, Encoding.UTF8);

			writer.Write("TILESET"); // ID
			writer.Write((int)1); // Version Major
			writer.Write((int)0); // Version Minor
			writer.Write(this.sizeX);
			writer.Write(this.sizeY);
			writer.Write(this.Source);
			for (int i = 0; i < this.tiles.Length; i++)
			{
				var tdef = this.tiles[i];

				writer.Write(tdef.Source.X);
				writer.Write(tdef.Source.Y);
				writer.Write(tdef.Source.Width);
				writer.Write(tdef.Source.Height);

				writer.Write(tdef.Colliders.Count);
				foreach (var collider in tdef.Colliders)
				{
					writer.Write(collider.Name);
					writer.Write(collider.Rectangle.X);
					writer.Write(collider.Rectangle.Y);
					writer.Write(collider.Rectangle.Width);
					writer.Write(collider.Rectangle.Height);
				}
			}
		}

		public static TileSet Load(IGameServices services, Stream stream)
		{
			BinaryReader reader = new BinaryReader(stream, Encoding.UTF8);

			if (reader.ReadString() != "TILESET") // ID
				throw new InvalidDataException();
			if (reader.ReadInt32() != 1) // Version Major
				throw new InvalidDataException();
			if (reader.ReadInt32() != 0) // Version Minor
				throw new InvalidDataException();

			int sizeX = reader.ReadInt32();
			int sizeY = reader.ReadInt32();

			var tileSet = new TileSet(services, sizeX, sizeY);
			tileSet.Source = reader.ReadString();
			for (int i = 0; i < tileSet.tiles.Length; i++)
			{
				var tdef = tileSet.tiles[i];

				tdef.Source.X = reader.ReadInt32();
				tdef.Source.Y = reader.ReadInt32();
				tdef.Source.Width = reader.ReadInt32();
				tdef.Source.Height = reader.ReadInt32();

				int count = reader.ReadInt32();
				for (int j = 0; j < count; j++)
				{
					TileCollider collider = new TileCollider();
					collider.Name = reader.ReadString();

					Rectangle rect = new Rectangle();
					rect.X = reader.ReadInt32();
					rect.Y = reader.ReadInt32();
					rect.Width = reader.ReadInt32();
					rect.Height = reader.ReadInt32();
					collider.Rectangle = rect;

					tdef.Colliders.Add(collider);
				}
			}

			return tileSet;
		}

		#endregion

		public TileDefinition this[int id]
		{
			get
			{
				if (id <= 0 || id > this.tiles.Length)
					return this.emptyTile;
				return this.tiles[id - 1];
			}
		}

		public string Source { get; set; }

		public int SizeX
		{
			get { return sizeX; }
		}

		public int SizeY
		{
			get { return sizeY; }
		}

		public IGameServices Services
		{
			get { return services; }
		}
	}

	public class TileDefinition
	{
		readonly TileSet tileSet;

		private List<TileCollider> colliders = new List<TileCollider>();

		internal TileDefinition(TileSet set)
		{
			this.tileSet = set;
		}

		/// <summary>
		/// Gets or sets the source rectangle.
		/// </summary>
		public Rectangle Source;

		/// <summary>
		/// Creates the collision environment for this tile.
		/// </summary>
		/// <param name="x">X-coordinate of the environment in tile coordinates.</param>
		/// <param name="y">Y-coordinate of the environment in tile coordinates.</param>
		/// <returns>Array of rectangles containing the collision environment.</returns>
		public Rectangle[] CreateEnvironment(int x, int y)
		{
			List<Rectangle> environment = new List<Rectangle>();

			foreach (var collider in this.colliders)
			{
				Rectangle rect = new Rectangle(
					32 * x + collider.Rectangle.X,
					32 * y + collider.Rectangle.Y,
					collider.Rectangle.Width,
					collider.Rectangle.Height);
				environment.Add(rect);
			}

			return environment.ToArray();
		}

		/// <summary>
		/// Draws the tile at the given tile position.
		/// </summary>
		/// <param name="x">X-coordinate to draw the tile at.</param>
		/// <param name="y">Y-coordinate to draw the tile at.</param>
		public void Draw(IGraphics g, int x, int y)
		{
			var texmap = this.tileSet.Services.Resources.Bitmaps[this.tileSet.Source];
			g.DrawImage(
				texmap,
				new Rectangle(x, y, 32, 32),
				this.Source);
		}

		/// <summary>
		/// Gets a collection of colliders for this tile.
		/// </summary>
		public ICollection<TileCollider> Colliders { get { return this.colliders; } }

		/// <summary>
		/// Gets the tile set this tile definition is in.
		/// </summary>
		public TileSet TileSet
		{
			get { return tileSet; }
		}
	}

	public class TileCollider
	{
		public static Brush DebugBrush = new SolidBrush(Color.FromArgb(128, 255, 0, 255));

		public TileCollider()
		{
			this.Name = "Collider";
			this.Rectangle = new Rectangle(4, 4, 24, 24);
		}

		public TileCollider Clone()
		{
			return new TileCollider()
			{
				Rectangle = this.Rectangle,
				Name = this.Name
			};
		}

		/// <summary>
		/// Gets or sets the collider name.
		/// </summary>
		public string Name { get; set; }

		/// <summary>
		/// Gets or sets the collider rectangle.
		/// </summary>
		public Rectangle Rectangle { get; set; }

		public override string ToString()
		{
			return this.Name ?? "<null>";
		}
	}
}
