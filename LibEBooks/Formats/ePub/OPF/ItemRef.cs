using System;

namespace Bau.Libraries.LibEBook.Formats.ePub.OPF
{
	/// <summary>
	///		Referencia a un archivo
	/// </summary>
	public class ItemRef : Base.eBookBase
	{
		/// <summary>
		///		ID del elemento al que hace referencia
		/// </summary>
		public string IDRef { get; set; }
		
		/// <summary>
		///		Indica si es lineal o no
		/// </summary>
		public bool IsLinear { get; set; }
	}
}
