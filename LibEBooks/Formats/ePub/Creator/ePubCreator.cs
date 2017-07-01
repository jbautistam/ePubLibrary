using System;
using System.IO;

using Bau.Libraries.LibCompressor;
using Bau.Libraries.LibEBook.Formats.ePub.Container;
using Bau.Libraries.LibEBook.Formats.ePub.NCX;
using Bau.Libraries.LibEBook.Formats.ePub.OPF;
using Bau.Libraries.LibCommonHelper.Extensors;
using Bau.Libraries.LibSystem.Files;
using Bau.Libraries.LibMarkupLanguage;
using Bau.Libraries.LibMarkupLanguage.Services.XML;

namespace Bau.Libraries.LibEBook.Formats.ePub.Creator
{
	/// <summary>
	///		Creador de archivos ePub
	/// </summary>
	public class ePubCreator
	{
		/// <summary>
		///		Crea un archivo ePub a partir de un objeto <see cref="eBook.Book"/>
		/// </summary>
		public void Create(string fileName, eBook.Book eBook)
		{
			Create(fileName, new eBookConvertEPub().Convert(eBook));
		}

		/// <summary>
		///		Crea un archivo ePub
		/// </summary>
		public void Create(string fileName, ePubEBook eBook)
		{
			string pathTemporal = Path.Combine(Path.GetDirectoryName(fileName),
											   "temp" + Path.GetFileNameWithoutExtension(fileName));
			Compressor objCompressor = new Compressor();

				// Crea el directorio raíz
				HelperFiles.MakePath(pathTemporal);
				// Crea el archivo mimeType
				HelperFiles.SaveTextFile(Path.Combine(pathTemporal, "mimetype"), "application/epub+zip", System.Text.Encoding.Default);
				// Crea el archivo Container.xml
				CreateContainer(pathTemporal, eBook);
				// Crea los archivos de contenido
				CreateContent(pathTemporal, eBook);
				// Comprime el directorio
				objCompressor.Compress(fileName, pathTemporal);
				// Elimina el directorio temporal
				HelperFiles.KillPath(pathTemporal);
		}

		/// <summary>
		///		Crea el archivo contenedor (container.xml) en el directorio META-INF
		/// </summary>
		private void CreateContainer(string path, ePubEBook eBook)
		{
			string pathMeta = Path.Combine(path, "META-INF");
			MLFile mlFile = new MLFile();
			MLNode mlRoot = mlFile.Nodes.Add(ContainerConstants.TagRoot);
			MLNode mlRootFiles;

				// Asigna la codificación al archivo
				mlFile.Encoding = MLFile.EncodingMode.UTF8;
				// Añade los atributos al nodo raíz
				mlRoot.Attributes.Add(ContainerConstants.TagRootVersion, ContainerConstants.TagRootVersionValue);
				mlRoot.Attributes.Add(ContainerConstants.TagRootNameSpace, ContainerConstants.TagRootNameSpaceValue);
				// Añade el nodo de archivos raíz
				mlRootFiles = mlRoot.Nodes.Add(ContainerConstants.TagRootFilesRoot);
				// Añade los rootFiles
				for (int index = 0; index < eBook.Container.RootFiles.Count; index++)
				{
					MLNode mlRootFile = mlRootFiles.Nodes.Add(ContainerConstants.TagRootFileRoot);

						// Añade los atributos al nodo rootFile
						mlRootFile.Attributes.Add(ContainerConstants.TagRootFilePath, GetRootFileOPFFileName(index));
						mlRootFile.Attributes.Add(ContainerConstants.TagRootFileMediaType,
												  ContainerConstants.TagRootFileMediaTypeValue);
				}
				// Graba el archivo
				SaveMLFile(Path.Combine(pathMeta, "container.xml"), mlFile);
		}

		/// <summary>
		///		Crea los archivos de contenido
		/// </summary>
		private void CreateContent(string path, ePubEBook eBook)
		{
			int index = 0;

				foreach (RootFile rootFile in eBook.Container.RootFiles)
				{
					string pathPackage = Path.Combine(path, GetRootFilePath(index).Replace("/", "\\"));

						// Crea el directorio
						HelperFiles.MakePath(pathPackage);
						// Crea el paquete OPF
						foreach (OPFPackage package in rootFile.Packages)
						{ 
							// Copia los archivos
							CopyFiles(pathPackage, package.Manifest);
							// Crea el archivo OPF
							CreateOPF(index, pathPackage, package);
							// Crea el archivo NCX
							CreateNCX(pathPackage, package);
						}
						// Incrementa el índice de rootFile
						index++;
				}
		}

		/// <summary>
		///		Copia los archivos
		/// </summary>
		private void CopyFiles(string path, ItemsCollection items)
		{
			foreach (Item item in items)
				HelperFiles.CopyFile(item.URL, Path.Combine(path, Path.GetFileName(item.URL)));
		}

		/// <summary>
		///		Crea el archivo del paquete OPF
		/// </summary>
		private void CreateOPF(int rootFile, string path, OPFPackage package)
		{
			MLFile mlFile = new MLFile();
			MLNode mlRoot = mlFile.Nodes.Add(OPFPackageConstants.TagRoot);

				// Añade los espacios de nombres
				mlRoot.NameSpaces.Add("", OPFPackageConstants.TagRootNameSpace);
				// Añade los atributos
				mlRoot.Attributes.Add(OPFPackageConstants.TagRootVersion, OPFPackageConstants.TagRootVersionValue);
				mlRoot.Attributes.Add(OPFPackageConstants.TagRootUniqueIdentifier,
									  OPFPackageConstants.TagRootUniqueIdentifierValue);
				// Añade los metadatos
				mlRoot.Nodes.Add(GetMetadata(package));
				// Añade el manifiesto con los archivos
				mlRoot.Nodes.Add(GetManifest(package));
				// Añade el spine
				mlRoot.Nodes.Add(GetSpine(package));
				// Graba el archivo
				SaveMLFile(Path.Combine(path, Path.GetFileName(GetRootFileOPFFileName(rootFile).Replace("/", "\\"))), 
						   mlFile);
		}

		/// <summary>
		///		Obtiene el nodo con los metadatos del paquete
		/// </summary>
		private MLNode GetMetadata(OPFPackage package)
		{
			MLNode mlNode = new MLNode(OPFPackageConstants.TagMetadata);

				// Añade los espacios de nombres
				mlNode.NameSpaces.Add(OPFPackageConstants.TagRootNameSpacePrefix, OPFPackageConstants.TagRootNameSpace);
				mlNode.NameSpaces.Add(Extensions.DC.DublinCoreConstants.NameSpacePrefix,
									  Extensions.DC.DublinCoreConstants.NameSpace);
				// Añade los metadatos
				mlNode.Nodes.Add(Extensions.DC.DublinCoreConstants.NameSpacePrefix,
								 Extensions.DC.DublinCoreConstants.TagRights,
								 package.Metadata.Rights, true);
				mlNode.Nodes.Add(Extensions.DC.DublinCoreConstants.NameSpacePrefix,
								 Extensions.DC.DublinCoreConstants.TagIdentifier,
								 package.Metadata.ID, true);
				mlNode.Nodes.Add(Extensions.DC.DublinCoreConstants.NameSpacePrefix,
								 Extensions.DC.DublinCoreConstants.TagCreator,
								 package.Metadata.Author, true);
				mlNode.Nodes.Add(Extensions.DC.DublinCoreConstants.NameSpacePrefix,
								 Extensions.DC.DublinCoreConstants.TagTitle,
								 package.Metadata.Title, true);
				mlNode.Nodes.Add(Extensions.DC.DublinCoreConstants.NameSpacePrefix,
								 Extensions.DC.DublinCoreConstants.TagLanguage,
								 package.Metadata.Language, true);
				mlNode.Nodes.Add(Extensions.DC.DublinCoreConstants.NameSpacePrefix,
								 Extensions.DC.DublinCoreConstants.TagSubject,
								 package.Metadata.Subject, true);
				mlNode.Nodes.Add(Extensions.DC.DublinCoreConstants.NameSpacePrefix,
								 Extensions.DC.DublinCoreConstants.TagDate,
								 ConvertDate(package.Metadata.DateOriginalPublished), false);
				mlNode.Nodes.Add(Extensions.DC.DublinCoreConstants.NameSpacePrefix,
								 Extensions.DC.DublinCoreConstants.TagDate,
								 ConvertDate(package.Metadata.DatePublished), false);
				mlNode.Nodes.Add(Extensions.DC.DublinCoreConstants.NameSpacePrefix,
								 Extensions.DC.DublinCoreConstants.TagSource,
								 package.Metadata.Source, true);
				// Devuelve el nodo
				return mlNode;
		}

		/// <summary>
		///		Convierte la fecha
		/// </summary>
		private string ConvertDate(string date)
		{
			return $"{date.GetDateTime(DateTime.Now):yyyy-MM-dd}";
		}

		/// <summary>
		///		Obtiene el manifiesto
		/// </summary>
		private MLNode GetManifest(OPFPackage package)
		{
			MLNode mlNode = new MLNode(OPFPackageConstants.TagManifest);

				// Añade los archivos
				foreach (Item item in package.Manifest)
					mlNode.Nodes.Add(GetNodeItem(item.ID, Path.GetFileName(item.URL), item.MediaType));
				// Añade el archivo NCX que se va a crear
				for (int index = 0; index < package.TablesOfContents.Count; index++)
					mlNode.Nodes.Add(GetNodeItem("ncx", GetNCXFileName(index), NCXConstants.MediaType));
				// Devuelve el nodo
				return mlNode;
		}

		/// <summary>
		///		Obtiene un nodo con un elemento (Item)
		/// </summary>
		private MLNode GetNodeItem(string id, string url, string mediaType)
		{
			MLNode mlItem = new MLNode(OPFPackageConstants.TagManifestItem);

				// Añade los atributos
				mlItem.Attributes.Add(OPFPackageConstants.TagManifestID, id);
				mlItem.Attributes.Add(OPFPackageConstants.TagManifestHref, url);
				mlItem.Attributes.Add(OPFPackageConstants.TagManifestMediatype, mediaType);
				// Devuelve el nodo
				return mlItem;
		}

		/// <summary>
		///		Obtiene el índice principal
		/// </summary>
		private MLNode GetSpine(OPFPackage package)
		{
			MLNode mlNode = new MLNode(OPFPackageConstants.TagSpine);

				// Añade los atributos
				mlNode.Attributes.Add(OPFPackageConstants.TagSpineToc, OPFPackageConstants.TagSpineTocValue);
				// Añade las referencias
				foreach (ItemRef item in package.Spine)
				{
					MLNode mlItem = new MLNode(OPFPackageConstants.TagSpineItemRef);

						// Añade los atributos
						mlItem.Attributes.Add(OPFPackageConstants.TagSpineIdRef, item.IDRef);
						mlItem.Attributes.Add(OPFPackageConstants.TagSpineLinear, item.IsLinear);
						// Añade el nodo
						mlNode.Nodes.Add(mlItem);
				}
				// Devuelve el nodo
				return mlNode;
		}

		/// <summary>
		///		Crea el archivo NCX del paquete
		/// </summary>
		private void CreateNCX(string path, OPFPackage package)
		{
			int index = 0;

				foreach (NCXFile ncxFile in package.TablesOfContents)
				{
					MLFile mlFile = new MLFile();
					MLNode mlRoot = mlFile.Nodes.Add(NCXConstants.TagRoot);
					MLNode mlTitle;

						// Añade los atributos
						mlRoot.Attributes.Add(NCXConstants.TagRootAttrNameSpace, NCXConstants.TagRootAttrNameSpaceValue);
						mlRoot.Attributes.Add(NCXConstants.TagRootAttrVersion, NCXConstants.TagRootAttrVersionValue);
						mlRoot.Attributes.Add(NCXConstants.TagRootAttrNameSpace, NCXConstants.TagRootAttrLanguage,
																		 NCXConstants.TagRootAttrLanguageSpanish, false);
						// Añade el nodo con la cabecera
						mlRoot.Nodes.Add(NCXConstants.TagHead);
						// Añade el nodo con el título
						mlTitle = mlRoot.Nodes.Add(NCXConstants.TagDocTitle);
						mlTitle.Nodes.Add(NCXConstants.TagText, package.Metadata.Title);
						// Añade el índice
						mlRoot.Nodes.Add(NCXConstants.TagNavMap).Nodes.Add(GetNodesNavPoint(ncxFile.Pages));
						// Graba el archivo
						SaveMLFile(Path.Combine(path, GetNCXFileName(index)), mlFile);
						// Incrementa el índice
						index++;
				}
		}

		/// <summary>
		///		Obtiene el nodo de una colección de puntos de navegación
		/// </summary>
		private MLNodesCollection GetNodesNavPoint(NavPointsCollection navPoints)
		{
			MLNodesCollection mlNodes = new MLNodesCollection();

				// Añade los puntos de navegación
				foreach (NavPoint navPoint in navPoints)
				{
					MLNode objMLNavPoint = new MLNode(NCXConstants.TagNavPoint);
					MLNode objMLContent;

						// Añade los atributos
						objMLNavPoint.Attributes.Add(NCXConstants.TagNavPointID, navPoint.ID);
						objMLNavPoint.Attributes.Add(NCXConstants.TagNavPointOrder, navPoint.Order.ToString());
						// Añade la etiqueta
						objMLNavPoint.Nodes.Add(NCXConstants.TagNavPointLabel).Nodes.Add(NCXConstants.TagText,
																						 navPoint.Title);
						// Añade el contenido
						objMLContent = objMLNavPoint.Nodes.Add(NCXConstants.TagNavPointContent);
						objMLContent.Attributes.Add(NCXConstants.TagNavPointContentSrc,
													Path.GetFileName(navPoint.URL));
						// Añade los hijos
						objMLNavPoint.Nodes.Add(GetNodesNavPoint(navPoint.Pages));
						// Añade el nodo a la colección
						mlNodes.Add(objMLNavPoint);
				}
				// Devuelve la colección de nodos
				return mlNodes;
		}

		/// <summary>
		///		Obtiene el nombre del directorio para los RootFiles
		/// </summary>
		private string GetRootFilePath(int index)
		{
			if (index == 0)
				return "OPS";
			else
				return "OPS" + index.ToString();
		}

		/// <summary>
		///		Obtiene el nombre del archivo OPF
		/// </summary>
		private string GetRootFileOPFFileName(int index)
		{
			if (index == 0)
				return "OPS/ebook.opf";
			else
				return "OPS" + index.ToString() + "/ebook.opf";
		}

		/// <summary>
		///		Obtiene el nombre de un archivo NCX
		/// </summary>
		private string GetNCXFileName(int index)
		{
			if (index == 0)
				return "toc.ncx";
			else
				return "toc" + index.ToString() + ".ncx";
		}

		/// <summary>
		///		Graba un archivo XML
		/// </summary>
		private void SaveMLFile(string fileName, MLFile mlFile)
		{
			HelperFiles.MakePath(Path.GetDirectoryName(fileName));
			HelperFiles.SaveTextFile(fileName, new XMLWriter().ConvertToString(mlFile));
		}
	}
}
