using Lidgren.Network;
using NextMMO.Networking;
using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;
using OpenTK.Input;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NextMMO
{
	public partial class Game : GameWindow, IGameServices, INetworkService
	{
		bool isConnected = false;
		int currentFrame = 0;
		ResourceManager<Bitmap> bitmapSource;
		ResourceManager<TileSet> tileSetSource;
		ResourceManager<AnimatedBitmap> characterSprites;
		Graphics graphics;
		Bitmap backBuffer;
		ControllablePlayer player;

		World world;

		NetClient network;
		MessageDispatcher dispatcher;

		Dictionary<int, ProxyPlayer> proxyPlayers;
		Font[] fonts;
		PlayerData playerData;

		public Game()
			: base(
			640, 480,
			GraphicsMode.Default,
			"NextMMO - OpenGL",
			GameWindowFlags.Default,
			DisplayDevice.Default,
			1, 0, 
			GraphicsContextFlags.Debug)
		{
			
		}

		protected override void OnLoad(EventArgs e)
		{
			this.bitmapSource = new ResourceManager<Bitmap>(
				"./Data/Images/",
				(stream) => (Bitmap)Image.FromStream(stream, false, true),
				null,
				".png", ".bmp", ".jpg");
			this.tileSetSource = new ResourceManager<TileSet>(
				"./Data/TileSets/",
				(stream) => TileSet.Load(this, stream),
				(stream, resource) => resource.Save(stream),
				".tset");
			this.characterSprites = new ResourceManager<AnimatedBitmap>(
				"./Data/Images/Characters/",
				(stream) => new AnimatedBitmap(new Bitmap(stream), 4, 4),
				null,
				".png", ".bmp", ".jpg");

			this.playerData = new PlayerData();
			this.playerData.Name = "Unnamed";
			this.playerData.Sprite = "Fighter";

			this.backBuffer = new Bitmap(640, 480);

			this.fonts = new[]
				{
					new Font(FontFamily.GenericSansSerif, 10.0f),
					new Font(FontFamily.GenericSansSerif, 20.0f),
					new Font(FontFamily.GenericSansSerif, 40.0f),
				};

			this.graphics = Graphics.FromImage(this.backBuffer);

			var map = new TileMap(20, 15);
			for (int x = 0; x < map.Width; x++)
			{
				for (int y = 0; y < map.Height; y++)
				{
					if (y == 10)
					{
						if (x == 0)
							map[x, y][0] = 33;
						else if (x == 19)
							map[x, y][0] = 35;
						else
							map[x, y][0] = 34;
					}
					else if (y > 10)
					{
						if (x == 0)
							map[x, y][0] = 41;
						else if (x == 19)
							map[x, y][0] = 43;
						else
							map[x, y][0] = 42;
					}
				}
			}

			map[6, 11][1] = 10;
			map[10, 11][1] = 10;

			this.world = new World(this);
			this.world.TileMap = map;
			this.world.TileSet = this.tileSetSource["DesertTown"];

			this.player = new ControllablePlayer(this.world, 8, 11);
			this.player.Sprite = new AnimatedSprite(this.characterSprites[this.playerData.Sprite], new Point(16, 42));
			this.world.Entities.Add(this.player);

			var config = new NetPeerConfiguration("mq32.de.NextMMO");
			this.network = new NetClient(config);
			this.network.Start();
			this.network.Connect(NextMMO.Properties.Settings.Default.Server, 26000);

			this.proxyPlayers = new Dictionary<int, ProxyPlayer>();

			this.dispatcher = new MessageDispatcher();
			this.dispatcher[MessageType.UpdatePlayerPosition] = this.UpdatePlayerPosition;
			this.dispatcher[MessageType.UpdatePlayer] = this.UpdatePlayer;
			this.dispatcher[MessageType.DestroyPlayer] = this.DestroyPlayer;

			this.Keyboard.KeyDown += Keyboard_KeyDown;
			this.Keyboard.KeyUp += Keyboard_KeyUp;
		}

		void Keyboard_KeyDown(object sender, OpenTK.Input.KeyboardKeyEventArgs e)
		{
			if (!this.isConnected) return;
			switch (e.Key)
			{
				case Key.Left:
					this.player.Direction |= MoveDirection.Left;
					break;
				case Key.Right:
					this.player.Direction |= MoveDirection.Right;
					break;
				case Key.Up:
					this.player.Direction |= MoveDirection.Up;
					break;
				case Key.Down:
					this.player.Direction |= MoveDirection.Down;
					break;
				default:
					break;
			}
		}

		void Keyboard_KeyUp(object sender, OpenTK.Input.KeyboardKeyEventArgs e)
		{
			if (!this.isConnected) return;
			switch (e.Key)
			{
				case Key.Left:
					this.player.Direction &= ~MoveDirection.Left;
					break;
				case Key.Right:
					this.player.Direction &= ~MoveDirection.Right;
					break;
				case Key.Up:
					this.player.Direction &= ~MoveDirection.Up;
					break;
				case Key.Down:
					this.player.Direction &= ~MoveDirection.Down;
					break;
				default:
					break;
			}
		}

		protected override void OnUpdateFrame(FrameEventArgs e)
		{
			NetIncomingMessage msg;
			while ((msg = this.network.ReadMessage()) != null)
			{
				switch (msg.MessageType)
				{
					case NetIncomingMessageType.StatusChanged:
						NetConnectionStatus status = (NetConnectionStatus)msg.ReadByte();
						switch (status)
						{
							case NetConnectionStatus.Connected:
								this.OnConnection();
								break;
							case NetConnectionStatus.Disconnected:
								break;
						}
						this.isConnected = status == NetConnectionStatus.Connected;
						break;
					case NetIncomingMessageType.Data:
						this.dispatcher.Dispatch(msg);
						break;
				}
			}
			currentFrame++;
			this.world.Update();
		}

		protected override void OnRenderFrame(FrameEventArgs e)
		{
			GL.ClearColor(0.0f, 1.0f, 0.0f, 1.0f);
			GL.ClearDepth(1.0f);
			GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

			this.world.Draw();

			// Prepare for OpenGL
			this.backBuffer.RotateFlip(RotateFlipType.RotateNoneFlipY);

			var lockData =this.backBuffer.LockBits(
				new Rectangle(0, 0, 640, 480), 
				System.Drawing.Imaging.ImageLockMode.ReadOnly, 
				System.Drawing.Imaging.PixelFormat.Format24bppRgb);

			// Just copy the image to OpenGL backbuffer.
			GL.DrawPixels(
				640, 480,
				PixelFormat.Bgr,
				PixelType.UnsignedByte,
				lockData.Scan0);

			this.backBuffer.UnlockBits(lockData);

			this.SwapBuffers();
		}

		private void UpdatePlayer(MessageType type, NetIncomingMessage msg)
		{
			int playerID = msg.ReadInt32();

			PlayerData data = new PlayerData();
			data.ReadFrom(msg);

			ProxyPlayer player = GetProxyPlayer(playerID);
			player.Data = data;
		}

		private void DestroyPlayer(MessageType type, NetIncomingMessage msg)
		{
			int playerID = msg.ReadInt32();

			if (this.proxyPlayers.ContainsKey(playerID))
			{
				this.world.Entities.Remove(this.proxyPlayers[playerID]);
				this.proxyPlayers.Remove(playerID);
			}
		}

		private void UpdatePlayerPosition(MessageType type, NetIncomingMessage msg)
		{
			int playerID = msg.ReadInt32();
			float x = msg.ReadFloat();
			float y = msg.ReadFloat();
			byte rotation = msg.ReadByte(7);
			bool walking = msg.ReadBoolean();

			ProxyPlayer player = GetProxyPlayer(playerID);

			player.X = x;
			player.Y = y;
			player.Direction = rotation;
			player.Sprite.AnimationSpeed = walking ? 8 : 0;
		}

		private ProxyPlayer GetProxyPlayer(int playerID)
		{
			ProxyPlayer player;
			if (!this.proxyPlayers.TryGetValue(playerID, out player))
			{
				player = new ProxyPlayer(this);
				player.Sprite = new AnimatedSprite(this.characterSprites["Thief"], new Point(16, 42));
				this.proxyPlayers.Add(playerID, player);
				this.world.Entities.Add(player);
			}
			return player;
		}

		private void OnConnection()
		{
			NetOutgoingMessage msg;

			msg = this.CreateMessage(MessageType.GetUpdate);
			this.network.SendMessage(msg, NetDeliveryMethod.ReliableUnordered);

			UpdatePlayerData();
		}

		private void UpdatePlayerData()
		{
			var msg = this.CreateMessage(MessageType.UpdatePlayer);
			this.playerData.WriteTo(msg);
			this.network.SendMessage(msg, NetDeliveryMethod.ReliableUnordered);
		}

		protected override void OnUnload(EventArgs e)
		{
			this.network.Disconnect("window-closed"); 
		}

		private void exitToolStripMenuItem_Click(object sender, EventArgs e)
		{
			this.Exit();
		}

		#region IGameServices

		Graphics IGameServices.Graphics { get { return this.graphics; } }

		ResourceManager<Bitmap> IGameServices.Bitmaps { get { return this.bitmapSource; } }

		int IGameServices.CurrentFrame { get { return this.currentFrame; } }

		INetworkService IGameServices.Network { get { return this; } }

		ResourceManager<AnimatedBitmap> IGameServices.Characters { get { return this.characterSprites; } }
		Font IGameServices.GetFont(FontSize size) { return this.fonts[(int)size]; }

		#endregion

		#region INetworkService

		void INetworkService.Send(NetOutgoingMessage msg, NetDeliveryMethod method)
		{
			this.network.SendMessage(msg, method);
		}

		public NetOutgoingMessage CreateMessage(MessageType type)
		{
			var msg = this.network.CreateMessage();
			msg.Write((byte)type);
			return msg;

		}

		#endregion
	}
}
