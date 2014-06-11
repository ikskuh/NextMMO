using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NextMMO
{
	/// <summary>
	/// Provides a simple script hosting.
	/// </summary>
	public interface IScriptHost
	{
		/// <summary>
		/// Executes a piece of script.
		/// </summary>
		/// <param name="script">The script to be executed.</param>
		void DoString(string script);

		/// <summary>
		/// Registers a script interface.
		/// </summary>
		/// <param name="name">Name of the interface.</param>
		/// <param name="iface">The object that contains the interface.</param>
		/// <remarks>If iface is null, the interface will be deleted.</remarks>
		void RegisterInterface(string name, object iface);
	}
}
