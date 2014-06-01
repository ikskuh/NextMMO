using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace NextMMO
{
	public partial class FormGame : Form, IGameServices
	{
		int currentFrame = 0;
		ResourceManager<Bitmap> bitmapSource;
		ResourceManager<TileSet> tileSetSource;
		Graphics graphics;
		Bitmap backBuffer;
		ControllablePlayer player;

		World world;

		public FormGame()
		{
			InitializeComponent();
			this.ClientSize = new Size(640, 480);

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

			this.backBuffer = new Bitmap(640, 480);

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
			this.player.Sprite = new AnimatedSprite(
				new AnimatedBitmap(this.bitmapSource["Characters/018-Thief03"], 4, 4),
				new Point(16, 42));
			this.world.Entities.Add(this.player);
		}

		private void timerFramerate_Tick(object sender, EventArgs e)
		{
			currentFrame++;
			this.world.Update();
			this.Invalidate();
		}

		private void FormGame_Paint(object sender, PaintEventArgs e)
		{
			this.world.Draw();
			e.Graphics.DrawImageUnscaled(
				this.backBuffer,
				(this.ClientSize.Width - this.backBuffer.Width) / 2,
				(this.ClientSize.Height - this.backBuffer.Height) / 2);
		}

		private void FormGame_KeyDown(object sender, KeyEventArgs e)
		{
			switch (e.KeyCode)
			{
				case Keys.Left:
					this.player.Direction |= MoveDirection.Left;
					break;
				case Keys.Right:
					this.player.Direction |= MoveDirection.Right;
					break;
				case Keys.Up:
					this.player.Direction |= MoveDirection.Up;
					break;
				case Keys.Down:
					this.player.Direction |= MoveDirection.Down;
					break;
				default:
					break;
			}
		}

		private void FormGame_KeyUp(object sender, KeyEventArgs e)
		{
			switch (e.KeyCode)
			{
				case Keys.Left:
					this.player.Direction &= ~MoveDirection.Left;
					break;
				case Keys.Right:
					this.player.Direction &= ~MoveDirection.Right;
					break;
				case Keys.Up:
					this.player.Direction &= ~MoveDirection.Up;
					break;
				case Keys.Down:
					this.player.Direction &= ~MoveDirection.Down;
					break;
				default:
					break;
			}
		}

		private void exitToolStripMenuItem_Click(object sender, EventArgs e)
		{
			Application.Exit();
		}

		Graphics IGameServices.Graphics { get { return this.graphics; } }

		ResourceManager<Bitmap> IGameServices.Bitmaps { get { return this.bitmapSource; } }

		int IGameServices.CurrentFrame { get { return this.currentFrame; } }
	}
}
