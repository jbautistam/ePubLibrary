using System;
using System.Collections.Generic;
using System.IO;

using SharpCompress.Archives;
using SharpCompress.Common;
using SharpCompress.Writers;

namespace Bau.Libraries.LibCompressor
{
	/// <summary>
	///		Compresor de archivos
	/// </summary>
	public class Compressor
	{
		// Eventos públicos
		public event EventHandler<CompressionEventArgs.ProgressEventArgs> Progress;
		public event EventHandler<CompressionEventArgs.FileActionEventArgs> MakePathRequired;
		public event EventHandler<CompressionEventArgs.FileActionEventArgs> KillFileRequired;

		/// <summary>
		///		Comprime un directorio
		/// </summary>
		public void Compress(string fileTarget, string path)
		{
			Compress(fileTarget, GetFiles(path));
		}

		/// <summary>
		///		Comprime un archivo
		/// </summary>
		public void Compress(string fileTarget, List<string> files)
		{
			using (FileStream zipStream = File.OpenWrite(fileTarget))
			{
				using (var zipWriter = WriterFactory.Open(zipStream, ArchiveType.Zip, new WriterOptions(CompressionType.GZip)))
				{
					 foreach (string fileName in files)
						zipWriter.Write(Path.GetFileName(fileName), fileName);
				}
			}
		}

		/// <summary>
		///		Añade un directorio al Zip
		/// </summary>
		private List<string> GetFiles(string pathBase)
		{ 
			List<string> files = new List<string>();

				// Añade los archivos y directoros hijo
				if (Directory.Exists(pathBase))
				{
					foreach (string path in Directory.GetDirectories(pathBase))
						files.AddRange(GetFiles(path));
					foreach (string file in Directory.GetFiles(pathBase))
						files.Add(file);
				}
				else if (File.Exists(pathBase))
					files.Add(pathBase);
				// Devuelve la colección de archivos
				return files;
		}

		/// <summary>
		///		Añade un directorio al base
		/// </summary>
		private string GetPathToBase(string path, string fileName)
		{
			if (string.IsNullOrEmpty(path))
				return Path.GetFileName(fileName);
			else
				return path + "/" + fileName;
		}

		/// <summary>
		///		Descomprime un archivo en un directorio
		/// </summary>
		public void Uncompress(string fileSource, string pathTarget)
		{
			IArchive archive = ArchiveFactory.Open(fileSource);
			int file = 0;

				// Descomprime los archivos
				foreach (IArchiveEntry entry in archive.Entries)
					if (!entry.IsDirectory)
					{
						string fileTarget;

							// Descomprime un archivo
							UncompressFile(entry, pathTarget, out fileTarget);
							// Lanza el evento
							RaiseEventProgess(++file, file, fileTarget);
					}
		}

		/// <summary>
		///		Descomprime un archivo
		/// </summary>
		private void UncompressFile(IArchiveEntry entry, string path, out string fileTarget)
		{ 
			// Obtiene el nombre del archivo de salida
			fileTarget = Path.Combine(path, NormalizeFileName(entry.Key));
			// Crea el directorio
			MakePath(Path.GetDirectoryName(fileTarget));
			// Borra el archivo de salida (por si acaso)
			KillFile(fileTarget);
			// Descomprime el archivo
			entry.WriteToFile(fileTarget);
		}

		/// <summary>
		///		Carga la lista de archivos
		/// </summary>
		private List<string> ListFiles(string fileName)
		{
			IArchive objRarArchive = ArchiveFactory.Open(fileName);
			List<string> files = new List<string>();
			int fileIndex = 0;

				// Lista los archivos
				foreach (IArchiveEntry entry in objRarArchive.Entries)
					if (!entry.IsDirectory)
					{ 
						// Añade un archivo
						files.Add(NormalizeFileName(entry.Key));
						// Lanza el evento
						RaiseEventProgess(fileIndex++, fileIndex + 2, files[files.Count - 1]);
					}
				// Devuelve la colección de archivos
				return files;
		}

		/// <summary>
		///		Lanza el evento de progreso
		/// </summary>
		private void RaiseEventProgess(int actual, int total, string fileName)
		{
			Progress?.Invoke(this, new CompressionEventArgs.ProgressEventArgs(actual, total, fileName));
		}

		/// <summary>
		///		Crea un directorio
		/// </summary>
		private bool MakePath(string path)
		{
			MakePathRequired?.Invoke(this, new CompressionEventArgs.FileActionEventArgs(path));
			return true;
		}

		/// <summary>
		///		Borra un archivo
		/// </summary>
		private void KillFile(string fileName)
		{
			KillFileRequired?.Invoke(this, new CompressionEventArgs.FileActionEventArgs(fileName));
		}

		/// <summary>
		///		Normaliza un nombre de archivo
		/// </summary>
		protected string NormalizeFileName(string fileName)
		{ 
			// Reemplaza los caracteres extraños
			fileName = fileName.Replace('/', '\\');
			// Devuelve el nombre de archivo
			return fileName;
		}
	}
}
