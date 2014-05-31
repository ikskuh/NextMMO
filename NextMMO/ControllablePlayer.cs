using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NextMMO
{
	public class ControllablePlayer : Entity
	{
		public ControllablePlayer(World world, int spawnX, int spawnY)
			: base(world, spawnX, spawnY)
		{

		}

		public override void Update()
		{
			int dx = 0;
			int dy = 0;

			if (this.Direction.HasFlag(MoveDirection.Left))
				dx -= 1;
			if (this.Direction.HasFlag(MoveDirection.Right))
				dx += 1;
			if (this.Direction.HasFlag(MoveDirection.Up))
				dy -= 1;
			if (this.Direction.HasFlag(MoveDirection.Down))
				dy += 1;

			this.Translate(0.05f * dx, 0.05f * dy);
		}

		public MoveDirection Direction { get; set; }

		public new AnimatedSprite Sprite { get; set; }
	}


	[Flags]
	public enum MoveDirection
	{
		None,
		Left = (1 << 0),
		Right = (1 << 1),
		Up = (1 << 2),
		Down = (1 << 3)
	}
}
