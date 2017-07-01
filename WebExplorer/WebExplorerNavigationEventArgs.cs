using System;

namespace Bau.Controls.WebExplorer
{
	/// <summary>
	///		Argumentos de los eventos del control <see cref="WebExplorer"/>
	/// </summary>
	public class WebExplorerNavigationEventArgs : EventArgs
	{	// Enumerados públicos
			public enum ExplorerAction
				{ Unknown,
					Navigating,
					Navigated,
					NewWindow
				}
			
		public WebExplorerNavigationEventArgs(ExplorerAction intAction, string url)
		{ Action = intAction;
			URL = url;
		}
		
		/// <summary>
		///		Acción
		/// </summary>
		public ExplorerAction Action { get; private set; }
		
		/// <summary>
		///		URL
		/// </summary>
		public string URL { get; private set; }
		
		/// <summary>
		///		Indica si se debe cancelar el evento
		/// </summary>
		public bool Cancel { get; set; }
	}
}
