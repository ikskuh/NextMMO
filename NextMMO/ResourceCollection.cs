using System.Drawing;
using System.Globalization;
using System.IO;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;

namespace NextMMO
{
	public class ResourceCollection : GameObject, IGameResources
	{
		private ResourceManager<AnimatedBitmap> animations;
		private ResourceManager<Bitmap> bitmapSource;
		private ResourceManager<AnimatedBitmap> characterSprites;
		private ResourceManager<TileMap> mapSource;
		private ResourceManager<Sound> soundSource;
		private ResourceManager<EntityTemplate> templates;
		private ResourceManager<TileSet> tileSetSource;
		private ResourceManager<Sprite> spriteSource;

		public ResourceCollection(IGameServices services, string allRoot)
			: base(services)
		{
			this.bitmapSource = new FileSystemResourceManager<Bitmap>(
				allRoot + "./Images/",
				(stream) => (Bitmap)Image.FromStream(stream, false, true),
				null,
				".png", ".bmp", ".jpg");
			this.tileSetSource = new FileSystemResourceManager<TileSet>(
				allRoot + "./TileSets/",
				(stream) => TileSet.Load(this.Services, stream),
				(stream, resource) => resource.Save(stream),
				".tset");
			this.characterSprites = new FileSystemResourceManager<AnimatedBitmap>(
				allRoot + "./Images/Characters/",
				(stream) => new AnimatedBitmap(new Bitmap(stream), 4, 4),
				null,
				".png", ".bmp", ".jpg");
			this.animations = new RegisterOnlyResourceManager<AnimatedBitmap>();
			this.soundSource = new FileSystemResourceManager<Sound>(
				allRoot + "./Sounds/",
				(stream) => new Sound(stream),
				null,
				".ogg");
			this.mapSource = new FileSystemResourceManager<TileMap>(
				allRoot + "./Maps/",
				(stream) => TileMap.Load(stream),
				(stream, map) => map.Save(stream),
				".map");
			this.templates = new RegisterOnlyResourceManager<EntityTemplate>();
			this.Sprites = new RegisterOnlyResourceManager<Sprite>();
		}

		public void LoadFile(string fileName)
		{
			this.LoadXml(File.ReadAllText(fileName));
		}

		public void LoadXml(string xml)
		{
			var resources = xml.Deserialize<XmlResourceCollection>();
			foreach (var anim in resources.Animations)
			{
				AnimatedBitmap bmp = new AnimatedBitmap(
					this.Bitmaps[anim.Source],
					anim.Frames,
					anim.Animations);
				this.Animations.Register(anim.Key, bmp);
			}
			foreach (var sprite in resources.Sprites)
			{
				XmlPoint pt = new XmlPoint();
				pt.Load(sprite.Center);

				var sp = new AnimatedSprite(this.Animations[sprite.Source]);
				sp.Animation = sprite.Animation;
				sp.Offset = new Point((int)pt.X, (int)pt.Y);
				sp.Speed = sprite.Speed;

				this.Sprites.Register(sprite.Key, sp);
            }
			foreach (var templ in resources.Templates)
			{
				EntityTemplate template = new EntityTemplate(this.Services);

				template.Colliders.Clear();
				foreach (var collider in templ.Colliders)
				{
					template.Colliders.Add(new Rectangle(
						collider.Left, collider.Top,
						collider.Width, collider.Height));
				}

				template.ActionName = templ.ActionName;
				template.ActionParam = templ.ActionParam;

				template.Sprite = this.Sprites[templ.Sprite];
				this.Templates.Register(templ.Key, template);
			}
		}

		public ResourceManager<AnimatedBitmap> Animations { get { return animations; } }

		public ResourceManager<Bitmap> Bitmaps { get { return this.bitmapSource; } }

		public ResourceManager<AnimatedBitmap> Characters { get { return this.characterSprites; } }
		public ResourceManager<TileMap> Maps { get { return this.mapSource; } }

		public ResourceManager<Sound> Sounds { get { return this.soundSource; } }

		public ResourceManager<EntityTemplate> Templates { get { return templates; } }

		public ResourceManager<TileSet> TileSets { get { return tileSetSource; } }

		public ResourceManager<Sprite> Sprites
		{
			get
			{
				return spriteSource;
			}

			set
			{
				spriteSource = value;
			}
		}
	}

	public abstract class XmlResource
	{

		[XmlAttribute("key")]
		public string Key { get; set; }
	}

	public sealed class XmlAnimation : XmlResource
	{
		[XmlAttribute("animations")]
		public int Animations { get; set; }

		[XmlAttribute("frames")]
		public int Frames { get; set; }

		[XmlAttribute("source")]
		public string Source { get; set; }
	}

	public sealed class XmlEntityTemplate : XmlResource
	{
		[XmlAttribute("action")]
		public string ActionName { get; set; }

		[XmlAttribute("param")]
		public string ActionParam { get; set; }

		[XmlElement("collider")]
		public XmlRectangle[] Colliders { get; set; }

		[XmlAttribute("sprite")]
		public string Sprite { get; set; }
	}

	public sealed class XmlPoint : IXmlSerializable
	{
		public void Load(string str)
		{
			string[] xy = str.Split(',');

			if (double.TryParse(xy[0], NumberStyles.Float, CultureInfo.InvariantCulture, out double x))
				this.X = x;
			if (double.TryParse(xy[1], NumberStyles.Float, CultureInfo.InvariantCulture, out double y))
				this.Y = y;
		}

		XmlSchema IXmlSerializable.GetSchema()
		{
			return null;
		}

		void IXmlSerializable.ReadXml(XmlReader reader)
		{
			string str = reader.ReadContentAsString();
			this.Load(str);
		}

		void IXmlSerializable.WriteXml(XmlWriter writer)
		{
			writer.WriteString(string.Format(
				CultureInfo.InvariantCulture,
				"{0},{1}",
				this.X,
				this.Y));
		}

		public double X { get; set; }

		public double Y { get; set; }
	}

	public sealed class XmlRectangle
	{
		[XmlAttribute("height")]
		public int Height { get; set; }

		[XmlAttribute("left")]
		public int Left { get; set; }

		[XmlAttribute("top")]
		public int Top { get; set; }

		[XmlAttribute("width")]
		public int Width { get; set; }
	}

	[XmlRoot("resources")]
	public sealed class XmlResourceCollection
	{
		[XmlElement("animation")]
		public XmlAnimation[] Animations { get; set; }

		[XmlElement("sprite")]
		public XmlSprite[] Sprites { get; set; }

		[XmlElement("template")]
		public XmlEntityTemplate[] Templates { get; set; }
	}

	public sealed class XmlSprite : XmlResource
	{
		[XmlAttribute("animation")]
		public int Animation { get; set; }

		[XmlAttribute("center")]
		public string Center { get; set; }

		[XmlAttribute("source")]
		public string Source { get; set; }
		[XmlAttribute("speed")]
		public double Speed { get; set; }
	}
}