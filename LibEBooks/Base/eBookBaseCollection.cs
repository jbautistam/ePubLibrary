using System;
using System.Collections.Generic;
using System.Linq;

namespace Bau.Libraries.LibEBook.Base
{
	/// <summary>
	///		Colección de <see cref="ePubBase"/>
	/// </summary>
	public class eBookBaseCollection<TypeData> : List<TypeData> where TypeData : eBookBase
	{
		/// <summary>
		///		Busca un elemento por su ID
		/// </summary>
		public TypeData Search(string id)
		{ 
			return this.FirstOrDefault(item => item.ID == id);
		}
	}
}
