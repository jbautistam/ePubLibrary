using System;
using System.Collections.Generic;
using System.IO;

namespace Bau.Libraries.LibSystem.Files
{
	/// <summary>
	///		Clase de ayuda para manejo de archivos
	/// </summary>
	public class HelperFiles
	{
		/// <summary>
		///		Elimina un archivo (no tiene en cuenta las excepciones)
		/// </summary>
		public static bool KillFile(string fileName)
		{	
			// Quita el atributo de s�lo lectura
			//! Est� en un try separado porque puede dar problemas de "Acceso denegado"
			try
			{ 
				File.SetAttributes(fileName, FileAttributes.Normal);
			}
			catch {}
			// Borra el archivo
			try
			{ 
				// Elimina el archivo
				File.Delete(fileName);
				// Indica que se ha borrado correctamente
				return true;
			}
			catch (Exception objException)
			{ 
				// Muestra el mensaje
				System.Diagnostics.Debug.WriteLine(objException.Message);
				// Indica que no se ha podido borrar
				return false;
			}
		}	
		
		/// <summary>
		///		Elimina el archivo y lo guarda en la papelera de reciclaje
		/// </summary>
		public static bool KillFileForRecycle(string fileName)
		{ 
			try
			{ 
				API.ShellFileOperationApi.SHFILEOPSTRUCT shf = new API.ShellFileOperationApi.SHFILEOPSTRUCT();

					// Inicializa las propiedades
					shf.wFunc = API.ShellFileOperationApi.FO_DELETE;
					shf.fFlags = API.ShellFileOperationApi.FOF_ALLOWUNDO | API.ShellFileOperationApi.FOF_NOCONFIRMATION;
					shf.pFrom = fileName;
					// Ejecuta el m�todo
					API.ShellFileOperationApi.SHFileOperation(ref shf);
					// Indica que se ha borrado correctamente
					return true;
			}
			catch
			{ 
				return false;
			}
		}

		/// <summary>
		///		Elimina los archivos anteriores a una fecha
		/// </summary>
		public static int KillFilesPrevious(string path, DateTime maximumDate, bool recycle = false)
		{ 
			int files = 0;

				// Borra los archivos
				if (Directory.Exists(path))
				{ 
					string [] filesList = Directory.GetFiles(path, "*.*");

						foreach (string fileName in filesList)
						{ 
							FileInfo file = new FileInfo(fileName);

								if (file.CreationTime < maximumDate)
								{ 
									// Borra el archivo
									if (recycle)
										KillFileForRecycle(fileName);
									else
										KillFile(fileName);
									// Indica que se ha borrado uno m�s
									files++;
								}
						}
				}
				// Devuelve el n�mero de archivos borrados
				return files;
		}

		/// <summary>
		///		Comprueba si existe una unidad
		/// </summary>
		public static bool ExistsDrive(string path)
		{ 
			// Comprueba si existe la unidad
			if (!string.IsNullOrEmpty(path))
			{ 
				DriveInfo [] drives = DriveInfo.GetDrives();
						  
					// Quita los espacios
					path = path.Trim();
					// Comprueba si existe la unidad
					foreach (DriveInfo drvDrive in drives)
						if (drvDrive.IsReady && path.StartsWith(drvDrive.Name, StringComparison.CurrentCultureIgnoreCase))
							return true;
			}
			// Si ha llegado hasta aqu� es porque la unidad no existe
			return false;
		}

		/// <summary>
		///		Crea un directorio sin tener en cuenta las excepciones
		/// </summary>
		public static bool MakePath(string path)
		{ 
			string strError;

				return MakePath(path, out strError);
		}

		/// <summary>
		/// 
		///		Crea un directorio sin tener en cuenta las excepciones
		/// </summary>
		public static bool MakePath(string path, out string error)
		{ 
			bool made = false;

				// Inicializa los argumentos de salida
				error = "";
				// Crea el directorio
				try
				{ 
					if (path.StartsWith("\\") || ExistsDrive(path))
					{ 
						if (Directory.Exists(path)) // ... si ya exist�a, intentar crearlo devolver�a un error
							made = true;
						else // ... intenta crea el directorio
						{ 
							Directory.CreateDirectory(path);
							made = true;
						}
					}
					else
						error = "No existe la unidad para crear: " + path;
				}
				catch (Exception exception)
				{ 
						error = "Error en la creaci�n del directorio " + path + Environment.NewLine +
									exception.Message + Environment.NewLine + exception.StackTrace;
				}
				// Devuelve el valor que indica si se ha creado
				return made;
		}

		/// <summary>
		///		Elimina un directorio sin tener en cuenta las excepciones
		/// </summary>
		public static bool KillPath(string path)
		{ 
			if (Directory.Exists(path))
			{	
				string [] files;
				
					// Elimina los archivos
					files = Directory.GetFiles(path);
					foreach (string file in files)
						if (File.Exists(file))
							KillFile(file);
					// Elimina los directorios
					files = Directory.GetDirectories(path);
					foreach (string file in files)
						KillPath(file);
					// Elimina este directorio
					try
					{ 
						Directory.Delete(path);
						return true;
					}
					catch { return false; }
			}
			else
				return true;
		}
		
		/// <summary>
		/// 	Carga un archivo de texto en una cadena
		/// </summary>
		public static string LoadTextFile(string fileName)
		{ 
			return LoadTextFile(fileName, GetFileEncoding(fileName, System.Text.Encoding.Default));
		}

		/// <summary>
		/// 	Carga un archivo de texto con un encoding determinado en una cadena
		/// </summary>
		public static string LoadTextFile(string fileName, System.Text.Encoding objEncoding)
		{	
			System.Text.StringBuilder content = new System.Text.StringBuilder();

				// Carga el archivo
				using (StreamReader file = new StreamReader(fileName, objEncoding))
				{ 
					string data;

						// Lee los datos
						while ((data = file.ReadLine()) != null)
						{ 
							// Le a�ade un salto de l�nea si es necesario
							if (content.Length > 0)
								content.Append("\n");
							// A�ade la l�nea le�da
							content.Append(data);
						}
						// Cierra el stream
						file.Close();
				}
				// Devuelve el contenido
				return content.ToString();
		}

		/// <summary>
		///		Obtiene la codificaci�n de un archivo
		/// </summary>
		public static System.Text.Encoding GetFileEncoding(string fileName, System.Text.Encoding encodingDefault = null)
		{ 
			System.Text.Encoding encoding;
			byte[] buffer = new byte[5];

				// Lee los primeros cinco bytes del archivo
				using (FileStream file = new FileStream(fileName, FileMode.Open, FileAccess.Read))
				{ 
					file.Read(buffer, 0, 5);
					file.Close();
				}
				// Obtiene la codificaci�n a partir de los bytes iniciales del archivo
				if (buffer[0] == 0xEF && buffer[1] == 0xBB && buffer[2] == 0xBF)
					encoding = System.Text.Encoding.UTF8;
				else if (buffer[0] == 0xFE && buffer[1] == 0xFF)
					encoding = System.Text.Encoding.Unicode;
				else if (buffer[0] == 0 && buffer[1] == 0 && buffer[2] == 0xFE && buffer[3] == 0xFF)
					encoding = System.Text.Encoding.UTF32;
				else if (buffer[0] == 0x2B && buffer[1] == 0x2F && buffer[2] == 0x76)
					encoding = System.Text.Encoding.UTF7;
				else if (encodingDefault == null)
					encoding = System.Text.Encoding.UTF8;
				else
					encoding = encodingDefault;
				// Devuelve la codificaci�n
				return encoding;
		}

		/// <summary>
		/// 	Graba una cadena en un archivo de texto
		/// </summary>
		public static void SaveTextFile(string fileName, string text)
		{	
			SaveTextFile(fileName, text, System.Text.Encoding.UTF8);
		}

		/// <summary>
		/// 	Graba una cadena en un archivo de texto
		/// </summary>
		public static void SaveTextFile(string fileName, string text, string encoding)
		{	
			SaveTextFile(fileName, text, System.Text.Encoding.GetEncoding(encoding));
		}

		/// <summary>
		/// 	Graba una cadena en un archivo de texto
		/// </summary>
		public static void SaveTextFile(string fileName, string text, System.Text.Encoding encoding)
		{	
			using (StreamWriter file = new StreamWriter(fileName, false, encoding))
			{ 
				// Escribe la cadena
				file.Write(text);
				// Cierra el stream
				file.Close();
			}
		}		

		/// <summary>
		/// Graba los datos de un array de bytes en un archivo
		/// </summary>
		public static void SaveBinayFile(byte [] source, string fileName)
		{ 
			using (FileStream output = new FileStream(fileName, FileMode.CreateNew, FileAccess.Write, FileShare.None))
			{ 
				// Escribe los bytes en el stream
				output.Write(source, 0, source.Length);
				// Cierra el stream de salida
				output.Close();
			}
		}

		/// <summary>
		/// Copia un archivo
		/// </summary>
		public static bool CopyFile(string fileNameSource, string fileNameTarget)
		{ 
			try
			{ 
				// Crea el directorio destino
				MakePath(Path.GetDirectoryName(fileNameTarget));
				// Elimina el archivo antiguo
				KillFile(fileNameTarget);
				// Copia el archivo origen en el destino
				File.Copy(fileNameSource, fileNameTarget);
				// Indica que se ha copiado correctamente
				return true;
			}
			catch
			{ 
				return false;
			}
		}

		/// <summary>
		///		Mueve un archivo
		/// </summary>
		public static bool MoveFile(string fileNameSource, string fileNameTarget)
		{ 
			try
			{ 
				// Elimina el archivo antiguo
				KillFile(fileNameTarget);
				// Copia el archivo origen en el destino
				File.Copy(fileNameSource, fileNameTarget);
				// Elimina el archivo movido
				KillFile(fileNameSource);
				// Indica que se ha movido correctamente
				return true;
			}
			catch
			{	
				return false;
			}
		}

		/// <summary>
		///		Mueve un directorio
		/// </summary>
		public static bool MovePath(string pathSource, string pathTarget)
		{ 
			try
			{ 
				// Crea el directorio destino
				MakePath(pathTarget);
				// Copia el directorio origen en el destino
				CopyPath(pathSource, pathTarget);
				// Borra los archivos (si se han copiado)
				KillComparePath(pathSource, pathTarget);
				// Indica que se ha movido correctamente
				return true;
			}
			catch
			{	
				return false;
			}
		}

		/// <summary>
		///		Compara dos directorios y elimina los archivos iguales
		/// </summary>
		private static void KillComparePath(string pathSource, string pathTarget)
		{ 
			if (Directory.Exists(pathTarget))
			{ 
				string [] files;

					// Borra los archivos que existen en los dos directorios
					files = Directory.GetFiles(pathSource);
					foreach (string file in files)
						if (File.Exists(Path.Combine(pathTarget, Path.GetFileName(file))))
							KillFile(file);
					// Compara subdirectorios
					files = Directory.GetDirectories(pathSource);
					foreach (string path in files)
						KillComparePath(path, Path.Combine(pathTarget, Path.GetFileName(path)));
					// Borra los subdirectorios vac�os
					files = Directory.GetDirectories(pathSource);
					foreach (string path in files)
						if (CheckIsEmptyPath(path))
							KillPath(path);
			}
		}

		/// <summary>
		///		Comprueba si un directorio est� vac�o
		/// </summary>
		public static bool CheckIsEmptyPath(string path)
		{ 
			try
			{ 
				return Directory.GetDirectories(path).Length == 0 && Directory.GetFiles(path).Length == 0;
			}
			catch
			{ 
				return false;
			}
		}

		/// <summary>
		///		Copia un directorio en otro
		/// </summary>
		public static void CopyPath(string sourcePath, string targetPath)
		{ 
			string [] files = Directory.GetFiles(sourcePath);
			string [] paths = Directory.GetDirectories(sourcePath);
		
				// Crea el directorio destino
				MakePath(targetPath);
				// Copia los archivos del directorio origen en el destino
				foreach (string fileName in files)
					CopyFile(fileName, Path.Combine(targetPath, Path.GetFileName(fileName)));
				// Copia los directorios
				foreach (string path in paths)
					CopyPath(path, Path.Combine(targetPath, Path.GetFileName(path)));
		}

		/// <summary>
		///		Crea un directorio consecutivo, es decir, si existe ya el nombre del directorio crea
		///	uno con el mismo nombre y un �ndice: "Nueva carpeta", "Nueva carpeta 1", "Nueva carpeta 2" ...
		/// </summary>
		public static bool MakeConsecutivePath(string pathBase, string path)
		{ 
			return MakePath(Path.Combine(pathBase, GetConsecutivePath(pathBase, path)));						
		}

		/// <summary>
		///		Cambia el nombre de un archivo o directorio
		/// </summary>
		public static bool Rename(string oldName, string newName)
		{ 
			bool blnChanged = false;
		
				// Cambia el nombre
				if (!oldName.Equals(newName, StringComparison.CurrentCulture))
					try
					{ 
						// Cambia el nombre
						if (Directory.Exists(oldName))
							Directory.Move(oldName, newName);
						else if (File.Exists(oldName))
							File.Move(oldName, newName);
						// Indica que se ha modificado el nombre
						blnChanged = true;
					}
					catch {}
				// Devuelve el valor que indica si se ha cambiado
				return blnChanged;
		}

		/// <summary>
		///		Obtiene un nombre consecutivo de archivo del tipo NombreArchivo.ext, NombreArchivo 2.ext, NombreArchivo 3.ext
		/// </summary>
		public static string GetConsecutiveFileName(string path, string fileName)
		{ 
			string newFile = Path.GetFileName(fileName);
			string extension = NormalizeExtension(Path.GetExtension(fileName));
			SortedDictionary<string, string> dctFiles = GetDictionaryFilesName(path, 
																			   Path.GetFileNameWithoutExtension(newFile) + "*" + extension);
			int index = 1;
		
				// Si existe el archivo destino, lo cambia
				while (ExistsFile(dctFiles, newFile))
				{ 
					// Obtiene el nombre nuevo
					newFile = Path.GetFileNameWithoutExtension(Path.GetFileName(fileName)) + $"_{index}{extension}";
					// Incrementa el �ndice
					index++;
				}
				// Devuelve el nombre destino
				return Path.Combine(path, newFile);
		}

		/// <summary>
		///		Obtiene un nombre consecutivo de archivo del tipo 00000001.Ext
		/// </summary>
		public static string GetConsecutiveFileNameByExtension(string path, string extension)
		{ 
			SortedDictionary<string, string> files;
			string newFile;
			int index = 2;
		
				// Normaliza la extensi�n
				extension = NormalizeExtension(extension);
				// Carga el diccionario de archivos
				files = GetDictionaryFilesName(path, "*" + extension);
				// Inicializa el nombre del primer archivo
				newFile = GetNextFileName(1, extension);
				// Si existe el archivo destino, lo cambia
				while (ExistsFile(files, newFile))
					newFile = GetNextFileName(++index, extension);
				// Devuelve el nombre destino
				return Path.Combine(path, newFile);
		}

		/// <summary>
		///		Normaliza una extensi�n del tipo .ext
		/// </summary>
		private static string NormalizeExtension(string extension)
		{	
			// Normaliza la extensi�n
			if (!string.IsNullOrEmpty(extension))
			{ 
				// Quita los espacios
				extension = extension.Trim();
				// A�ade el punto inicial
				if (!extension.StartsWith(".") && extension.Length > 1)
					extension = $".{extension}";
			}
			// Devuelve la extensi�n
			return extension;
		}
		
		/// <summary>
		///		Obtiene un nombre de archivo a partir de un �ndice
		/// </summary>
		private static string GetNextFileName(int index, string extension)
		{ 
			return $"{index:0000000}{extension}";
		}

		/// <summary>
		///		Obtiene un diccionario con los nombres de archivos en may�sculas
		/// </summary>
		private static SortedDictionary<string, string> GetDictionaryFilesName(string path, string mask)
		{ 
			SortedDictionary<string, string> filesSorted = new SortedDictionary<string, string>();

				// Comprueba que exista el directorio
				if (!string.IsNullOrEmpty(path) && Directory.Exists(path))
				{ 
					string [] files = Directory.GetFiles(path, mask);

						// A�ade al diccionario los nombres de archivos en may�sculas
						foreach (string file in files)
							if (!string.IsNullOrEmpty(file) && !filesSorted.ContainsKey(Path.GetFileName(file).ToUpper()))
								filesSorted.Add(Path.GetFileName(file).ToUpper(), file.ToUpper());
				}
				// Devuelve el diccionario
				return filesSorted;
		}

		/// <summary>
		///		Comprueba si existe un nombre de archivo en el diccionario
		/// </summary>
		private static bool ExistsFile(SortedDictionary<string, string> files, string newFile)
		{ 
			// Comprueba si existe en el diccionario
			if (!string.IsNullOrEmpty(newFile))
				if (files.ContainsKey(newFile.ToUpper()))
					return true;
			// Si ha llegado hasta aqu� es porque no ha encontrado nada
			return false;
		}

		/// <summary>
		///		Obtiene un nombre consecutivo de directorio del tipo Directorio, Directorio 1, Directorio 2 ...
		/// </summary>
		public static string GetConsecutivePath(string pathBase, string path)
		{	
			string newPath = path;
			int index = 1;
					
				// Crea el nombre del directorio
				while (Directory.Exists(Path.Combine(pathBase, newPath)))
				{ 
					// Crea el nuevo nombre
					newPath = $"{path}_{index}";
					// Incrementa el �ndice
					index++;
				}
				// Crea el nombre de directorio
				newPath = Path.Combine(pathBase, newPath);
				// Devuelve el nombre del archivo
				return newPath;
		}

		/// <summary>
		///		Obtiene el �ltimo directorio de un directorio
		/// </summary>
		public static string GetLastPath(string path)
		{ 
			string lastPath = "";
		
				// Busca la �ltima parte del directorio
				if (!string.IsNullOrEmpty(path))
				{ 
					string [] paths = path.Split(Path.DirectorySeparatorChar);
						
						if (paths.Length > 0)
							lastPath = paths[paths.Length - 1];
				}
				// Devuelve la �ltima parte del directorio
				return lastPath;
		}

		/// <summary>
		///		Normaliza un nombre de archivo
		/// </summary>
		public static string Normalize(string fileName)
		{ 
			return Normalize(fileName, 0);
		}
		
		/// <summary>
		/// Normaliza un nombre de archivo
		/// </summary>
		public static string Normalize(string fileName, int length)
		{ 
			const string ValidChars = "01234567890abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ .-,";
			const string WithAccent = "��������������������";
			const string WithOutAccent = "aeiouaeiouAEIOUAEIOU";
			string target = "";

				// Normaliza el nombre de archivo
				foreach (char chr in fileName)
					if (chr == '\\' || chr == '/')
						target += "_";
					else if (WithAccent.IndexOf(chr) >= 0)
						target += WithOutAccent[WithAccent.IndexOf(chr)];
					else if (ValidChars.IndexOf(chr) >= 0)
						target += chr;
				// Limpia los espacios
				target = target.Trim();
				// Quita los puntos iniciales
				while (target.Length > 0 && target[0] == '.')
					target = target.Substring(1);
				// Obtiene los n primeros caracteres del nombre de archivo
				if (length > 0 && target.Length >	length)
					target = target.Substring(0, length);
				// Devuelve la cadena de salida
				return target;
		}

		/// <summary>
		///		Abre un documento utilizando el shell
		/// </summary>
		public static void OpenDocumentShell(string executable, string fileName)
		{ 
			new Processes.SystemProcessHelper().ExecuteApplication(executable, fileName);
		}
		
		/// <summary>
		///		Abre el documento utilizando el shell
		/// </summary>
		public static void OpenDocumentShell(string fileName)
		{	
			OpenDocumentShell(null, fileName);
		}

		/// <summary>
		///		Abre el explorador con un archivo
		/// </summary>
		public static void OpenBrowser(string url)
		{ 
			OpenDocumentShell("iexplore.exe", url);
		}

		/// <summary>
		///		Ejecuta una aplicaci�n
		/// </summary>
		public static void ExecuteApplication(string executable)
		{ 
			OpenDocumentShell(executable, null);
		}

		/// <summary>
		///		Ejecuta una aplicaci�n
		/// </summary>
		public static void ExecuteApplication(string executable, string arguments) 
		{ 
			OpenDocumentShell(executable, arguments);
		}

		/// <summary>
		///		Elimina los directorios vac�os
		/// </summary>
		public static void KillEmptyPaths(string path)
		{ 
			if (Directory.Exists(path))
			{
				string[] childs = Directory.GetDirectories(path);

					// Borra los directorios hijo
					foreach (string child in childs)
						KillEmptyPaths(child);
					// Si no tiene directorios ni archivos, borra este directorio
					if (Directory.GetFiles(path).Length == 0 &&  Directory.GetDirectories(path).Length == 0)
						KillPath(path);
			}
		}

		/// <summary>
		///		Comprueba si un archivo tiene extensi�n de imagen
		/// </summary>
		public static bool CheckIsImage(string fileName)
		{ 
			return !string.IsNullOrWhiteSpace(fileName) &&
						 (fileName.EndsWith(".jpg", StringComparison.CurrentCultureIgnoreCase) || 
						  fileName.EndsWith(".jpeg", StringComparison.CurrentCultureIgnoreCase) ||
						  fileName.EndsWith(".bmp", StringComparison.CurrentCultureIgnoreCase) ||
						  fileName.EndsWith(".gif", StringComparison.CurrentCultureIgnoreCase) ||
						  fileName.EndsWith(".png", StringComparison.CurrentCultureIgnoreCase) ||
						  fileName.EndsWith(".tif", StringComparison.CurrentCultureIgnoreCase) ||
						  fileName.EndsWith(".tiff", StringComparison.CurrentCultureIgnoreCase));
		}

		/// <summary>
		///		Obtiene un nombre de archivo temporal
		/// </summary>
		public static string GetTemporalFileName(string path, string extension)
		{ 
			if (Directory.Exists(path))
				return GetConsecutiveFileName(path, Guid.NewGuid().ToString() + NormalizeExtension(extension));
			else
				return Path.Combine(path, Guid.NewGuid().ToString() + NormalizeExtension(extension));
		}

		/// <summary>
		///		Comprueba si se puede acceder a un directorio
		/// </summary>
		public static bool CanAccessPath(string path)
		{ 
			try
			{ 
				// Obtiene los directorios, si no pudiera acceder lanzar�a una excepci�n
				Directory.GetDirectories(path);
				// Si se ha podido ejecutar la instrucci�n anterior es porque se puede acceder al directorio
				return true;
			}
			catch
			{ 
				return false;
			}
		}

		/// <summary>
		///		Convierte un archivo a Base64
		/// </summary>
		public static string ConvertToBase64(string fileName)
		{	
			return Convert.ToBase64String(File.ReadAllBytes(fileName));
		}

		/// <summary>
		///		Graba un archivo desde una cadena en Base64
		/// </summary>
		public static void SaveFromBase64(string fileName, string base64)
		{ 
			File.WriteAllBytes(fileName, Convert.FromBase64String(base64));
		}
	}
}