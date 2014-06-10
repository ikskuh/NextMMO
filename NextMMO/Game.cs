using Lidgren.Network;
using NextMMO.Gui;
using NextMMO.Networking;
using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;
using OpenTK.Input;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NextMMO
{
	public partial class Game : GameWindow, IGameServices, INetworkService
	{
		private readonly Random random = new Random();

		bool isConnected = false;
		ResourceCollection resources;
		GDIGraphics graphicsWrapper;
		Graphics graphics;
		Bitmap backBuffer;
		ControllablePlayer player;

		World world;

		NetClient network;
		MessageDispatcher dispatcher;

		Dictionary<int, ProxyPlayer> proxyPlayers;
		PlayerData playerData;

		EffectManager effects;

		GameTime time = new GameTime();

		GuiManager gui;
		ListContainer ingameMenu;
		ListContainer debugMenu;
		ListContainer characterMenu;

		ScriptHost scriptHost;
		ScriptInterface scriptInterface;

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
			this.resources = new ResourceCollection(this, "./Data/");

			this.scriptHost = new ScriptHost();
			this.scriptInterface = new ScriptInterface(this);
			this.scriptHost.Interface = this.scriptInterface;

			this.playerData = new PlayerData();
			this.playerData.Name = "Unnamed";
			this.playerData.Sprite = "Lancer";

			this.backBuffer = new Bitmap(640, 480);

			this.graphics = Graphics.FromImage(this.backBuffer);
			this.graphicsWrapper = new GDIGraphics(this.graphics);

			var map = this.resources.Maps["Simple"];

			this.world = new World(this);
			this.world.TileMap = map;
			this.world.TileSet = this.resources.TileSets["DesertTown"];

			this.player = new ControllablePlayer(this.world, 8, 11);
			this.player.Sprite = new AnimatedSprite(this.resources.Characters[this.playerData.Sprite], new Point(16, 42));
			this.world.Focus = this.player;
			this.world.Entities.Add(this.player);

			var thingy = this.resources.Templates["Static/Well"].Instantiate(this.world);
			thingy.Teleport(8, 13);
			this.world.Entities.Add(thingy);

			InitializeNetwork();

			this.Keyboard.KeyDown += Keyboard_KeyDown;
			this.Keyboard.KeyUp += Keyboard_KeyUp;

			this.effects = new EffectManager(this);

			InitializeGui();
		}

		private void InitializeNetwork()
		{
			var config = new NetPeerConfiguration("mq32.de.NextMMO");
			this.network = new NetClient(config);
			this.network.Start();
			this.network.Connect(NextMMO.Properties.Settings.Default.Server, 26000);

			this.proxyPlayers = new Dictionary<int, ProxyPlayer>();

			this.dispatcher = new MessageDispatcher();
			this.dispatcher[MessageType.UpdatePlayerPosition] = this.UpdatePlayerPosition;
			this.dispatcher[MessageType.UpdatePlayer] = this.UpdatePlayer;
			this.dispatcher[MessageType.DestroyPlayer] = this.DestroyPlayer;
		}

		private void InitializeGui()
		{
			this.gui = new GuiManager(this);

			this.ingameMenu = this.CreateBaseContainer();
			this.ingameMenu.Area = new Rectangle(16, 16, 192, 384);
			this.ingameMenu.HorizontalSizeMode = AutoSizeMode.Default;
			this.ingameMenu.VerticalSizeMode = AutoSizeMode.AutoSize;


			this.ingameMenu.Elements.Add(new Label("Game:"));
			this.ingameMenu.Elements.Add(new Button("Character", (s, ea) => this.gui.NavigateTo(this.characterMenu)));
			this.ingameMenu.Elements.Add(new Label("Options:"));
			this.ingameMenu.Elements.Add(new Button("Debug", (s, ea) => { this.gui.NavigateTo(this.debugMenu); }));
			this.ingameMenu.Elements.Add(new Button("Quit Game", (s, ea) => { this.Exit(); }));

			this.debugMenu = this.CreateBaseContainer();
			this.debugMenu.Area = new Rectangle(this.ingameMenu.Area.Right + 16, 16, 192, 256);
			this.debugMenu.HorizontalSizeMode = AutoSizeMode.Default;
			this.debugMenu.VerticalSizeMode = AutoSizeMode.AutoSize;

			this.debugMenu.Elements.Add(new Button("Spawn Effect", (s, ea) => this.SpawnTestEffect()));
			this.debugMenu.Elements.Add(new Button("Show Colliders", (s, ea) => this.world.EnableDebug = true));
			this.debugMenu.Elements.Add(new Button("Hide Colliders", (s, ea) => this.world.EnableDebug = false));
			this.debugMenu.Elements.Add(new Button("Back", (s, ea) => this.gui.NavigateBack()));


			this.characterMenu = this.CreateBaseContainer();
			this.characterMenu.Area = new Rectangle(this.ingameMenu.Area.Right + 16, 16, 192, 256);
			this.characterMenu.HorizontalSizeMode = AutoSizeMode.Default;
			this.characterMenu.VerticalSizeMode = AutoSizeMode.AutoSize;


			this.characterMenu.Elements.Add(new Label("Name:"));

			var textInput = new TextInput("Unnamed");
			textInput.Confirmed += textInput_Confirmed;
			this.characterMenu.Elements.Add(textInput);

			var characterSelector = new CharacterSelector(this);
			characterSelector.SelectionChanged += characterSelector_SelectionChanged;
			this.characterMenu.Elements.Add(characterSelector);

			this.characterMenu.Elements.Add(new Button("Back", (s, ea) => this.gui.NavigateBack()));
		}

		void textInput_Confirmed(object sender, EventArgs e)
		{
			TextInput textInput = sender as TextInput;
			this.playerData.Name = textInput.Text;
			this.UpdatePlayerData();
		}

		void characterSelector_SelectionChanged(object sender, EventArgs e)
		{
			CharacterSelector selector = sender as CharacterSelector;
			this.playerData.Sprite = selector.SelectedCharacter;
			this.player.Sprite = new AnimatedSprite(
				this.resources.Characters[selector.SelectedCharacter],
				new Point(16, 42));
			this.UpdatePlayerData();
		}

		private static TileMap CreateSimpleMap()
		{
			var map = new TileMap(30, 25);
			for (int x = 0; x < map.Width; x++)
			{
				for (int y = 0; y < map.Height; y++)
				{
					if (y == 10)
					{
						if (x == 0)
							map[x, y][0] = 33;
						else if (x == map.Width - 1)
							map[x, y][0] = 35;
						else
							map[x, y][0] = 34;
					}
					else if (y > 10)
					{
						if (x == 0)
							map[x, y][0] = 41;
						else if (x == map.Width - 1)
							map[x, y][0] = 43;
						else
							map[x, y][0] = 42;
					}
				}
			}

			map[6, 11][1] = 10;
			map[10, 11][1] = 10;
			return map;
		}

		private ListContainer CreateBaseContainer()
		{
			var baseSkin = this.resources.Bitmaps["Skins/Blue"];

			var ctrl = new ListContainer(this);
			ctrl.Background = new StretchedSkin(baseSkin, new Rectangle(0, 0, 128, 128));
			ctrl.Border = new SkinnedSkin(baseSkin, new Rectangle(128, 0, 64, 64)) { FillCenter = false };
			ctrl.ElementSkin = new SkinnedSkin(baseSkin, new Rectangle(128, 64, 32, 32)) { WrapMode = BorderWrapMode.Stretch };

			return ctrl;
		}

		private void SpawnTestEffect()
		{
			Effect effect = new Effect()
			{
				Position = new PointF(
					(float)random.NextDouble() * 640.0f,
					(float)random.NextDouble() * 480.0f),
				Speed = 8.0f,
				Animation = 0,
				Sprite = new AnimatedBitmap(this.resources.Bitmaps["Effects/Support03"], 5, 1)
			};
			this.effects.Spawn(effect);
		}

		void Keyboard_KeyDown(object sender, OpenTK.Input.KeyboardKeyEventArgs e)
		{
			if (!this.isConnected) return;
			switch (e.Key)
			{
				case Key.Left:
					if (!this.gui.IsActive)
						this.player.Direction |= MoveDirection.Left;
					else
						this.gui.Interact(GuiInteraction.NavigateLeft);
					break;
				case Key.Right:
					if (!this.gui.IsActive)
						this.player.Direction |= MoveDirection.Right;
					else
						this.gui.Interact(GuiInteraction.NavigateRight);
					break;
				case Key.Up:
					if (!this.gui.IsActive)
						this.player.Direction |= MoveDirection.Up;
					else
						this.gui.Interact(GuiInteraction.NavigateUp);
					break;
				case Key.Down:
					if (!this.gui.IsActive)
						this.player.Direction |= MoveDirection.Down;
					else
						this.gui.Interact(GuiInteraction.NavigateDown);
					break;
				case Key.Enter:
				case Key.KeypadEnter:
				case Key.Space:
					if (this.gui.IsActive)
						this.gui.Interact(GuiInteraction.Action);
					else
						this.player.Interact(this.world);
					break;
				case Key.Escape:
					if (this.gui.IsActive)
					{
						this.gui.Interact(GuiInteraction.Escape);
					}
					else
					{
						this.gui.NavigateTo(this.ingameMenu);
						//this.soundSource["Gui/MenuSpawn"].Play();
					}
					break;
				case Key.E:
					this.SpawnTestEffect();
					break;
				case Key.BackSpace:
					this.gui.SignalKeyPress((char)127);
					break;
				default:
					break;
			}
		}

		protected override void OnKeyPress(KeyPressEventArgs e)
		{
			this.gui.SignalKeyPress(e.KeyChar);
			base.OnKeyPress(e);
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
			this.time.Advance(e.Time);
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

			this.world.Update();
			this.effects.Update();
		}

		protected override void OnRenderFrame(FrameEventArgs e)
		{
			GL.ClearColor(0.0f, 1.0f, 0.0f, 1.0f);
			GL.ClearDepth(1.0f);
			GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

			this.world.Draw();
			this.effects.Draw();
			this.gui.Draw();

			// Prepare for OpenGL
			this.backBuffer.RotateFlip(RotateFlipType.RotateNoneFlipY);

			var lockData = this.backBuffer.LockBits(
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
			player.Sprite.Speed = walking ? 8 : 0;
		}

		private ProxyPlayer GetProxyPlayer(int playerID)
		{
			ProxyPlayer player;
			if (!this.proxyPlayers.TryGetValue(playerID, out player))
			{
				player = new ProxyPlayer(this);
				player.Sprite = new AnimatedSprite(this.resources.Characters["Thief"], new Point(16, 42));
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

		IGraphics IGameServices.Graphics { get { return this.graphicsWrapper; } }

		INetworkService IGameServices.Network { get { return this; } }

		Random IGameServices.Random { get { return this.random; } }

		GameTime IGameServices.Time { get { return this.time; } }

		IGameResources IGameServices.Resources { get { return this.resources; } }

		IScriptHost IGameServices.ScriptHost { get { return this.scriptHost; } }

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
