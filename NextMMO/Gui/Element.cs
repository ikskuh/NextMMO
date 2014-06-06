using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NextMMO.Gui
{
	public class Element
	{
		public event EventHandler Activated;

		public Element()
			: this("")
		{

		}

		public Element(string text)
		{
			this.Text = text;
		}

		public Element(string text, EventHandler activated)
			: this(text)
		{
			if (activated != null)
				this.Activated += activated;
		}

		public virtual void Trigger()
		{
			if (this.Activated != null)
				this.Activated(this, EventArgs.Empty);
		}

		public virtual string Text { get; set; }
	}
}
