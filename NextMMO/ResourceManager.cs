using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NextMMO
{
	/// <summary>
	/// Provides loading and caching of resources.
	/// </summary>
	public sealed class ResourceManager<T>
	{
		private readonly Dictionary<string, T> resources = new Dictionary<string,T>();
		private readonly ResourceLoaderDelegate<T> loader;
		private readonly string root;
		private readonly string[] extensions;

		/// <summary>
		/// Creates a new resource manager.
		/// </summary>
		/// <param name="root">Root directory.</param>
		/// <param name="loader">Loader delegate to load the resource.</param>
		/// <param name="extensions">Valid file extensions.</param>
		public ResourceManager(string root, ResourceLoaderDelegate<T> loader, params string[] extensions)
		{
			this.root = root;
			this.loader = loader;
			this.extensions = extensions;
		}

		/// <summary>
		/// Loads the resource with the given name.
		/// </summary>
		/// <param name="name">Name of the resource.</param>
		/// <returns>Loaded resource</returns>
		public T this[string name]
		{
			get
			{
				lock (this.resources)
				{
					if (this.resources.ContainsKey(name))
						return this.resources[name];

					T resource = default(T);
					bool loaded = false;
					foreach (var ext in this.extensions)
					{
						var fileName = this.root + "/" + name + ext;
						if (!File.Exists(fileName))
							continue;
						using (var stream = File.Open(fileName, FileMode.Open))
						{
							resource = loader(stream);
						}
						loaded = true;
						break;
					}

					if (!loaded)
						throw new FileNotFoundException(name);

					this.resources.Add(name, resource);

					return resource;
				}
			}
		}
	}
}
