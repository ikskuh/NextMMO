using Lidgren.Network;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NextMMO.Networking
{
	public delegate void MessageHandlerDelegate(MessageType type, NetIncomingMessage msg);

	public sealed class MessageDispatcher
	{
		readonly Dictionary<MessageType, MessageHandlerDelegate> handlers = new Dictionary<MessageType, MessageHandlerDelegate>();

		public MessageDispatcher()
		{
			this[MessageType.None] = (type, msg) => { };
		}

		public void Dispatch(NetIncomingMessage msg)
		{
			MessageType type = (MessageType)msg.ReadByte();
			if (this.handlers.ContainsKey(type))
			{
				this.handlers[type](type, msg);
			}
			else
			{
				Console.WriteLine("Unhandled message type: {0}", type);
			}
		}

		public MessageHandlerDelegate this[MessageType type]
		{
			get
			{
				if (this.handlers.ContainsKey(type))
					return this.handlers[type];
				else
					return null;
			}
			set
			{
				if (this.handlers.ContainsKey(type))
					this.handlers[type] = value;
				else
					this.handlers.Add(type, value);
			}
		}
	}

	public enum MessageType : byte
	{
		None = 0,
		UpdatePlayer,
	}
}
