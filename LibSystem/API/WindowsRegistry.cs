using System;
using Microsoft.Win32;

namespace Bau.Libraries.LibSystem.API
{
	/// <summary>
	///		Clase con funciones �tiles de acceso al registro de Windos
	/// </summary>
	public class WindowsRegistry
	{ 
		// Constantes privadas 
		private const string WindowsRunKey = @"Software\Microsoft\Windows\CurrentVersion\Run";

		/// <summary>
		///		Asocia una extensi�n con un ejecutable y una acci�n
		/// </summary>
		public void LinkExtension(string extension, string executableFileName, string programId,
								  string command = "open", string description = "")
		{ 
			string linkedProgramID;
			RegistryKey registryKey = null;
			RegistryKey registryKeyShell = null;

				// El comando predeterminado es open
				if (string.IsNullOrEmpty(command))
					command = "open";
				// Obtiene la descripci�n
				if (string.IsNullOrEmpty(description))
					description = $"{extension} Descripci�n de {programId}";
				// Normaliza la extensi�n
				extension = NormalizeExtension(extension);
				// Obtiene el ID del programa a partir de la extensi�n
				linkedProgramID = GetProgIdFromExtension(extension);
				// Si no hay nada asociado, se crean las claves, si hay algo asociado se modifican
				if (string.IsNullOrEmpty(linkedProgramID) || linkedProgramID.Length == 0)
				{	
					// Crear la clave con la extensi�n
					registryKey = Registry.ClassesRoot.CreateSubKey(extension);
					registryKey?.SetValue("", programId);
					// Crea la clave con el programa
					registryKey = Registry.ClassesRoot.CreateSubKey(programId);
					registryKey?.SetValue("", description);
					// Crea la clave con el comando
					registryKeyShell = registryKey?.CreateSubKey($"shell\\{command}\\command");
				}
				else
				{	
					// Abrimos la clave clave indicando que vamos a escribir para que nos permita crear nuevas subclaves.
					registryKey = Registry.ClassesRoot.OpenSubKey(linkedProgramID, true);
					registryKeyShell = registryKey?.OpenSubKey($"shell\\{command}\\command", true);
					// Si es un comando que se a�ade, no existir�
				    if (registryKeyShell == null)
						registryKeyShell = registryKey?.CreateSubKey(programId);

				}
				// Si tenemos la clave de registro del Shell
				if (registryKeyShell != null)
				{	
					registryKeyShell.SetValue("", $"\"{executableFileName}\" \"%1\"");
					registryKeyShell.Close();
				}
		}

		/// <summary>
		///		Permite quitar un comando asociado con una extensi�n
		/// </summary>
		public void DeleteLinkCommandExtension(string extension, string command)
		{ 
			if (!string.IsNullOrEmpty(command) && !command.Equals("open", StringComparison.CurrentCultureIgnoreCase))
			{ 
				string programID;
				
					// Normaliza la extensi�n
					extension = NormalizeExtension(extension);
					// Obtiene el ID de programa de la extensi�n
					programID = GetProgIdFromExtension(extension);
					// Elimina la clave del registro
					if (!string.IsNullOrEmpty(programID) && programID.Length > 0)
						using(RegistryKey registryKey = Registry.ClassesRoot.OpenSubKey(programID, true))
						{	
							registryKey?.DeleteSubKeyTree($"shell\\{command}");
						}
			}
		}

		/// <summary>
		///		Quita la extensi�n indicada de los tipos de archivos registrados
		/// </summary>
		public void DeleteLinkProgramExtension(string extension)
		{ 
			string programID;
    
				// Obtiene la extensi�n
				extension = NormalizeExtension(extension);
				// Obtiene el ID del programa
				programID = GetProgIdFromExtension(extension);
				// Elimina las claves
				if (!string.IsNullOrEmpty(programID) && programID.Length > 0)
				{	
					// Elimina la subclave
					Registry.ClassesRoot.DeleteSubKeyTree(extension);
					// Elimina la clave
					Registry.ClassesRoot.DeleteSubKeyTree(programID);
				}
		}

		/// <summary>
		///		Comprueba si la extensi�n indicada est� registrada.
		/// </summary>
		public bool ExistsLink(string extension)
		{ 
			string programID = GetProgIdFromExtension(NormalizeExtension(extension));

				return !string.IsNullOrEmpty(programID) && programID.Length > 0;
		}

		/// <summary>
		///		A�ade un punto al inicio de una extensi�n
		/// </summary>
		private string NormalizeExtension(string extension)
		{ 
			if (!extension.StartsWith("."))
				return "." + extension;
			else
				return extension;
		}
		
		/// <summary>
		///		M�todo para obtener el ID de programa de una extensi�n
		/// </summary>
		public string GetProgIdFromExtension(string extension)
		{ 
			string strProgramID = "";

				// Obtiene el ID del programa
				using (RegistryKey registryKey = Registry.ClassesRoot.OpenSubKey(extension))
				{	
					if (registryKey?.GetValue("") != null)
					{	
						// Obtiene el ID
						strProgramID = registryKey.GetValue("").ToString();
						// Cierra la clave
						registryKey.Close();
					}
				}
				// Devuelve el ID del programa
				return strProgramID;
		}

		/// <summary>
		///		A�ade al registro de Windows la aplicaci�n usando el t�tulo indicado
		/// </summary>
		public void AddToWindowsStart(string title, string executableFileName)
		{	
			using (RegistryKey registryKey = Registry.LocalMachine.OpenSubKey(WindowsRunKey, true))
			{	
				registryKey?.SetValue(title, $"\"{executableFileName}\"");
			}
		}

		/// <summary>
		///		Quita del registro de Windows la aplicaci�n relacionada con el t�tulo indicado
		/// </summary>
		public void RemoveFromWindowsStart(string title)
		{	
			using(RegistryKey registryKey = Registry.LocalMachine.OpenSubKey(WindowsRunKey, true))
			{	
				registryKey?.DeleteValue(title, false);
			}
		}

		/// <summary>
		///		Comprueba si una aplicaci�n con el t�tulo indicado est� en el inicio de Windows
		/// </summary>
		public bool IsAtWindowsStart(string title, out string fileName)
		{	
			// Obtiene el valor del registro
			using (RegistryKey registryKey = Registry.LocalMachine.OpenSubKey(WindowsRunKey, true))
			{	
				fileName = registryKey?.GetValue(title).ToString();
			}
			// Devuelve el valor que indica si est� en el inicio de Windows
			return !string.IsNullOrEmpty(fileName);
		}
	}
}
