using Lidgren.Network;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NextMMO.Networking
{
	/// <summary>
	/// Provides methods to transfer a piece of data over network.
	/// </summary>
	public interface ITransferable
	{
		void WriteTo(NetOutgoingMessage msg);

		void ReadFrom(NetIncomingMessage msg);
	}
}
