using System;
using System.Collections.Generic;
using System.Text;

namespace Sistran.Co.Previsora.Application.FullServices.Models
{
	public class INDIVIDUAL_TAX_EXEMPTIONKeys
	{

		#region Data Members

		int _ind_tax_exemption_id;

		#endregion

		#region Constructor

		public INDIVIDUAL_TAX_EXEMPTIONKeys()
		{
				
		}

		public INDIVIDUAL_TAX_EXEMPTIONKeys(int iND_TAX_EXEMPTION_ID)
		{
			 _ind_tax_exemption_id = iND_TAX_EXEMPTION_ID; 
		}

		#endregion

		#region Properties

		public int  IND_TAX_EXEMPTION_ID
		{
			 get { return _ind_tax_exemption_id; }
		}

		#endregion

	}
}
