using System;

namespace Bau.Libraries.LibEBook.Formats.ePub.NCX
{
	/// <summary>
	///		Página
	/// </summary>
	public class NavPoint : Base.eBookBase
	{
		public NavPoint()
		{ Pages = new NavPointsCollection();
		}
		
		/// <summary>
		///		Título
		/// </summary>
		public string Title { get; set; }
		
		/// <summary>
		///		URL
		/// </summary>
		public string URL { get; set; }
		
		/// <summary>
		///		Orden
		/// </summary>
		public int Order { get; set; }
		
		/// <summary>
		///		Páginas
		/// </summary>
		public NavPointsCollection Pages { get; private set; }
	}
}
