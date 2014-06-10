using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NextMMO
{
	public interface IScriptHost
	{
		void DoString(string script);
	}

	public interface IScriptInterface
	{
		void PlaySound(string name);
	}
}
