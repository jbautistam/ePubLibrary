﻿using System;

namespace Bau.Libraries.LibEBook.Formats.ePub.OPF
{
	/// <summary>
	///		Colección de <see cref="Item"/>
	/// </summary>
	public class ItemsCollection : Base.eBookBaseCollection<Item>
	{
		/// <summary>
		///		Obtiene los elementos de determinado tipo
		/// </summary>
		internal ItemsCollection SearchMediaType(string mediaType)
		{ ItemsCollection items = new ItemsCollection();
		
				// Obtiene los elementos con ese tipo de medio
					foreach (Item item in this)
						if (item.MediaType.Equals(mediaType, StringComparison.CurrentCultureIgnoreCase))
							items.Add(item);
				// Devuelve la colección de elementos
					return items;
		}
	}
}
