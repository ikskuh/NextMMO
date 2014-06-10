using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace NextMMO
{
	/// <summary>
	/// Provides an abstraction layer for GDI+ graphics.
	/// </summary>
	public sealed class GDIGraphics : IGraphics
	{
		private readonly Font[] fonts;
		private readonly Graphics graphics;

		public GDIGraphics(Graphics graphics)
		{
			this.graphics = graphics;
			this.fonts = new[]
				{
					new Font(FontFamily.GenericSansSerif, 10.0f),
					new Font(FontFamily.GenericSansSerif, 16.0f),
					new Font(FontFamily.GenericSansSerif, 32.0f),
				};
		}

		public void DrawImage(Bitmap bitmap, Rectangle rectangle)
		{
			this.graphics.DrawImage(bitmap, rectangle);
		}

		public void FillRectangle(Color color, Rectangle rect)
		{
			this.graphics.FillRectangle(new SolidBrush(color), rect);
		}

		public void DrawImage(Bitmap bitmap, Rectangle destination, Rectangle source)
		{
			this.graphics.DrawImage(bitmap, destination, source, GraphicsUnit.Pixel);
		}

		public SizeF MeasureString(string text, Font font)
		{
			return this.graphics.MeasureString(text, font);
		}

		public void DrawString(string text, Font font, Color color, float x, float y)
		{
			this.graphics.DrawString(text, font, new SolidBrush(color), x, y);
		}

		public void ResetClip()
		{
			this.graphics.ResetClip();
		}

		public void SetClip(Rectangle rect)
		{
			this.graphics.SetClip(rect);
		}

		public Font GetFont(FontSize size)
		{
			return this.fonts[(int)size];
		}
	}
}
