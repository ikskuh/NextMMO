using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

namespace NextMMO
{
	public class ResourceCollection : GameObject, IGameResources
	{
		ResourceManager<Bitmap> bitmapSource;
		ResourceManager<TileSet> tileSetSource;
		ResourceManager<AnimatedBitmap> characterSprites;
		ResourceManager<AnimatedBitmap> animations;
		ResourceManager<Sound> soundSource;
		ResourceManager<TileMap> mapSource;
		ResourceManager<EntityTemplate> templates;

		public ResourceCollection(IGameServices services, string allRoot)
			: base(services)
		{
			this.bitmapSource = new ResourceManager<Bitmap>(
				allRoot + "./Images/",
				(stream) => (Bitmap)Image.FromStream(stream, false, true),
				null,
				".png", ".bmp", ".jpg");
			this.tileSetSource = new ResourceManager<TileSet>(
				allRoot + "./TileSets/",
				(stream) => TileSet.Load(this.Services, stream),
				(stream, resource) => resource.Save(stream),
				".tset");
			this.characterSprites = new ResourceManager<AnimatedBitmap>(
				allRoot + "./Images/Characters/",
				(stream) => new AnimatedBitmap(new Bitmap(stream), 4, 4),
				null,
				".png", ".bmp", ".jpg");
			this.animations = new ResourceManager<AnimatedBitmap>(
				allRoot + "./Images/",
				(stream) => new AnimatedBitmap(new Bitmap(stream), 4, 4),
				null,
				".png", ".bmp", ".jpg");
			this.soundSource = new ResourceManager<Sound>(
				allRoot + "./Sounds/",
				(stream) => new Sound(stream),
				null,
				".ogg");
			this.mapSource = new ResourceManager<TileMap>(
				allRoot + "./Maps/",
				(stream) => TileMap.Load(stream),
				(stream, map) => map.Save(stream),
				".map");
			this.templates = new ResourceManager<EntityTemplate>(
				allRoot + "./Objects/",
				(stream) => new EntityTemplate(this.Services, stream),
				(stream, template) => template.Save(stream),
				".otl");
		}

		public ResourceManager<Bitmap> Bitmaps { get { return this.bitmapSource; } }

		public ResourceManager<AnimatedBitmap> Characters { get { return this.characterSprites; } }

		public ResourceManager<AnimatedBitmap> Animations { get { return animations; } }

		public ResourceManager<Sound> Sounds { get { return this.soundSource; } }

		public ResourceManager<TileSet> TileSets { get { return tileSetSource; } }

		public ResourceManager<TileMap> Maps { get { return this.mapSource; } }

		public ResourceManager<EntityTemplate> Templates { get { return templates; } }
	}
}
