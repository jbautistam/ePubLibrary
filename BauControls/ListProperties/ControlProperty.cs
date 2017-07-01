using System;
using System.Drawing;
using System.Windows.Forms;

namespace Bau.Controls.ListProperties
{
	/// <summary>
	///		Control para mostrar un valor
	/// </summary>
	public partial class ControlProperty : UserControl
	{ // Variables privadas
			private Control ctlControl = null;
			
		public ControlProperty()
		{	InitializeComponent();
		}

		/// <summary>
		///		Carga un valor
		/// </summary>
		internal void LoadValue(string strKey, ListProperties.ControlType type, string title, string value, bool blnRequired)
		{ // Obtiene la descripción del resultado
				Key = strKey;
				ControlType = type;
				if (!string.IsNullOrEmpty(title))
					lblName.Text = title + ":";
				else
					lblName.Text = "";
				IsRequired = blnRequired;
			// Crea el control
				ctlControl = LoadControl(value);
		}

		/// <summary>
		///		Crea e inicializa el control
		/// </summary>
		private Control LoadControl(string value)
		{ Control ctlNewControl = null;

					// Crea el control		
						switch (ControlType)
							{	case ListProperties.ControlType.Label:
										ctlNewControl = CreateControlLabel(value);
									break;
								case ListProperties.ControlType.CheckBox:
										ctlNewControl = CreateControlBoolean(value);
									break;
								case ListProperties.ControlType.Date:
										ctlNewControl = CreateControlDate(value);
									break;
								case ListProperties.ControlType.List:
										ctlNewControl = CreateControlList(value);
									break;
								case ListProperties.ControlType.Number:
										ctlNewControl = CreateControlNumber(value);
									break;
								case ListProperties.ControlType.MultiLine:
										ctlNewControl = CreateControlMultiline(value);
									break;
								case ListProperties.ControlType.TextBox:
										ctlNewControl = CreateControlText(value);
									break;
								default:
									throw new NotImplementedException("Tipo de resultado no definido");
							}
					// Si realmente se ha creado un control
						if (ctlNewControl != null)
							{	// Elimina el control antiguo (si no lo hiciera así, no se vería el control nuevo
								// porque el antiguo está ajustado a toda la celda)
									if (this.ctlControl != null)
										this.ctlControl.Dispose();
								// Añade el control
									Controls.Add(ctlNewControl);
									ctlNewControl.Parent = tblValue;
									tblValue.SetColumn(ctlNewControl, 1);
								// Ajusta el control a toda la celda
									ctlNewControl.Dock = DockStyle.Fill;
									ctlNewControl.Visible = true;
								// Cambia el color de fondo si es obligatorio
									if (IsRequired)
										ctlNewControl.BackColor = Color.FromArgb(255, 255, 192);
							}
					// Devuelve el control
						return ctlNewControl;
		}

		/// <summary>
		///		Crea un control de etiqueta
		/// </summary>
		private Control CreateControlLabel(string value)
		{ System.Windows.Forms.Label lblControl = new System.Windows.Forms.Label();

				// Inicializa las propiedades
					lblControl.Text = value;
				// Devuelve el control
					return lblControl;
		}

		/// <summary>
		///		Crea un control lógico
		/// </summary>
		private Control CreateControlBoolean(string value)
		{ CheckBox chkControl = new CheckBox();
			bool blnValue;

				// Convierte el valor
					if (!bool.TryParse(value, out blnValue))
						blnValue = false;		
				// Inicializa las propiedades
					chkControl.Text = "";
				// Inicializa el valor
					chkControl.Checked = blnValue;
				// Devuelve el control
					return chkControl;
		}
		
		/// <summary>
		///		Crea un control de fecha
		/// </summary>
		private Control CreateControlDate(string value)
		{ DateTimeSelect.DateTimePickerExtended fudControl = new DateTimeSelect.DateTimePickerExtended();

				// Inicializa las propiedades
					if (!string.IsNullOrEmpty(value))
						{ DateTime dtmValue;

								// Asigna el valor
									if (!DateTime.TryParse(value, out dtmValue))
										fudControl.Value = dtmValue;
						}
				// Devuelve el control
					return fudControl;
		}

		/// <summary>
		///		Crea un control de lista
		/// </summary>
		private Control CreateControlList(string value)
		{ Combos.ComboBoxExtended cboList = new Combos.ComboBoxExtended();
			int intValue = 0;

				// Obtiene el valor
					if (!int.TryParse(value, out intValue))
						intValue = 0;
				//// Carga los elementos de la lista
				//	objColListItems.LoadByIDValue(objValue.ID);
				//// Asigna las propiedades
				//	if (ModuleValueDefinition.IsRequired)
				//		cboList.BackColor = Color.FromArgb(255, 255, 192);
				//// Muestra los elementos en el combo
				//	cboList.AddItem(clsValueDefinitionListItem.IntNull, "<Seleccione un elemento>");
				//	foreach (clsValueDefinitionListItem objListItem in objColListItems)
				//		cboList.AddItem(objListItem.ID, objListItem.Description);
				// Selecciona el elemento
					cboList.SelectedID = (int) intValue;
				// Devuelve el combo
					return cboList;
		}
		
		/// <summary>
		///		Crea un control numérico
		/// </summary>
		private Control CreateControlNumber(string value)
		{ TextBox.NumericUpDowExtended nudControl = new TextBox.NumericUpDowExtended();
			double dblValue = 0;

				// Obtiene el valor
					if (double.TryParse(value, out dblValue))
						dblValue = 0;
				// Asigna el valor
					nudControl.Value = (decimal) dblValue;
				// Devuelve el control
					return nudControl;
		}
		
		/// <summary>
		///		Crea un control de texto
		/// </summary>
		private Control CreateControlText(string value)
		{ System.Windows.Forms.TextBox txtControl = new System.Windows.Forms.TextBox();
		
				// Asigna el resultado
					txtControl.Text = value;
				// Devuelve el control
					return txtControl;
		}
		
		/// <summary>
		///		Crea un control de texto multilínea
		/// </summary>
		private Control CreateControlMultiline(string value)
		{ System.Windows.Forms.TextBox txtControl = new System.Windows.Forms.TextBox();
		
				// Indica que es un control multilínea y cambia el alto
					txtControl.Multiline = true;
					txtControl.ScrollBars = ScrollBars.Vertical;
					txtControl.Height = 3 * txtControl.Height;
					Height = txtControl.Height;
				// Asigna el resultado
					txtControl.Text = value;
				// Devuelve el control
					return txtControl;
		}
		
		/// <summary>
		///		Comprueba los datos introducidos en el control
		/// </summary>
		internal bool Validate(out string strError)
		{ string value;
			bool blnValidate = false;
		
				// Inicializa los argumentos de salida
					strError = null;
				// Obtiene el resultado
					value = GetResultFromControl();
				// Comprueba el error
					if (ControlType != ListProperties.ControlType.Label && string.IsNullOrEmpty(value) && IsRequired)
						strError = "El valor de " + lblName.Text + " es obligatorio";
					else
						blnValidate = true;
				// Devuelve el valor que indica si los datos son correctos
					return blnValidate;
		}
		
		/// <summary>
		///		Obtiene el resultado introducido en el control
		/// </summary>
		private string GetResultFromControl()
		{	string strResult = "";

				// Obtiene el valor
					switch (ControlType)
						{	case ListProperties.ControlType.Label:
									strResult = (ctlControl as System.Windows.Forms.Label).Text;
								break;
							case ListProperties.ControlType.CheckBox:
									strResult = (ctlControl as CheckBox).Checked.ToString();	
								break;
							case ListProperties.ControlType.Date:
									strResult = (ctlControl as DateTimeSelect.DateTimePickerExtended).Value?.ToString();
								break;
							case ListProperties.ControlType.TextBox:
									strResult = (ctlControl as System.Windows.Forms.TextBox).Text;
								break;
							case ListProperties.ControlType.List:
									strResult = (ctlControl as Combos.ComboBoxExtended).SelectedID?.ToString();
								break;
							case ListProperties.ControlType.MultiLine:
									strResult = (ctlControl as System.Windows.Forms.TextBox).Text;
								break;
							case ListProperties.ControlType.Number:
									strResult = ((double) (ctlControl as TextBox.NumericUpDowExtended).Value).ToString();
								break;
							default:
								throw new NotImplementedException("Tipo de valor no definido");
						}
				// Devuelve el resultado
					return strResult;
		}

		/// <summary>
		///		Clave del control
		/// </summary>
		internal string Key { get; private set; }		

		/// <summary>
		///		Tipo de control
		/// </summary>
		internal ListProperties.ControlType ControlType { get; private set; }

		/// <summary>
		///		Indica si el valor es obligatorio
		/// </summary>
		internal bool IsRequired { get; private set; }
		
		/// <summary>
		///		Obtiene el valor seleccionado en el control
		/// </summary>
		internal string Value
		{ get { return GetResultFromControl(); }
		}
	}
}
