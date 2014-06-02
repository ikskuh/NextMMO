using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NextMMO
{
	public sealed class ProxyPlayer : IEntity
	{
		public void Update()
		{
			
		}

		public void Draw(System.Drawing.Graphics graphics)
		{
			this.Sprite.Draw(
				graphics,
				(int)(32 * this.X),
				(int)(32 * this.Y));
		}

		public double X { get; set; }

		public double Y { get; set; }

		public int Direction
		{
			get { return this.Sprite.Animation; }
			set { this.Sprite.Animation = value; }
		}

		public AnimatedSprite Sprite { get; set; }

		Sprite IEntity.Sprite { get { return this.Sprite; } }
	}
}
