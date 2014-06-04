namespace NextMMO
{
	partial class FormTileEditor
	{
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		/// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
		protected override void Dispose(bool disposing)
		{
			if (disposing && (components != null))
			{
				components.Dispose();
			}
			base.Dispose(disposing);
		}

		#region Windows Form Designer generated code

		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.panelTile = new System.Windows.Forms.Panel();
			this.listBoxColliders = new System.Windows.Forms.ListBox();
			this.toolStrip1 = new System.Windows.Forms.ToolStrip();
			this.buttonCreateCollider = new System.Windows.Forms.ToolStripButton();
			this.buttonRemoveCollider = new System.Windows.Forms.ToolStripButton();
			this.propertyGrid = new System.Windows.Forms.PropertyGrid();
			this.toolStrip1.SuspendLayout();
			this.SuspendLayout();
			// 
			// panelTile
			// 
			this.panelTile.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.panelTile.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(84)))), ((int)(((byte)(138)))), ((int)(((byte)(150)))));
			this.panelTile.Location = new System.Drawing.Point(12, 168);
			this.panelTile.Name = "panelTile";
			this.panelTile.Size = new System.Drawing.Size(128, 128);
			this.panelTile.TabIndex = 0;
			this.panelTile.Paint += new System.Windows.Forms.PaintEventHandler(this.panelTile_Paint);
			// 
			// listBoxColliders
			// 
			this.listBoxColliders.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
			this.listBoxColliders.FormattingEnabled = true;
			this.listBoxColliders.IntegralHeight = false;
			this.listBoxColliders.Location = new System.Drawing.Point(12, 35);
			this.listBoxColliders.Name = "listBoxColliders";
			this.listBoxColliders.Size = new System.Drawing.Size(128, 127);
			this.listBoxColliders.TabIndex = 1;
			this.listBoxColliders.SelectedIndexChanged += new System.EventHandler(this.listBoxColliders_SelectedIndexChanged);
			// 
			// toolStrip1
			// 
			this.toolStrip1.AutoSize = false;
			this.toolStrip1.Dock = System.Windows.Forms.DockStyle.None;
			this.toolStrip1.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
			this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.buttonCreateCollider,
            this.buttonRemoveCollider});
			this.toolStrip1.Location = new System.Drawing.Point(12, 9);
			this.toolStrip1.Name = "toolStrip1";
			this.toolStrip1.RenderMode = System.Windows.Forms.ToolStripRenderMode.System;
			this.toolStrip1.Size = new System.Drawing.Size(128, 25);
			this.toolStrip1.TabIndex = 2;
			this.toolStrip1.Text = "toolStrip1";
			// 
			// buttonCreateCollider
			// 
			this.buttonCreateCollider.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.buttonCreateCollider.Image = global::NextMMO.Editor.Properties.Resources.AddRect_24x24;
			this.buttonCreateCollider.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.buttonCreateCollider.Name = "buttonCreateCollider";
			this.buttonCreateCollider.Size = new System.Drawing.Size(23, 22);
			this.buttonCreateCollider.Text = "toolStripButton1";
			this.buttonCreateCollider.Click += new System.EventHandler(this.buttonCreateCollider_Click);
			// 
			// buttonRemoveCollider
			// 
			this.buttonRemoveCollider.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.buttonRemoveCollider.Enabled = false;
			this.buttonRemoveCollider.Image = global::NextMMO.Editor.Properties.Resources.RemoveRect_24x24;
			this.buttonRemoveCollider.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.buttonRemoveCollider.Name = "buttonRemoveCollider";
			this.buttonRemoveCollider.Size = new System.Drawing.Size(23, 22);
			this.buttonRemoveCollider.Text = "toolStripButton2";
			this.buttonRemoveCollider.Click += new System.EventHandler(this.buttonRemoveCollider_Click);
			// 
			// propertyGrid
			// 
			this.propertyGrid.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.propertyGrid.Location = new System.Drawing.Point(146, 12);
			this.propertyGrid.Name = "propertyGrid";
			this.propertyGrid.Size = new System.Drawing.Size(173, 284);
			this.propertyGrid.TabIndex = 3;
			this.propertyGrid.PropertyValueChanged += new System.Windows.Forms.PropertyValueChangedEventHandler(this.propertyGrid_PropertyValueChanged);
			// 
			// FormTileEditor
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(331, 308);
			this.Controls.Add(this.propertyGrid);
			this.Controls.Add(this.toolStrip1);
			this.Controls.Add(this.listBoxColliders);
			this.Controls.Add(this.panelTile);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
			this.Name = "FormTileEditor";
			this.Text = "Tile Editor";
			this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.FormTileEditor_FormClosed);
			this.toolStrip1.ResumeLayout(false);
			this.toolStrip1.PerformLayout();
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.Panel panelTile;
		private System.Windows.Forms.ListBox listBoxColliders;
		private System.Windows.Forms.ToolStrip toolStrip1;
		private System.Windows.Forms.ToolStripButton buttonCreateCollider;
		private System.Windows.Forms.ToolStripButton buttonRemoveCollider;
		private System.Windows.Forms.PropertyGrid propertyGrid;
	}
}