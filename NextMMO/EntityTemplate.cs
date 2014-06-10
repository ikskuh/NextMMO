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
		string script = "";
		List<Rectangle> rectangles = new List<Rectangle>();

		Func<Sprite> createSprite;

		public EntityTemplate(IGameServices services, Stream stream)
			: base(services)
		{
			var fp = CultureInfo.InvariantCulture;
			StreamReader reader = new StreamReader(stream, Encoding.UTF8);

			createSprite = () => null;

			string line;
			while(!string.IsNullOrWhiteSpace(line = reader.ReadLine()))
			{
				int idx = line.IndexOf(":");
				if(idx >= 0)
				{
					string key = line.Substring(0, idx).Trim();
					string value = line.Substring(idx + 1).Trim();
					string[] options = value.Split(';');
					for (int i = 0; i < options.Length; i++)
					{
						// Trim all options
						options[i] = options[i].Trim();
					}
					switch(key.ToLower())
					{
						case "sprite":
							switch(options[0].ToLower())
							{
								case "animation":
									string animation = options[1];
									double centerX = double.Parse(options[2], fp);
									double centerY = double.Parse(options[3], fp);
									int animationID = int.Parse(options[4], fp);
									double speed = double.Parse(options[5], fp);

									createSprite = () => new AnimatedSprite(
										this.Services.Resources.Animations[animation],
										new Point((int)centerX, (int)centerY))
										{
											Animation = animationID,
											Speed = speed
										};

									break;
								case "static":
									// TODO: Implement static sprites for entity templates.
									throw new NotImplementedException();
								default:
									throw new InvalidDataException();
							}
							break;
						case "collider":
							int top, left, width, height;
							left = int.Parse(options[0], fp);
							top = int.Parse(options[1], fp);
							width = int.Parse(options[2], fp);
							height = int.Parse(options[3], fp);

							this.rectangles.Add(new Rectangle(left, top, width, height));
							
							break;
						default:
							throw new InvalidDataException();
					}
				}
				else
				{
					throw new InvalidDataException();
				}
			}

			this.script = reader.ReadToEnd();
		}

		public void Save(Stream stream)
		{
			throw new NotImplementedException();
		}

		public ScriptableEntity Instantiate(World world)
		{
			var entity = new ScriptableEntity(world);

			entity.Sprite = this.createSprite();
			entity.Script = this.script;
			foreach(var rect in this.rectangles)
			{
				entity.Colliders.Add(rect);
			}

			return entity;
		}
	}
}
