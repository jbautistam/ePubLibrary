using System;

namespace Bau.Libraries.LibMarkupLanguage
{
	/// <summary>
	///		Base para nodos y atributos
	/// </summary>
	public class MLItemBase
	{ 
		// Constantes privadas
		internal const string Yes = "yes";
		internal const string No = "no";
		internal const string True = "true";
		internal const string False = "false";

		public MLItemBase()
		{
		}

		public MLItemBase(string name) : this(null, name, null, false) { }

		public MLItemBase(string name, string value) : this(null, name, value, true) { }

		public MLItemBase(string name, bool blnValue) : this(null, name, blnValue) { }

		public MLItemBase(string name, double dblValue) : this(null, name, dblValue) { }

		public MLItemBase(string name, DateTime dtmValue) : this(null, name, dtmValue) { }

		public MLItemBase(string name, string value, bool blnIsCData) : this(null, name, value, blnIsCData) { }

		public MLItemBase(string strPrefix, string name, double dblValue) :
								this(strPrefix, name, Format(dblValue), false)
		{ }

		public MLItemBase(string strPrefix, string name, DateTime dtmValue) :
								this(strPrefix, name, Format(dtmValue), false)
		{ }

		public MLItemBase(string strPrefix, string name, string value, bool blnIsCData)
		{
			Prefix = strPrefix;
			Name = name;
			if (!string.IsNullOrWhiteSpace(Name) && Name.IndexOf(':') >= 0)
			{
				string [] arrname = name.Split(':');

				if (arrname.Length > 1)
				{
					Prefix = arrname [0];
					Name = arrname [1];
				}
			}
			Value = value;
			IsCData = blnIsCData;
		}

		/// <summary>
		///		Formatea un valor lógico
		/// </summary>
		internal static string Format(bool blnValue)
		{
			if (blnValue)
				return Yes;
			else
				return No;
		}

		/// <summary>
		///		Formatea un valor numérico
		/// </summary>
		internal static string Format(double? dblValue)
		{
			if (dblValue == null)
				return "";
			else
				return dblValue.ToString().Replace(',', '.');
		}

		/// <summary>
		///		Formatea un valor de tipo fecha
		/// </summary>
		internal static string Format(DateTime dtmValue)
		{
			return string.Format("{0:yyyy-MM-dd HH:mm:ss}", dtmValue);
		}

		/// <summary>
		///		Prefijo del espacio de nombres
		/// </summary>
		public string Prefix { get; set; }

		/// <summary>
		///		Nombre del elemento
		/// </summary>
		public string Name { get; set; }

		/// <summary>
		///		Valor del elemento
		/// </summary>
		public string Value { get; set; }

		/// <summary>
		///		Indica si se debe tratar como CData
		/// </summary>
		public bool IsCData { get; set; }
	}
}
