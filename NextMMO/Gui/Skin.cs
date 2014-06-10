using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NextMMO.Gui
{
	public abstract class Skin
	{
		protected Skin(Bitmap skin)
		{
			this.Bitmap = skin;
		}

		public abstract void Draw(IGraphics g, Rectangle target);

		public Bitmap Bitmap { get; set; }
	}
}
