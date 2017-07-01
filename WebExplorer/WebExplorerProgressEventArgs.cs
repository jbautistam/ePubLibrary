using System;

namespace Bau.Controls.WebExplorer
{
	/// <summary>
	///		Evento de progreso de <see cref="WebExplorer"/>
	/// </summary>
	public class WebExplorerProgressEventArgs : EventArgs
	{ 	
		public WebExplorerProgressEventArgs(long lngProgress, long lngTotal)
		{ Progress = lngProgress;
			Total = lngTotal;
		}
		
		/// <summary>
		///		Progreso
		/// </summary>
		public long Progress { get; private set; }
		
		/// <summary>
		///		Número total
		/// </summary>
		public long Total { get; private set; }
	}
}
