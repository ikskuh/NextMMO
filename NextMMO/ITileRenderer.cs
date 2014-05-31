using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NextMMO
{
	public interface ITileRenderer
	{
		void RenderTile(int x, int y, int tileID);
	}
}
