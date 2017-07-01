namespace Bau.Controls.WebExplorer
{
	partial class WebExplorer
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
			this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
			this.wbBrowser = new Bau.Controls.WebExplorer.WebExplorerExtended();
			this.tlbLinks = new Bau.Controls.WebExplorer.ToolBarsExplorer.ToolBarLinks();
			this.tableLayoutPanel1.SuspendLayout();
			this.SuspendLayout();
			// 
			// tableLayoutPanel1
			// 
			this.tableLayoutPanel1.ColumnCount = 1;
			this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.tableLayoutPanel1.Controls.Add(this.wbBrowser, 0, 1);
			this.tableLayoutPanel1.Controls.Add(this.tlbLinks, 0, 0);
			this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
			this.tableLayoutPanel1.Name = "tableLayoutPanel1";
			this.tableLayoutPanel1.RowCount = 2;
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 25F));
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.tableLayoutPanel1.Size = new System.Drawing.Size(734, 583);
			this.tableLayoutPanel1.TabIndex = 1;
			// 
			// wbBrowser
			// 
			this.wbBrowser.Dock = System.Windows.Forms.DockStyle.Fill;
			this.wbBrowser.Location = new System.Drawing.Point(3, 28);
			this.wbBrowser.MinimumSize = new System.Drawing.Size(20, 20);
			this.wbBrowser.Name = "wbBrowser";
			this.wbBrowser.Size = new System.Drawing.Size(728, 552);
			this.wbBrowser.TabIndex = 0;
			// 
			// tlbLinks
			// 
			this.tlbLinks.Dock = System.Windows.Forms.DockStyle.Fill;
			this.tlbLinks.Explorer = this.wbBrowser;
			this.tlbLinks.Location = new System.Drawing.Point(0, 0);
			this.tlbLinks.Margin = new System.Windows.Forms.Padding(0);
			this.tlbLinks.Name = "tlbLinks";
			this.tlbLinks.Size = new System.Drawing.Size(734, 25);
			this.tlbLinks.TabIndex = 1;
			// 
			// WebExplorer
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.Controls.Add(this.tableLayoutPanel1);
			this.Name = "WebExplorer";
			this.Size = new System.Drawing.Size(734, 583);
			this.tableLayoutPanel1.ResumeLayout(false);
			this.ResumeLayout(false);

		}

		#endregion

		private WebExplorerExtended wbBrowser;
		private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
		private Bau.Controls.WebExplorer.ToolBarsExplorer.ToolBarLinks tlbLinks;
	}
}
