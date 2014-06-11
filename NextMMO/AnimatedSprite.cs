using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

namespace NextMMO
{
	public class AnimatedSprite : Sprite
	{
		public AnimatedSprite(AnimatedBitmap bitmap)
		{
			this.Bitmap = bitmap;
			this.Speed = 8.0f; // 8 FPS
		}

		public AnimatedSprite(AnimatedBitmap bitmap, Point offset)
			: this(bitmap)
		{
			this.Offset = offset;
		}

		protected override void OnDraw(IGraphics g, int x, int y)
		{
			if(this.Bitmap == null) return;
			this.Bitmap.Draw(
				g,
				x,
				y,
				this.Animation,
				(int)(AnimationSource.Time * this.Speed));
		}

		public override Sprite Clone()
		{
			return new AnimatedSprite(this.Bitmap)
			{
				Animation = this.Animation,
				Offset = this.Offset,
				Speed = this.Speed,
			};
		}

		public AnimatedBitmap Bitmap { get; set; }

		public int Animation { get; set; }

		public double Speed { get; set; }
	}
}
