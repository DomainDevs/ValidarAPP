using System;
using System.Collections.Generic;
using System.Text;

namespace Sistran.Co.Previsora.Application.FullServices.Models
{
	public class Tvarias_ult_nroKeys
	{

		#region Data Members

		string _txt_nom_tabla;

		#endregion

		#region Constructor

		public Tvarias_ult_nroKeys()
		{
				
		}

		public Tvarias_ult_nroKeys(string txt_nom_tabla)
		{
			 _txt_nom_tabla = txt_nom_tabla; 
		}

		#endregion

		#region Properties

		public string  txt_nom_tabla
		{
			 get { return _txt_nom_tabla; }
		}

		#endregion

	}
}
