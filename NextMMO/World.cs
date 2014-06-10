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
			this.debugDraws.Enqueue(() =>
				{
					this.Services.Graphics.FillRectangle(
						Color.FromArgb(128, color.R, color.G, color.B),
						rect);
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
	}
}
