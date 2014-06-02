using Lidgren.Network;
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
		}

		public void Notify(PlayerNotification notification)
		{
			
		}

		public void Send(NetOutgoingMessage msg, NetDeliveryMethod method)
		{
			this.Connection.SendMessage(msg, method, 0);
		}

		public NetConnection Connection { get { return this.connection; } }

		public int ID { get { return this.id; } }
	}

	public enum PlayerNotification
	{
		None,
		Reset,
	}
}
