namespace Bau.Applications.ePub
{
	partial class frmMain
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

		#region Código generado por el Diseñador de Windows Forms

		/// <summary>
		/// Método necesario para admitir el Diseñador. No se puede modificar
		/// el contenido del método con el editor de código.
		/// </summary>
		private void InitializeComponent()
		{
			this.cmdParse = new System.Windows.Forms.Button();
			this.label1 = new System.Windows.Forms.Label();
			this.fnEBook = new Bau.Controls.Files.TextBoxSelectFile();
			this.collapsiblePanelSplitter1 = new Bau.Controls.Split.CollapsiblePanelSplitter();
			this.trvPages = new Bau.Controls.Tree.TreeViewExtended();
			this.wbExplorer = new Bau.Controls.WebExplorer.WebExplorer();
			((System.ComponentModel.ISupportInitialize)(this.collapsiblePanelSplitter1)).BeginInit();
			this.collapsiblePanelSplitter1.Panel1.SuspendLayout();
			this.collapsiblePanelSplitter1.Panel2.SuspendLayout();
			this.collapsiblePanelSplitter1.SuspendLayout();
			this.SuspendLayout();
			// 
			// cmdParse
			// 
			this.cmdParse.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.cmdParse.Location = new System.Drawing.Point(674, 5);
			this.cmdParse.Name = "cmdParse";
			this.cmdParse.Size = new System.Drawing.Size(72, 28);
			this.cmdParse.TabIndex = 0;
			this.cmdParse.Text = "Interpretar";
			this.cmdParse.UseVisualStyleBackColor = true;
			this.cmdParse.Click += new System.EventHandler(this.cmdParse_Click);
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(192)))));
			this.label1.Location = new System.Drawing.Point(8, 13);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(46, 13);
			this.label1.TabIndex = 1;
			this.label1.Text = "Archivo:";
			// 
			// fnEBook
			// 
			this.fnEBook.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.fnEBook.BackColorEdit = System.Drawing.SystemColors.Window;
			this.fnEBook.FileName = "C:\\Documents and Settings\\jbautistam\\Mis documentos\\Visual Studio 2008\\Projects\\L" +
    "ibrerias\\ePub\\Samples\\Don Quijote.epub";
			this.fnEBook.Filter = "Archivos ePub (*.ePub)|*.epub|Todos los archivos|*.*";
			this.fnEBook.Location = new System.Drawing.Point(68, 9);
			this.fnEBook.Margin = new System.Windows.Forms.Padding(0);
			this.fnEBook.MaximumSize = new System.Drawing.Size(10000, 20);
			this.fnEBook.MinimumSize = new System.Drawing.Size(200, 20);
			this.fnEBook.Name = "fnEBook";
			this.fnEBook.Size = new System.Drawing.Size(600, 20);
			this.fnEBook.TabIndex = 2;
			this.fnEBook.Type = Bau.Controls.Files.TextBoxSelectFile.FileSelectType.Load;
			// 
			// collapsiblePanelSplitter1
			// 
			this.collapsiblePanelSplitter1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.collapsiblePanelSplitter1.BackColorSplitter = System.Drawing.SystemColors.Control;
			this.collapsiblePanelSplitter1.CollapseAction = Bau.Controls.Split.CollapsiblePanelSplitter.CollapseMode.CollapsePanel1;
			this.collapsiblePanelSplitter1.Location = new System.Drawing.Point(2, 35);
			this.collapsiblePanelSplitter1.Name = "collapsiblePanelSplitter1";
			// 
			// collapsiblePanelSplitter1.Panel1
			// 
			this.collapsiblePanelSplitter1.Panel1.Controls.Add(this.trvPages);
			this.collapsiblePanelSplitter1.Panel1MinSize = 0;
			// 
			// collapsiblePanelSplitter1.Panel2
			// 
			this.collapsiblePanelSplitter1.Panel2.Controls.Add(this.wbExplorer);
			this.collapsiblePanelSplitter1.Panel2MinSize = 0;
			this.collapsiblePanelSplitter1.Size = new System.Drawing.Size(746, 403);
			this.collapsiblePanelSplitter1.SplitterDistance = 247;
			this.collapsiblePanelSplitter1.SplitterStyle = Bau.Controls.Split.CollapsiblePanelSplitter.VisualStyles.Mozilla;
			this.collapsiblePanelSplitter1.SplitterWidth = 8;
			this.collapsiblePanelSplitter1.TabIndex = 3;
			// 
			// trvPages
			// 
			this.trvPages.CheckRecursive = false;
			this.trvPages.Dock = System.Windows.Forms.DockStyle.Fill;
			this.trvPages.Location = new System.Drawing.Point(0, 0);
			this.trvPages.Name = "trvPages";
			this.trvPages.ShowNodeToolTips = true;
			this.trvPages.Size = new System.Drawing.Size(247, 403);
			this.trvPages.TabIndex = 0;
			this.trvPages.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.trvPages_AfterSelect);
			// 
			// wbExplorer
			// 
			this.wbExplorer.Dock = System.Windows.Forms.DockStyle.Fill;
			this.wbExplorer.Location = new System.Drawing.Point(0, 0);
			this.wbExplorer.Name = "wbExplorer";
			this.wbExplorer.ScriptErrorsSuppressed = false;
			this.wbExplorer.Size = new System.Drawing.Size(491, 403);
			this.wbExplorer.TabIndex = 0;
			// 
			// frmMain
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(752, 439);
			this.Controls.Add(this.collapsiblePanelSplitter1);
			this.Controls.Add(this.fnEBook);
			this.Controls.Add(this.label1);
			this.Controls.Add(this.cmdParse);
			this.Name = "frmMain";
			this.Text = "Lector de ePubs";
			this.collapsiblePanelSplitter1.Panel1.ResumeLayout(false);
			this.collapsiblePanelSplitter1.Panel2.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.collapsiblePanelSplitter1)).EndInit();
			this.collapsiblePanelSplitter1.ResumeLayout(false);
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.Button cmdParse;
		private System.Windows.Forms.Label label1;
		private Bau.Controls.Files.TextBoxSelectFile fnEBook;
		private Bau.Controls.Split.CollapsiblePanelSplitter collapsiblePanelSplitter1;
		private Bau.Controls.Tree.TreeViewExtended trvPages;
		private Bau.Controls.WebExplorer.WebExplorer wbExplorer;
	}
}

