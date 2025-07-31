using System;
using System.Collections.Generic;
using System.Text;

namespace Sistran.Co.Previsora.Application.FullServices.Models
{
	public class Magente_organizadorKeys
	{

		#region Data Members

		double _cod_tipo_agente_ppal;
		double _cod_agente_ppal;
		double _cod_tipo_agente;
		int _cod_agente;

		#endregion

		#region Constructor

		public Magente_organizadorKeys()
		{
				
		}

		public Magente_organizadorKeys(double cod_tipo_agente_ppal, double cod_agente_ppal, double cod_tipo_agente, int cod_agente)
		{
			 _cod_tipo_agente_ppal = cod_tipo_agente_ppal; 
			 _cod_agente_ppal = cod_agente_ppal; 
			 _cod_tipo_agente = cod_tipo_agente; 
			 _cod_agente = cod_agente; 
		}

		#endregion

		#region Properties

		public double  cod_tipo_agente_ppal
		{
			 get { return _cod_tipo_agente_ppal; }
		}
		public double  cod_agente_ppal
		{
			 get { return _cod_agente_ppal; }
		}
		public double  cod_tipo_agente
		{
			 get { return _cod_tipo_agente; }
		}
		public int  cod_agente
		{
			 get { return _cod_agente; }
		}

		#endregion

	}
}
