using System;
using System.Collections.Generic;
using System.Text;

namespace Sistran.Co.Previsora.Application.FullServices.Models
{
	public class Mpersona_impuestoKeys
	{

		#region Data Members

		int _id_persona     ;
		double _cod_abona      ;
		double _cod_impuesto   ;

		#endregion

		#region Constructor

		public Mpersona_impuestoKeys()
		{
				
		}

		public Mpersona_impuestoKeys(int id_persona     , double cod_abona      , double cod_impuesto   )
		{
			 _id_persona      = id_persona     ; 
			 _cod_abona       = cod_abona      ; 
			 _cod_impuesto    = cod_impuesto   ; 
		}

		#endregion

		#region Properties

		public int  id_persona     
		{
			 get { return _id_persona     ; }
		}
		public double  cod_abona      
		{
			 get { return _cod_abona      ; }
		}
		public double  cod_impuesto   
		{
			 get { return _cod_impuesto   ; }
		}

		#endregion

	}
}
