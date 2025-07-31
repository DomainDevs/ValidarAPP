using System;
using System.Collections.Generic;
using System.Text;

namespace Sistran.Co.Previsora.Application.FullServices.Models
{
	public class Maseg_asocKeys
	{

		#region Data Members

		int _cod_aseg_asociacion;
		byte _nro_correla_asoc;
		DateTime _fec_asociacion;

		#endregion

		#region Constructor

		public Maseg_asocKeys()
		{
				
		}

		public Maseg_asocKeys(int cod_aseg_asociacion, byte nro_correla_asoc, DateTime fec_asociacion)
		{
			 _cod_aseg_asociacion = cod_aseg_asociacion; 
			 _nro_correla_asoc = nro_correla_asoc; 
			 _fec_asociacion = fec_asociacion; 
		}

		#endregion

		#region Properties

		public int  cod_aseg_asociacion
		{
			 get { return _cod_aseg_asociacion; }
		}
		public byte  nro_correla_asoc
		{
			 get { return _nro_correla_asoc; }
		}
		public DateTime  fec_asociacion
		{
			 get { return _fec_asociacion; }
		}

		#endregion

	}
}
