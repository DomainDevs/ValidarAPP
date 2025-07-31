using System;
using System.Collections.Generic;
using System.Text;

namespace Sistran.Co.Previsora.Application.FullServices.Models
{
	public class Mpres_cptoKeys
	{

		#region Data Members

		double _cod_pres       ;
		double _cod_cpto       ;
		double _cod_suc        ;

		#endregion

		#region Constructor

		public Mpres_cptoKeys()
		{
				
		}

		public Mpres_cptoKeys(double cod_pres       , double cod_cpto       , double cod_suc        )
		{
			 _cod_pres        = cod_pres       ; 
			 _cod_cpto        = cod_cpto       ; 
			 _cod_suc         = cod_suc        ; 
		}

		#endregion

		#region Properties

		public double  cod_pres       
		{
			 get { return _cod_pres       ; }
		}
		public double  cod_cpto       
		{
			 get { return _cod_cpto       ; }
		}
		public double  cod_suc        
		{
			 get { return _cod_suc        ; }
		}

		#endregion

	}
}
