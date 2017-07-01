using System;

namespace Bau.Libraries.LibEBook.Formats.ePub.OPF
{
	/// <summary>
	///		Elemento de un libro (página, archivo de estilo, imagen ...)
	/// </summary>
	public class Item : Base.eBookBase
	{ // Variables privadas
			private string url;
			
		/// <summary>
		///		URL del archivo
		/// </summary>
		public string	URL 
		{ get { return url; } 
			set 
				{ // Asigna la cadena
						url = value;
					// Reemplaza las barras
						if (!string.IsNullOrEmpty(url))
							url = url.Replace('/', '\\');
				}
		}
		
		/// <summary>
		///		Tipo del archivo
		/// </summary>
		public string	MediaType { get; set; }
	}
}
