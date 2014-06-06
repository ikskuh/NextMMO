using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace NextMMO
{
	public class SkinnedPanel : Panel
	{
		public SkinnedPanel()
		{
			this.MinimumSize = new Size(32, 32);
		}

		protected override void OnPaint(PaintEventArgs pevent)
		{
			// Draw button here...
			var g = pevent.Graphics;
			var clientSize = this.ClientSize;
			var width = clientSize.Width;
			var height = clientSize.Height;

			if (this.Skin == null)
			{
				g.Clear(Color.DeepPink);
			}
			else
			{
				g.Clear(Color.Transparent);

				g.DrawImage(
					this.Skin,
					new Rectangle(0, 0, width, height),
					new Rectangle(0, 0, 128, 128),
					GraphicsUnit.Pixel);

				NextMMO.SkinnedSkin renderer = new NextMMO.SkinnedSkin(this.Skin, new Rectangle(128, 0, 64, 64));
				renderer.WrapMode= BorderWrapMode.Tile;
				renderer.FillCenter = false;
				renderer.Draw(g, new Rectangle(0, 0, width, height));
			}
			var size = g.MeasureString(this.Text, this.Font);
			g.DrawString(
				this.Text,
				this.Font,
				new SolidBrush(this.ForeColor),
				(width - size.Width) / 2.0f,
				(height - size.Height) / 2.0f);
		}

		public Bitmap Skin { get; set; }

		public BorderWrapMode WrapMode { get; set; }
	}
}
