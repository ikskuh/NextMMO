using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NextMMO.Gui
{
	public class Button : Element
	{
		public event EventHandler Click;

		public Button() 
			: base()
		{

		}

		public Button(string text)
			: base(text)
		{

		}

		public Button(string text, EventHandler click)
			: base(text)
		{
			if (click != null)
				this.Click += click;
		}

		public override bool Interact(GuiInteraction interaction)
		{
			switch(interaction)
			{
				case GuiInteraction.Action:
					if (this.Click != null)
						this.Click(this, EventArgs.Empty);
					return true;
				default:
					return base.Interact(interaction);
			}
		}
	}
}
