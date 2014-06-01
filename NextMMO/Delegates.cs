using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NextMMO
{
	/// <summary>
	/// An abstract resource loader method. Used to load any kind of class from a stream.
	/// </summary>
	/// <typeparam name="T">Type of the class that should be loaded.</typeparam>
	/// <param name="stream">Stream that contains the resource.</param>
	/// <returns>Loaded resource.</returns>
	public delegate T ResourceLoaderDelegate<T>(Stream stream);

	public delegate void ResourceSaverDelegate<T>(Stream stream, T resource);
}
