using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

namespace NextMMO
{
	public class World
	{
		private readonly IGameServices services;
		private readonly HashSet<Entity> entities = new HashSet<Entity>();
		private readonly Queue<Action> debugDraws = new Queue<Action>();
		private TileMap map;
		private TileSet tileSet;

		public World(IGameServices services)
		{
			this.services = services;
		}

		public void Update()
		{
			// Clone entities into array to allow
			// removal or addition of entities while updating.
			foreach(var entity in this.entities.ToArray())
			{
				entity.Update();
			}
		}

		public void Draw()
		{
			this.TileMap.Draw(new Rectangle(0, 0, 20, 15));
			while(this.debugDraws.Count > 0)
			{
				this.debugDraws.Dequeue()();
			}
		}

		public void Debug(Rectangle rect, Color color)
		{
			this.debugDraws.Enqueue(() =>
				{
					this.Services.Graphics.FillRectangle(
						new SolidBrush(Color.FromArgb(128, color.R, color.G, color.B)),
						rect);
				});
		}

		#region Rendering

		void map_PostRenderLayer(object sender, RenderLayerEventArgs e)
		{
			switch (e.Layer)
			{
				case 1:
					foreach(var ent in this.entities)
					{
						ent.Draw(this.services.Graphics);
					}
					break;
			}
		}

		void map_PreRenderMap(object sender, EventArgs e)
		{
			this.services.Graphics.DrawImage(
				this.services.Bitmaps["007-Ocean01"],
				new Rectangle(0, 0, 640, 480));
		}

		void map_RenderTile(object sender, RenderTileEventArgs e)
		{
			this.tileSet[e.ID].Draw(e.X, e.Y);
		}

		#endregion

		public ICollection<Entity> Entities { get { return this.entities; } }

		public TileMap TileMap
		{
			get { return this.map; }
			set
			{
				if(this.map != null)
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

		public IGameServices Services
		{
			get { return services; }
		} 
	}
}
