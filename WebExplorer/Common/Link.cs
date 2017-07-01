using System;

using Bau.Libraries.LibMarkupLanguage;

namespace Bau.Controls.WebExplorer.Common
{
	/// <summary>
	///		Datos de un vínculo
	/// </summary>
	public class Link
	{ // Constantes privadas
			internal const string TagRoot = "Link";
			
		public Link() : this(null) {}
		
		public Link(string url)
		{ URL = url;
		}

		/// <summary>
		///		Carga los datos de un nodo
		/// </summary>
		internal void Load(MLNode mlNode)
		{ if (mlNode.Name == TagRoot)
				URL = mlNode.Value;
		}
		
		/// <summary>
		///		Obtiene el XML de un vínculo
		/// </summary>
		internal MLNode GetXML()
		{ return new MLNode(TagRoot, URL);
		}
		
		/// <summary>
		///		URL a la que se asocia el vínculo
		/// </summary>
		public string URL { get; set; }
	}
}
