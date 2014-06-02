namespace NextMMO
{
	partial class FormGame
	{
		/// <summary>
		/// Erforderliche Designervariable.
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary>
		/// Verwendete Ressourcen bereinigen.
		/// </summary>
		/// <param name="disposing">True, wenn verwaltete Ressourcen gelöscht werden sollen; andernfalls False.</param>
		protected override void Dispose(bool disposing)
		{
			if (disposing && (components != null))
			{
				components.Dispose();
			}
			base.Dispose(disposing);
		}

		#region Vom Windows Form-Designer generierter Code

		/// <summary>
		/// Erforderliche Methode für die Designerunterstützung.
		/// Der Inhalt der Methode darf nicht mit dem Code-Editor geändert werden.
		/// </summary>
		private void InitializeComponent()
		{
			this.components = new System.ComponentModel.Container();
			this.timerFramerate = new System.Windows.Forms.Timer(this.components);
			this.labelConnecting = new System.Windows.Forms.Label();
			this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
			this.debugToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.newSessionToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
			this.quitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.playerToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.nameToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.textBoxPlayerName = new System.Windows.Forms.ToolStripTextBox();
			this.setToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.spriteToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.lancerToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.mageToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.soldierToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.thiefToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.butlerToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.clericToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.fighterToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.contextMenuStrip1.SuspendLayout();
			this.SuspendLayout();
			// 
			// timerFramerate
			// 
			this.timerFramerate.Enabled = true;
			this.timerFramerate.Interval = 30;
			this.timerFramerate.Tick += new System.EventHandler(this.timerFramerate_Tick);
			// 
			// labelConnecting
			// 
			this.labelConnecting.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(192)))), ((int)(((byte)(192)))), ((int)(((byte)(255)))));
			this.labelConnecting.Dock = System.Windows.Forms.DockStyle.Fill;
			this.labelConnecting.Font = new System.Drawing.Font("Miramonte", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.labelConnecting.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(192)))), ((int)(((byte)(255)))));
			this.labelConnecting.Location = new System.Drawing.Point(0, 0);
			this.labelConnecting.Name = "labelConnecting";
			this.labelConnecting.Size = new System.Drawing.Size(659, 452);
			this.labelConnecting.TabIndex = 0;
			this.labelConnecting.Text = "Connecting...";
			this.labelConnecting.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// contextMenuStrip1
			// 
			this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.debugToolStripMenuItem,
            this.toolStripSeparator1,
            this.quitToolStripMenuItem,
            this.playerToolStripMenuItem});
			this.contextMenuStrip1.Name = "contextMenuStrip1";
			this.contextMenuStrip1.Size = new System.Drawing.Size(153, 98);
			// 
			// debugToolStripMenuItem
			// 
			this.debugToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.newSessionToolStripMenuItem});
			this.debugToolStripMenuItem.Name = "debugToolStripMenuItem";
			this.debugToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
			this.debugToolStripMenuItem.Text = "Debug";
			// 
			// newSessionToolStripMenuItem
			// 
			this.newSessionToolStripMenuItem.Name = "newSessionToolStripMenuItem";
			this.newSessionToolStripMenuItem.Size = new System.Drawing.Size(140, 22);
			this.newSessionToolStripMenuItem.Text = "New Session";
			this.newSessionToolStripMenuItem.Click += new System.EventHandler(this.newSessionToolStripMenuItem_Click);
			// 
			// toolStripSeparator1
			// 
			this.toolStripSeparator1.Name = "toolStripSeparator1";
			this.toolStripSeparator1.Size = new System.Drawing.Size(149, 6);
			// 
			// quitToolStripMenuItem
			// 
			this.quitToolStripMenuItem.Name = "quitToolStripMenuItem";
			this.quitToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
			this.quitToolStripMenuItem.Text = "Quit";
			this.quitToolStripMenuItem.Click += new System.EventHandler(this.quitToolStripMenuItem_Click);
			// 
			// playerToolStripMenuItem
			// 
			this.playerToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.nameToolStripMenuItem,
            this.spriteToolStripMenuItem});
			this.playerToolStripMenuItem.Name = "playerToolStripMenuItem";
			this.playerToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
			this.playerToolStripMenuItem.Text = "Player";
			// 
			// nameToolStripMenuItem
			// 
			this.nameToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.textBoxPlayerName,
            this.setToolStripMenuItem});
			this.nameToolStripMenuItem.Name = "nameToolStripMenuItem";
			this.nameToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
			this.nameToolStripMenuItem.Text = "Name";
			// 
			// textBoxPlayerName
			// 
			this.textBoxPlayerName.Name = "textBoxPlayerName";
			this.textBoxPlayerName.Size = new System.Drawing.Size(100, 23);
			// 
			// setToolStripMenuItem
			// 
			this.setToolStripMenuItem.Name = "setToolStripMenuItem";
			this.setToolStripMenuItem.Size = new System.Drawing.Size(160, 22);
			this.setToolStripMenuItem.Text = "Update";
			this.setToolStripMenuItem.Click += new System.EventHandler(this.SetPlayerName);
			// 
			// spriteToolStripMenuItem
			// 
			this.spriteToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.lancerToolStripMenuItem,
            this.mageToolStripMenuItem,
            this.soldierToolStripMenuItem,
            this.thiefToolStripMenuItem,
            this.butlerToolStripMenuItem,
            this.clericToolStripMenuItem,
            this.fighterToolStripMenuItem});
			this.spriteToolStripMenuItem.Name = "spriteToolStripMenuItem";
			this.spriteToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
			this.spriteToolStripMenuItem.Text = "Sprite";
			// 
			// lancerToolStripMenuItem
			// 
			this.lancerToolStripMenuItem.Name = "lancerToolStripMenuItem";
			this.lancerToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
			this.lancerToolStripMenuItem.Text = "Lancer";
			this.lancerToolStripMenuItem.Click += new System.EventHandler(this.SetCharacterSprite);
			// 
			// mageToolStripMenuItem
			// 
			this.mageToolStripMenuItem.Name = "mageToolStripMenuItem";
			this.mageToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
			this.mageToolStripMenuItem.Text = "Mage";
			this.mageToolStripMenuItem.Click += new System.EventHandler(this.SetCharacterSprite);
			// 
			// soldierToolStripMenuItem
			// 
			this.soldierToolStripMenuItem.Name = "soldierToolStripMenuItem";
			this.soldierToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
			this.soldierToolStripMenuItem.Text = "Soldier";
			this.soldierToolStripMenuItem.Click += new System.EventHandler(this.SetCharacterSprite);
			// 
			// thiefToolStripMenuItem
			// 
			this.thiefToolStripMenuItem.Name = "thiefToolStripMenuItem";
			this.thiefToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
			this.thiefToolStripMenuItem.Text = "Thief";
			this.thiefToolStripMenuItem.Click += new System.EventHandler(this.SetCharacterSprite);
			// 
			// butlerToolStripMenuItem
			// 
			this.butlerToolStripMenuItem.Name = "butlerToolStripMenuItem";
			this.butlerToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
			this.butlerToolStripMenuItem.Text = "Butler";
			this.butlerToolStripMenuItem.Click += new System.EventHandler(this.SetCharacterSprite);
			// 
			// clericToolStripMenuItem
			// 
			this.clericToolStripMenuItem.Name = "clericToolStripMenuItem";
			this.clericToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
			this.clericToolStripMenuItem.Text = "Cleric";
			this.clericToolStripMenuItem.Click += new System.EventHandler(this.SetCharacterSprite);
			// 
			// fighterToolStripMenuItem
			// 
			this.fighterToolStripMenuItem.Name = "fighterToolStripMenuItem";
			this.fighterToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
			this.fighterToolStripMenuItem.Text = "Fighter";
			this.fighterToolStripMenuItem.Click += new System.EventHandler(this.SetCharacterSprite);
			// 
			// FormGame
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.BackColor = System.Drawing.Color.Black;
			this.ClientSize = new System.Drawing.Size(659, 452);
			this.ContextMenuStrip = this.contextMenuStrip1;
			this.Controls.Add(this.labelConnecting);
			this.DoubleBuffered = true;
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
			this.Name = "FormGame";
			this.Text = "NextMMO";
			this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FormGame_FormClosing);
			this.Paint += new System.Windows.Forms.PaintEventHandler(this.FormGame_Paint);
			this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.FormGame_KeyDown);
			this.KeyUp += new System.Windows.Forms.KeyEventHandler(this.FormGame_KeyUp);
			this.contextMenuStrip1.ResumeLayout(false);
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.Timer timerFramerate;
		private System.Windows.Forms.Label labelConnecting;
		private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
		private System.Windows.Forms.ToolStripMenuItem debugToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem newSessionToolStripMenuItem;
		private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
		private System.Windows.Forms.ToolStripMenuItem quitToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem playerToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem nameToolStripMenuItem;
		private System.Windows.Forms.ToolStripTextBox textBoxPlayerName;
		private System.Windows.Forms.ToolStripMenuItem setToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem spriteToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem lancerToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem mageToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem soldierToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem thiefToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem butlerToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem clericToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem fighterToolStripMenuItem;

	}
}

