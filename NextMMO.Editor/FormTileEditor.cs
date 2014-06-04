using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace NextMMO
{
	public partial class FormTileEditor : Form
	{
		TileDefinition tile;

		public FormTileEditor(TileDefinition tile)
		{
			InitializeComponent();
			this.tile = tile;
			foreach(var collider in this.tile.Colliders)
			{
				this.listBoxColliders.Items.Add(collider);
			}
		}

		private void listBoxColliders_SelectedIndexChanged(object sender, EventArgs e)
		{
			this.buttonRemoveCollider.Enabled = this.listBoxColliders.SelectedItem != null;
			this.propertyGrid.SelectedObject = this.listBoxColliders.SelectedItem;
		}

		private void panelTile_Paint(object sender, PaintEventArgs e)
		{
			var mode = e.Graphics.InterpolationMode;
			e.Graphics.InterpolationMode = InterpolationMode.NearestNeighbor;
			e.Graphics.DrawImage(
				this.tile.TileSet.Services.Bitmaps[this.tile.TileSet.Source],
				new Rectangle(0, 0, this.panelTile.ClientSize.Width, this.panelTile.ClientSize.Height),
				this.tile.Source,
				GraphicsUnit.Pixel);
			foreach(TileCollider collider in this.listBoxColliders.Items)
			{
				Rectangle target = new Rectangle(
					4 * collider.Rectangle.X,
					4 * collider.Rectangle.Y,
					4 * collider.Rectangle.Width,
					4 * collider.Rectangle.Height);
				e.Graphics.FillRectangle(
					TileCollider.DebugBrush,
					target);
				e.Graphics.DrawRectangle(
					Pens.Black,
					target);
			}

			e.Graphics.InterpolationMode = mode;
		}

		private void FormTileEditor_FormClosed(object sender, FormClosedEventArgs e)
		{
			this.tile.Colliders.Clear();
			foreach(var collider in this.listBoxColliders.Items)
			{
				var c = collider as TileCollider;
				if(c != null)
					this.tile.Colliders.Add(c);
			}
		}

		private void buttonCreateCollider_Click(object sender, EventArgs e)
		{
			var c = new TileCollider();
			this.listBoxColliders.Items.Add(c);
			this.listBoxColliders.SelectedItem = c;
			this.panelTile.Invalidate();
		}

		private void buttonRemoveCollider_Click(object sender, EventArgs e)
		{
			this.listBoxColliders.Items.Remove(this.listBoxColliders.SelectedItem);
			this.panelTile.Invalidate();
		}

		private void propertyGrid_PropertyValueChanged(object s, PropertyValueChangedEventArgs e)
		{
			if(this.listBoxColliders.SelectedIndex != -1)
			{
				this.listBoxColliders.Items[this.listBoxColliders.SelectedIndex] = this.listBoxColliders.SelectedItem;
			}
			this.panelTile.Invalidate();
		}
	}
}
