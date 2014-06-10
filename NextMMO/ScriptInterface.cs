using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NextMMO
{
	public class ScriptInterface : GameObject, IScriptInterface
	{
		public ScriptInterface(IGameServices services)
			: base(services)
		{

		}

		public void PlaySound(string name)
		{
			this.Services.Resources.Sounds[name].Play();
		}
	}
}
