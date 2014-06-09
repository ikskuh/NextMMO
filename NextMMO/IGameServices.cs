using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

namespace NextMMO
{
	/// <summary>
	/// Provides global objects for the current game.
	/// </summary>
	public interface IGameServices
	{
		/// <summary>
		/// Gets the graphics object to draw 2D graphics.
		/// </summary>
		IGraphics Graphics { get; }

		/// <summary>
		/// Provides bitmap resources.
		/// </summary>
		ResourceManager<Bitmap> Bitmaps { get; }

		/// <summary>
		/// Provides animated character sprites.
		/// </summary>
		ResourceManager<AnimatedBitmap> Characters { get; }

		/// <summary>
		/// Provides sounds.
		/// </summary>
		ResourceManager<Sound> Sounds { get; }

		/// <summary>
		/// Gets a font of the selected font size.
		/// </summary>
		/// <param name="size">Size of the font.</param>
		/// <returns></returns>
		Font GetFont(FontSize size);

		/// <summary>
		/// Gets the network module.
		/// </summary>
		INetworkService Network { get; }

		/// <summary>
		/// Gets a global random module.
		/// </summary>
		Random Random { get; }

		/// <summary>
		/// Gets the current game time.
		/// </summary>
		GameTime Time { get; }
	}

	/// <summary>
	/// Provides methods to draw 2D graphics.
	/// </summary>
	public interface IGraphics
	{
		void DrawImage(Bitmap bitmap, Rectangle rectangle);

		void FillRectangle(Color color, Rectangle rect);

		void DrawImage(Bitmap bitmap, Rectangle rect, Rectangle rectangle);

		SizeF MeasureString(string text, Font font);

		void DrawString(string text, Font font, Color color, float x, float y);

		void ResetClip();

		void SetClip(Rectangle rect);
	}

	/// <summary>
	/// Provides information about the current game time.
	/// </summary>
	public class GameTime
	{
		private double total, delta;

		public GameTime()
		{

		}

		public void Advance(double delta)
		{
			this.total += delta;
			this.delta = delta;
		}

		public double Delta
		{
			get { return delta; }
		}

		public double Total
		{
			get { return total; }
		}
	}

	public enum FontSize { Small, Medium, Large };
}
