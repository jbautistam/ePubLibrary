using System;

namespace Bau.Libraries.LibMarkupLanguage
{
	/// <summary>
	///		Datos de un nodo
	/// </summary>
	public class MLNode : MLItemBase
	{
		public MLNode() : this(null, null) { }

		public MLNode(string name) : this(name, null) { }

		public MLNode(string name, string value) : base(name, value)
		{
			Attributes = new MLAttributesCollection();
			Nodes = new MLNodesCollection();
			NameSpaces = new MLNameSpacesCollection();
		}

		/// <summary>
		///		Atributos
		/// </summary>
		public MLAttributesCollection Attributes { get; private set; }

		/// <summary>
		///		Nodos
		/// </summary>
		public MLNodesCollection Nodes { get; private set; }

		/// <summary>
		///		Espacios de nombres
		/// </summary>
		public MLNameSpacesCollection NameSpaces { get; private set; }
	}
}
