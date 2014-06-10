using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

namespace NextMMO
{
	public interface IEntity
	{
		void Update();

		void Trigger(Entity other);

		void Draw(IGraphics graphics, float deltaX, float deltaY);

		double X { get; }

		double Y { get; }

		Sprite Sprite { get; }

		IEnumerable<Collider> GetColliders();
	}

	public class IEntityComparer : IComparer<IEntity>
	{
		/// <summary>
		/// Compares two entities.
		/// </summary>
		/// <param name="a"></param>
		/// <param name="b"></param>
		/// <returns></returns>
		public int Compare(IEntity a, IEntity b)
		{
			return Math.Sign(a.Y - b.Y);
		}
	}

	public abstract class Entity : IEntity
	{
		internal static readonly Collider[] emptyColliders = new Collider[0];
		private readonly World world;
		private double x;
		private double y;

		public Entity(World world)
		{
			this.world = world;
			this.Size = 0.7; // Make entity 0.5 tiles large.
		}

		public Entity(World world, int x, int y)
			: this(world)
		{
			this.x = x;
			this.y = y;
		}

		public virtual void Update() { }

		public virtual void Trigger(Entity other) { }

		public virtual void Draw(IGraphics graphics, float deltaX, float deltaY)
		{
			this.Sprite.Draw(
				graphics,
				(int)(32 * this.x - deltaX),
				(int)(32 * this.y - deltaY));
		}

		public virtual IEnumerable<Collider> GetColliders()
		{
			return Entity.emptyColliders;
		}

		/// <summary>
		/// Teleports the entity to the given position.
		/// </summary>
		/// <param name="x">X coordinate to teleport to.</param>
		/// <param name="y">Y coordinate to teleport to.</param>
		/// <returns>true if the teleportation succeeded.</returns>
		public bool Teleport(double x, double y)
		{
			var testCollision = this.world.BuildEnvironment(32 * this.x, 32 * this.y);

			if (testCollision((int)(32 * x + 16), (int)(32 * y + 16), this.Size) != null)
				return false;

			this.x = x;
			this.y = y;

			return true;
		}

		/// <summary>
		/// Moves the entity with collision detection.
		/// </summary>
		/// <param name="deltaX">X delta to move.</param>
		/// <param name="deltaY">Y delta to move.</param>
		public void Translate(double deltaX, double deltaY)
		{
			var testCollision = this.world.BuildEnvironment(32 * this.x, 32 * this.y);

			var newX = this.x + deltaX;
			var newY = this.y + deltaY;

			if (testCollision((int)(32 * newX + 16), (int)(32 * this.y + 16), this.Size) != null)
				deltaX = 0; // Elimitate x movement
			if (testCollision((int)(32 * this.X + 16), (int)(32 * newY + 16), this.Size) != null)
				deltaY = 0; // Elimitate x movement

			this.x += deltaX;
			this.y += deltaY;
		}

		public double X { get { return x; } }

		public double Y { get { return y; } }

		public double Size { get; set; }

		public Sprite Sprite { get; set; }

		public IGameServices Services { get { return this.world.Services; } }
	}
}
