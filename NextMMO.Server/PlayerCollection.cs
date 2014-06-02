using Lidgren.Network;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NextMMO.Server
{
	public class PlayerCollection : IEnumerable<Player>
	{
		readonly GameHost host;
		readonly Dictionary<NetConnection, Player> players = new Dictionary<NetConnection, Player>();
		int currentID = 1;

		public PlayerCollection(GameHost host)
		{
			this.host = host;
		}

		/// <summary>
		/// Gets a player or resets the connection.
		/// </summary>
		/// <param name="con">Connection</param>
		/// <returns>Player for this connection.</returns>
		public Player this[NetConnection con]
		{
			get
			{
				if (con == null)
					return null;
				lock (this.players)
				{
					// Always return a valid, unique player.
					if (this.players.ContainsKey(con))
					{
						return this.players[con];
					}
					else
					{
						// TODO: Implement better ID system.
						Player player = new Player(this.host, this, con, this.currentID++);
						this.players.Add(con, player);
						return player;
					}
				}
			}
			set
			{
				if (con == null)
					return;
				lock (this.players)
				{
					if (this.players.ContainsKey(con))
					{
						var player = this.players[con];
						if (player == value)
						{
							return; // Do nothing
						}
						if (value != null)
						{
							throw new InvalidOperationException("Cannot change the player for a given connection.");
						}

						// Remove the player. Connection reset.
						this.players.Remove(con);
						player.Notify(PlayerNotification.Reset);
					}
				}

			}
		}

		/// <summary>
		/// Sends a message to all players.
		/// </summary>
		/// <param name="msg"></param>
		/// <param name="method"></param>
		public void BroadcastMessage(NetOutgoingMessage msg, NetDeliveryMethod method)
		{
			this.BroadcastMessage(msg, method, new Player[0]);
		}

		/// <summary>
		/// Sends a message to all players except ignorants.
		/// </summary>
		/// <param name="msg"></param>
		/// <param name="method"></param>
		/// <param name="ignorants"></param>
		public void BroadcastMessage(NetOutgoingMessage msg, NetDeliveryMethod method, params Player[] ignorants)
		{
			this.BroadcastMessage(msg, method, (IEnumerable<Player>)ignorants);
		}

		/// <summary>
		/// Sends a message to all players except ignorants.
		/// </summary>
		/// <param name="msg"></param>
		/// <param name="method"></param>
		/// <param name="ignorants"></param>
		public void BroadcastMessage(NetOutgoingMessage msg, NetDeliveryMethod method, IEnumerable<Player> ignorants)
		{
			HashSet<Player> ignorantSet = new HashSet<Player>(ignorants);
			List<NetConnection> receivers = new List<NetConnection>();
			foreach (var player in this.players.Values)
			{
				if (ignorantSet.Contains(player))
				{
					continue;
				}
				receivers.Add(player.Connection);
			}
			this.host.SendMessage(msg, method, receivers);
			
		}

		/// <summary>
		/// Gets an enumerator over all active players.
		/// </summary>
		/// <returns></returns>
		public IEnumerator<Player> GetEnumerator()
		{
			return this.players.Values.GetEnumerator();
		}

		System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
		{
			return this.GetEnumerator();
		}
	}
}
