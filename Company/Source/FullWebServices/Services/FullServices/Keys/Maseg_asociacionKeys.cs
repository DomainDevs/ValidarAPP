using System;
using System.Collections.Generic;
using System.Text;

namespace Sistran.Co.Previsora.Application.FullServices.Models
{
	public class Maseg_asociacionKeys
	{

		#region Data Members

		int _cod_aseg_asociacion;
		int _nro_correla_asoc;

		#endregion

		#region Constructor

		public Maseg_asociacionKeys()
		{
				
		}

		public Maseg_asociacionKeys(int cod_aseg_asociacion, int nro_correla_asoc)
		{
			 _cod_aseg_asociacion = cod_aseg_asociacion; 
			 _nro_correla_asoc = nro_correla_asoc; 
		}

		#endregion

		#region Properties

		public int  cod_aseg_asociacion
		{
			 get { return _cod_aseg_asociacion; }
		}
		public int  nro_correla_asoc
		{
			 get { return _nro_correla_asoc; }
		}

		#endregion

	}
}
