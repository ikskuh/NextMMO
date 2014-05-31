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
			this.Size = 0.5;
		}

		public override void Update()
		{
			int dx = 0;
			int dy = 0;

			if (this.Direction.HasFlag(MoveDirection.Left) && !this.Direction.HasFlag(MoveDirection.Right))
			{
				this.Sprite.Animation = 1;
				dx -= 1;
			}
			if (this.Direction.HasFlag(MoveDirection.Right) && !this.Direction.HasFlag(MoveDirection.Left))
			{
				this.Sprite.Animation = 2;
				dx += 1;
			}
			if (this.Direction.HasFlag(MoveDirection.Up) && !this.Direction.HasFlag(MoveDirection.Down))
			{
				this.Sprite.Animation = 3;
				dy -= 1;
			}
			if (this.Direction.HasFlag(MoveDirection.Down) && !this.Direction.HasFlag(MoveDirection.Up))
			{
				this.Sprite.Animation = 0;
				dy += 1;
			}

			if(this.Direction == MoveDirection.None)
			{
				this.Sprite.AnimationSpeed = 0;
			}
			else
			{
				this.Sprite.AnimationSpeed = 8;
			}

			this.Translate(0.05f * dx, 0.05f * dy);
		}

		public void Interact()
		{

		}

		public MoveDirection Direction { get; set; }

		public new AnimatedSprite Sprite
		{
			get { return base.Sprite as AnimatedSprite; }
			set { base.Sprite = value; }
		}
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
