using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NextMMO.Gui
{
	public class StretchedSkin : Skin
	{
		public StretchedSkin(Bitmap skin, Rectangle source)
			: base(skin)
		{
			this.Source = source;
		}

		public override void Draw(IGraphics g, Rectangle target)
		{
			g.DrawImage(this.Bitmap, target, this.Source);
		}

		public Rectangle Source { get; set; }
	}
}
