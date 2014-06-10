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
		/// Gets the resource loader for this game.
		/// </summary>
		IGameResources Resources { get; }

		/// <summary>
		/// Gets the network module.
		/// </summary>
		INetworkService Network { get; }

		IScriptHost ScriptHost { get; }

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

		/// <summary>
		/// Gets a font of the selected font size.
		/// </summary>
		/// <param name="size">Size of the font.</param>
		/// <returns></returns>
		Font GetFont(FontSize size);

		void DrawImage(Bitmap bitmap, Rectangle rectangle);

		void FillRectangle(Color color, Rectangle rect);

		void DrawImage(Bitmap bitmap, Rectangle rect, Rectangle rectangle);

		SizeF MeasureString(string text, Font font, bool measureWhitespace);

		void DrawString(string text, Font font, Color color, float x, float y);

		void ResetClip();

		void SetClip(Rectangle rect);

		void DrawLine(Color color, float x1, float y1, float x2, float y2);

		float DPI { get; }
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
