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
			if(this.Script != null)
			{
				this.Script.Call(this, other);
			}	
		}

		public override IEnumerable<Collider> GetColliders()
		{
			return from c in colliders select new Collider.EntityCollider(this, c);
		}

		public NLua.LuaFunction Script { get; set; }

		public List<Rectangle> Colliders { get { return colliders; } }
	}
}
