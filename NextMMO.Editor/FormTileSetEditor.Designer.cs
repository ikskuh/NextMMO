namespace NextMMO
{
	partial class FormTileSetEditor
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormTileSetEditor));
			this.toolStrip1 = new System.Windows.Forms.ToolStrip();
			this.neuToolStripButton = new System.Windows.Forms.ToolStripButton();
			this.öffnenToolStripButton = new System.Windows.Forms.ToolStripButton();
			this.speichernToolStripButton = new System.Windows.Forms.ToolStripButton();
			this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
			this.panelTileSet = new System.Windows.Forms.Panel();
			this.textBoxName = new System.Windows.Forms.ToolStripTextBox();
			this.toolStrip1.SuspendLayout();
			this.SuspendLayout();
			// 
			// toolStrip1
			// 
			this.toolStrip1.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
			this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.neuToolStripButton,
            this.öffnenToolStripButton,
            this.speichernToolStripButton,
            this.toolStripSeparator1,
            this.textBoxName});
			this.toolStrip1.Location = new System.Drawing.Point(0, 0);
			this.toolStrip1.Name = "toolStrip1";
			this.toolStrip1.Size = new System.Drawing.Size(257, 25);
			this.toolStrip1.TabIndex = 2;
			this.toolStrip1.Text = "toolStrip1";
			// 
			// neuToolStripButton
			// 
			this.neuToolStripButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.neuToolStripButton.Image = ((System.Drawing.Image)(resources.GetObject("neuToolStripButton.Image")));
			this.neuToolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.neuToolStripButton.Name = "neuToolStripButton";
			this.neuToolStripButton.Size = new System.Drawing.Size(23, 22);
			this.neuToolStripButton.Text = "&Neu";
			this.neuToolStripButton.Click += new System.EventHandler(this.neuToolStripButton_Click);
			// 
			// öffnenToolStripButton
			// 
			this.öffnenToolStripButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.öffnenToolStripButton.Image = ((System.Drawing.Image)(resources.GetObject("öffnenToolStripButton.Image")));
			this.öffnenToolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.öffnenToolStripButton.Name = "öffnenToolStripButton";
			this.öffnenToolStripButton.Size = new System.Drawing.Size(23, 22);
			this.öffnenToolStripButton.Text = "Ö&ffnen";
			this.öffnenToolStripButton.Click += new System.EventHandler(this.öffnenToolStripButton_Click);
			// 
			// speichernToolStripButton
			// 
			this.speichernToolStripButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.speichernToolStripButton.Image = ((System.Drawing.Image)(resources.GetObject("speichernToolStripButton.Image")));
			this.speichernToolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.speichernToolStripButton.Name = "speichernToolStripButton";
			this.speichernToolStripButton.Size = new System.Drawing.Size(23, 22);
			this.speichernToolStripButton.Text = "&Speichern";
			this.speichernToolStripButton.Click += new System.EventHandler(this.speichernToolStripButton_Click);
			// 
			// toolStripSeparator1
			// 
			this.toolStripSeparator1.Name = "toolStripSeparator1";
			this.toolStripSeparator1.Size = new System.Drawing.Size(6, 25);
			// 
			// panelTileSet
			// 
			this.panelTileSet.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(84)))), ((int)(((byte)(138)))), ((int)(((byte)(150)))));
			this.panelTileSet.Location = new System.Drawing.Point(0, 25);
			this.panelTileSet.Margin = new System.Windows.Forms.Padding(0);
			this.panelTileSet.Name = "panelTileSet";
			this.panelTileSet.Size = new System.Drawing.Size(256, 706);
			this.panelTileSet.TabIndex = 3;
			this.panelTileSet.Paint += new System.Windows.Forms.PaintEventHandler(this.panelTileSet_Paint);
			this.panelTileSet.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.panelTileSet_MouseDoubleClick);
			// 
			// textBoxName
			// 
			this.textBoxName.Name = "textBoxName";
			this.textBoxName.Size = new System.Drawing.Size(100, 25);
			// 
			// TileSetEditor
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(257, 731);
			this.Controls.Add(this.panelTileSet);
			this.Controls.Add(this.toolStrip1);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
			this.Name = "TileSetEditor";
			this.Text = "TileSet Editor";
			this.toolStrip1.ResumeLayout(false);
			this.toolStrip1.PerformLayout();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.ToolStrip toolStrip1;
		private System.Windows.Forms.ToolStripButton neuToolStripButton;
		private System.Windows.Forms.ToolStripButton öffnenToolStripButton;
		private System.Windows.Forms.ToolStripButton speichernToolStripButton;
		private System.Windows.Forms.Panel panelTileSet;
		private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
		private System.Windows.Forms.ToolStripTextBox textBoxName;
	}
}