using Lidgren.Network;
using NextMMO.Networking;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NextMMO
{
	public sealed class PlayerData : ITransferable
	{
		public void WriteTo(NetOutgoingMessage msg)
		{
			msg.Write(this.Name);
			msg.Write(this.Sprite);
		}

		public void ReadFrom(NetIncomingMessage msg)
		{
			this.Name = msg.ReadString();
			this.Sprite = msg.ReadString();
		}

		public string Name { get; set; }

		public string Sprite { get; set; }
	}
}
