using Lidgren.Network;
using NextMMO.Networking;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace NextMMO.Server
{
	class GameHost
	{
		NetServer server;
		MessageDispatcher dispatcher;

		public GameHost(int port)
		{
			var config = new NetPeerConfiguration("mq32.de.NextMMO");
			config.Port = port;
			this.server = new NetServer(config);

			this.dispatcher = new MessageDispatcher();
		}

		public void Start()
		{
			this.server.Start();
			while (this.server.Status != NetPeerStatus.NotRunning)
			{
				NetIncomingMessage msg;
				while((msg = this.server.ReadMessage()) != null)
				{
					switch(msg.MessageType)
					{
						case NetIncomingMessageType.Data:
							this.dispatcher.Dispatch(msg);
							break;
						default:
							Console.WriteLine("Unhandled message type: {0}", msg.MessageType);
							break;
					}
				}
				Thread.Sleep(10);
			}
		}
	}
}
