using System;
using System.Text;

namespace Bau.Libraries.LibMarkupLanguage.Services.XML
{
	/// <summary>
	///		Clase de ayuda para generación de XML
	/// </summary>
	public class XMLWriter : IWriter
	{ 
		// Variables privadas
		private StringBuilder sbXML = new StringBuilder();

		/// <summary>
		///		Convierte los datos de un MLFile en una cadena
		/// </summary>
		public string ConvertToString(MLFile mlFile, bool blnAddHeader = true)
		{ 
			// Crea el stringBuilder del archivo
			Create(mlFile, blnAddHeader);
			// Devuelve la cadena
			return sbXML.ToString();
		}

		/// <summary>
		///		Convierte los datos de un nodo a una cadena
		/// </summary>
		public string ConvertToString(MLNode mlNode)
		{ 
			// Limpia el contenido
			sbXML.Clear();
			// Añade la información del nodo y sus hijos
			Add(0, mlNode);
			// Devuelve la cadena
			return sbXML.ToString();
		}

		/// <summary>
		///		Convierte los datos de una colección de nodos en una cadena
		/// </summary>
		public string ConvertToString(MLNodesCollection mlNodes)
		{ 
			// Limpia el contenido
			sbXML.Clear();
			// Añade la información del nodo y sus hijos
			Add(0, mlNodes);
			// Devuelve la cadena
			return sbXML.ToString();
		}

		/// <summary>
		///		Crea el texto de un archivo
		/// </summary>
		private void Create(MLFile file, bool blnAddHeader)
		{ 
			// Limpia el contenido
			sbXML.Clear();
			// Añade la cabecera
			if (blnAddHeader)
				sbXML.Append("<?xml version='1.0' encoding='utf-8'?>" + Environment.NewLine);
			// Añade los nodos
			Add(0, file.Nodes);
		}

		/// <summary>
		///		Añade los nodos
		/// </summary>
		private void Add(int intIndent, MLNodesCollection mlNodes)
		{
			foreach (MLNode mlNode in mlNodes)
				Add(intIndent, mlNode);
		}

		/// <summary>
		///		Añade los datos de un nodo
		/// </summary>
		private void Add(int intIndent, MLNode mlNode)
		{ 
			// Indentación
			AddIndent(intIndent);
			// Cabecera
			sbXML.Append("<");
			// Nombre
			AddName(mlNode);
			// Espacios de nombres
			Add(mlNode.NameSpaces);
			// Atributos
			Add(mlNode.Attributes);
			// Final y datos del nodo (en su caso)
			if (IsAutoClose(mlNode))
				sbXML.Append("/>" + Environment.NewLine);
			else
			{ // Cierre de la etiqueta de apertura
				sbXML.Append(">");
				// Datos
				if (mlNode.Nodes.Count > 0)
				{
					sbXML.Append(Environment.NewLine);
					Add(intIndent + 1, mlNode.Nodes);
				}
				else if (mlNode.IsCData)
				{ // Añade un salto de línea
					sbXML.Append(Environment.NewLine);
					// Añade el CData
					if (mlNode.Value.IndexOf("<![CDATA[") < 0) // ... si la cadena no tenía ya un CDATA
						Add(intIndent + 1, "<![CDATA[" + mlNode.Value + "]]>");
					else
						Add(intIndent + 1, mlNode.Value);
					// Prepara la línea para el cierre
					sbXML.Append(Environment.NewLine);
					AddIndent(intIndent);
				}
				else if (!string.IsNullOrEmpty(mlNode.Value))
					sbXML.Append(mlNode.Value);
				// Cierre
				sbXML.Append("</");
				AddName(mlNode);
				sbXML.Append(">" + Environment.NewLine);
			}
		}

		/// <summary>
		///		Añade la indentación
		/// </summary>
		private void AddIndent(int intIndent)
		{
			for (int index = 0; index < intIndent; index++)
				sbXML.Append("\t");
		}

		/// <summary>
		///		Añade un texto con indentación
		/// </summary>
		private void Add(int intIndent, string strText)
		{ 
			// Añade la indentación
			AddIndent(intIndent);
			// Añade el texto
			sbXML.Append(strText);
		}

		/// <summary>
		///		Añade el nombre de un elemento
		/// </summary>
		private void AddName(MLItemBase mlNode)
		{   
			// Espacio de nombres
			if (!string.IsNullOrEmpty(mlNode.Prefix))
				sbXML.Append(mlNode.Prefix + ":");
			// Nombre
			sbXML.Append(mlNode.Name);
		}

		/// <summary>
		///		Espacios de nombres
		/// </summary>
		private void Add(MLNameSpacesCollection objColNameSpaces)
		{
			foreach (MLNameSpace objNameSpace in objColNameSpaces)
			{ 
				// Nombre
				sbXML.Append(" xmlns");
				if (!string.IsNullOrEmpty(objNameSpace.Prefix))
					sbXML.Append(":" + objNameSpace.Prefix);
				// Atributos
				sbXML.Append(" = \"" + objNameSpace.NameSpace + "\" ");
			}
		}

		/// <summary>
		///		Atributos
		/// </summary>
		private void Add(MLAttributesCollection objColAttributes)
		{
			foreach (MLAttribute objAttribute in objColAttributes)
				sbXML.Append(" " + objAttribute.Name + " = \"" + EncodeHTML(objAttribute.Value) + "\" ");
		}

		/// <summary>
		///		Codifica una cadena HTML quitando los caracteres raros
		/// </summary>
		private string EncodeHTML(string value)
		{ 
			// Quita los caracteres raros
			if (!string.IsNullOrEmpty(value))
			{
				value = value.Replace("&", "&amp;");
				value = value.Replace("<", "&lt;");
				value = value.Replace(">", "&gt;");
				value = value.Replace("\"", "&quot;");
				//value = value.Replace("á", "&aacute;");
				//value = value.Replace("é", "&eacute;");
				//value = value.Replace("í", "&iacute;");
				//value = value.Replace("ó", "&oacute;");
				//value = value.Replace("ú", "&uacute;");
			}
			// Devuelve la cadena
			return value;
		}

		/// <summary>
		///		Indica que es un nodo que se debe autocerrar
		/// </summary>
		private bool IsAutoClose(MLNode mlNode)
		{
			return string.IsNullOrEmpty(mlNode.Value) && (mlNode.Nodes == null || mlNode.Nodes.Count == 0);
		}
	}
}
