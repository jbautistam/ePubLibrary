using System;

namespace Bau.Controls.WebExplorer
{
	/// <summary>
	///		Argumentos del evento de llamada desde una función JavaScript desde <see cref="WebExplorer"/>
	/// </summary>
	public class WebExplorerJavaScriptFunctionEventArgs : EventArgs
	{ 	
		public WebExplorerJavaScriptFunctionEventArgs(string strArgument)
		{ Argument = strArgument;
		}
		
		/// <summary>
		///		Argumento de la llamada de la función JavaScript
		/// </summary>
		public string Argument { get; private set; }
	}
}
