using System;

namespace Bau.Libraries.LibCompressor.CompressionEventArgs
{
	/// <summary>
	///		Argumentos del envento de progreso
	/// </summary>
	public class ProgressEventArgs : EventArgs
	{
		public ProgressEventArgs(int actual, int total, string fileName)
		{
			Actual = actual;
			Total = total;
			FileName = fileName;
		}

		/// <summary>
		///		Elemento que se está procesando actualmente
		/// </summary>
		public int Actual { get; private set; }

		/// <summary>
		///		Número total de elementos
		/// </summary>
		public int Total { get; private set; }

		/// <summary>
		///		Nombre de archivo
		/// </summary>
		public string FileName { get; private set; }
	}
}
