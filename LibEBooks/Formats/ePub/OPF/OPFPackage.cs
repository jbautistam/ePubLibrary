using System;

namespace Bau.Libraries.LibEBook.Formats.ePub.OPF
{	
	/// <summary>
	///		Datos del archivo OPF
	/// </summary>
	public class OPFPackage : Base.eBookBase
	{
		public OPFPackage()
		{ Metadata = new Metadata();
			Manifest = new ItemsCollection();
			Spine =  new ItemsRefCollection();
			TablesOfContents = new NCX.NCXFilesCollection();
		}
		
		/// <summary>
		///		Metadatos del archivo
		/// </summary>
		public Metadata Metadata { get; internal set; }
		
		/// <summary>
		///		Manifiesto: archivos del eBook (páginas, imágenes, estilos ...)
		/// </summary>
		public ItemsCollection Manifest { get; private set; }
		
		/// <summary>
		///		Indice principal
		/// </summary>
		public ItemsRefCollection Spine { get; private set; }
		
		/// <summary>
		///		Tablas de contenidos
		/// </summary>
		public NCX.NCXFilesCollection TablesOfContents { get; private set; }
	}
}
