using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

namespace NextMMO.Gui
{
	public abstract class Element
	{
		bool isSelected = false;
		float width = float.NaN;
		float height = float.NaN;

		protected Element()
			: this("")
		{

		}

		protected Element(string text)
		{
			this.Text = text;
		}

		public virtual bool Interact(GuiInteraction interaction)
		{
			return false;
		}

		public virtual void Draw(IGraphics graphics, Rectangle rect)
		{
			var size = graphics.MeasureString(this.Text, graphics.GetFont(FontSize.Medium), false);
			graphics.DrawString(
				this.Text,
				graphics.GetFont(FontSize.Medium),
				Color.Black,
				rect.X + 0.5f * (rect.Width - size.Width),
				rect.Y + 0.5f * (rect.Height - size.Height));
		}

		public virtual SizeF GetAutoSize(IGraphics graphics)
		{
			var size = graphics.MeasureString(this.Text, graphics.GetFont(FontSize.Medium), false);

			// Add spacing
			size.Width += 4;
			size.Height += 2;

			return size;
		}

		/// <summary>
		/// Signal a key press to the element.
		/// </summary>
		/// <param name="c"></param>
		public void SignalKeyPress(char c)
		{
			this.OnKeyPress(c);
		}

		protected virtual void OnKeyPress(char c) { }

		/// <summary>
		/// Informs the element that it just got selected.
		/// </summary>
		public void BeginSelect() { this.isSelected = true; this.OnBeginSelect(); }

		/// <summary>
		/// Informs the element that it is no longer selected.
		/// </summary>
		public void EndSelect() { this.isSelected = false; this.OnEndSelect(); }

		protected virtual void OnBeginSelect() { }

		protected virtual void OnEndSelect() { }

		/// <summary>
		/// Returns the width of this element.
		/// </summary>
		/// <remarks>NaN is automatic width.</remarks>
		public float Width
		{
			get { return width; }
			set { width = value; }
		}

		/// <summary>
		/// Returns the height of this element.
		/// </summary>
		/// <remarks>NaN is automatic height.</remarks>
		public float Height
		{
			get { return height; }
			set { height = value; }
		}

		public virtual string Text { get; set; }

		public virtual bool IsSelectable { get { return true; } }

		public bool IsSelected
		{
			get { return isSelected; }
		}
	}
}
