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
			// FormGame
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.BackColor = System.Drawing.Color.Black;
			this.ClientSize = new System.Drawing.Size(659, 452);
			this.Controls.Add(this.labelConnecting);
			this.DoubleBuffered = true;
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
			this.Name = "FormGame";
			this.Text = "NextMMO";
			this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FormGame_FormClosing);
			this.Paint += new System.Windows.Forms.PaintEventHandler(this.FormGame_Paint);
			this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.FormGame_KeyDown);
			this.KeyUp += new System.Windows.Forms.KeyEventHandler(this.FormGame_KeyUp);
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.Timer timerFramerate;
		private System.Windows.Forms.Label labelConnecting;

	}
}

