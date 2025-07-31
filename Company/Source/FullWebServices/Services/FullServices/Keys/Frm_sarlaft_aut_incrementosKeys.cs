using System;
using System.Collections.Generic;
using System.Text;

namespace Sistran.Co.Previsora.Application.FullServices.Models
{
	public class Frm_sarlaft_aut_incrementosKeys
	{

		#region Data Members

		int _id_formulario;

		#endregion

		#region Constructor

		public Frm_sarlaft_aut_incrementosKeys()
		{
				
		}

		public Frm_sarlaft_aut_incrementosKeys(int id_formulario)
		{
			 _id_formulario = id_formulario; 
		}

		#endregion

		#region Properties

		public int  id_formulario
		{
			 get { return _id_formulario; }
		}

		#endregion

	}
}
