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

		public Container(IGameServices services)
			: base(services)
		{

		}

		public void Draw()
		{
			var state = this.Services.Graphics.Save();

			if (this.Background != null)
			{
				this.Background.Draw(
					this.Services.Graphics,
					this.Area);
			}
			if (this.Border != null)
			{
				this.Border.Draw(
					this.Services.Graphics,
					this.Area);
			}

			this.Services.Graphics.IntersectClip(this.Area);
			this.OnDraw(this.Services.Graphics);

			this.Services.Graphics.Restore(state);
		}

		public void Interact(GuiInteraction interaction)
		{
			switch(interaction)
			{
				case GuiInteraction.Action:
					var e = this.SelectedElement;
					if (e != null)
						e.Trigger();
					break;
				case GuiInteraction.Escape:
					// TODO: Implement "escape" behaviour
					break;
				default:
					this.OnInteract(interaction);
					break;
			}
		}

		protected abstract void OnDraw(Graphics g);

		protected abstract void OnInteract(GuiInteraction interaction);

		public Rectangle Area { get; set; }

		public IList<Element> Elements
		{
			get { return elements; }
		}

		public Skin Background { get; set; }

		public Skin Border { get; set; }

		public abstract Element SelectedElement { get; }
	}

	public class ListContainer : Container
	{
		int selectedID = 0;

		public ListContainer(IGameServices services)
			: base(services)
		{
			this.BorderWidth = 16;
		}

		protected override void OnDraw(Graphics g)
		{
			var state = this.Services.Graphics.Save();

			this.Services.Graphics.IntersectClip(new Rectangle(
				this.Area.Left + this.BorderWidth,
				this.Area.Top + this.BorderWidth,
				this.Area.Width - 2 * this.BorderWidth,
				this.Area.Height - 2 * this.BorderWidth));

			int id = 0;
			foreach (var element in this.Elements)
			{
				Rectangle eRect = new Rectangle(
					this.Area.Left + this.BorderWidth,
					this.Area.Top + this.BorderWidth + 32 * id,
					this.Area.Width - 2 * this.BorderWidth,
					24);

				if (eRect.Top > this.Area.Bottom)
					break;

				if (id == this.selectedID)
				{
					this.ElementSkin.Draw(
						g,
						eRect);
				}

				var size = g.MeasureString(element.Text, this.Services.GetFont(FontSize.Medium));

				g.DrawString(
					element.Text,
					this.Services.GetFont(FontSize.Medium),
					Brushes.Black,
					eRect.X + 0.5f * (eRect.Width - size.Width),
					eRect.Y + 0.5f * (eRect.Height - size.Height));

				id++;
			}

			this.Services.Graphics.Restore(state);
		}

		protected override void OnInteract(GuiInteraction interaction)
		{
			switch(interaction)
			{
				case GuiInteraction.NavigateUp:
					if (this.selectedID > 0)
						this.selectedID--;
					break;
				case GuiInteraction.NavigateDown:
					if (this.selectedID < this.Elements.Count - 1)
						this.selectedID++;
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
