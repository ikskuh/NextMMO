using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NextMMO
{
	public class ControllablePlayer : Entity
	{
		bool isWalking = false;
		int lastX, lastY;

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
				this.Sprite.Speed = 0;
			}
			else
			{
				this.Sprite.Speed = 8;
			}

			bool walking = dx != 0 || dy != 0;

			this.Translate(0.05f * dx, 0.05f * dy);

			int currentX = (int)(32.0 * this.X);
			int currentY = (int)(32.0 * this.Y);
			if(this.lastX != currentX || this.lastY != currentY || this.isWalking != walking)
			{
				// Send position update to server.
				var msg = this.Services.Network.CreateMessage(Networking.MessageType.UpdatePlayerPosition);
				msg.Write((float)this.X);
				msg.Write((float)this.Y);
				msg.Write((byte)this.Sprite.Animation, 7);
				msg.Write(walking);
				this.Services.Network.Send(msg, Lidgren.Network.NetDeliveryMethod.Unreliable);
			}
			this.lastX = currentX;
			this.lastY = currentY;
			this.isWalking = walking;
		}

		public void Interact(World world)
		{
			double dx = 0;
			double dy = 0;
			switch(this.Sprite.Animation)
			{
				case 0:
					dx = 0;
					dy = 32 * this.Size;
					break;
				case 1:
					dx = -32 * this.Size;
					dy = 0;
					break;
				case 2:
					dx = 32 * this.Size;
					dy = 0;
					break;
				case 3:
					dx = 0;
					dy = -32 * this.Size;
					break;
			}

			double x = 32 * this.X + 16 + dx;
			double y = 32 * this.Y + 16 + dy;

			world.TriggerAt(
				this,
				x, y,
				0.25);
			world.Debug(new Rectangle((int)x - 3, (int)y - 3, 6, 6), Color.Magenta);
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
