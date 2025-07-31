using System;
using System.Collections.Generic;
using System.Text;

namespace Sistran.Co.Previsora.Application.FullServices.Models
{
	public class Mpersona_emailKeys
	{

		#region Data Members

		int _id_persona;
		int _cod_tipo_email;

		#endregion

		#region Constructor

		public Mpersona_emailKeys()
		{
				
		}

		public Mpersona_emailKeys(int id_persona, int cod_tipo_email)
		{
			 _id_persona = id_persona; 
			 _cod_tipo_email = cod_tipo_email; 
		}

		#endregion

		#region Properties

		public int  id_persona
		{
			 get { return _id_persona; }
		}
		public int  cod_tipo_email
		{
			 get { return _cod_tipo_email; }
		}

		#endregion

	}
}
