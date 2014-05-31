using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NextMMO
{
	public class Entity
	{
		private int x;
		private int y;

		public Entity(int x, int y)
		{
			this.x = x;
			this.y = y;
		}

		public void Draw(System.Drawing.Graphics graphics)
		{
			this.Sprite.Draw(
				graphics,
				32 * this.x,
				32 * this.y);
		}

		public Sprite Sprite { get; set; }

		public int X
		{
			get { return x; }
			set { x = value; }
		}

		public int Y
		{
			get { return y; }
			set { y = value; }
		}
	}
}
