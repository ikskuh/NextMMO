using Lidgren.Network;
using NextMMO.Networking;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace NextMMO.Server
{
	public class GameHost
	{
		NetServer server;
		MessageDispatcher dispatcher;

		PlayerCollection players;

		public GameHost(int port)
		{
			var config = new NetPeerConfiguration("mq32.de.NextMMO");
			config.Port = port;
			this.server = new NetServer(config);

			this.players = new PlayerCollection(this);

			this.dispatcher = new MessageDispatcher();
			this.dispatcher[MessageType.UpdatePlayerPosition] = this.UpdatePlayerPosition;
			this.dispatcher[MessageType.GetUpdate] = this.GetUpdate;
			this.dispatcher[MessageType.UpdatePlayer] = this.UpdatePlayer;
		}

		private void UpdatePlayer(MessageType type, NetIncomingMessage msg)
		{
			var player = this.players[msg.SenderConnection];
			player.Data.ReadFrom(msg);

			var response = this.CreateMessage(MessageType.UpdatePlayer);
			response.Write(player.ID);
			player.Data.WriteTo(response);
			this.players.BroadcastMessage(response, NetDeliveryMethod.ReliableUnordered, player);
		}

		private void UpdatePlayerPosition(MessageType type, NetIncomingMessage msg)
		{
			var player = this.players[msg.SenderConnection];

			player.X = msg.ReadFloat();
			player.Y = msg.ReadFloat();
			player.Animation = msg.ReadByte(7);
			player.IsWalking = msg.ReadBoolean();

			int playerID = player.ID;

			var updateMsg = player.CreateUpdatePlayerPositionMessage();

			this.players.BroadcastMessage(updateMsg, NetDeliveryMethod.Unreliable, player);
		}

		private void GetUpdate(MessageType type, NetIncomingMessage msg)
		{
			var player = this.players[msg.SenderConnection];

			foreach (var p in this.players)
			{
				if (p == player) continue;
				
				// Update player information
				var response = this.CreateMessage(MessageType.UpdatePlayer);
				response.Write(p.ID);
				p.Data.WriteTo(response);
				player.Send(response, NetDeliveryMethod.ReliableUnordered);

				// Update player data
				player.Send(p.CreateUpdatePlayerPositionMessage(), NetDeliveryMethod.ReliableUnordered);
			}
		}

		public void Start()
		{
			this.server.Start();
			while (this.server.Status != NetPeerStatus.NotRunning)
			{
				NetIncomingMessage msg;
				while ((msg = this.server.ReadMessage()) != null)
				{
					switch (msg.MessageType)
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

		public NetOutgoingMessage CreateMessage(MessageType type)
		{
			var msg = this.server.CreateMessage();
			msg.Write((byte)type);
			return msg;
		}

		public void SendMessage(NetOutgoingMessage msg, NetDeliveryMethod method, params NetConnection[] receivers)
		{
			this.SendMessage(msg, method, new List<NetConnection>(receivers));
		}

		public void SendMessage(NetOutgoingMessage msg, NetDeliveryMethod method, List<NetConnection> receivers)
		{
			if (receivers.Count == 0) return;
			this.server.SendMessage(
				msg,
				receivers,
				method,
				0);
		}
	}
}
