using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NextMMO
{
	public abstract class GameObject
	{
		readonly IGameServices services;

		/// <summary>
		/// Initializes the game object with the fitting game services.
		/// </summary>
		/// <param name="services"></param>
		protected GameObject(IGameServices services)
		{
			this.services = services;
		}

		/// <summary>
		/// Gets the game services.
		/// </summary>
		public IGameServices Services
		{
			get { return services; }
		} 
	}
}
