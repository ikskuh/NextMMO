using NextMMO.Gui;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace NextMMO
{
	public class SkinnedButton : System.Windows.Forms.Button
	{
		public SkinnedButton()
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
				SkinnedSkin renderer = new SkinnedSkin(this.Skin, new Rectangle(128, 64, 32, 32));
				renderer.WrapMode = BorderWrapMode.Stretch;
				//renderer.Draw(g, new Rectangle(0, 0, width, height));
				throw new NotImplementedException();
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
