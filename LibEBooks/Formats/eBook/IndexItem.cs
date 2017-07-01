using System;

namespace Bau.Libraries.LibEBook.Formats.eBook
{
	/// <summary>
	///		Elemento del �ndice
	/// </summary>
	public class IndexItem : Base.eBookBase
	{
		public IndexItem() : this(null, null, null) { }

		public IndexItem(string name, string idPage, string url)
		{ 
			// Asigna las propiedades
			Name = name;
			IDPage = idPage;
			URL = url;
			// Crea la colecci�n de elementos
			Items = new IndexItemsCollection();
		}

		/// <summary>
		///		Identificador del �ndice
		/// </summary>
		public string Name { get; set; }

		/// <summary>
		///		Identificador de la p�gina
		/// </summary>
		public string IDPage { get; set; }

		/// <summary>
		///		Nombre del archivo con la p�gina
		/// </summary>
		public string URL { get; set; }

		/// <summary>
		///		N�mero de p�ginas
		/// </summary>
		public int PageNumber { get; set; }

		/// <summary>
		///		Sub�ndices
		/// </summary>
		public IndexItemsCollection Items { get; private set; }
	}
}
