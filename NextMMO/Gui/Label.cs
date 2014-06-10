using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NextMMO.Gui
{
	public class Label : Element
	{
		public Label() : base() { }

		public Label(string text) : base(text) { }

		public override void Draw(IGraphics graphics, System.Drawing.Rectangle rect)
		{
			var size = graphics.MeasureString(this.Text, graphics.GetFont(FontSize.Medium));
			graphics.DrawString(
				this.Text,
				graphics.GetFont(FontSize.Medium),
				Color.Silver,
				rect.X + 0.5f * (rect.Height - size.Height),
				rect.Y + 0.5f * (rect.Height - size.Height));
		}

		public override bool IsSelectable { get { return false; } }
	}
}
