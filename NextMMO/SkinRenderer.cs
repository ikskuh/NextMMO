using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NextMMO
{
	public class SkinRenderer
	{
		public SkinRenderer(Bitmap skin)
			: this(skin, new Rectangle(0, 0, skin.Width, skin.Height))
		{

		}

		public SkinRenderer(Bitmap skin, Rectangle source)
		{
			int x = source.Left;
			int y = source.Top;
			int w = source.Width;
			int h = source.Height;
			int w4 = source.Width / 4;
			int h4 = source.Height / 4;

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

			this.Skin = skin;

			this.FillCenter = true;
		}

		public void Draw(Graphics g, Rectangle target)
		{
			var skin = this.Skin;
			var mode = this.WrapMode;
			var x = target.X;
			var y = target.Y;
			var width = target.Width;
			var height = target.Height;

			switch (this.WrapMode)
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

		public BorderWrapMode WrapMode { get; set; }

		public Bitmap Skin { get; set; }


		public bool FillCenter { get; set; }

		public Rectangle Center { get; set; }
	}

	public enum BorderWrapMode
	{
		Default = 0,
		Tile = 0,
		Stretch
	}
}
