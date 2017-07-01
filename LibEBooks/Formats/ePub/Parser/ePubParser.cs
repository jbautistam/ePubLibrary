using System;

using Bau.Libraries.LibCompressor;

namespace Bau.Libraries.LibEBook.Formats.ePub.Parser
{
	/// <summary>
	///		Intérprete de archivos OPF
	/// </summary>
	internal class ePubParser
	{
		/// <summary>
		///		Interpreta un archivo OPF
		/// </summary>
		internal ePubEBook Parse(string fileName, string pathTarget)
		{
			ePubEBook book = new ePubEBook();
			Compressor compressor = new Compressor();

				// Asigna los manejadores de eventos
				compressor.MakePathRequired += (sender, eventArgs) => LibSystem.Files.HelperFiles.MakePath(eventArgs.FileName);
				compressor.KillFileRequired += (sender, eventArgs) => LibSystem.Files.HelperFiles.KillFile(eventArgs.FileName);
				// Descomprime el libro
				compressor.Uncompress(fileName, pathTarget);
				// Interpreta el archivo container.xml
				book.Container = ePubParserContainer.Parse(pathTarget);
				// Interpreta los metadatos
				ePubParserPackage.Parse(book, pathTarget);
				// Devuelve el libro
				return book;
		}
	}
}