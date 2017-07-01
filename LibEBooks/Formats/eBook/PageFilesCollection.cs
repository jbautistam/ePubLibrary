using System;

namespace Bau.Libraries.LibEBook.Formats.eBook
{
	/// <summary>
	///		Colección de <see cref="PageFile"/>
	/// </summary>
	public class PageFilesCollection : Base.eBookBaseCollection<PageFile>
	{
		/// <summary>
		///		Añade un archivo
		/// </summary>
		public void Add(string id, string name, string fileName, string mediaType)
		{
			Add(new PageFile(id, name, fileName, mediaType));
		}

		/// <summary>
		///		Busca una página a partir de la URL
		/// </summary>
		internal PageFile SearchByURL(string url)
		{ 
			// Busca la página a partir de la URL
			foreach (PageFile page in this)
				if (!string.IsNullOrEmpty(page.FileName))
				{
					string[] urls = page.FileName.Split('#');

						if (urls[0].Equals(url) || page.FileName.Equals(url))
							return page;
				}
			// Si ha llegado hasta aquí es porque no ha encontrado nada
			return null;
		}

		/// <summary>
		///		Busca una página a partir de un nombre de archivo
		/// </summary>
		internal PageFile SearchByFileName(string fileName)
		{ 
			// Busca la página a partir de la URL
			foreach (PageFile page in this)
				if (!string.IsNullOrEmpty(page.FileName) && (page.FileName.Equals(fileName) ||
						System.IO.Path.GetFileName(page.FileName).Equals(fileName)))
					return page;
			// Si ha llegado hasta aquí es porque no ha encontrado nada
			return null;
		}

		/// <summary>
		///		Normaliza los IDs de las páginas
		/// </summary>
		internal void NormalizeID()
		{
			int index = 1;

				foreach (PageFile file in this)
					file.ID = $"Page{index++}";
		}
	}
}
