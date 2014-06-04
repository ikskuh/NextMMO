using Lidgren.Network;
using NextMMO.Networking;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NextMMO.Server
{
	public class Player
	{
		readonly GameHost host;
		readonly PlayerCollection players;
		readonly NetConnection connection;
		readonly int id;
		readonly PlayerData data;

		public Player(GameHost host, PlayerCollection players, NetConnection connection, int id)
		{
			if (host == null) throw new ArgumentNullException("host");
			if (players == null) throw new ArgumentNullException("players");
			if (connection == null) throw new ArgumentNullException("connection");
			if (id == 0) throw new ArgumentException("id must be inequal to 0.", "id");

			this.host = host;
			this.players = players;
			this.connection = connection;
			this.id = id;

			this.data = new PlayerData();
		}

		public void Notify(PlayerNotification notification)
		{
			NetOutgoingMessage msg;
			switch(notification)
			{
				case PlayerNotification.Disconnected:
					// Send destroy message.
					msg = this.host.CreateMessage(MessageType.DestroyPlayer);
					msg.Write(this.ID);
					this.players.BroadcastMessage(msg, NetDeliveryMethod.ReliableUnordered, this);
					break;
			}
		}

		public void Send(NetOutgoingMessage msg, NetDeliveryMethod method)
		{
			this.Connection.SendMessage(msg, method, 0);
		}

		public NetOutgoingMessage CreateUpdatePlayerPositionMessage()
		{
			var updateMsg = this.host.CreateMessage(MessageType.UpdatePlayerPosition);
			updateMsg.Write(this.ID);
			updateMsg.Write(this.X);
			updateMsg.Write(this.Y);
			updateMsg.Write(this.Animation, 7);
			updateMsg.Write(this.IsWalking);
			return updateMsg;
		}

		public NetConnection Connection { get { return this.connection; } }

		public int ID { get { return this.id; } }

		public float X { get; set; }

		public float Y { get; set; }

		public byte Animation { get; set; }

		public bool IsWalking { get; set; }

		public PlayerData Data { get { return data; } } 
	}

	public enum PlayerNotification
	{
		None,
		Reset,
		Connected,
		Disconnected,
	}
}
