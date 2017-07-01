using System;

namespace Bau.Libraries.LibEBook.Formats.ePub.NCX
{
	/// <summary>
	///		Archivo NCX con la tabla de contenidos
	/// </summary>
	public class NCXFile : Base.eBookBase
	{
		public NCXFile()
		{ Pages =	new NavPointsCollection();
		}
		
		/// <summary>
		///		Título
		/// </summary>
		public string Title { get; set; }
		
		/// <summary>
		///		Generador
		/// </summary>
		public string Generator { get; set; }
		
		/// <summary>
		///		Profundidad
		/// </summary>
		public int Depth { get; set; }
		
		/// <summary>
		///		Número de páginas
		/// </summary>
		public int PageCount { get; set; }
		
		/// <summary>
		///		Número de páginas
		/// </summary>
		public int PageNumber { get; set; }
		
		/// <summary>
		///		Páginas
		/// </summary>
		public NavPointsCollection Pages { get; private set; }
	}
}
