using System;
using System.Collections.Generic;
using System.Text;

namespace Sistran.Co.Previsora.Application.FullServices.Models
{
	public class Frm_sarlaft_oper_internacKeys
	{

		#region Data Members

		int _id_formulario;
		double _consecutivo_oper;

		#endregion

		#region Constructor

		public Frm_sarlaft_oper_internacKeys()
		{
				
		}

		public Frm_sarlaft_oper_internacKeys(int id_formulario, double consecutivo_oper)
		{
			 _id_formulario = id_formulario; 
			 _consecutivo_oper = consecutivo_oper; 
		}

		#endregion

		#region Properties

		public int  id_formulario
		{
			 get { return _id_formulario; }
		}
		public double  consecutivo_oper
		{
			 get { return _consecutivo_oper; }
		}

		#endregion

	}
}
