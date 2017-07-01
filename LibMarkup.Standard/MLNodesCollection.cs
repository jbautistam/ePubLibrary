using System;

namespace Bau.Libraries.LibMarkupLanguage
{
	/// <summary>
	///		Colección de <see cref="MLNode"/>
	/// </summary>
	public class MLNodesCollection : MLItemsBaseCollection<MLNode>
	{
		/// <summary>
		///		Añade una serie de nodos a un nodo
		/// </summary>
		public void Add(string name, MLNodesCollection mlNodes)
		{
			MLNode mlNode = Add(name);

				// Añade los nodos
				mlNode.Nodes.AddRange(mlNodes);
		}
	}
}
