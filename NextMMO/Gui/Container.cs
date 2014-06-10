using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NextMMO.Gui
{
	public abstract class Container : GameObject
	{
		readonly List<Element> elements = new List<Element>();

		public event EventHandler Cancelled;

		public Container(IGameServices services)
			: base(services)
		{

		}

		public void Draw()
		{
			Rectangle rect = this.Area;

			if (this.HorizontalSizeMode == AutoSizeMode.AutoSize)
			{
				rect.Width = this.GetAutoWidth(this.Services.Graphics);
			}
			if (this.VerticalSizeMode == AutoSizeMode.AutoSize)
			{
				rect.Height = this.GetAutoHeight(this.Services.Graphics);
			}

			if (this.Background != null)
			{
				this.Background.Draw(
					this.Services.Graphics,
					rect);
			}
			if (this.Border != null)
			{
				this.Border.Draw(
					this.Services.Graphics,
					rect);
			}

			this.Services.Graphics.SetClip(rect);
			this.OnDraw(this.Services.Graphics, rect);

			this.Services.Graphics.ResetClip();
		}

		public void Interact(GuiInteraction interaction)
		{
			switch (interaction)
			{
				case GuiInteraction.Action:
					var e = this.SelectedElement;
					if (e != null)
					{
						this.Services.Resources.Sounds["Gui/MenuClick"].Play();
						e.Trigger();
					}
					break;
				case GuiInteraction.Escape:
					if(this.Cancelled != null)
					{
						this.Services.Resources.Sounds["Gui/MenuEscape"].Play();
						this.Cancelled(this, EventArgs.Empty);
					}
					break;
				default:
					this.OnInteract(interaction);
					break;
			}
		}

		protected abstract void OnDraw(IGraphics g, Rectangle rect);

		protected abstract void OnInteract(GuiInteraction interaction);

		protected virtual int GetAutoWidth(IGraphics g) { return this.Area.Width; }

		protected virtual int GetAutoHeight(IGraphics g) { return this.Area.Height; }

		public Rectangle Area { get; set; }

		public IList<Element> Elements
		{
			get { return elements; }
		}

		public Skin Background { get; set; }

		public Skin Border { get; set; }

		public abstract Element SelectedElement { get; }

		public AutoSizeMode HorizontalSizeMode { get; set; }

		public AutoSizeMode VerticalSizeMode { get; set; }
	}

	public enum AutoSizeMode { Default, AutoSize }

	public class ListContainer : Container
	{
		int selectedID = 0;

		public ListContainer(IGameServices services)
			: base(services)
		{
			this.BorderWidth = 16;
		}

		protected override void OnDraw(IGraphics g, Rectangle rect)
		{
			this.Services.Graphics.SetClip(new Rectangle(
				rect.Left + this.BorderWidth,
				rect.Top + this.BorderWidth,
				rect.Width - 2 * this.BorderWidth,
				rect.Height - 2 * this.BorderWidth));

			int id = 0;
			foreach (var element in this.Elements)
			{
				Rectangle eRect = new Rectangle(
					rect.Left + this.BorderWidth,
					rect.Top + this.BorderWidth + 32 * id,
					rect.Width - 2 * this.BorderWidth,
					24);

				if (eRect.Top > rect.Bottom)
					break;

				if (id == this.selectedID)
				{
					this.ElementSkin.Draw(
						g,
						eRect);
				}

				var size = g.MeasureString(element.Text, g.GetFont(FontSize.Medium));

				g.DrawString(
					element.Text,
					g.GetFont(FontSize.Medium),
					Color.Black,
					eRect.X + 0.5f * (eRect.Width - size.Width),
					eRect.Y + 0.5f * (eRect.Height - size.Height));

				id++;
			}

		}

		protected override int GetAutoWidth(IGraphics g)
		{
			int maxWidth = 0;
			foreach (var element in this.Elements)
			{
				maxWidth = Math.Max(maxWidth, (int)(g.MeasureString(element.Text, g.GetFont(FontSize.Medium)).Width + 2));
			}
			return 3 * this.BorderWidth + maxWidth;
		}

		protected override int GetAutoHeight(IGraphics g)
		{
			return 2 * this.BorderWidth + 24 * this.Elements.Count + 8 * (this.Elements.Count - 1);
		}

		protected override void OnInteract(GuiInteraction interaction)
		{
			switch (interaction)
			{
				case GuiInteraction.NavigateUp:
					if (this.selectedID > 0)
					{
						this.Services.Resources.Sounds["Gui/MenuSelect"].Play();
						this.selectedID--;
					}
					break;
				case GuiInteraction.NavigateDown:
					if (this.selectedID < this.Elements.Count - 1)
					{
						this.Services.Resources.Sounds["Gui/MenuSelect"].Play();
						this.selectedID++;
					}
					break;
			}
		}

		public override Element SelectedElement
		{
			get
			{
				if (this.selectedID >= 0)
					return this.Elements[this.selectedID];
				else
					return null;
			}
		}

		public Skin ElementSkin { get; set; }

		public int BorderWidth { get; set; }
	}

	public enum GuiInteraction
	{
		None,
		NavigateUp,
		NavigateDown,
		NavigateLeft,
		NavigateRight,
		Action,
		Escape
	}
}
