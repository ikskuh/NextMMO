using NLua;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NextMMO
{
	public class ScriptHost : IScriptHost
	{
		public const string ScriptInterfaceVariable = "Game";
		private readonly Lua lua = new Lua();

		public void DoString(string script)
		{
			this.lua.DoString(script, "ExternalScript");
		}

		public IScriptInterface Interface
		{
			get
			{
				return this.lua[ScriptInterfaceVariable] as IScriptInterface;
			}
			set
			{
				this.lua[ScriptInterfaceVariable] = value;
			}
		}
	}
}
