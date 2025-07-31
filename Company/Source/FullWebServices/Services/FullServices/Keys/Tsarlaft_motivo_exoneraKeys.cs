using System;
using System.Collections.Generic;
using System.Text;

namespace Sistran.Co.Previsora.Application.FullServices.Models
{
	public class Tsarlaft_motivo_exoneraKeys
	{

		#region Data Members

		decimal _cod_motivo_exonera;

		#endregion

		#region Constructor

		public Tsarlaft_motivo_exoneraKeys()
		{
				
		}

		public Tsarlaft_motivo_exoneraKeys(decimal cod_motivo_exonera)
		{
			 _cod_motivo_exonera = cod_motivo_exonera; 
		}

		#endregion

		#region Properties

		public decimal  cod_motivo_exonera
		{
			 get { return _cod_motivo_exonera; }
		}

		#endregion

	}
}
