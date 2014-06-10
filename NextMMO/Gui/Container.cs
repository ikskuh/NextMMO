using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NextMMO.Gui
{
	public abstract class Container : GameObject
	{
		//readonly List<Element> elements = new List<Element>();
		readonly ObservableCollection<Element> elements = new ObservableCollection<Element>();

		public event EventHandler Cancelled;

		public Container(IGameServices services)
			: base(services)
		{
			elements.CollectionChanged += (s, e) => this.OnElementsChanged();
		}

		protected virtual void OnElementsChanged()
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
				case GuiInteraction.Escape:
					if (this.Cancelled != null)
					{
						this.Services.Resources.Sounds["Gui/MenuEscape"].Play();
						this.Cancelled(this, EventArgs.Empty);
					}
					break;
				default:
					var e = this.SelectedElement;
					if (e != null)
					{
						if (e.Interact(interaction))
						{
							if (interaction == GuiInteraction.Action)
							{
								this.Services.Resources.Sounds["Gui/MenuClick"].Play();
							}
							return;
						}
					}
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
		private Element selection;
		public ListContainer(IGameServices services)
			: base(services)
		{
			this.BorderWidth = 16;
			this.DefaultElementHeight = 24;
		}

		protected override void OnElementsChanged()
		{
			// Make sure we have a selection if possible
			var prev = this.selection;
			this.selection = this.selection ?? this.Elements.FirstOrDefault((x) => x.IsSelectable);
			if (prev != this.selection)
			{
				if (prev != null)
					prev.EndSelect();
				if(this.selection != null)
					this.selection.BeginSelect();
			}
		}

		protected override void OnDraw(IGraphics g, Rectangle rect)
		{
			this.Services.Graphics.SetClip(new Rectangle(
				rect.Left + this.BorderWidth,
				rect.Top + this.BorderWidth,
				rect.Width - 2 * this.BorderWidth,
				rect.Height - 2 * this.BorderWidth));

			int offset = 0;
			foreach (var element in this.Elements)
			{
				int height;
				if (float.IsNaN(element.Height))
					height = (int)element.GetAutoSize(g).Height;
				else
					height = (int)element.Height;

				Rectangle eRect = new Rectangle(
					rect.Left + this.BorderWidth,
					rect.Top + this.BorderWidth + offset,
					rect.Width - 2 * this.BorderWidth,
					height);

				if (eRect.Top > rect.Bottom)
					break;

				if (element == this.SelectedElement)
				{
					this.ElementSkin.Draw(
						g,
						eRect);
				}

				element.Draw(g, eRect);

				offset += height + 8;
			}

		}

		protected override int GetAutoWidth(IGraphics g)
		{
			int maxWidth = 0;
			foreach (var element in this.Elements)
			{
				// Is auto sized. Ignore element
				if (float.IsNaN(element.Width))
					maxWidth = Math.Max(maxWidth, (int)element.GetAutoSize(g).Width);
				else
					maxWidth = Math.Max(maxWidth, (int)element.Width);
			}
			return 3 * this.BorderWidth + maxWidth;
		}

		protected override int GetAutoHeight(IGraphics g)
		{
			int height = 0;
			foreach (var element in this.Elements)
			{
				height += 8;
				// Is auto sized. Ignore element
				if (float.IsNaN(element.Height))
					height += (int)element.GetAutoSize(g).Height;
				else
					height += (int)element.Height;
			}
			return 2 * this.BorderWidth + height - 8;
		}

		protected override void OnInteract(GuiInteraction interaction)
		{
			switch (interaction)
			{
				case GuiInteraction.NavigateUp:
					// Select the previous element if possible
					for (int i = this.Elements.IndexOf(this.selection) - 1; i >= 0; i--)
					{
						if (this.Elements[i].IsSelectable)
						{
							if (this.selection != null)
								this.selection.EndSelect();
							this.selection = this.Elements[i];
							if (this.selection != null)
								this.selection.BeginSelect();
							this.Services.Resources.Sounds["Gui/MenuSelect"].Play();
							break;
						}
					}
					break;
				case GuiInteraction.NavigateDown:
					// Select the next element if possible
					for (int i = this.Elements.IndexOf(this.selection) + 1; i < this.Elements.Count; i++)
					{
						if (this.Elements[i].IsSelectable)
						{
							if (this.selection != null)
								this.selection.EndSelect();
							this.selection = this.Elements[i];
							if (this.selection != null)
								this.selection.BeginSelect();
							this.Services.Resources.Sounds["Gui/MenuSelect"].Play();
							break;
						}
					}
					break;
			}
		}

		public override Element SelectedElement { get { return this.selection; } }

		public Skin ElementSkin { get; set; }

		public int BorderWidth { get; set; }

		public int DefaultElementHeight { get; set; }
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
