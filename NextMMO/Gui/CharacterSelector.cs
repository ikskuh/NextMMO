using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NextMMO.Gui
{
	public sealed class CharacterSelector : Element
	{
		public event EventHandler SelectionChanged;

		readonly IGameServices services;
		int selection = 0;
		int fixedSelection = 0;
		string[] characters;

		public CharacterSelector(IGameServices services)
		{
			this.services = services;
			this.characters = new[]
			{
				"Butler",
				"CBabe",
				"Cleric",
				"Fighter",
				"Lancer",
				"Mage",
				"Soldier",
				"Thief",
			};
		}

		protected override void OnBeginSelect()
		{
			this.selection = this.fixedSelection;
		}

		public override bool Interact(GuiInteraction interaction)
		{
			switch (interaction)
			{
				case GuiInteraction.NavigateLeft:
					if (this.selection > 0)
						this.selection--;
					return true;
				case GuiInteraction.NavigateRight:
					if (this.selection < this.characters.GetUpperBound(0))
						this.selection++;
					return true;
				case GuiInteraction.Action:
					if (this.fixedSelection != this.selection && this.SelectionChanged != null)
					{
						this.fixedSelection = this.selection;
						this.SelectionChanged(this, EventArgs.Empty);
					}
					else
					{
						this.fixedSelection = this.selection;
					}
					return true;
				default:
					return base.Interact(interaction);
			}
		}

		public override SizeF GetAutoSize(IGraphics graphics)
		{
			return new SizeF(34, 50);
		}

		public override void Draw(IGraphics graphics, Rectangle rect)
		{
			services.Resources.Characters[this.characters[this.selection]].Draw(
				graphics,
				new Rectangle(rect.Left + (rect.Width - 32) / 2, rect.Top + (rect.Height - 48) / 2, 32, 48),
				0,
				(int)((this.IsSelected ? 8.0f : 0.0f) * this.services.Time.Total));
		}

		public string SelectedCharacter { get { return this.characters[this.selection]; } }
	}
}
