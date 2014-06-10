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

		public SizeF MeasureString(string text, Font font, bool measureWhitespace)
		{
			StringFormat format = new StringFormat()
			{
				FormatFlags = StringFormatFlags.MeasureTrailingSpaces
			};
			if(measureWhitespace)
				return this.graphics.MeasureString(text, font, 10000, format);
			else
				return this.graphics.MeasureString(text, font);
		}

		public void DrawString(string text, Font font, Color color, float x, float y)
		{
			this.graphics.DrawString(text, font, new SolidBrush(color), x, y);
		}

		public void DrawLine(Color color, float x1, float y1, float x2, float y2)
		{
			this.graphics.DrawLine(new Pen(color), x1, y1, x2, y2);
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

		public float DPI { get { return this.graphics.DpiY; } }
	}
}
