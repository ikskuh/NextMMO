using Lidgren.Network;
using NextMMO.Networking;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NextMMO
{
	public interface INetworkService
	{
		NetOutgoingMessage CreateMessage(MessageType type);

		void Send(NetOutgoingMessage msg, NetDeliveryMethod method);
	}
}
