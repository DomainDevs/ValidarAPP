using System;
using System.Collections.Generic;
using System.Text;

namespace Sistran.Co.Previsora.Application.FullServices.Models
{
	public class Magente_productoKeys
	{

		#region Data Members

		double _cod_tipo_agente;
		int _cod_agente;
		double _cod_producto;

		#endregion

		#region Constructor

		public Magente_productoKeys()
		{
				
		}

		public Magente_productoKeys(double cod_tipo_agente, int cod_agente, double cod_producto)
		{
			 _cod_tipo_agente = cod_tipo_agente; 
			 _cod_agente = cod_agente; 
			 _cod_producto = cod_producto; 
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
		public double  cod_producto
		{
			 get { return _cod_producto; }
		}

		#endregion

	}
}
