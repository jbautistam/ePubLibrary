using System;
using System.Collections.Generic;

namespace Bau.Libraries.LibMarkupLanguage
{
	/// <summary>
	///		Colección de <see cref="MLItemBase"/>
	/// </summary>
	public class MLItemsBaseCollection<TypeData> : List<TypeData> where TypeData : MLItemBase, new()
	{
		/// <summary>
		///		Añade una colección de elementos
		/// </summary>
		public void Add(MLItemsBaseCollection<TypeData> objColData)
		{
			foreach (TypeData objData in objColData)
				Add(objData);
		}

		/// <summary>
		///		Añade un nodo a la colección
		/// </summary>
		public TypeData Add(string name)
		{
			return Add(null, name, null, true);
		}

		/// <summary>
		///		Añade un nodo a la colección
		/// </summary>
		public TypeData Add(string name, string value)
		{
			return Add(null, name, value, true);
		}

		/// <summary>
		///		Añade un nodo a la colección
		/// </summary>
		public TypeData Add(string name, bool blnValue)
		{
			return Add(null, name, blnValue);
		}

		/// <summary>
		///		Añade un nodo a la colección
		/// </summary>
		public TypeData Add(string name, double? dblValue)
		{
			return Add(null, name, dblValue);
		}

		/// <summary>
		///		Añade un nodo a la colección
		/// </summary>
		public TypeData Add(string name, DateTime dtmValue)
		{
			return Add(null, name, dtmValue);
		}

		/// <summary>
		///		Añade un nodo a la colección
		/// </summary>
		public TypeData Add(string strPrefix, string name, bool blnValue)
		{
			return Add(strPrefix, name, MLItemBase.Format(blnValue), false);
		}

		/// <summary>
		///		Añade un nodo a la colección
		/// </summary>
		public TypeData Add(string strPrefix, string name, double? dblValue)
		{
			return Add(strPrefix, name, MLItemBase.Format(dblValue), false);
		}

		/// <summary>
		///		Añade un nodo a la colección
		/// </summary>
		public TypeData Add(string strPrefix, string name, DateTime dtmValue)
		{
			return Add(strPrefix, name, MLItemBase.Format(dtmValue), false);
		}

		/// <summary>
		///		Añade un nodo a la colección
		/// </summary>
		public TypeData Add(string strPrefix, string name, string value, bool blnIsCData)
		{
			TypeData objData = new TypeData();

				// Asigna los valores
				objData.Prefix = strPrefix;
				objData.Name = name;
				objData.Value = value;
				objData.IsCData = blnIsCData;
				// Añade el nodo
				Add(objData);
				// Devuelve el objeto
				return objData;
		}

		/// <summary>
		///		Busca un elemento en la colección
		/// </summary>
		public TypeData Search(string name)
		{ 
			// Busca el elemento en la colección
			foreach (TypeData objData in this)
				if (objData.Name.Equals(name))
					return objData;
			// Si ha llegado hasta aquí es porque no ha encontrado nada
			return null;
		}

		/// <summary>
		///		Indizador por el nombre del elemento
		/// </summary>
		public TypeData this [string name]
		{
			get
			{
				TypeData objData = Search(name);

					if (objData == null)
						return new TypeData();
					else
						return objData;
			}
			set
			{
				TypeData objData = Search(name);

					if (objData != null)
						objData = value;
			}
		}
	}
}
