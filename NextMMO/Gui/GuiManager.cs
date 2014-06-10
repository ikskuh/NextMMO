using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NextMMO.Gui
{
	public sealed class GuiManager : GameObject
	{
		private readonly Stack<Container> activeContainers = new Stack<Container>();

		public GuiManager(IGameServices services)
			: base(services)
		{

		}

		internal void Draw()
		{
			foreach (var container in this.activeContainers)
			{
				container.Draw();
			}
		}

		public void NavigateTo(Container container)
		{
			if (this.activeContainers.Count <= 0)
			{
				this.Services.Resources.Sounds["Gui/MenuSpawn"].Play();
			}
			this.activeContainers.Push(container);
		}

		public void NavigateBack()
		{
			this.Services.Resources.Sounds["Gui/MenuEscape"].Play();
			this.activeContainers.Pop();
		}

		internal void Interact(GuiInteraction guiInteraction)
		{
			if (this.activeContainers.Count <= 0)
			{
				return;
			}
			var active = this.activeContainers.Peek();
			switch (guiInteraction)
			{
				case GuiInteraction.Action:
				case GuiInteraction.NavigateDown:
				case GuiInteraction.NavigateLeft:
				case GuiInteraction.NavigateRight:
				case GuiInteraction.NavigateUp:
					active.Interact(guiInteraction);
					break;
				case GuiInteraction.Escape:
					this.NavigateBack();
					break;
			}
		}

		public void SignalKeyPress(char c)
		{
			if (this.activeContainers.Count <= 0)
			{
				return;
			}
			var active = this.activeContainers.Peek();
			active.SignalKeyPress(c);
		}

		public bool IsActive { get { return this.activeContainers.Count > 0; } }
	}
}
