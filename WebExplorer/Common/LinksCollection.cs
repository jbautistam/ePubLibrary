using System;
using System.Collections.Generic;

using Bau.Libraries.LibMarkupLanguage;

namespace Bau.Controls.WebExplorer.Common
{
	/// <summary>
	///		Colección de <see cref="Link"/>
	/// </summary>
	public class LinksCollection : List<Link>
	{ // Constantes privadas
			internal const string TagRoot = "Links";
			
		/// <summary>
		///		Añade una URL a la colección
		/// </summary>
		public void Add(string url)
		{ // Si hay algún vínculo igual lo elimina
				Remove(url);
			// Añade el vínculo
				if (!url.StartsWith("res://", StringComparison.CurrentCultureIgnoreCase))
					Add(new Link(url));
		}

		/// <summary>
		///		Elimina una URL si existe
		/// </summary>
		public void Remove(string url)
		{ int index = IndexOf(url);
		
				// Elimina la URL	
					if (index >= 0)
						RemoveAt(index);
		}
		
		/// <summary>
		///		Obtiene el índice de una URL
		/// </summary>
		public int IndexOf(string url)
		{ // Recorre la colección buscando el índice
				for (int index = 0; index < Count; index++)
					if (url.Equals(this[index].URL, StringComparison.CurrentCultureIgnoreCase))
						return index;
			// Si ha llegado hasta aquí es porque no ha encontrado nada
				return -1;
		}
		
		/// <summary>
		///		Carga los datos de un nodo
		/// </summary>
		internal void Load(MLNode mlNode)
		{ if (mlNode.Name == TagRoot)
				foreach (MLNode mlChild in mlNode.Nodes)
					if (mlChild.Name == Link.TagRoot)
						{ Link objLink = new Link();
						
								// Carga los datos
									objLink.Load(mlChild);
								// Añade el vínculo a la colección
									if (!string.IsNullOrEmpty(objLink.URL))
										Add(objLink);
						}
		}
		
		/// <summary>
		///		Obtiene el XML de la colección
		/// </summary>
		internal MLNode GetXML()
		{ MLNode mlNode = new MLNode(TagRoot);
		
				// Nodos
					foreach (Link objLink in this)
						mlNode.Nodes.Add(objLink.GetXML());
				// Devuelve el nodo
					return mlNode;
		}
	}
}
