using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

namespace NextMMO
{
	public class GameScriptInterface : GameObject
	{
		public GameScriptInterface(IGameServices services)
			: base(services)
		{

		}

		public void PlaySound(string name)
		{
			this.Services.Resources.Sounds[name].Play();
		}
	}

	public class ResourceScriptInterface : GameObject
	{
		public ResourceScriptInterface(IGameServices services)
			: base(services)
		{

		}

		public Rectangle Rectangle(int x, int y, int width, int height)
		{
			return new Rectangle(x, y, width, height);
		}

		public AnimatedBitmap Animation(string key)
		{
			return this.Services.Resources.Animations[key];
		}

		public AnimatedBitmap Animation(string key, string source, int width, int height)
		{
			try
			{
				var bmp = new AnimatedBitmap(this.Services.Resources.Bitmaps[source], width, height);
				this.Services.Resources.Animations.Register(key, bmp);
				return bmp;
			}
			catch (InvalidOperationException)
			{
				return this.Animation(key);
			}
		}

		public EntityTemplate Template(string key)
		{
			return this.Services.Resources.Templates[key];
		}

		public EntityTemplate Template(string key, Sprite sprite, NLua.LuaTable colliders, NLua.LuaFunction @event)
		{
			try
			{
				var template = new EntityTemplate(this.Services);
				template.Sprite = sprite;
				foreach (Rectangle rect in colliders.Values)
				{
					template.Colliders.Add(rect);
				}
				template.Script = @event;
				this.Services.Resources.Templates.Register(key, template);
				return template;
			}
			catch (InvalidOperationException)
			{
				return this.Template(key);
			}
		}

		public Sprite CreateSprite(AnimatedBitmap animation, int centerX, int centerY, int id, double speed)
		{
			return new AnimatedSprite(animation, new Point(centerX, centerY))
			{
				Animation = id,
				Speed = speed
			};
		}
	}
}
