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

		Font GetFont(FontSize size);

		int CurrentFrame { get; }

		INetworkService Network { get; }
	}

	public enum FontSize { Small, Medium, Large };
}
