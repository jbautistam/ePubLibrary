using System;

using Bau.Libraries.LibMarkupLanguage.Services.Tools;

namespace Bau.Libraries.LibMarkupLanguage.Services.JSON
{
	/// <summary>
	///		Intérprete de Json
	/// </summary>
	public class JsonParser : IParser
	{ 
		// Enumerados privados
	  /// <summary>
	  ///		Tipos de token
	  /// </summary>
		private enum TokenType
		{
			/// <summary>Desconocido. No se debería utilizar</summary>
			Unknown,
			/// <summary>Llave de apertura</summary>
			BracketOpen,
			/// <summary>Llave de cierre</summary>
			BracketClose,
			/// <summary>Cadena</summary>
			String,
			/// <summary>Dos puntos</summary>
			Colon,
			/// <summary>Coma</summary>
			Comma,
			/// <summary>Corchete de apertura</summary>
			BraceOpen,
			/// <summary>Corchete de cierre</summary>
			BraceClose,
			/// <summary>Cadena con el valor lógico "True"</summary>
			True,
			/// <summary>Cadena con el valor lógico "False"</summary>
			False,
			/// <summary>Valor numérico</summary>
			Numeric,
			/// <summary>Cadena con el valor "null"</summary>
			Null
		}
		// Variables privadas
		private Token objActualToken;
		private TokenType intIDActualType;

		/// <summary>
		///		Interpreta un archivo
		/// </summary>
		public MLFile Parse(string fileName)
		{
			return new MLFile();
			// return ParseText(LibMarkupLanguage.Tools.FileHelper.LoadTextFile(fileName));
		}

		/// <summary>
		///		Interpreta un texto
		/// </summary>
		public MLFile ParseText(string strText)
		{
			ParserTokenizer objTokenizer = InitTokenizer();
			MLFile mlFile = new MLFile();

			// Inicializa el contenido
			objTokenizer.Init(strText);
			// Interpreta el archivo
			mlFile.Nodes.Add(ParseNode("Root", objTokenizer, true));
			// Devuelve el archivo
			return mlFile;
		}

		/// <summary>
		///		Interpreta un nodo
		/// </summary>
		private MLNode ParseNode(string name, ParserTokenizer objTokenizer, bool blnSearchBracket)
		{
			MLNode mlNode = new MLNode(name);

			// Captura el siguiente nodo
			if (!objTokenizer.IsEof())
			{
				if (blnSearchBracket)
				{   // Obtiene el siguiente token
					GetNextToken(objTokenizer);
					// Debería ser una llave de apertura
					if (intIDActualType == TokenType.BraceOpen)
						mlNode.Nodes.Add(ParseNodesArray(objTokenizer));
					else if (intIDActualType != TokenType.BracketOpen)
						throw new ParserException("Se esperaba una llave de apertura");
					else
						ParseNodeAttributes(objTokenizer, mlNode);
				}
				else
					ParseNodeAttributes(objTokenizer, mlNode);
			}
			// Devuelve el nodo interpretado
			return mlNode;
		}

		/// <summary>
		///		Interpreta los atributos de un nodo "id":"value","id":"value", ... ó "id":{object} ó "id":[array]
		/// </summary>
		private void ParseNodeAttributes(ParserTokenizer objTokenizer, MLNode mlNodeParent)
		{
			bool blnEnd = false;

			// Obtiene los nodos
			while (!objTokenizer.IsEof() && !blnEnd)
			{ // Lee el siguiente Token, debería ser un identificador
				GetNextToken(objTokenizer);
				// Comprueba que sea correcto
				if (intIDActualType == TokenType.BracketClose) // ... es un objeto vacío
					blnEnd = true;
				else if (intIDActualType != TokenType.String) // ... no se ha encontrado el identificador
					throw new ParserException("Se esperaba el identificador del elemento");
				else
				{
					MLAttribute objMLAttribute = new MLAttribute();

					// Asigna el código del atributo
					objMLAttribute.Name = objActualToken.Lexema;
					// Lee el siguiente token. Deberían ser dos puntos
					GetNextToken(objTokenizer);
					// Comprueba que sea correcto
					if (intIDActualType != TokenType.Colon)
						throw new ParserException("Se esperaban dos puntos (separador de identificador / valor)");
					else
					{ // Lee el siguiente token...
						GetNextToken(objTokenizer);
						// Interpreta el valor
						switch (intIDActualType)
						{
							case TokenType.String:
							case TokenType.True:
							case TokenType.False:
							case TokenType.Numeric:
							case TokenType.Null:
								// Asigna el valor al atributo
								switch (intIDActualType)
								{
									case TokenType.Null:
										objMLAttribute.Value = "";
										break;
									case TokenType.String:
										objMLAttribute.Value = ParseUnicode(objActualToken.Lexema);
										break;
									default:
										objMLAttribute.Value = objActualToken.Lexema;
										break;
								}
								// Añade el atributo al nodo
								mlNodeParent.Attributes.Add(objMLAttribute);
								break;
							case TokenType.BracketOpen: // ... definición de objeto
								MLNode mlNode = ParseNode(objMLAttribute.Name, objTokenizer, false);

								// Añade el nodo como objeto
								mlNodeParent.Nodes.Add(mlNode);
								break;
							case TokenType.BraceOpen: // ... definición de array
								mlNodeParent.Nodes.Add(ParseNodesArray(objMLAttribute.Name, objTokenizer));
								break;
							default:
								throw new ParserException("Cadena desconocida. " + objActualToken.Lexema);
						}
					}
					// Lee el siguiente token
					GetNextToken(objTokenizer);
					// Si es una coma, seguir con el siguiente atributo del nodo, si es una llave de cierre, terminar
					switch (intIDActualType)
					{
						case TokenType.Comma:
							// ... no hace nada, simplemente pasa a la creación del siguiente nodo
							break;
						case TokenType.BracketClose:
							blnEnd = true;
							break;
						default:
							throw new ParserException("Cadena desconocida. " + objActualToken.Lexema);
					}
				}
			}
		}

		/// <summary>
		///		Interpreta los nodos de un array
		/// </summary>
		private MLNode ParseNodesArray(ParserTokenizer objTokenizer)
		{
			return ParseNodesArray("Array", objTokenizer);
		}

		/// <summary>
		///		Interpreta los nodos de un array
		/// </summary>
		private MLNode ParseNodesArray(string strNodeParent, ParserTokenizer objTokenizer)
		{
			MLNode mlNode = new MLNode(strNodeParent);
			bool blnEnd = false;
			int index = 0;

			// Obtiene el siguiente token (puede que se trate de un array vacío)
			while (!objTokenizer.IsEof() && !blnEnd)
			{ // Obtiene el siguiente token
				GetNextToken(objTokenizer);
				// Interpreta el nodo
				switch (intIDActualType)
				{
					case TokenType.BracketOpen:
						mlNode.Nodes.Add(ParseNode("Struct", objTokenizer, false));
						break;
					case TokenType.BraceOpen:
						mlNode.Nodes.Add(ParseNodesArray(objTokenizer));
						break;
					case TokenType.String:
					case TokenType.Numeric:
					case TokenType.True:
					case TokenType.False:
					case TokenType.Null:
						mlNode.Nodes.Add("Item", objActualToken.Lexema);
						break;
					case TokenType.Comma: // ... no hace nada, simplemente pasa al siguiente incrementando el índice
						index++;
						break;
					case TokenType.BraceClose: // ... corchete de cierre, indica que ha terminado
						blnEnd = true;
						break;
					default:
						throw new NotImplementedException("No se ha encontrado un token válido ('" + objActualToken.Lexema + "')");
				}
			}
			// Si no se ha encontrado un corchete, lanza una excepción
			if (!blnEnd)
				throw new ParserException("No se ha encontrado el carácter de fin del array ']'");
			// Devuelve la colección de nodos
			return mlNode;
		}

		/// <summary>
		///		Obtiene los datos del siguiente token
		/// </summary>
		private void GetNextToken(ParserTokenizer objTokenizer)
		{
			objActualToken = objTokenizer.GetToken();
			intIDActualType = GetIDType(objActualToken);
		}

		/// <summary>
		///		Obtiene el tipo de un token
		/// </summary>
		private TokenType GetIDType(Token objToken)
		{
			if (objToken != null && objToken.Definition != null)
				return (TokenType) (objToken.Definition.Type ?? 0);
			else
				return TokenType.Unknown;
		}

		/// <summary>
		///		Interpreta una cadena Unicode
		/// </summary>
		private string ParseUnicode(string value)
		{
			int indexOf;
			bool blnEnd = false;

			// Convierte los caracteres Unicode de la cadena
			do
			{ // Indica que ya se ha terminado
				blnEnd = true;
				// Obtiene el índice de la cadena Unicode
				indexOf = value.IndexOf("\\u", StringComparison.CurrentCultureIgnoreCase);
				// Si se ha obtenido algo, comprueba que se puedan coger cuatro caracteres
				if (indexOf >= 0 && indexOf + 6 <= value.Length)
				{
					string strUnicode = value.Substring(indexOf, 6);
					string strFirst;

					// Obtiene la parte izquierda de la cadena (con el carácter Unicode convertido)
					strFirst = value.Substring(0, indexOf) +
											(char) int.Parse(strUnicode.Substring(2), System.Globalization.NumberStyles.HexNumber);
					// Obtiene la parte derecha de la cadena
					if (value.Length >= indexOf + 6)
						value = value.Substring(indexOf + 6);
					// Obtiene la cadena completa
					value = strFirst + value;
					// Indica que debe continuar
					blnEnd = false;
				}
			}
			while (!blnEnd);
			// Convierte los saltos de línea
			value = value.Replace("\n", Environment.NewLine);
			// Devuelve la cadena convertida		
			return value;
		}

		/// <summary>
		///		Inicializa el objeto de creación de tokens
		/// </summary>
		private ParserTokenizer InitTokenizer()
		{
			ParserTokenizer objTokenizer = new ParserTokenizer();

			// Asigna los tokens
			objTokenizer.TokensDefinitions.Add(GetTokenDefinition(TokenType.BracketOpen, "{"));
			objTokenizer.TokensDefinitions.Add(GetTokenDefinition(TokenType.BracketClose, "}"));
			objTokenizer.TokensDefinitions.Add(GetTokenDefinition(TokenType.String, "\"", "\""));
			objTokenizer.TokensDefinitions.Add(GetTokenDefinition(TokenType.Colon, ":"));
			objTokenizer.TokensDefinitions.Add(GetTokenDefinition(TokenType.Comma, ","));
			objTokenizer.TokensDefinitions.Add(GetTokenDefinition(TokenType.BraceOpen, "["));
			objTokenizer.TokensDefinitions.Add(GetTokenDefinition(TokenType.BraceClose, "]"));
			objTokenizer.TokensDefinitions.Add(GetTokenDefinition(TokenType.True, "true"));
			objTokenizer.TokensDefinitions.Add(GetTokenDefinition(TokenType.False, "false"));
			objTokenizer.TokensDefinitions.Add(GetTokenDefinition(TokenType.Null, "null"));
			objTokenizer.TokensDefinitions.Add((int) TokenType.Numeric, "Numeric");
			// Devuelve el objeto de creación de tokens
			return objTokenizer;
		}

		/// <summary>
		///		Definición del token
		/// </summary>
		private TokenDefinition GetTokenDefinition(TokenType intIDToken, string strStart, string strEnd = null)
		{
			return new TokenDefinition((int) intIDToken, intIDToken.ToString(), strStart, strEnd);
		}
	}
}
