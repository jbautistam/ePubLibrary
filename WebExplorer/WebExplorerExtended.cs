using System;

namespace Bau.Controls.WebExplorer
{
	/// <summary>
	///		Control derivado de WebBrowser
	/// </summary>
	[System.Security.Permissions.PermissionSet(System.Security.Permissions.SecurityAction.Demand, Name="FullTrust")]
	[System.Runtime.InteropServices.ComVisibleAttribute(true)]
	public class WebExplorerExtended : System.Windows.Forms.WebBrowser
	{ // Eventos publicos
			public event EventHandler<WebExplorerNavigationEventArgs> ChangeNavigationStatus;
			public event EventHandler ChangeStatus;
			public event EventHandler<WebExplorerProgressEventArgs> ChangeProgress;
			public event EventHandler<WebExplorerJavaScriptFunctionEventArgs> JavaScriptFunctionCalled;
		
		public WebExplorerExtended()
		{ ObjectForScripting = this;
		}
		
		/// <summary>
		///		Carga el browser a partir de una cadena HTML
		/// </summary>
		public void LoadHTML(string strHTML)
		{ DocumentText = strHTML;
		}
		
		/// <summary>
		///		Carga el browser a partir de una URL
		/// </summary>
		public void LoadURL(string url)
		{ Navigate(url);
		}
		
		/// <summary>
		///		Trata los mensajes lanzados desde el javaScript de la página
		/// </summary>
		public void ScriptMessageHandler(string strMessage)
		{ if (JavaScriptFunctionCalled != null)
				JavaScriptFunctionCalled(this, new WebExplorerJavaScriptFunctionEventArgs(strMessage));
		}
		
		/// <summary>
		///		Ejecuta un script
		/// </summary>
		public void ExecuteScript(string strScriptName, object [] objArgs)
		{ Document.InvokeScript(strScriptName, objArgs);
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
							// Si la acción es que se ha cargado un documento, añade la URL a la colección de elementos cargados
								if (!blnCancel && intAction == WebExplorerNavigationEventArgs.ExplorerAction.Navigated && !string.IsNullOrEmpty(url))
									Common.Globals.AddLink(url);
						}
				// Devuelve el valor que indica si se debe cancelar
					return blnCancel;
		}
		
		/// <summary>
		///		Lanza el evento de progreso
		/// </summary>
		private void RaiseEventChangeProgress(long lngProgress, long lngTotal)
		{ if (ChangeProgress != null)
				ChangeProgress(this, new WebExplorerProgressEventArgs(lngProgress, lngTotal));
		}
		
		/// <summary>
		///		Lanza el evento de cambio de estado
		/// </summary>
		private void RaiseEventChangeStatus()
		{ if (ChangeStatus != null)
				ChangeStatus(this, EventArgs.Empty);
		}

		protected override void OnNavigated(System.Windows.Forms.WebBrowserNavigatedEventArgs e)
		{	RaiseEventChangeNavigationStatus(WebExplorerNavigationEventArgs.ExplorerAction.Navigated, e.Url.ToString());
			base.OnNavigated(e);
		}

		protected override void OnNavigating(System.Windows.Forms.WebBrowserNavigatingEventArgs e)
		{	e.Cancel = RaiseEventChangeNavigationStatus(WebExplorerNavigationEventArgs.ExplorerAction.Navigating, e.Url.ToString());
			base.OnNavigating(e);
		}

		protected override void OnNewWindow(System.ComponentModel.CancelEventArgs e)
		{	e.Cancel = RaiseEventChangeNavigationStatus(WebExplorerNavigationEventArgs.ExplorerAction.NewWindow, LastLinkSelected);
			if (!e.Cancel)
				base.OnNewWindow(e);
		}

		protected override void OnProgressChanged(System.Windows.Forms.WebBrowserProgressChangedEventArgs e)
		{	RaiseEventChangeProgress(e.CurrentProgress, e.MaximumProgress);
			base.OnProgressChanged(e);
		}

		protected override void OnDocumentCompleted(System.Windows.Forms.WebBrowserDocumentCompletedEventArgs e)
		{	RaiseEventChangeNavigationStatus(WebExplorerNavigationEventArgs.ExplorerAction.Navigated, e.Url.ToString());
			base.OnDocumentCompleted(e);
		}

		protected override void OnDocumentTitleChanged(EventArgs e)
		{	RaiseEventChangeStatus();
			base.OnDocumentTitleChanged(e);
		}

		protected override void OnStatusTextChanged(EventArgs e)
		{	if (!string.IsNullOrEmpty(base.StatusText) && 
					(base.StatusText.StartsWith("http://", StringComparison.CurrentCultureIgnoreCase) ||
					 base.StatusText.StartsWith("file://", StringComparison.CurrentCultureIgnoreCase)))
				LastLinkSelected = base.StatusText;
			RaiseEventChangeStatus();
			base.OnStatusTextChanged(e);
		}

		protected override void OnCanGoBackChanged(EventArgs e)
		{	RaiseEventChangeStatus();
			base.OnCanGoBackChanged(e);
		}

		protected override void OnCanGoForwardChanged(EventArgs e)
		{ RaiseEventChangeStatus();
			base.OnCanGoForwardChanged(e);
		}
		
		[System.ComponentModel.Browsable(false)]
		public string LastLinkSelected { get; private set; }
	}
}
