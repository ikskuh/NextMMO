using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

namespace NextMMO
{
	public class World : GameObject
	{
		private readonly List<IEntity> entities;
		private readonly Queue<Action> debugDraws = new Queue<Action>();
		private TileMap map;
		private TileSet tileSet;
		private float scrollX, scrollY;

		public World(IGameServices services)
			: base(services)
		{
			this.entities = new List<IEntity>();
		}

		public void Update()
		{
			// Clone entities into array to allow
			// removal or addition of entities while updating.
			foreach (var entity in this.entities.ToArray())
			{
				entity.Update();
			}

			if (this.Focus != null)
			{
				float delta = 32.0f * 3.5f;
				float fx = 32.0f * (float)this.Focus.X;
				float fy = 32.0f * (float)this.Focus.Y;
				float sw = 640;
				float sh = 480;

				// Horizontal scrolling
				if (fx > sw + this.scrollX - delta)
					this.scrollX = Math.Min(fx - sw + delta, 32 * this.map.Width - sw);
				if (fx < this.scrollX + delta)
					this.scrollX = Math.Max(fx - delta, 0);

				// Vertical scrolling
				if (fy > sh + this.scrollY - delta)
					this.scrollY = Math.Min(fy - sh + delta, 32 * this.map.Height - sh);
				if (fy < this.scrollY + delta)
					this.scrollY = Math.Max(fy - delta, 0);
			}

		}

		public delegate Collider EnvironmentCheckDelegate(double x, double y, double size);

		/// <summary>
		/// Triggers an entity at a given position.
		/// </summary>
		/// <param name="actuator">The actuatuor that triggers the entity.</param>
		/// <param name="x"></param>
		/// <param name="y"></param>
		/// <param name="radius"></param>
		/// <returns></returns>
		public bool TriggerAt(Entity actuator, double x, double y, double radius)
		{
			var environment = BuildEnvironment(x, y);

			var collider = environment(x, y, 2.0 * radius);

			if (collider == null)
				return false;

			Collider.EntityCollider entCol = collider as Collider.EntityCollider;
			if (entCol == null)
				return false;

			if (entCol.Entity == null)
				throw new InvalidOperationException("Invalid collider detected!");

			entCol.Entity.Trigger(actuator);

			return true;
		}

		/// <summary>
		/// Creates a delegate that can check for collisions at the given position.
		/// </summary>
		/// <returns>Collision checking delegate.</returns>
		public EnvironmentCheckDelegate BuildEnvironment(double x, double y)
		{
			int cx = (int)(x / 32.0 + 0.5);
			int cy = (int)(y / 32.0 + 0.5);
			var map = this.TileMap;

			// Build "environment" of collision rectangles
			List<Collider> environment = new List<Collider>();

			// Add "static" environment (contains world boundaries)
			environment.Add(new Collider.BoundaryCollider(new Rectangle(0, 0, 2, 32 * this.TileMap.Height)));
			environment.Add(new Collider.BoundaryCollider(new Rectangle(32 * this.TileMap.Width - 2, 0, 2, 32 * this.TileMap.Height)));

			environment.Add(new Collider.BoundaryCollider(new Rectangle(0, 0, 32 * this.TileMap.Width, 2)));
			environment.Add(new Collider.BoundaryCollider(new Rectangle(0, 32 * this.TileMap.Height - 2, 32 * this.TileMap.Width, 2)));

			// Add "dynamic" environment (contains tile information around the player)
			for (int layer = 0; layer < 2; layer++) // Use only the lower two layers
			{
				for (int px = Math.Max(0, cx - 1); px < Math.Min(map.Width, cx + 2); px++)
				{
					for (int py = Math.Max(0, cy - 1); py < Math.Min(map.Height, cy + 2); py++)
					{
						environment.AddRange(this.TileSet[this.TileMap[px, py][layer]].CreateEnvironment(px, py));
					}
				}
			}

			foreach (var entity in this.Entities)
			{
				foreach (var collider in entity.GetColliders())
				{
					collider.Translate(32 * entity.X + 16, 32 * entity.Y + 16);
					environment.Add(collider);
				}
			}

			// Debug environment

			foreach (var rect in environment)
			{
				this.Debug(rect.Rectangle, rect.Color);
			}

			return (_x, _y, _size) =>
			{
				int size = (int)(32 * _size);
				Rectangle entity = new Rectangle(
					(int)_x - size / 2,
					(int)_y - size / 2,
					size,
					size);

				// Debug entity collider
				//this.world.Debug(entity, Color.Magenta);

				foreach (var collider in environment)
				{
					if (collider.IntersectsWith(entity))
						return collider; // Cancel translation, we will intersect with a wall
				}
				return null;
			};
		}

		public void Draw()
		{
			this.TileMap.Draw(this.Services.Graphics);
			while (this.debugDraws.Count > 0)
			{
				this.debugDraws.Dequeue()();
			}
		}

		public void Debug(Rectangle rect, Color color)
		{
			if (!this.EnableDebug)
				return;
			this.debugDraws.Enqueue(() =>
				{
					this.Services.Graphics.FillRectangle(
						Color.FromArgb(128, color.R, color.G, color.B),
						new Rectangle(
							rect.Left - (int)this.scrollX,
							rect.Top - (int)this.scrollY,
							rect.Width,
							rect.Height));
				});
		}

		#region Rendering

		void map_PostRenderLayer(object sender, RenderLayerEventArgs e)
		{
			switch (e.Layer)
			{
				case 1:
					this.entities.Sort((a, b) => Math.Sign(a.Y - b.Y));
					foreach (var ent in this.entities)
					{
						ent.Draw(e.Graphics, this.scrollX, this.scrollY);
					}
					break;
			}
		}

		void map_PreRenderMap(object sender, EventArgs e)
		{
			this.Services.Graphics.DrawImage(
				this.Services.Resources.Bitmaps["007-Ocean01"],
				new Rectangle(0, 0, 640, 480));
		}

		void map_RenderTile(object sender, RenderTileEventArgs e)
		{
			this.tileSet[e.ID].Draw(
				e.Graphics,
				(int)(32 * e.X - this.scrollX),
				(int)(32 * e.Y - this.scrollY));
		}

		#endregion

		public ICollection<IEntity> Entities { get { return this.entities; } }

		public TileMap TileMap
		{
			get { return this.map; }
			set
			{
				if (this.map != null)
				{
					this.map.RenderTile -= map_RenderTile;
					this.map.PreRenderMap -= map_PreRenderMap;
					this.map.PostRenderLayer -= map_PostRenderLayer;
				}
				this.map = value;
				if (this.map != null)
				{
					this.map.RenderTile += map_RenderTile;
					this.map.PreRenderMap += map_PreRenderMap;
					this.map.PostRenderLayer += map_PostRenderLayer;
				}
			}
		}

		public TileSet TileSet
		{
			get { return tileSet; }
			set { tileSet = value; }
		}

		public Bitmap Background { get; set; }

		public Entity Focus { get; set; }

		public bool EnableDebug { get; set; }
	}
}
