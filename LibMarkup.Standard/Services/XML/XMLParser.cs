using System;
using System.Xml;

namespace Bau.Libraries.LibMarkupLanguage.Services.XML
{
	/// <summary>
	///		Interpreta un archivo XML
	/// </summary>
	public class XMLParser : IParser
	{
		public XMLParser(bool blnIncludeComments = false)
		{
			IncludeComments = blnIncludeComments;
		}

		/// <summary>
		///		Interpreta un texto XML
		/// </summary>
		public MLFile ParseText(string strText)
		{
			return Load(XmlReader.Create(new System.IO.StringReader(strText)));
		}

		/// <summary>
		///		Abre un Reader XML sin comprobación de caracteres extraños
		/// </summary>
		private XmlReader Open(string fileName)
		{
			XmlReaderSettings objSettings = new XmlReaderSettings();

				// Carga el documento
				objSettings.CheckCharacters = false;
				objSettings.CloseInput = true;
				objSettings.DtdProcessing = DtdProcessing.Ignore;
				// Devuelve el documento XML abierto
				return XmlReader.Create(fileName, objSettings);
		}

		/// <summary>
		///		Carga un archivo
		/// </summary>
		public MLFile Load(XmlReader objXmlReader)
		{
			MLFile mlFile = new MLFile();

				// Carga los datos del archivo
				while (objXmlReader.Read())
					switch (objXmlReader.NodeType)
					{
						case XmlNodeType.Element:
							mlFile.Nodes.Add(LoadNode(objXmlReader));
							break;
					}
				// Devuelve el archivo
				return mlFile;
		}

		/// <summary>
		///		Carga los datos de un nodo
		/// </summary>
		private MLNode LoadNode(XmlReader objXmlReader)
		{
			MLNode mlNode = new MLNode(objXmlReader.Name);

				// Asigna los atributos
				mlNode.Attributes.AddRange(LoadAttributes(objXmlReader));
				// Lee los nodos
				if (!objXmlReader.IsEmptyElement)
				{
					bool blnIsEnd = false;

					// Lee los datos del nodo
					while (!blnIsEnd && objXmlReader.Read())
						switch (objXmlReader.NodeType)
						{
							case XmlNodeType.Element:
								mlNode.Nodes.Add(LoadNode(objXmlReader));
								break;
							case XmlNodeType.Text:
								mlNode.Value = Decode(objXmlReader.Value);
								break;
							case XmlNodeType.CDATA:
								mlNode.Value = Decode(objXmlReader.Value);
								break;
							case XmlNodeType.EndElement:
								blnIsEnd = true;
								break;
						}
				}
				else
					mlNode.Value = objXmlReader.Value;
				// Devuelve el nodo
				return mlNode;
		}

		/// <summary>
		///		Carga los atributos
		/// </summary>
		private MLAttributesCollection LoadAttributes(XmlReader objXmlReader)
		{
			MLAttributesCollection objColAttributes = new MLAttributesCollection();

				// Obtiene los atributos
				if (objXmlReader.AttributeCount > 0)
				{ // Carga los atributos
					for (int index = 0; index < objXmlReader.AttributeCount; index++)
					{ // Pasa al atributo
						objXmlReader.MoveToAttribute(index);
						// Asigna los valores del atributo
						if (objXmlReader.NodeType == XmlNodeType.Attribute)
							objColAttributes.Add(objXmlReader.Name, objXmlReader.Value);
					}
					// Pasa al primer atributo de nuevo
					objXmlReader.MoveToElement();
				}
				// Devuelve la colección de atributos
				return objColAttributes;
		}

		///// <summary>
		/////		Carga los espacios de nombres
		///// </summary>
		//private MLNameSpacesCollection LoadNameSpaces(XmlAttributeCollection objColXMLAttributes)
		//{ MLNameSpacesCollection objColNameSpaces = new MLNameSpacesCollection();

		//		// Carga los espacios de nombres
		//			if (objColXMLAttributes != null)
		//				foreach (XmlAttribute objXMLAttribute in objColXMLAttributes)
		//					if (objXMLAttribute.Prefix == "xmlns")
		//						{ MLNameSpace objNameSpace = new MLNameSpace(objXMLAttribute.LocalName, Decode(objXMLAttribute.InnerText));

		//								// Añade el espacio de nombres
		//									objColNameSpaces.Add(objNameSpace);
		//						}
		//		// Devuelve los espacios de nombres
		//			return objColNameSpaces;
		//}

		/// <summary>
		///		Decodifica una cadena HTML
		/// </summary>
		private string Decode(string value)
		{ 
			// Quita los caracteres raros
			if (!string.IsNullOrEmpty(value))
			{
				value = value.Replace("&amp;", "&");
				value = value.Replace("&lt;", "<");
				value = value.Replace("&gt;", ">");
				value = value.Replace("&quot;", "\"");
				value = value.Replace("&aacute;", "á");
				value = value.Replace("&eacute;", "é");
				value = value.Replace("&iacute;", "í");
				value = value.Replace("&oacute;", "ó");
				value = value.Replace("&uacute;", "ú");
			}
			// Devuelve la cadena
			return value;
		}

		/// <summary>
		///		Indica si se deben incluir los comentarios en los nodos
		/// </summary>
		public bool IncludeComments { get; set; }
	}
}
