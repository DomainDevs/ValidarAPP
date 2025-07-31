using System;
using System.Collections.Generic;
using System.Text;

namespace Sistran.Co.Previsora.Application.FullServices.Models
{
	public class Frm_sarlaft_accionistas_asocKeys
	{

		#region Data Members

		int _id_persona;
		double _nro_asociacion;

		#endregion

		#region Constructor

		public Frm_sarlaft_accionistas_asocKeys()
		{
				
		}

		public Frm_sarlaft_accionistas_asocKeys(int id_persona, double nro_asociacion)
		{
			 _id_persona = id_persona; 
			 _nro_asociacion = nro_asociacion; 
		}

		#endregion

		#region Properties

		public int  id_persona
		{
			 get { return _id_persona; }
		}
		public double  nro_asociacion
		{
			 get { return _nro_asociacion; }
		}

		#endregion

	}
}
