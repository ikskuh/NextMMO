using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

namespace NextMMO
{
	public abstract class Collider
	{
		private Rectangle rect;

		protected Collider(Rectangle rect)
		{
			this.rect = rect;
		}

		public bool IntersectsWith(Rectangle rect)
		{
			return this.rect.IntersectsWith(rect);
		}

		public void Translate(double deltaX, double deltaY)
		{
			this.rect.X += (int)deltaX;
			this.rect.Y += (int)deltaY;
		}

		public Rectangle Rectangle { get { return this.rect; } }

		public sealed class TileCollider : Collider
		{
			readonly TileDefinition tile;

			public TileCollider(TileDefinition tile, Rectangle rect) : base(rect) 
			{
				this.tile = tile;
			}

			public TileDefinition Tile
			{
				get { return tile; }
			}

			public override Color Color { get { return Color.Blue; } }
		}

		public sealed class EntityCollider : Collider
		{
			readonly NextMMO.Entity tile;

			public EntityCollider(NextMMO.Entity entity, Rectangle rect)
				: base(rect)
			{
				this.tile = entity;
			}

			public NextMMO.Entity Entity
			{
				get { return tile; }  
			}

			public override Color Color { get { return Color.Lime; } }
		}

		public sealed class BoundaryCollider : Collider
		{
			public BoundaryCollider(Rectangle rect) : base(rect) { }

			public override Color Color { get { return Color.Red; } }
		}

		public abstract Color Color { get; }
	}
}
