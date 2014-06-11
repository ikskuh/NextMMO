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

		public void DoFile(string fileName)
		{
			this.lua.DoFile(fileName);
		}

		public void RegisterInterface(string name, object iface)
		{
			this.lua[name] = iface;
		}
	}
}
