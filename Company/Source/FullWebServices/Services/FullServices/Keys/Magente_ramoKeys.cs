using System;
using System.Collections.Generic;
using System.Text;

namespace Sistran.Co.Previsora.Application.FullServices.Models
{
	public class Magente_ramoKeys
	{

		#region Data Members

		double _cod_tipo_agente;
		int _cod_agente;
		double _cod_ramo;

		#endregion

		#region Constructor

		public Magente_ramoKeys()
		{
				
		}

		public Magente_ramoKeys(double cod_tipo_agente, int cod_agente, double cod_ramo)
		{
			 _cod_tipo_agente = cod_tipo_agente; 
			 _cod_agente = cod_agente; 
			 _cod_ramo = cod_ramo; 
		}

		#endregion

		#region Properties

		public double  cod_tipo_agente
		{
			 get { return _cod_tipo_agente; }
		}
		public int  cod_agente
		{
			 get { return _cod_agente; }
		}
		public double  cod_ramo
		{
			 get { return _cod_ramo; }
		}

		#endregion

	}
}
