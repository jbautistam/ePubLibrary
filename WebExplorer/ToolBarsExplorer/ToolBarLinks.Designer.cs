namespace Bau.Controls.WebExplorer.ToolBarsExplorer
{
	partial class ToolBarLinks
	{
		/// <summary> 
		/// Variable del diseñador requerida.
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary> 
		/// Limpiar los recursos que se estén utilizando.
		/// </summary>
		/// <param name="disposing">true si los recursos administrados se deben eliminar; false en caso contrario, false.</param>
		protected override void Dispose(bool disposing)
		{
			if (disposing && (components != null))
			{
				components.Dispose();
			}
			base.Dispose(disposing);
		}

		#region Código generado por el Diseñador de componentes

		/// <summary> 
		/// Método necesario para admitir el Diseñador. No se puede modificar
		/// el contenido del método con el editor de código.
		/// </summary>
		private void InitializeComponent()
		{
			this.tlbLinks = new System.Windows.Forms.ToolStrip();
			this.cmdPrevious = new System.Windows.Forms.ToolStripButton();
			this.cmdNext = new System.Windows.Forms.ToolStripButton();
			this.cboURL = new System.Windows.Forms.ToolStripComboBox();
			this.cmdGo = new System.Windows.Forms.ToolStripButton();
			this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
			this.cmdRefresh = new System.Windows.Forms.ToolStripButton();
			this.cmdStop = new System.Windows.Forms.ToolStripButton();
			this.tlbLinks.SuspendLayout();
			this.SuspendLayout();
			// 
			// tlbLinks
			// 
			this.tlbLinks.Dock = System.Windows.Forms.DockStyle.Fill;
			this.tlbLinks.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
			this.tlbLinks.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.cmdPrevious,
            this.cmdNext,
            this.cboURL,
            this.cmdGo,
            this.toolStripSeparator1,
            this.cmdRefresh,
            this.cmdStop});
			this.tlbLinks.Location = new System.Drawing.Point(0, 0);
			this.tlbLinks.Name = "tlbLinks";
			this.tlbLinks.Size = new System.Drawing.Size(641, 23);
			this.tlbLinks.TabIndex = 2;
			this.tlbLinks.Text = "toolStrip1";
			this.tlbLinks.SizeChanged += new System.EventHandler(this.tlbLinks_SizeChanged);
			// 
			// cmdPrevious
			// 
			this.cmdPrevious.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.cmdPrevious.Image = global::Bau.Controls.WebExplorer.Properties.Resources.arrow_left;
			this.cmdPrevious.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.cmdPrevious.Name = "cmdPrevious";
			this.cmdPrevious.Size = new System.Drawing.Size(23, 20);
			this.cmdPrevious.Text = "Anterior";
			this.cmdPrevious.Click += new System.EventHandler(this.cmdPrevious_Click);
			// 
			// cmdNext
			// 
			this.cmdNext.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.cmdNext.Image = global::Bau.Controls.WebExplorer.Properties.Resources.arrow_right;
			this.cmdNext.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.cmdNext.Name = "cmdNext";
			this.cmdNext.Size = new System.Drawing.Size(23, 20);
			this.cmdNext.Text = "Siguiente";
			this.cmdNext.Click += new System.EventHandler(this.cmdNext_Click);
			// 
			// cboURL
			// 
			this.cboURL.Name = "cboURL";
			this.cboURL.Size = new System.Drawing.Size(200, 23);
			this.cboURL.DropDown += new System.EventHandler(this.cboURL_DropDown);
			this.cboURL.SelectedIndexChanged += new System.EventHandler(this.cboURL_SelectedIndexChanged);
			this.cboURL.KeyDown += new System.Windows.Forms.KeyEventHandler(this.cboURL_KeyDown);
			// 
			// cmdGo
			// 
			this.cmdGo.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.cmdGo.Image = global::Bau.Controls.WebExplorer.Properties.Resources.resultset_next;
			this.cmdGo.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.cmdGo.Name = "cmdGo";
			this.cmdGo.Size = new System.Drawing.Size(23, 20);
			this.cmdGo.Text = "Ir";
			this.cmdGo.Click += new System.EventHandler(this.cmdGo_Click);
			// 
			// toolStripSeparator1
			// 
			this.toolStripSeparator1.Name = "toolStripSeparator1";
			this.toolStripSeparator1.Size = new System.Drawing.Size(6, 23);
			// 
			// cmdRefresh
			// 
			this.cmdRefresh.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.cmdRefresh.Image = global::Bau.Controls.WebExplorer.Properties.Resources.page_refresh;
			this.cmdRefresh.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.cmdRefresh.Name = "cmdRefresh";
			this.cmdRefresh.Size = new System.Drawing.Size(23, 20);
			this.cmdRefresh.Text = "Actualizar";
			this.cmdRefresh.Click += new System.EventHandler(this.cmdRefresh_Click);
			// 
			// cmdStop
			// 
			this.cmdStop.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.cmdStop.Image = global::Bau.Controls.WebExplorer.Properties.Resources.cross;
			this.cmdStop.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.cmdStop.Name = "cmdStop";
			this.cmdStop.Size = new System.Drawing.Size(23, 20);
			this.cmdStop.Text = "Detener";
			this.cmdStop.Click += new System.EventHandler(this.cmdStop_Click);
			// 
			// ToolBarLinks
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.Controls.Add(this.tlbLinks);
			this.Name = "ToolBarLinks";
			this.Size = new System.Drawing.Size(641, 23);
			this.tlbLinks.ResumeLayout(false);
			this.tlbLinks.PerformLayout();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.ToolStrip tlbLinks;
		private System.Windows.Forms.ToolStripButton cmdPrevious;
		private System.Windows.Forms.ToolStripButton cmdNext;
		private System.Windows.Forms.ToolStripComboBox cboURL;
		private System.Windows.Forms.ToolStripButton cmdGo;
		private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
		private System.Windows.Forms.ToolStripButton cmdRefresh;
		private System.Windows.Forms.ToolStripButton cmdStop;
	}
}
