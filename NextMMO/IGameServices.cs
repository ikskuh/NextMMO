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

		int CurrentFrame { get; }

		INetworkService Network { get; }
	}
}
