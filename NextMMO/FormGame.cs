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
	public partial class FormGame : Form
	{
		int currentFrame = 0;
		ResourceManager<Bitmap> bitmaps;
		TileMap map;
		Graphics graphics;
		Bitmap backBuffer;
		Entity character;

		public FormGame()
		{
			InitializeComponent();
			this.ClientSize = new Size(640, 480);
			this.bitmaps = new ResourceManager<Bitmap>("./Data/Images/", (stream) => (Bitmap)Image.FromStream(stream, false, true), ".png", ".bmp", ".jpg");

			this.backBuffer = new Bitmap(640, 480);

			this.graphics = Graphics.FromImage(this.backBuffer);

			this.map = new TileMap(20, 15);
			this.map.RenderTile += map_RenderTile;
			this.map.PreRenderMap += map_PreRenderMap;
			this.map.PostRenderLayer += map_PostRenderLayer;

			for (int x = 0; x < this.map.Width; x++)
			{
				for (int y = 0; y < this.map.Height; y++)
				{
					if (y == 10)
					{
						if (x == 0)
							this.map[x, y][0] = 33;
						else if (x == 19)
							this.map[x, y][0] = 35;
						else
							this.map[x, y][0] = 34;
					}
					else if (y > 10)
					{
						if (x == 0)
							this.map[x, y][0] = 41;
						else if (x == 19)
							this.map[x, y][0] = 43;
						else
							this.map[x, y][0] = 42;
					}
				}
			}

			this.map[8, 11][1] = 10;

			this.character = new Entity(8, 11);
			this.character.Sprite = new AnimatedSprite(
				new AnimatedBitmap(this.bitmaps["Characters/134-Butler01"], 4, 4),
				new Point(16, 42));
		}

		void map_PostRenderLayer(object sender, RenderLayerEventArgs e)
		{
			switch (e.Layer)
			{
				case 1:
					this.character.Draw(this.graphics);
					break;
			}
		}

		void map_PreRenderMap(object sender, EventArgs e)
		{
			this.graphics.DrawImage(
				this.bitmaps["007-Ocean01"],
				new Rectangle(0, 0, 640, 480));
		}

		void map_RenderTile(object sender, RenderTileEventArgs e)
		{
			var texmap = this.bitmaps["019-DesertTown01"];
			this.graphics.DrawImage(
				texmap,
				new Rectangle(32 * e.X, 32 * e.Y, 32, 32),
				new Rectangle(
					32 * (e.ID % (texmap.Width / 32)),
					32 * (e.ID / (texmap.Width / 32)),
					32, 32),
				GraphicsUnit.Pixel);
		}

		private void timerFramerate_Tick(object sender, EventArgs e)
		{
			currentFrame++;
			this.Invalidate();
		}

		private void FormGame_Paint(object sender, PaintEventArgs e)
		{
			this.map.Draw(new Rectangle(0, 0, 20, 15));
			e.Graphics.DrawImageUnscaled(
				this.backBuffer,
				(this.ClientSize.Width - this.backBuffer.Width) / 2,
				(this.ClientSize.Height - this.backBuffer.Height) / 2);
		}

		private void FormGame_KeyDown(object sender, KeyEventArgs e)
		{
			System.Diagnostics.Debug.WriteLine(e.KeyCode);
			switch (e.KeyCode)
			{
				default:
					break;
			}
		}

		private void exitToolStripMenuItem_Click(object sender, EventArgs e)
		{
			Application.Exit();
		}
	}
}
