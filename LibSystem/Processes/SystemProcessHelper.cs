using System;
using System.Diagnostics;
using System.Collections.Generic;

using Bau.Libraries.LibCommonHelper.Extensors;

namespace Bau.Libraries.LibSystem.Processes
{
	/// <summary>
	///		Clase de ayuda para el tratamiento de procesos (ejecutables)
	/// </summary>
	public class SystemProcessHelper
	{
		/// <summary>
		///		Ejecuta la aplicación asociada a un tipo de documento
		/// </summary>
		public void ExecuteApplicationForFile(string fileName)
		{
			ExecuteApplication(null, fileName);
		}

		/// <summary>
		///		Ejecuta una aplicación pasándole los argumentos especificados
		/// </summary>
		public bool ExecuteApplication(string executable, string arguments = null, bool checkPrevious = false)
		{
			bool blnExecuted = false;

				// Ejecuta la aplicación
				if (!checkPrevious || !CheckProcessing(executable))
				{
					Process objProcess = new Process();

						// Inicializa las propiedades del proceso
						objProcess.StartInfo.UseShellExecute = true;
						objProcess.StartInfo.RedirectStandardOutput = false;
						if (string.IsNullOrEmpty(executable))
						{
							objProcess.StartInfo.FileName = arguments ?? "";
							objProcess.StartInfo.Arguments = "";
						}
						else
						{
							objProcess.StartInfo.FileName = executable;
							objProcess.StartInfo.Arguments = arguments ?? "";
						}
						objProcess.StartInfo.WindowStyle = ProcessWindowStyle.Normal;
						objProcess.StartInfo.CreateNoWindow = true;
						// Ejecuta el proceso
						blnExecuted = objProcess.Start();
				}
				// Devuelve el valor que indica si se ha ejecutado
				return blnExecuted;
		}

		/// <summary>
		///		Comprueba si se está procesando una aplicación
		/// </summary>
		public bool CheckProcessing(string executable)
		{
			return GetActiveProcesses(executable).Count > 0;
		}

		/// <summary>
		///		Obtiene los procesos activos de un ejecutable
		/// </summary>
		public List<Process> GetActiveProcesses(string executable)
		{
			List<Process> objColProcess = new List<Process>();

				// Obtiene los procesos del ejecutable
				if (!executable.IsEmpty())
				{
					Process[] arrProcesses = Process.GetProcesses();
					string name = System.IO.Path.GetFileNameWithoutExtension(executable);

						// Recorre los procesos comprobando el nombre de archivo y/o el nombre de proceso
						foreach (Process objProcess in arrProcesses)
							if (objProcess.ProcessName.EqualsIgnoreCase(name) ||
									objProcess.StartInfo.FileName.EqualsIgnoreCase(executable))
								objColProcess.Add(objProcess);
				}
				// Devuelve la colección de procesos
				return objColProcess;
		}

		/// <summary>
		///		Elimina un proceso de memoria
		/// </summary>
		public bool Kill(Process process, out string error)
		{ 
			// Inicializa los argumentos de salida
			error = "";
			// Elimina el proceso de memoria
			try
			{
				process.Kill();
			}
			catch (Exception objException)
			{
				error = objException.Message;
			}
			// Devuelve el valor que indica si se ha eliminado el proceso
			return error.IsEmpty();
		}
	}
}
