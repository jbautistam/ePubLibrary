using System;

namespace Bau.Controls.WebExplorer
{
	/// <summary>
	///		Argumentos del evento de llamada desde una funci�n JavaScript desde <see cref="WebExplorer"/>
	/// </summary>
	public class WebExplorerJavaScriptFunctionEventArgs : EventArgs
	{ 	
		public WebExplorerJavaScriptFunctionEventArgs(string strArgument)
		{ Argument = strArgument;
		}
		
		/// <summary>
		///		Argumento de la llamada de la funci�n JavaScript
		/// </summary>
		public string Argument { get; private set; }
	}
}
