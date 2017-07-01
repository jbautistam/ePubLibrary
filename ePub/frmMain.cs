using System;
using System.IO;
using System.Windows.Forms;

using Bau.Libraries.LibEBook;
using Bau.Libraries.LibEBook.Formats.ePub;
using Bau.Libraries.LibEBook.Formats.ePub.Container;
using Bau.Libraries.LibEBook.Formats.ePub.OPF;
using Bau.Libraries.LibEBook.Formats.ePub.NCX;

namespace Bau.Applications.ePub
{
	/// <summary>
	///		Formulario principal para la visualización de un eBook
	/// </summary>
	public partial class frmMain : Form
	{ 
		// Enumerados privados
		private enum KeyNode
		{
			Package,
			Item,
			NcxFile,
			NavPoint
		}
		// Variables privadas
		private ePubEBook book = new ePubEBook();

		public frmMain()
		{
			InitializeComponent();
		}

		/// <summary>
		///		Interpreta un libro
		/// </summary>
		private void Parse()
		{
			string path = Path.Combine(Path.GetTempPath(), Path.GetFileNameWithoutExtension(fnEBook.FileName));

				// Carga el libro
				book = new BookFactory().Load(fnEBook.FileName, path);
				// Limpia el árbol
				trvPages.Nodes.Clear();
				// Muestra las páginas en el árbol
				foreach (RootFile rootFile in book.Container.RootFiles)
					foreach (OPFPackage package in rootFile.Packages)
					{
						TreeNode trnNode = AddNode(null, KeyNode.Package, package.Metadata.Title, null);
						TreeNode trnNodeSpine = AddNode(trnNode, KeyNode.Package, "Spine", null);
						TreeNode trnNodeNCX = AddNode(trnNode, KeyNode.Package, "NCX", null);

							// Añade los elementos del spine
							foreach (ItemRef itemRef in package.Spine)
							{
								Item item = package.Manifest.Search(itemRef.IDRef);

									if (item != null)
										AddNode(trnNodeSpine, KeyNode.Item, item.URL, Path.Combine(path, item.URL));
							}
							// Añade los elementos del NCX
							foreach (NCXFile ncx in package.TablesOfContents)
							{
								TreeNode trnNCX = AddNode(trnNodeNCX, KeyNode.NcxFile, ncx.Title, ncx.ID);

									AddNavPoints(trnNCX, ncx.Pages,
												 Path.Combine(path, Path.GetDirectoryName(book.Container.RootFiles[0].URL)));
							}
					}
		}

		/// <summary>
		///		Añade un nodo al árbol
		/// </summary>
		private TreeNode AddNode(TreeNode trnParent, KeyNode type, string title, string tag)
		{
			return trvPages.AddNode(trnParent, new Controls.Tree.TreeNodeKey((int) type, null, tag), title, false);
		}

		/// <summary>
		///		Añade las páginas al árbol
		/// </summary>
		private void AddNavPoints(TreeNode trnParent, NavPointsCollection navPoints, string path)
		{
			foreach (NavPoint navPoint in navPoints)
			{
				TreeNode trnNode = AddNode(trnParent, KeyNode.NavPoint, navPoint.Title,
																	   Path.Combine(path, navPoint.URL));

					if (navPoint.Pages.Count > 0)
						AddNavPoints(trnNode, navPoint.Pages, path);
			}
		}

		/// <summary>
		///		Carga los datos de una página
		/// </summary>
		private void LoadPage(Controls.Tree.TreeNodeKey objKey)
		{
			if (objKey.IDType == (int) KeyNode.NavPoint && objKey.Tag != null && objKey.Tag is string)
				wbExplorer.LoadURL(objKey.Tag as string);
			else if (objKey.IDType == (int) KeyNode.Item && objKey.Tag != null && objKey.Tag is string)
				wbExplorer.LoadURL(objKey.Tag as string);
		}

		private void cmdParse_Click(object sender, EventArgs e)
		{
			Parse();
		}

		private void trvPages_AfterSelect(object sender, TreeViewEventArgs e)
		{
			if (e.Node != null && e.Node.Tag != null)
			{
				Controls.Tree.TreeNodeKey objKey = trvPages.GetKey(e.Node);

					if (objKey != null)
						LoadPage(objKey);
			}
		}
	}
}