using System;

using Bau.Libraries.LibMarkupLanguage;
using Bau.Libraries.LibMarkupLanguage.Services.XML;
using Bau.Libraries.LibSystem.Files;

namespace Bau.Controls.WebExplorer.Common
{
	/// <summary>
	///		Variables globales para los controles
	/// </summary>
	public static class Globals
	{ 
		// Constantes privadas
		private static string FileLastLinks = "LastLinks.xml";
		// Variables privadas
		private static LinksCollection _lastLinks = null;

		/// <summary>
		///		Carga los vínculos
		/// </summary>
		private static void Load()
		{
			if (System.IO.File.Exists(GetFileNameLastLinks()))
			{
				MLFile file = new XMLParser(false).ParseText(HelperFiles.LoadTextFile(GetFileNameLastLinks()));

				foreach (MLNode mlNode in file.Nodes)
					if (mlNode.Name == LinksCollection.TagRoot)
						LastLinks.Load(mlNode);
			}
		}

		/// <summary>
		///		Graba el vínculo
		/// </summary>
		private static void Save()
		{
			MLFile mlFile = new MLFile();

				// Añade los nodos
				mlFile.Nodes.Add(LastLinks.GetXML());
				// Graba el archivo
				HelperFiles.MakePath(System.IO.Path.GetDirectoryName(GetFileNameLastLinks()));
				HelperFiles.SaveTextFile(GetFileNameLastLinks(), new XMLWriter().ConvertToString(mlFile, true));
		}

		/// <summary>
		///		Obtiene el nombre de archivo donde se guardan los últimos vínculos
		/// </summary>
		private static string GetFileNameLastLinks()
		{
			return System.IO.Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
										  FileLastLinks);
		}

		/// <summary>
		///		Realiza las acciones de añadir un vínculo y grabación
		/// </summary>
		public static void AddLink(string url)
		{ 
			// Quita el último vínculo
			if (LastLinks.Count > 100)
				LastLinks.RemoveAt(0);
			// Añade el vínculo
			LastLinks.Add(url);
			// Guarda el archivo
			Save();
		}

		/// <summary>
		///		Ultimos vínculos
		/// </summary>
		public static LinksCollection LastLinks
		{
			get
			{ 
				// Crea la colección si no estaba en memoria
				if (_lastLinks == null)
				{ 
					// Crea la colección
					_lastLinks = new LinksCollection();
					// Carga los vínculos
					Load();
				}
				// Devuelve la colección
				return _lastLinks;
			}
		}
	}
}
