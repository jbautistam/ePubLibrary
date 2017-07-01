using System;

using Bau.Libraries.LibCommonHelper.Extensors;
using Bau.Libraries.LibEBook.Formats.ePub.OPF;
using Bau.Libraries.LibEBook.Formats.Extensions.DC;
using Bau.Libraries.LibMarkupLanguage;
using Bau.Libraries.LibMarkupLanguage.Services.XML;

namespace Bau.Libraries.LibEBook.Formats.ePub.Parser
{
	/// <summary>
	///		Intérprete de un paquete
	/// </summary>
	internal static class ePubParserPackage
	{
		/// <summary>
		///		Interpreta la información del paquete de un ePub
		/// </summary>
		internal static void Parse(ePubEBook book, string path)
		{
			foreach (Container.RootFile rootFile in book.Container.RootFiles)
			{ 
				// Limpia los paquetes
				rootFile.Packages.Clear();
				// Añade los paquetes del archivo
				rootFile.Packages.Add(ParsePackage(System.IO.Path.Combine(path, rootFile.URL)));
			}
		}

		/// <summary>
		///		Interpreta un paquete
		/// </summary>
		private static OPFPackage ParsePackage(string fileName)
		{
			OPFPackage package = new OPFPackage();
			MLFile mlFile = new XMLParser().ParseText(LibSystem.Files.HelperFiles.LoadTextFile(fileName));
			MLNode mlNode = mlFile.Nodes [OPFPackageConstants.TagRoot];

				// Interpreta el paquete
				if (mlNode != null)
				{ 
					// Interpreta los  metadatos
					package.Metadata = ParseMetadata(mlNode.Nodes [OPFPackageConstants.TagMetadata]);
					// Interpreta el manifiesto
					ParseManifest(package, mlNode.Nodes [OPFPackageConstants.TagManifest]);
					// Interpreta el índice
					ParseSpine(package, mlNode.Nodes [OPFPackageConstants.TagSpine]);
					// Interpreta el NCX
					ePubParserNCX.Parse(package, System.IO.Path.GetDirectoryName(fileName));
				}
				// Devuelve el paquete
				return package;
		}

		/// <summary>
		///		Interpreta los metadatos
		/// </summary>
		private static Metadata ParseMetadata(MLNode mlNode)
		{
			Metadata metadata = new Metadata();

			// Carga los datos		
			if (mlNode != null)
			{
				metadata.ID = mlNode.Nodes[DublinCoreConstants.TagIdentifier].Value;
				metadata.Title = mlNode.Nodes[DublinCoreConstants.TagTitle].Value;
				metadata.Author = Search(mlNode.Nodes, DublinCoreConstants.TagCreator,
										 OPFConstants.TagAttributesRole,
										 OPFConstants.TagValueRoleAut);
				metadata.Publisher = mlNode.Nodes [DublinCoreConstants.TagPublisher].Value;
				metadata.DateOriginalPublished = Search(mlNode.Nodes,
														DublinCoreConstants.TagDate,
														OPFConstants.TagAttributesEvent,
														OPFConstants.TagValueEventOriginalPublication);
				metadata.DatePublished = Search(mlNode.Nodes,
												DublinCoreConstants.TagDate,
												OPFConstants.TagAttributesEvent,
												OPFConstants.TagValueEventPublication);
				metadata.Subject = mlNode.Nodes[DublinCoreConstants.TagSubject].Value;
				metadata.Source = mlNode.Nodes[DublinCoreConstants.TagSource].Value;
				metadata.Rights = mlNode.Nodes[DublinCoreConstants.TagRights].Value;
				metadata.Language = mlNode.Nodes[DublinCoreConstants.TagLanguage].Value;
			}
			// Devuelve los metadatos
			return metadata;
		}

		/// <summary>
		///		Obtiene el valor del nodo con una etiqueta y un valor particular en un atributo
		/// </summary>
		private static string Search(MLNodesCollection mlNodes, string tag, string attribute, string value)
		{ 
			// Busca la etiqueta en los nodos
			foreach (MLNode mlNode in mlNodes)
				if (mlNode.Name == tag)
				{
					MLAttribute mlAttribute = mlNode.Attributes.Search(attribute);

						if (mlAttribute?.Value == attribute)
							return mlNode.Value;
				}
			// Si ha llegado hasta aquí es porque no ha encontrado nada
			return null;
		}

		/// <summary>
		///		Interpreta el manifiesto del libro
		/// </summary>
		private static void ParseManifest(OPFPackage package, MLNode mlNode)
		{ 
			// Limpia el manifiesto
			package.Manifest.Clear();
			// Carga los datos
			if (mlNode != null)
				foreach (MLNode mlChild in mlNode.Nodes)
					if (mlChild.Name == OPFPackageConstants.TagManifestItem)
					{
						Item item = new Item();

							// Interpreta el nodo
							item.ID = mlChild.Attributes [OPFPackageConstants.TagManifestID].Value;
							item.URL = mlChild.Attributes [OPFPackageConstants.TagManifestHref].Value;
							item.MediaType = mlChild.Attributes [OPFPackageConstants.TagManifestMediatype].Value;
							// Añade el elemento
							package.Manifest.Add(item);
					}
		}

		/// <summary>
		///		Interpreta el índice del libro
		/// </summary>
		private static void ParseSpine(OPFPackage package, MLNode mlNode)
		{ 
			// Limpia el índice
			package.Spine.Clear();
			// Carga los datos
			if (mlNode != null)
				foreach (MLNode mlChild in mlNode.Nodes)
					if (mlChild.Name == OPFPackageConstants.TagSpineItemRef)
					{
						ItemRef item = new ItemRef();

						// Interpreta el nodo
						item.IDRef = mlChild.Attributes [OPFPackageConstants.TagSpineIdRef].Value;
						item.IsLinear = mlChild.Attributes[OPFPackageConstants.TagSpineLinear].Value.GetBool(true);
						// Añade el elemento
						package.Spine.Add(item);
					}
		}
	}
}
