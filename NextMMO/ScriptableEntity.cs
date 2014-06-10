using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

namespace NextMMO
{
	public sealed class ScriptableEntity : Entity
	{
		List<Rectangle> colliders = new List<Rectangle>();

		public ScriptableEntity(World world)
			: base(world)
		{

		}

		public override void Trigger(Entity other)
		{
			if(!string.IsNullOrWhiteSpace(this.Script))
				this.Services.ScriptHost.DoString(this.Script);
		}

		public override IEnumerable<Collider> GetColliders()
		{
			return from c in colliders select new Collider.EntityCollider(this, c);
		}

		public string Script { get; set; }

		public List<Rectangle> Colliders { get { return colliders; } }
	}
}
