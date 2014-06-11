using NLua;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;

namespace NextMMO
{
	public sealed class RegisterOnlyResourceManager<T> : ResourceManager<T>
	{
		public RegisterOnlyResourceManager()
		{

		}

		protected override T Load(string name)
		{
			throw new InvalidOperationException("Cannot load animations via default construct. Register must be used instead.");
		}
	}
}
