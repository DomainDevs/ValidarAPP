using System;
using System.Collections.Generic;
using System.Text;

namespace Sistran.Co.Previsora.Application.FullServices.Models
{
	public class Mpersona_actividadKeys
	{

		#region Data Members

		int _id_persona;
		double _cod_abona;
		double _cod_dpto;
		double _cod_munic;
		double _cod_actividad;

		#endregion

		#region Constructor

		public Mpersona_actividadKeys()
		{
				
		}

		public Mpersona_actividadKeys(int id_persona, double cod_abona, double cod_dpto, double cod_munic, double cod_actividad)
		{
			 _id_persona = id_persona; 
			 _cod_abona = cod_abona; 
			 _cod_dpto = cod_dpto; 
			 _cod_munic = cod_munic; 
			 _cod_actividad = cod_actividad; 
		}

		#endregion

		#region Properties

		public int  id_persona
		{
			 get { return _id_persona; }
		}
		public double  cod_abona
		{
			 get { return _cod_abona; }
		}
		public double  cod_dpto
		{
			 get { return _cod_dpto; }
		}
		public double  cod_munic
		{
			 get { return _cod_munic; }
		}
		public double  cod_actividad
		{
			 get { return _cod_actividad; }
		}

		#endregion

	}
}
