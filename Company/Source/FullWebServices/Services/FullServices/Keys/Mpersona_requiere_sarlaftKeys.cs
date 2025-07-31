using System;
using System.Collections.Generic;
using System.Text;

namespace Sistran.Co.Previsora.Application.FullServices.Models
{
	public class Mpersona_requiere_sarlaftKeys
	{

		#region Data Members

		int _id_persona;

		#endregion

		#region Constructor

		public Mpersona_requiere_sarlaftKeys()
		{
				
		}

		public Mpersona_requiere_sarlaftKeys(int id_persona)
		{
			 _id_persona = id_persona; 
		}

		#endregion

		#region Properties

		public int  id_persona
		{
			 get { return _id_persona; }
		}

		#endregion

	}
}
