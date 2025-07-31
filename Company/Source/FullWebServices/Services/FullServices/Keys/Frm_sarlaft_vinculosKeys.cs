using System;
using System.Collections.Generic;
using System.Text;

namespace Sistran.Co.Previsora.Application.FullServices.Models
{
	public class Frm_sarlaft_vinculosKeys
	{

		#region Data Members

		int _id_persona;

		#endregion

		#region Constructor

		public Frm_sarlaft_vinculosKeys()
		{
				
		}

		public Frm_sarlaft_vinculosKeys(int id_persona)
		{
			 _id_persona = id_persona; 
		}

		#endregion

		#region Properties

		public int  id_persona
		{
			 get { return _id_persona; }
		}

		#endregion

	}
}
