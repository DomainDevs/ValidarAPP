using System;
using System.Collections.Generic;
using System.Text;

namespace Sistran.Co.Previsora.Application.FullServices.Models
{
	public class Maseg_conductoKeys
	{

		#region Data Members

		int _cod_aseg;
		int _ind_conducto;

		#endregion

		#region Constructor

		public Maseg_conductoKeys()
		{
				
		}

		public Maseg_conductoKeys(int cod_aseg, int ind_conducto)
		{
			 _cod_aseg = cod_aseg; 
			 _ind_conducto = ind_conducto; 
		}

		#endregion

		#region Properties

		public int  cod_aseg
		{
			 get { return _cod_aseg; }
		}
		public int  ind_conducto
		{
			 get { return _ind_conducto; }
		}

		#endregion

	}
}
