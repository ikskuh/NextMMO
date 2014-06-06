using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NextMMO
{
	public abstract class Skin
	{
		protected Skin(Bitmap skin)
		{
			this.Bitmap = skin;
		}

		public abstract void Draw(Graphics g, Rectangle target);

		public Bitmap Bitmap { get; set; }
	}

	public class StretchedSkin : Skin
	{
		public StretchedSkin(Bitmap skin, Rectangle source)
			: base(skin)
		{
			this.Source = source;
		}

		public override void Draw(Graphics g, Rectangle target)
		{
			g.DrawImage(this.Bitmap, target, this.Source, GraphicsUnit.Pixel);
		}

		public Rectangle Source { get; set; }
	}

	public class SkinnedSkin : Skin
	{
		public SkinnedSkin(Bitmap skin, Rectangle area)
			: base(skin)
		{
			int x = area.Left;
			int y = area.Top;
			int w = area.Width;
			int h = area.Height;
			int w4 = area.Width / 4;
			int h4 = area.Height / 4;

			this.TopLeft = new Rectangle(x, y, w4, h4);
			this.TopRight = new Rectangle(x + w - w4, y, w4, h4);

			this.BottomLeft = new Rectangle(x, y + h - h4, w4, h4);
			this.BottomRight = new Rectangle(x + w - w4, y + h - h4, w4, h4);

			this.Top = new Rectangle(x + w4, y, w - 2 * w4, h4);
			this.Bottom = new Rectangle(x + w4, y + h - h4, w - 2 * w4, h4);

			this.Left = new Rectangle(x, y + h4, w4, h - 2 * h4);
			this.Right = new Rectangle(x + w - w4, y + h4, w4, h - 2 * h4);

			this.Center = new Rectangle(
				x + w4,
				y + h4,
				w - 2 * w4,
				h - 2 * h4);

			this.FillCenter = true;
		}

		public override void Draw(Graphics g, Rectangle target)
		{
			var skin = this.Bitmap;
			var mode = this.WrapMode;
			var x = target.X;
			var y = target.Y;
			var width = target.Width;
			var height = target.Height;

			switch (mode)
			{
				case BorderWrapMode.Tile:
					for (int px = 0; px < width - this.TopLeft.Width - this.TopRight.Width; px += this.Top.Width)
					{
						g.DrawImage(
							skin,
							new Rectangle(
								x + this.TopLeft.Width + px,
								y,
								this.Top.Width,
								this.Top.Height),
							this.Top,
							GraphicsUnit.Pixel);
						// Bottom bar
						g.DrawImage(
							skin,
							new Rectangle(
								x + this.BottomLeft.Width + px,
								y + height - this.Bottom.Height,
								this.Bottom.Width,
								this.Bottom.Height),
							this.Bottom,
							GraphicsUnit.Pixel);
					}
					for (int py = 0; py < height - this.TopLeft.Height - this.BottomLeft.Height; py += 32)
					{
						// Left bar
						g.DrawImage(
							skin,
							new Rectangle(
								x,
								y + this.TopLeft.Height + py,
								this.Left.Width,
								this.Left.Height),
							this.Left,
							GraphicsUnit.Pixel);

						// Right bar
						g.DrawImage(
							skin,
							new Rectangle(
								x + width - this.Right.Width,
								y + this.TopRight.Height + py,
								this.Right.Width,
								this.Right.Height),
							this.Right,
							GraphicsUnit.Pixel);
					}
					break;
				case BorderWrapMode.Stretch:
					// Top bar
					g.DrawImage(
						skin,
						new Rectangle(
							x + this.TopLeft.Width,
							y,
							width - this.TopLeft.Width - this.TopRight.Width,
							this.Top.Height),
						this.Top,
						GraphicsUnit.Pixel);
					// Bottom bar
					g.DrawImage(
						skin,
						new Rectangle(
							x + this.BottomLeft.Width,
							y + height - this.Bottom.Height,
							width - this.BottomLeft.Width - this.BottomRight.Width,
							this.Bottom.Height),
						this.Bottom,
						GraphicsUnit.Pixel);

					// Left bar
					g.DrawImage(
						skin,
						new Rectangle(
							x,
							y + this.TopLeft.Height,
							this.Left.Width,
							height - this.TopLeft.Height - this.BottomLeft.Height),
						this.Left,
						GraphicsUnit.Pixel);

					// Right bar
					g.DrawImage(
						skin,
						new Rectangle(
							x + width - this.Right.Width,
							y + this.TopRight.Height,
							this.Right.Width,
							height - this.TopRight.Height - this.BottomRight.Height),
						this.Right,
						GraphicsUnit.Pixel);
					break;
			}

			// Top left corner
			g.DrawImage(
				skin,
				new Rectangle(x, y, this.TopLeft.Width, this.TopLeft.Height),
				this.TopLeft,
				GraphicsUnit.Pixel);

			// Top right corner
			g.DrawImage(
				skin,
				new Rectangle(x + width - this.TopRight.Width, y, this.TopRight.Width, this.TopRight.Height),
				this.TopRight,
				GraphicsUnit.Pixel);

			// Bottom left corner
			g.DrawImage(
				skin,
				new Rectangle(x, y + height - this.BottomLeft.Height, this.BottomLeft.Width, this.BottomLeft.Height),
				this.BottomLeft,
				GraphicsUnit.Pixel);

			// Bottom right corner
			g.DrawImage(
				skin,
				new Rectangle(x + width - this.BottomRight.Width, y + height - this.BottomRight.Height, this.BottomRight.Width, this.BottomRight.Height),
				this.BottomRight,
				GraphicsUnit.Pixel);

			if (this.FillCenter)
			{
				g.DrawImage(
					skin,
					new Rectangle(
						x + this.Left.Width,
						y + this.Top.Height,
						width - this.Left.Width - this.Right.Width,
						height - this.Top.Height - this.Bottom.Height),
					this.Center,
					GraphicsUnit.Pixel);
			}
		}

		public Rectangle TopLeft { get; set; }

		public Rectangle Top { get; set; }

		public Rectangle TopRight { get; set; }

		public Rectangle Right { get; set; }

		public Rectangle BottomRight { get; set; }

		public Rectangle Bottom { get; set; }

		public Rectangle BottomLeft { get; set; }

		public Rectangle Left { get; set; }

		public bool FillCenter { get; set; }

		public Rectangle Center { get; set; }

		public BorderWrapMode WrapMode { get; set; }
	}

	public enum BorderWrapMode
	{
		Default = 0,
		Tile = 0,
		Stretch
	}
}
