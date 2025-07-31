using System;
using System.Collections.Generic;
using System.Text;

namespace Sistran.Co.Previsora.Application.FullServices.Models
{
	public class MempleadoKeys
	{

		#region Data Members

		double _cod_empleado;

		#endregion

		#region Constructor

		public MempleadoKeys()
		{
				
		}

		public MempleadoKeys(double cod_empleado)
		{
			 _cod_empleado = cod_empleado; 
		}

		#endregion

		#region Properties

		public double  cod_empleado
		{
			 get { return _cod_empleado; }
		}

		#endregion

	}
}
