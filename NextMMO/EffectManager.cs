using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

namespace NextMMO
{
	public sealed class EffectManager : GameObject
	{
		readonly HashSet<Effect> effects = new HashSet<Effect>();

		public EffectManager(IGameServices services)
			: base(services)
		{

		}

		public void Update()
		{
			foreach (var effect in effects.ToArray())
				effect.Update((float)this.Services.Time.Delta);
		}

		public void Draw()
		{
			foreach (var effect in effects)
				effect.Draw(this.Services.Graphics);
		}

		public void Spawn(Effect effect)
		{
			this.effects.Add(effect);
			effect.Despawn += (s, e) => this.effects.Remove(effect);
		}
	}

	public class Effect
	{
		public event EventHandler Despawn;

		int frame = 0;
		float time = 0.0f;

		public void Update(float delta)
		{
			this.time += delta;

			this.frame = (int)(this.Speed * this.time);
			if (this.frame >= this.Sprite.FrameCount)
			{
				if (this.Despawn != null) this.Despawn(this, EventArgs.Empty);
			}
		}

		public void Draw(IGraphics g)
		{
			this.Sprite.Draw(
				g,
				(int)(this.Position.X) - this.Sprite.Width / 2,
				(int)(this.Position.Y) - this.Sprite.Height / 2,
				this.Animation,
				this.frame);
		}

		public int Animation { get; set; }

		public PointF Position { get; set; }

		public AnimatedBitmap Sprite { get; set; }

		public float Speed { get; set; }
	}
}
