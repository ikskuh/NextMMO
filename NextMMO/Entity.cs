using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

namespace NextMMO
{
	public class Entity
	{
		private readonly World world;
		private float x;
		private float y;

		public Entity(World world)
		{
			this.world = world;
			this.Size = 0.7; // Make entity 0.5 tiles large.
		}

		public Entity(World world, int x, int y)
			: this(world)
		{
			this.x = x;
			this.y = y;
		}

		public virtual void Update()
		{

		}

		public virtual void Draw(System.Drawing.Graphics graphics)
		{
			this.Sprite.Draw(
				graphics,
				(int)(32 * this.x),
				(int)(32 * this.y));
		}

		public void Translate(float deltaX, float deltaY)
		{
			var map = this.world.TileMap;

			int cx = (int)(this.x + 0.5);
			int cy = (int)(this.y + 0.5);
			int size = (int)(32 * this.Size);

			// Build "environment" of collision rectangles
			List<Rectangle> environment = new List<Rectangle>();

			// Add "static" environment (contains world boundaries)
			environment.Add(new Rectangle(0, 0, 2, 32 * this.world.TileMap.Height));
			environment.Add(new Rectangle(32 * this.world.TileMap.Width - 2, 0, 2, 32 * this.world.TileMap.Height));

			environment.Add(new Rectangle(0, 0, 32 * this.world.TileMap.Width, 2));
			environment.Add(new Rectangle(0, 32 * this.world.TileMap.Height- 2, 32 * this.world.TileMap.Width, 2));

			// Add "dynamic" environment (contains tile information around the player)
			for (int layer = 0; layer < 2; layer++) // Use only the lower two layers
			{
				for (int px = Math.Max(0, cx - 1); px < Math.Min(map.Width, cx + 2); px++)
				{
					for (int py = Math.Max(0, cy - 1); py < Math.Min(map.Height, cy + 2); py++)
					{
						var tile = this.world.TileSet.TilePassages[this.world.TileMap[px, py][layer]];
						if (!tile.HasFlag(TileSide.Center))
						{
							environment.Add(new Rectangle(
									32 * px,
									32 * py,
									32,
									32));
							continue; // Skip other sides, this tile is completly blocked
						}
						if (!tile.HasFlag(TileSide.Left))
						{
							environment.Add(new Rectangle(
									32 * px,
									32 * py,
									2,
									32));
						}
						if (!tile.HasFlag(TileSide.Right))
						{
							environment.Add(new Rectangle(
									32 * px + 30,
									32 * py,
									2,
									32));
						}
						if (!tile.HasFlag(TileSide.Top))
						{
							environment.Add(new Rectangle(
									32 * px,
									32 * py,
									32,
									2));
						}
						if (!tile.HasFlag(TileSide.Bottom))
						{
							environment.Add(new Rectangle(
									32 * px,
									32 * py + 30,
									2,
									32));
						}
					}
				}
			}

			foreach(var rect in environment)
			{
				this.world.Debug(rect, Color.Lime);
			}

			Func<int, int, bool> testCollision = (_x, _y) =>
				{
					Rectangle entity = new Rectangle(
						_x - size / 2,
						_y - size / 2,
						size, 
						size);

					this.world.Debug(entity, Color.Magenta);

					foreach(var rect in environment)
					{
						if (rect.IntersectsWith(entity))
							return true; // Cancel translation, we will intersect with a wall
					}
					return false;
				};

			var newX = this.x + deltaX;
			var newY = this.y + deltaY;

			if(testCollision((int)(32 * newX + 16), (int)(32 * this.y + 16)))
				deltaX = 0; // Elimitate x movement
			if(testCollision((int)(32 * this.X + 16), (int)(32 * newY + 16)))
				deltaY = 0; // Elimitate x movement

			this.x += deltaX;
			this.y += deltaY;
		}

		public double X { get { return x; } }

		public double Y { get { return y; } }

		public double Size { get; set; }

		public Sprite Sprite { get; set; }
	}
}
