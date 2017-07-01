using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace Bau.Controls.WebExplorer.ToolBarsExplorer
{
	/// <summary>
	///		Toolbar con vínculos
	/// </summary>
	public partial class ToolBarLinks : UserControl
	{ // Enumerados privados
			private enum ToolBarAction
				{ GoPrevious,
					GoForward,
					ChangeUrl,
					Refresh,
					Stop
				}
		// Variables privadas
			private WebExplorerExtended objWebExplorer = null;
			
		public ToolBarLinks()
		{	InitializeComponent();
		}

		/// <summary>
		///		Modifica el estado de los botones de la barra
		/// </summary>
		private void UpdateStatus()
		{ if (Explorer != null)
				{ cmdGo.Enabled = !string.IsNullOrEmpty(cboURL.Text);
					cmdNext.Enabled = Explorer.CanGoForward;
					cmdPrevious.Enabled = Explorer.CanGoBack;
					cmdRefresh.Enabled = true;
					cmdStop.Enabled = true;
				}
			else
				{ cmdGo.Enabled = false;
					cmdNext.Enabled = false;
					cmdPrevious.Enabled = false;
					cmdRefresh.Enabled = false;
					cmdStop.Enabled = false;
				}
		}
		/// <summary>
		///		Cuando se navega a una posición se añade el vínculo a la colección y se muestra en la dirección
		/// </summary>
		private void ShowLink(Uri uriURL)
		{ cboURL.Text = uriURL.ToString();
		}
		
		/// <summary>
		///		Carga el combo de vínculos
		/// </summary>
		private void LoadComboLinks()
		{ // Limpia el combo
				cboURL.Items.Clear();
			// Añade los vínculos
				foreach (Common.Link objLink in Common.Globals.LastLinks)
					cboURL.Items.Add(objLink.URL);
		}

		/// <summary>
		///		Cambia la URL
		/// </summary>
		private void ChangeURL()
		{	if (!string.IsNullOrEmpty(cboURL.Text))
				ExecuteAction(ToolBarAction.ChangeUrl, cboURL.Text);
		}
		
		/// <summary>
		///		Lanza un evento
		/// </summary>
		private void ExecuteAction(ToolBarAction intAction, string url)
		{ if (Explorer != null)
				switch (intAction)
					{ case ToolBarAction.ChangeUrl:
								if (!string.IsNullOrEmpty(url))
									Explorer.LoadURL(url);
							break;
						case ToolBarAction.GoForward:
								Explorer.GoForward();
							break;
						case ToolBarAction.GoPrevious:
								Explorer.GoBack();
							break;
						case ToolBarAction.Refresh:
								Explorer.Refresh();
							break;
						case ToolBarAction.Stop:
								Explorer.Stop();
							break;
						default:
							throw new NotImplementedException();
					}
		}
		
		/// <summary>
		///		Explorador asociado a la barra de herramientas
		/// </summary>
		[Browsable(true), Description("Explorador al que se asocia la barra de herramientas")]
		public WebExplorerExtended Explorer
		{ get { return objWebExplorer; }
			set 
				{ // Asinga el valor del explorador
						objWebExplorer = value;
					// Asigna los manejadores de eventos
						objWebExplorer.ChangeStatus += new EventHandler(objWebExplorer_ChangeStatus);
						objWebExplorer.ChangeNavigationStatus += new EventHandler<WebExplorerNavigationEventArgs>(objWebExplorer_ChangeNavigationStatus);
						objWebExplorer.Navigated += new WebBrowserNavigatedEventHandler(objWebExplorer_Navigated);
				}
		}
		
		private void cmdPrevious_Click(object sender, EventArgs e)
		{ ExecuteAction(ToolBarAction.GoPrevious, null);
		}

		private void cmdNext_Click(object sender, EventArgs e)
		{ ExecuteAction(ToolBarAction.GoForward, null);
		}

		private void cmdGo_Click(object sender, EventArgs e)
		{ ChangeURL();
		}

		private void cmdRefresh_Click(object sender, EventArgs e)
		{ ExecuteAction(ToolBarAction.Refresh, null);
		}

		private void cmdStop_Click(object sender, EventArgs e)
		{ ExecuteAction(ToolBarAction.Stop, null);
		}

		private void objWebExplorer_Navigated(object sender, WebBrowserNavigatedEventArgs e)
		{ ShowLink(e.Url);
		}

		private void objWebExplorer_ChangeStatus(object sender, EventArgs e)
		{ UpdateStatus();
		}

		private void objWebExplorer_ChangeNavigationStatus(object sender, WebExplorerNavigationEventArgs e)
		{ if (!string.IsNullOrEmpty(e.URL))
				ShowLink(new Uri(e.URL));
		}

		private void cboURL_SelectedIndexChanged(object sender, EventArgs e)
		{ ChangeURL();
		}

		private void cboURL_DropDown(object sender, EventArgs e)
		{ LoadComboLinks();
		}

		private void tlbLinks_SizeChanged(object sender, EventArgs e)
		{ cboURL.Size = new Size(tlbLinks.Width * 70 / 100, cboURL.Height);
		}

		private void cboURL_KeyDown(object sender, KeyEventArgs e)
		{ if (e.KeyCode == Keys.Enter)
				ChangeURL();
		}
	}
}
