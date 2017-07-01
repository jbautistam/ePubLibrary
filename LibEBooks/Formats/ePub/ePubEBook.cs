using System;

namespace Bau.Libraries.LibEBook.Formats.ePub
{
	/// <summary>
	///		Clase con los datos de un eBook en el formato ePub
	/// </summary>
	public class ePubEBook
	{
		public ePubEBook()
		{ Container = new Container.ContainerFile();
		}
		
		/// <summary>
		///		Archivo contenedor. Indica dónde está el archivo de índice
		/// </summary>
		public Container.ContainerFile Container { get; internal set; }
	}
}
