using System;

namespace Bau.Libraries.LibEBook.Base
{
	/// <summary>
	///		Elemento base de los objetos de la librer�a
	/// </summary>
	public class eBookBase
	{ 
		// Variables privadas
		private string id;

		/// <summary>
		///		ID del elemento
		/// </summary>
		public string ID
		{
			get
			{ 
				// Obtiene el ID si no exist�a
				if (string.IsNullOrEmpty(id))
					id = Guid.NewGuid().ToString();
				// Devuelve el ID	
				return id;
			}
			set { id = value; }
		}
	}
}
