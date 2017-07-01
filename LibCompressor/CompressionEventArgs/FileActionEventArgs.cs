using System;

namespace Bau.Libraries.LibCompressor.CompressionEventArgs
{
	/// <summary>
	///		Argumentos de los eventos de acciones
	/// </summary>
    public class FileActionEventArgs : EventArgs
    {
		public FileActionEventArgs(string fileName)
		{
			FileName = fileName;
		}

		/// <summary>
		///		Nombre de archivo
		/// </summary>
		public string FileName { get; }
    }
}
