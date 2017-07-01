using System;
using System.Collections.Generic;

namespace Bau.Libraries.LibCommonHelper.Extensors
{
	/// <summary>
	///		Métodos de extensión para cadenas
	/// </summary>
	public static class StringExtensor
	{
		/// <summary>
		///		Comprueba si dos cadenas son iguales sin tener en cuenta las mayúsculas y tratando los nulos
		/// </summary>
		public static bool EqualsIgnoreCase(this string strFirst, string strSecond)
		{
			if (string.IsNullOrEmpty(strFirst) && !string.IsNullOrEmpty(strSecond))
				return false;
			else if (!string.IsNullOrEmpty(strFirst) && string.IsNullOrEmpty(strSecond))
				return false;
			else if (string.IsNullOrEmpty(strFirst) && string.IsNullOrEmpty(strSecond))
				return true;
			else
				return strFirst.Equals(strSecond, StringComparison.CurrentCultureIgnoreCase);
		}

		/// <summary>
		///		Comprueba si dos cadenas son iguales sin tener en cuenta los nulos
		/// </summary>
		public static bool EqualsIgnoreNull(this string strFirst, string strSecond)
		{
			if (string.IsNullOrEmpty(strFirst) && !string.IsNullOrEmpty(strSecond))
				return false;
			else if (!string.IsNullOrEmpty(strFirst) && string.IsNullOrEmpty(strSecond))
				return false;
			else if (string.IsNullOrEmpty(strFirst) && string.IsNullOrEmpty(strSecond))
				return true;
			else
				return strFirst.Equals(strSecond, StringComparison.CurrentCulture);
		}

		/// <summary>
		///		Compara dos valores 
		/// </summary>
		public static int CompareIgnoreNullTo(this string strFirst, string strSecond)
		{
			if (string.IsNullOrEmpty(strFirst) && string.IsNullOrEmpty(strSecond))
				return 0;
			else if (string.IsNullOrEmpty(strFirst) && !string.IsNullOrEmpty(strSecond))
				return -1;
			else if (!string.IsNullOrEmpty(strFirst) && string.IsNullOrEmpty(strSecond))
				return 1;
			else
				return strFirst.CompareTo(strSecond);
		}

		/// <summary>
		///		Comprueba si una cadena es nula o está vacía (sin espacios)
		/// </summary>
		public static bool IsEmpty(this string value)
		{
			return string.IsNullOrEmpty(value) || string.IsNullOrEmpty(value.Trim());
		}

		/// <summary>
		///		Pasa a mayúsculas el primer carácter de una cadena 
		/// </summary>
		public static string ToUpperFirstLetter(this string value)
		{
			if (value.IsEmpty())
				return "";
			else
				return value.Left(1).ToUpper() + value.From(1);
		}

		/// <summary>
		///		Comprueba el inicio de una cadena evitando los nulos
		/// </summary>
		public static bool StartsWithIgnoreNull(this string value, string strStart, StringComparison intComparison = StringComparison.CurrentCultureIgnoreCase)
		{
			return !string.IsNullOrEmpty(value) && value.StartsWith(strStart, intComparison);
		}

		/// <summary>
		///		Quita los espacios ignorando los valores nulos
		/// </summary>
		public static string TrimIgnoreNull(this string value)
		{
			if (!IsEmpty(value))
				return value.Trim();
			else
				return "";
		}

		/// <summary>
		///		Obtiene un valor lógico a partir de una cadena
		/// </summary>
		public static bool GetBool(this string value, bool blnDefault = false)
		{
			bool blnValue = blnDefault;

			if (value.EqualsIgnoreCase("yes"))
				return true;
			else if (value.EqualsIgnoreCase("no"))
				return false;
			else if (value.IsEmpty() || !bool.TryParse(value, out blnValue))
				return blnDefault;
			else
				return blnValue;
		}

		/// <summary>
		///		Obtiene un valor decimal para una cadena
		/// </summary>
		public static double? GetDouble(this string value)
		{
			double dblValue;

				if (string.IsNullOrEmpty(value) || !double.TryParse(value.Replace('.', ','), out dblValue))
					return null;
				else
					return dblValue;
		}

		/// <summary>
		///		Obtiene un valor decimal para una cadena
		/// </summary>
		public static double GetDouble(this string value, double dblDefault)
		{
			return GetDouble(value) ?? dblDefault;
		}

		/// <summary>
		///		Obtiene un valor entero para una cadena
		/// </summary>
		public static int? GetInt(this string value)
		{
			int intValue;

				if (string.IsNullOrEmpty(value) || !int.TryParse(value, out intValue))
					return null;
				else
					return intValue;
		}

		/// <summary>
		///		Obtiene un valor entero para una cadena
		/// </summary>
		public static int GetInt(this string value, int intDefault)
		{
			return GetInt(value) ?? intDefault;
		}

		/// <summary>
		///		Obtiene un valor largo para una cadena
		/// </summary>
		public static long GetLong(this string value, long lngDefault)
		{
			return GetLong(value) ?? lngDefault;
		}

		/// <summary>
		///		Obtiene un valor largo para una cadena
		/// </summary>
		public static long? GetLong(this string value)
		{
			long lngValue;

				if (string.IsNullOrEmpty(value) || !long.TryParse(value, out lngValue))
					return null;
				else
					return lngValue;
		}

		/// <summary>
		///		Obtiene una fecha para una cadena
		/// </summary>
		public static DateTime? GetDateTime(this string value)
		{
			DateTime dtmValue;

				if (string.IsNullOrEmpty(value) || !DateTime.TryParse(value, out dtmValue))
					return null;
				else
					return dtmValue;
		}

		/// <summary>
		///		Obtiene una fecha para una cadena
		/// </summary>
		public static DateTime GetDateTime(this string value, DateTime dtmDefault)
		{
			return GetDateTime(value) ?? dtmDefault;
		}

		/// <summary>
		///		Obtiene el valor de un enumerado
		/// </summary>
		public static TypeEnum GetEnum<TypeEnum>(this string value, TypeEnum intDefault) where TypeEnum : struct
		{
			TypeEnum intResult;

				if (Enum.TryParse<TypeEnum>(value, out intResult))
					return intResult;
				else
					return intDefault;
		}

		/// <summary>
		///		Obtiene una Url a partir de una cadena
		/// </summary>
		public static Uri GetUrl(this string url, Uri objUriDefault = null)
		{
			Uri objUri = null;

				// Convierte la Url
				if (!Uri.TryCreate(url, UriKind.RelativeOrAbsolute, out objUri))
					objUri = objUriDefault;
				// Devuelv la Url
				return objUri;
		}

		/// <summary>
		///		Añade una cadena a otra con un separador si es necesario
		/// </summary>
		public static string AddWithSeparator(this string value, string strAdd, string strSeparator, bool blnWithSpace = true)
		{ 
			// Añade el separador
			if (!value.IsEmpty())
				value += strSeparator + (blnWithSpace ? " " : "");
			else // ... evita los posibles errores si la cadena es NULL
				value = "";
			// Devuelve la cadena con el valor añadido
			return value + strAdd;
		}

		/// <summary>
		///		Corta una cadena hasta un separador. Devuelve la parte inicial de la cadena antes del separador
		///	y deja en la cadena strTarget, a partir del separador
		/// </summary>
		public static string Cut(this string strSource, string strSeparator, out string strTarget)
		{
			int index;
			string strCut = "";

				// Inicializa los valores de salida
				strTarget = "";
				// Si hay algo que cortar ...
				if (!strSource.IsEmpty())
				{ // Obtiene el índice donde se encuentra el separador
					index = strSource.IndexOf(strSeparator, StringComparison.CurrentCultureIgnoreCase);
					// Corta la cadena
					if (index < 0)
						strCut = strSource;
					else
						strCut = strSource.Substring(0, index);
					// Borra al cadena cortada
					if ((strCut + strSeparator).Length - 1 < strSource.Length)
						strTarget = strSource.Substring((strCut + strSeparator).Length);
					else
						strTarget = "";
				}
				// Devuelve la primera parte de la cadena
				return strCut;
		}

		/// <summary>
		///		Corta una cadena por una longitud
		/// </summary>
		/// <param name="strSource">Cadena original</param>
		/// <param name="intLength">Longitud por la que se debe cortar</param>
		/// <param name="strLast">Segunda parte de la cadena subString(intLength + 1) o vacío si no queda suficiente</param>
		/// <returns>Primera parte de la cadena (0..intLength)</returns>
		public static string Cut(this string strSource, int intLength, out string strLast)
		{ 
			// Inicializa la cadena de salida
			strLast = "";
			// Corta la cadena
			if (!strSource.IsEmpty() && strSource.Length > intLength)
			{
				strLast = strSource.Substring(intLength);
				strSource = strSource.Substring(0, intLength);
			}
			// Devuelve la primera parte de la cadena
			return strSource;
		}

		/// <summary>
		///		Separa una cadena cuando en partes cuando el separador es una cadena (no se puede utilizar Split)
		/// </summary>
		public static List<string> SplitByString(this string strSource, string strSeparator)
		{
			List<string> objColSplit = new List<string>();

				// Corta la cadena
				if (!strSource.IsEmpty())
					do
					{
						string strPart = "";

						// Corta la cadena
						strSource = strSource.Cut(strSeparator, out strPart);
						// Añade la parte localizada a la colección y continúa con la cadena restante
						if (!strSource.IsEmpty())
							objColSplit.Add(strSource);
						// Pasa al siguiente
						strSource = strPart;
					}
					while (!strSource.IsEmpty());
				// Devuelve la primera parte de la cadena
				return objColSplit;
		}

		/// <summary>
		///		Elimina una cadena del inicio de otra
		/// </summary>
		public static string RemoveStart(this string strSource, string strStart)
		{
			if (strSource.IsEmpty() || !strSource.StartsWith(strStart, StringComparison.CurrentCultureIgnoreCase))
				return strSource;
			else if (strSource.Length == strStart.Length)
				return "";
			else
				return strSource.Substring(strStart.Length);
		}

		/// <summary>
		///		Elimina una cadena al final de otra
		/// </summary>
		public static string RemoveEnd(this string strSource, string strEnd)
		{
			if (strSource.IsEmpty() || !strSource.EndsWith(strEnd, StringComparison.CurrentCultureIgnoreCase))
				return strSource;
			else if (strSource.Length == strEnd.Length)
				return "";
			else
				return strSource.Substring(0, strSource.Length - strEnd.Length);
		}

		/// <summary>
		///		Reemplaza una cadena teniendo en cuenta el tipo de comparación
		/// </summary>
		public static string ReplaceWithStringComparison(this string strSource, string strSearch, string strReplace,
																										 StringComparison intComparison = StringComparison.CurrentCultureIgnoreCase)
		{
			int index;

				// Recorre la cadena sustituyendo los valores
				if (!strSource.IsEmpty() && !strSearch.EqualsIgnoreCase(strReplace))
					do
					{
						if ((index = strSource.IndexOf(strSearch, intComparison)) >= 0)
						{
							string strLast;
							string strFirst = strSource.Cut(strSearch, out strLast);

							strSource = strFirst + strReplace + strLast;
						}
					}
					while (index >= 0);
				// Devuelve la cadena
				return strSource;
		}

		/// <summary>
		///		Obtiene la parte izquierda de una cadena
		/// </summary>
		public static string Left(this string strSource, int intLength)
		{
			if (IsEmpty(strSource) || intLength <= 0)
				return "";
			else if (intLength > strSource.Length)
				return strSource;
			else
				return strSource.Substring(0, intLength);
		}

		/// <summary>
		///		Obtiene la parte derecha de una cadena
		/// </summary>
		public static string Right(this string strSource, int intLength)
		{
			if (IsEmpty(strSource) || intLength <= 0)
				return "";
			else if (intLength > strSource.Length)
				return strSource;
			else
				return strSource.Substring(strSource.Length - intLength, intLength);
		}

		/// <summary>
		///		Obtiene una cadena a partir de un carácter
		/// </summary>
		public static string From(this string strSource, int intFirst)
		{
			if (IsEmpty(strSource) || intFirst >= strSource.Length)
				return "";
			else if (intFirst <= 0)
				return strSource;
			else
				return strSource.Substring(intFirst);
		}

		/// <summary>
		///		Obtiene la cadena media
		/// </summary>
		public static string Mid(this string strSource, int intFirst, int intLength)
		{
			return strSource.From(intFirst).Left(intLength);
		}

		/// <summary>
		///		Codificar caracteres en Unicode
		/// </summary>
		public static string ToUnicode(this string value)
		{
			string strResult = "";

				// Codifica los caracteres
				foreach (char chrChar in value)
				{
					if (chrChar > 127)
						strResult += "\\u" + ((int) chrChar).ToString("x4");
					else
						strResult += chrChar;
				}
				// Devuelve el resultado
				return strResult;
		}

		/// <summary>
		///		Concatena una serie de líneas
		/// </summary>
		public static string Concatenate(this string value, List<string> objColStrings, string strCharSeparator = "\r\n",
																		 bool blnWithSpace = false)
		{
			string strResult = "";

				// Concatena las cadenas
				if (objColStrings != null)
					foreach (string strString in objColStrings)
						strResult = strResult.AddWithSeparator(strString, strCharSeparator, blnWithSpace);
				// Devuelve el resultado
				return strResult;
		}

		/// <summary>
		///		Separa una serie de cadenas
		/// </summary>
		public static List<string> SplitToList(this string value, string strCharSeparator = "\r\n",
																					 bool blnAddEmpty = false)
		{
			List<string> objColStrings = new List<string>();
			List<string> objColSource = value.SplitByString(strCharSeparator);

				// Añade las cadenas
				foreach (string strSource in objColSource)
					if (blnAddEmpty || (!blnAddEmpty && !strSource.TrimIgnoreNull().IsEmpty()))
						objColStrings.Add(strSource.TrimIgnoreNull());
				// Devuelve las cadenas
				return objColStrings;
		}

		/// <summary>
		///		Normaliza la cadena quitándole los acentos
		/// </summary>
		public static string NormalizeAccents(this string value)
		{
			int index;
			string strResult = "";
			string strWithAccents = "ÁÉÍÓÚáéíóúÀÈÌÒÙàèìòùÄËÏÖÜäëïöü";
			string strWithOutAccents = "AEIOUaeiouAEIOUaeiouAEIOUaeiou";

				// Normaliza la cadena
				if (!value.IsEmpty())
					foreach (char chrChar in value)
						if ((index = strWithAccents.IndexOf(chrChar)) >= 0)
							strResult += strWithOutAccents [index];
						else
							strResult += chrChar;
				// Devuelve el resultado
				return strResult;
		}

		/// <summary>
		///		Extrae las cadenas que se corresponden con un patrón
		/// </summary>
		public static List<string> Extract(this string strSource, string strStart, string strEnd)
		{
			List<string> objColResults = new List<string>();

				// Obtiene las coincidencias
				if (!strSource.IsEmpty())
					try
					{
						System.Text.RegularExpressions.Match objMatch;

						// Crea la expresión de búsqueda
						objMatch = System.Text.RegularExpressions.Regex.Match(strSource, strStart + "(.|\n)*?" + strEnd,
																			  System.Text.RegularExpressions.RegexOptions.IgnoreCase |
																					System.Text.RegularExpressions.RegexOptions.CultureInvariant,
																			  TimeSpan.FromSeconds(1));
						// Mientras haya una coincidencia
						while (objMatch.Success)
						{
							string value = objMatch.Groups [0].Value.TrimIgnoreNull();

								// Quita la cadena inicial y final
								value = value.RemoveStart(strStart);
								value = value.RemoveEnd(strEnd);
								// Añade la cadena encontrada
								objColResults.Add(value.TrimIgnoreNull());
								// Pasa a la siguiente coincidencia
								objMatch = objMatch.NextMatch();
						}
					}
					catch (System.Text.RegularExpressions.RegexMatchTimeoutException objException)
					{
						System.Diagnostics.Debug.WriteLine("TimeOut: " + objException.Message);
					}
				// Devuelve las coincidencis
				return objColResults;
		}
	}
}
