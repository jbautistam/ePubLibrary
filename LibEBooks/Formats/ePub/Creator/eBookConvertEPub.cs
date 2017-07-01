using System;

using Bau.Libraries.LibEBook.Formats.eBook;

namespace Bau.Libraries.LibEBook.Formats.ePub.Creator
{
	/// <summary>
	///		Conversor del formato eBook al formato ePub
	/// </summary>
	internal class eBookConvertEPub
	{
		/// <summary>
		///		Convierte un archivo eBook al formato ePub
		/// </summary>
		internal ePubEBook Convert(Book eBook)
		{
			ePubEBook ePub = new ePubEBook();
			OPF.OPFPackage package = CreatePackage(ePub);
			NCX.NCXFile ncxFile = new NCX.NCXFile();

				// Asigna los metadatos
				package.Metadata.Author = eBook.Author;
				package.Metadata.DateOriginalPublished = eBook.DateOriginalPublished;
				package.Metadata.DatePublished = eBook.DatePublished;
				package.Metadata.Language = eBook.Language;
				package.Metadata.Publisher = eBook.Publisher;
				package.Metadata.Rights = eBook.Rights;
				package.Metadata.Source = eBook.Source;
				package.Metadata.Subject = eBook.Subject;
				package.Metadata.Title = eBook.Title;
				// Normaliza los IDs de páginas
				NormalizePagesID(eBook);
				// Añade las páginas
				foreach (PageFile page in eBook.Files)
				{
					OPF.Item item = new OPF.Item();

						// Asigna los datos
						item.ID = page.ID;
						item.MediaType = page.MediaType;
						item.URL = page.FileName;
						// Añade la página
						package.Manifest.Add(item);
				}
				// Añade el spine
				package.Spine.AddRange(GetSpine(eBook, eBook.Index));
				// Añade las tablas de contenido al índice
				ncxFile.Pages.AddRange(GetNavPoints(eBook.TableOfContent));
				// Añade el índice al paquete
				package.TablesOfContents.Add(ncxFile);
				// Devuelve el archivo en formato ePub
				return ePub;
		}

		/// <summary>
		///		Normaliza los IDs de las páginas
		/// </summary>
		private void NormalizePagesID(Book eBook)
		{
			int index = 1;

				// Recorre las páginas
				foreach (PageFile page in eBook.Files)
				{
					string newId = "Page" + (index++).ToString();

						// Cambia los IDs de los índices
						ChangeIndexID(page.ID, newId, eBook.Index);
						ChangeIndexID(page.ID, newId, eBook.TableOfContent);
						// Cambia el ID de la página
						page.ID = newId;
				}
		}

		/// <summary>
		///		Cambia el índice de una página
		/// </summary>
		private void ChangeIndexID(string id, string newId, IndexItemsCollection indexItems)
		{
			foreach (IndexItem index in indexItems)
			{ 
				// Cambia el ID de la página
				if (index.IDPage.Equals(id, StringComparison.CurrentCultureIgnoreCase))
					index.IDPage = newId;
				// Cambia el ID de los índices internos
				ChangeIndexID(id, newId, index.Items);
			}
		}

		/// <summary>
		///		Obtiene los elementos para el índice principal
		/// </summary>
		private OPF.ItemsRefCollection GetSpine(Book eBook, IndexItemsCollection indexItems)
		{
			OPF.ItemsRefCollection itemsRef = new OPF.ItemsRefCollection();

				// Añade los elementos del índice
				foreach (IndexItem index in indexItems)
				{
					PageFile page = eBook.Files.SearchByFileName(index.URL);

						// Añade la página al índice
						if (page != null)
						{
							OPF.ItemRef itemRef = new OPF.ItemRef();

							// Asigna los datos
							itemRef.ID = index.ID;
							itemRef.IDRef = page.ID;
							itemRef.IsLinear = true;
							// Añade la referencia al archivo
							itemsRef.Add(itemRef);
						}
						// Añade las páginas hija
						itemsRef.AddRange(GetSpine(eBook, index.Items));
				}
				// Devuelve la colección
				return itemsRef;
		}

		/// <summary>
		///		Crea los puntos de navegación
		/// </summary>
		private NCX.NavPointsCollection GetNavPoints(IndexItemsCollection indexItems)
		{
			NCX.NavPointsCollection navPoints = new NCX.NavPointsCollection();

				// Crea los índices
				for (int index = 0; index < indexItems.Count; index++)
				{
					NCX.NavPoint navPoint = new NCX.NavPoint();

						// Asigna las propiedades
						navPoint.ID = indexItems[index].ID;
						navPoint.Order = index + 1;
						navPoint.Title = indexItems[index].Name;
						navPoint.URL = indexItems[index].URL;
						// Añade los índices
						navPoint.Pages.AddRange(GetNavPoints(indexItems[index].Items));
						// Añade el navPoint a la colección
						navPoints.Add(navPoint);
				}
				// Devuelve el índice
				return navPoints;
		}

		/// <summary>
		///		Crea el paquete
		/// </summary>
		private OPF.OPFPackage CreatePackage(ePubEBook ePub)
		{
			Container.RootFile rootFile = new Container.RootFile();
			OPF.OPFPackage package = new OPF.OPFPackage();

				// Añade el paquete al rootFile
				rootFile.Packages.Add(package);
				// Añade el rootFile
				ePub.Container.RootFiles.Add(rootFile);
				// Devuelve el paquete
				return package;
		}
	}
}
