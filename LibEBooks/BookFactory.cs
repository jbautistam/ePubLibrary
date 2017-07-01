using System;

using Bau.Libraries.LibEBook.Formats.eBook;
using Bau.Libraries.LibEBook.Formats.ePub;
using Bau.Libraries.LibEBook.Formats.ePub.Creator;
using Bau.Libraries.LibEBook.Formats.ePub.Parser;

namespace Bau.Libraries.LibEBook
{
	/// <summary>
	///		Factory de un libro
	/// </summary>
	public class BookFactory
	{ 
		// Enumerados públicos
		/// <summary>
		///		Tipo de libro
		/// </summary>
		public enum BookType
		{
			/// <summary>Formato neutral</summary>
			eBookNeutral,
			/// <summary>Formato ePub</summary>
			ePub
		}

		/// <summary>
		///		Carga los datos de un libro
		/// </summary>
		public ePubEBook Load(string fileName, string pathTarget)
		{
			LibSystem.Files.HelperFiles.MakePath(pathTarget);
			return new ePubParser().Parse(fileName, pathTarget);
		}

		/// <summary>
		///		Obtiene un <see cref="Book"/> a partir de un archivo
		/// </summary>
		public Book Load(BookType type, string fileName, string pathTarget)
		{
			LibSystem.Files.HelperFiles.MakePath(pathTarget);
			switch (type)
			{
				case BookType.ePub:
					return new ePubConvertEBook().Convert(new ePubParser().Parse(fileName, pathTarget));
				default:
					throw new NotImplementedException("Tipo de eBook desconocido");
			}
		}

		/// <summary>
		///		Graba un archivo
		/// </summary>
		public void Save(BookType type, Book eBook, string fileName)
		{
			switch (type)
			{
				case BookType.ePub:
					new ePubCreator().Create(fileName, eBook);
					break;
				default:
					throw new NotImplementedException("Tipo de eBook desconocido");
			}
		}
	}
}
