using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

namespace NextMMO
{
	public interface IGameServices
	{
		Graphics Graphics { get; }

		ResourceManager<Bitmap> Bitmaps { get; }

		ResourceManager<AnimatedBitmap> Characters { get; }

		ResourceManager<Sound> Sounds { get; }

		Font GetFont(FontSize size);

		INetworkService Network { get; }

		Random Random { get; }

		GameTime Time { get; }
	}

	public class GameTime
	{
		private double total, delta;

		public GameTime()
		{

		}

		public void Advance(double delta)
		{
			this.total += delta;
			this.delta = delta;
		}

		public double Delta
		{
			get { return delta; }
		}

		public double Total
		{
			get { return total; }
		}
	}

	public enum FontSize { Small, Medium, Large };
}
