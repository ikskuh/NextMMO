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
	public partial class FormTileSetEditor : Form, IGameServices
	{
		Bitmap backBuffer;
		Graphics graphics;
		TileSet tileSet;
		ResourceManager<Bitmap> bitmapSource;
		ResourceManager<TileSet> tileSetSource;

		public FormTileSetEditor()
		{
			InitializeComponent();

			this.ClientSize = new Size(256, this.toolStrip1.Height + 704);
			this.panelTileSet.Top = this.toolStrip1.Height;

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
			this.backBuffer = new Bitmap(256, 704);
			this.graphics = Graphics.FromImage(this.backBuffer);

			this.tileSet = new TileSet(this, 8, 22);
			this.tileSet.Source = "019-DesertTown01";

			this.RenderTileSet();
		}

		private void RenderTileSet()
		{
			for (int x = 0; x < this.tileSet.SizeX; x++)
			{
				for (int y = 0; y < this.tileSet.SizeY; y++)
				{
					int id = y * this.tileSet.SizeX + x + 1;

					this.tileSet[id].Draw(x, y);

					foreach (var rect in this.tileSet[id].CreateEnvironment(x, y))
					{
						this.graphics.FillRectangle(TileCollider.DebugBrush, rect);
					}
				}
			}
		}

		private void exitToolStripMenuItem_Click(object sender, EventArgs e)
		{
			Application.Exit();
		}

		private void neuToolStripButton_Click(object sender, EventArgs e)
		{
			this.textBoxName.Name = "Unnamed";
		}

		IGraphics IGameServices.Graphics { get { return null; } }

		INetworkService IGameServices.Network { get { return null; } }

		private void panelTileSet_Paint(object sender, PaintEventArgs e)
		{
			e.Graphics.DrawImageUnscaled(this.backBuffer, 0, 0);
		}

		private void panelTileSet_MouseDoubleClick(object sender, MouseEventArgs e)
		{
			int tileX = e.X / TileMap.TileSize;
			int tileY = e.Y / TileMap.TileSize;

			int id = tileY * this.tileSet.SizeX + tileX + 1;

			FormTileEditor edit = new FormTileEditor(this.tileSet[id]);
			edit.ShowDialog(this);

			// Refresh tile set visualisation
			this.RenderTileSet();

			this.panelTileSet.Invalidate();
		}

		private void öffnenToolStripButton_Click(object sender, EventArgs e)
		{
			try
			{
				this.tileSet = this.tileSetSource[this.textBoxName.Text];

				// Refresh tile set visualisation
				this.RenderTileSet();

				this.panelTileSet.Invalidate();
			}
			catch(Exception ex)
			{
				MessageBox.Show(
					this, 
					"Could not load tile set '" + this.textBoxName.Text + "':\n" + ex.Message, 
					this.Text, 
					MessageBoxButtons.OK,
					MessageBoxIcon.Warning);
			}
		}

		private void speichernToolStripButton_Click(object sender, EventArgs e)
		{
			if (MessageBox.Show(
				this,
				"Are you sure you want to save the current tile set as '" + this.textBoxName.Text + "'?",
				this.Text,
				MessageBoxButtons.YesNo,
				MessageBoxIcon.Question) == System.Windows.Forms.DialogResult.No)
			{
				return;
			}
			try
			{
				this.tileSetSource.Save(this.textBoxName.Text, this.tileSet);
			}
			catch (Exception ex)
			{
				MessageBox.Show(
					this, 
					"Could not save tile set '" + this.textBoxName.Text + "':\n" + ex.Message, 
					this.Text, 
					MessageBoxButtons.OK, 
					MessageBoxIcon.Warning);
			}
		}

		public Random Random
		{
			get { throw new NotImplementedException(); }
		}


		public GameTime Time
		{
			get { throw new NotImplementedException(); }
		}

		public IGameResources Resources
		{
			get { throw new NotImplementedException(); }
		}
	}
}
