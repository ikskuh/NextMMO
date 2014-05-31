using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NextMMO
{
	public class AnimatedBitmap
	{
		public AnimatedBitmap(Bitmap source, int frames, int animations)
		{
			this.Source = source;
			this.FrameCount = frames;
			this.AnimationCount = animations;
		}

		public void Draw(Graphics g, int x, int y, int animation, int frame)
		{
			this.Draw(g, new Rectangle(x, y, this.Width, this.Height), animation, frame);
		}

		public void Draw(Graphics g, Point p, int animation, int frame)
		{
			this.Draw(g, new Rectangle(p.X, p.Y, this.Width, this.Height), animation, frame);
		}

		public void Draw(Graphics g, Rectangle rect, int animation, int frame)
		{
			g.DrawImage(
				this.Source,
				rect,
				new Rectangle(
					this.Width * (frame % this.FrameCount),
					this.Height * (animation % this.AnimationCount),
					this.Width, this.Height),
				GraphicsUnit.Pixel);
		}

		public Bitmap Source { get; private set; }

		public int FrameCount { get; private set; }
		public int AnimationCount { get; private set; }

		public int Width { get { return this.Source.Width / this.FrameCount; } }

		public int Height { get { return this.Source.Height / this.AnimationCount; } }
	}
}
