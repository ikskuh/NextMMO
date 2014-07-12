using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;

namespace NextMMO
{
	public sealed class EntityTemplate : GameObject
	{
		List<Rectangle> rectangles = new List<Rectangle>();

		public EntityTemplate(IGameServices services)
			: base(services)
		{

		}

		public ScriptableEntity Instantiate(World world)
		{
			var entity = new ScriptableEntity(world);

			entity.Sprite = this.Sprite.Clone();
			
			foreach(var rect in this.rectangles)
			{
				entity.Colliders.Add(rect);
			}

			return entity;
		}

		public Sprite Sprite { get; set; }

		public ICollection<Rectangle> Colliders
		{
			get { return rectangles; }
		}

		public string ActionName { get; set; }

		public string ActionParam { get; set; }
	}

}
