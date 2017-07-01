using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace Bau.Controls.WebExplorer
{
	/// <summary>
	///		Control para exploración Web
	/// </summary>
	public partial class WebExplorer : UserControl
	{ // Eventos publicos
			public event EventHandler<WebExplorerNavigationEventArgs> ChangeNavigationStatus;
			public event EventHandler ChangeStatus;
			public event EventHandler<WebExplorerProgressEventArgs> ChangeProgress;
			public event EventHandler<WebExplorerJavaScriptFunctionEventArgs> JavaScriptFunctionCalled;
			
		public WebExplorer()
		{	// Inicializa el componente
				InitializeComponent();
			// Añade los manejadores de eventos
				wbBrowser.ChangeNavigationStatus += new EventHandler<WebExplorerNavigationEventArgs>(wbBrowser_ChangeNavigationStatus);
				wbBrowser.ChangeProgress += new EventHandler<WebExplorerProgressEventArgs>(wbBrowser_ChangeProgress);
				wbBrowser.ChangeStatus += new EventHandler(wbBrowser_ChangeStatus);
				wbBrowser.JavaScriptFunctionCalled += new EventHandler<WebExplorerJavaScriptFunctionEventArgs>(wbBrowser_JavaScriptFunctionCalled);
		}
		
		/// <summary>
		///		Carga el browser a partir de una cadena HTML
		/// </summary>
		public void LoadHTML(string strHTML)
		{ wbBrowser.DocumentText = strHTML;
		}
		
		/// <summary>
		///		Carga el browser a partir de una URL
		/// </summary>
		public void LoadURL(string url)
		{ wbBrowser.Navigate(url);
		}

		/// <summary>
		///		Actualiza el explorador
		/// </summary>
		public void DocumentRefresh()
		{ wbBrowser.Refresh();
		}
		
		/// <summary>
		///		Salta a la página anterior
		/// </summary>
		public void GoBack()
		{ wbBrowser.GoBack();
		}
		
		/// <summary>
		///		Salta a la página siguiente
		/// </summary>
		public void GoForward()
		{ wbBrowser.GoForward();
		}
		
		/// <summary>
		///		Salta a la página de inicio
		/// </summary>
		public void GoHome()
		{ wbBrowser.GoHome();
		}
		
		
		/// <summary>
		///		Muestra el cuadro de diálogo "Guardar como"
		/// </summary>
		public void ShowSaveDialog()
		{ wbBrowser.ShowSaveAsDialog();
		}
		
		/// <summary>
		///		Muestra el cuadro de diálogo de configuración de página
		/// </summary>
		public void ShowPageSetupDialog()
		{	wbBrowser.ShowPageSetupDialog();
		}
		
		/// <summary>
		///		Muestra el cuadro de diálogo de impresión
		/// </summary>
		public void ShowPrintDialog()
		{ wbBrowser.ShowPrintDialog();
		}
		
		/// <summary>
		///		Muestra el cuadro de diálogo de previsualización
		/// </summary>
		public void ShowPrintPreviewDialog()
		{ wbBrowser.ShowPrintPreviewDialog();
		}
		
		/// <summary>
		///		Muestra el cuadro de diálogo de propiedades
		/// </summary>
		public void ShowPropertiesDialog()
		{ wbBrowser.ShowPropertiesDialog();
		}
		
		/// <summary>
		///		Lanza un evento
		/// </summary>
		private bool RaiseEventChangeNavigationStatus(WebExplorerNavigationEventArgs.ExplorerAction intAction, 
																									string url)
		{ bool blnCancel = false;
		
			// Lanza el evento de cambio de estado
				if (ChangeNavigationStatus != null)
					{ WebExplorerNavigationEventArgs objEvent = new WebExplorerNavigationEventArgs(intAction, url);
						
							// Lanza el evento
								ChangeNavigationStatus(this, objEvent);
							// Guarda el valor de cancelación
								blnCancel = objEvent.Cancel;
					}
			// Devuelve el valor que indica si se debe cancelar
				return blnCancel;
		}
		
		/// <summary>
		///		Lanza el evento de progreso
		/// </summary>
		private void RaiseEventChangeProgress(long lngProgress, long lngTotal)
		{ if (lngProgress > 0 && ChangeProgress != null)
				ChangeProgress(this, new WebExplorerProgressEventArgs(lngProgress, lngTotal));
		}
		
		/// <summary>
		///		Lanza el evento de cambio de estado
		/// </summary>
		private void RaiseEventChangeStatus()
		{ if (ChangeStatus != null)
				ChangeStatus(this, EventArgs.Empty);
		}
				
		/// <summary>
		///		Indica si puede ir hacia atrás
		/// </summary>
		public bool CanGoBack
		{ get { return wbBrowser.CanGoBack; }
		}
		
		/// <summary>
		///		Indica si puede ir hacia delante
		/// </summary>
		public bool CanGoForward
		{ get { return wbBrowser.CanGoForward; }
		}
		
		/// <summary>
		///		Indica si se ha cambiado el estado
		/// </summary>
		public string Status
		{ get { return wbBrowser.StatusText; }
		}

		/// <summary>
		///		Indica si se deben mostrar mensajes con los errores de JavaScript
		/// </summary>
		public bool ScriptErrorsSuppressed
		{ get { return wbBrowser.ScriptErrorsSuppressed; }
			set { wbBrowser.ScriptErrorsSuppressed = value; }
		}
		
		private void wbBrowser_ChangeStatus(object sender, EventArgs e)
		{ RaiseEventChangeStatus();
		}

		private void wbBrowser_ChangeProgress(object sender, WebExplorerProgressEventArgs e)
		{ RaiseEventChangeProgress(e.Progress, e.Total);
		}

		private void wbBrowser_ChangeNavigationStatus(object sender, WebExplorerNavigationEventArgs e)
		{ e.Cancel = RaiseEventChangeNavigationStatus(e.Action, e.URL);
		}

		private void wbBrowser_JavaScriptFunctionCalled(object sender, WebExplorerJavaScriptFunctionEventArgs e)
		{ if (JavaScriptFunctionCalled != null)
				JavaScriptFunctionCalled(this, e);
		}

/*
        #region FAVICON
       
        // favicon
        public static Image favicon(String u, string file)
        {
                Uri url = new Uri(u);
                String iconurl = "http://" + url.Host + "/favicon.ico";

                WebRequest request = WebRequest.Create(iconurl);
                try
                {
                    WebResponse response = request.GetResponse();

                    Stream s = response.GetResponseStream();
                    return Image.FromStream(s);
                }
                catch (Exception ex)
                {
                    return Image.FromFile(file);
                }
            
           
        }
        //favicon index
        private int faviconIndex(string url)
        {
            Uri key = new Uri(url);
            if (!imgList.Images.ContainsKey(key.Host.ToString()))
                imgList.Images.Add(key.Host.ToString(), favicon(url, "link.png"));
            return imgList.Images.IndexOfKey(key.Host.ToString());
        }
        //getFavicon from key
        private Image getFavicon(string key)
        {
            Uri url = new Uri(key);
            if (!imgList.Images.ContainsKey(url.Host.ToString()))
                imgList.Images.Add(url.Host.ToString(), favicon(key
                    , "link.png"));
            return imgList.Images[url.Host.ToString()];
        }
        #endregion

*/
	}
}