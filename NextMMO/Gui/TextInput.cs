using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NextMMO.Gui
{
	public class TextInput : Element
	{
		public TextInput() : base() { }

		public TextInput(string text) : base(text) { }


		protected override void OnKeyPress(char c)
		{
			switch (c)
			{
				case (char)127:
					if (this.Text.Length > 0)
						this.Text = this.Text.Substring(0, this.Text.Length - 1);
					break;
				default:
					this.Text += c;
					break;
			}
		}

		public override SizeF GetAutoSize(IGraphics graphics)
		{
			var size = base.GetAutoSize(graphics);
			size.Height = graphics.GetFont(FontSize.Medium).GetHeight(graphics.DPI) + 2;
			return size;
		}

		public override void Draw(IGraphics graphics, Rectangle rect)
		{
			var size = graphics.MeasureString(this.Text, graphics.GetFont(FontSize.Medium), true);
			graphics.DrawString(
				this.Text,
				graphics.GetFont(FontSize.Medium),
				Color.Black,
				rect.X + 0.5f * (rect.Height - size.Height),
				rect.Y + 0.5f * (rect.Height - size.Height));

			if (this.IsSelected)
			{
				graphics.DrawLine(
					Color.Black,
					rect.Left + size.Width - 3,
					rect.Top + 3,
					rect.Left + size.Width - 3,
					rect.Bottom - 4);
			}
		}
	}
}
