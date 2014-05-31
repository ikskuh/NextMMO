using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NextMMO
{
	public static class AnimationSource
	{
		static Stopwatch watch;

		static AnimationSource()
		{
			watch = new Stopwatch();
			watch.Start();
		}

		public static double Time
		{
			get
			{
				return watch.Elapsed.TotalSeconds;
			}
		}
	}
}
