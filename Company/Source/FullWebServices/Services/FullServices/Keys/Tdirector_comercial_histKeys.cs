using System;
using System.Collections.Generic;
using System.Text;

namespace Sistran.Co.Previsora.Application.FullServices.Models
{
	public class Tdirector_comercial_histKeys
	{

		#region Data Members

		int _cod_director_comercial;
		int _cod_director_nacional;
		double _id_correla_estado;

		#endregion

		#region Constructor

		public Tdirector_comercial_histKeys()
		{
				
		}

		public Tdirector_comercial_histKeys(int cod_director_comercial, int cod_director_nacional, double id_correla_estado)
		{
			 _cod_director_comercial = cod_director_comercial; 
			 _cod_director_nacional = cod_director_nacional; 
			 _id_correla_estado = id_correla_estado; 
		}

		#endregion

		#region Properties

		public int  cod_director_comercial
		{
			 get { return _cod_director_comercial; }
		}
		public int  cod_director_nacional
		{
			 get { return _cod_director_nacional; }
		}
		public double  id_correla_estado
		{
			 get { return _id_correla_estado; }
		}

		#endregion

	}
}
