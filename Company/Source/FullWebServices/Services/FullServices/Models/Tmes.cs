using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.Serialization;

namespace Sistran.Co.Previsora.Application.FullServices.Models
{
	[DataContract]
	public class Tmes
	{

		#region InnerClass
		public enum TmesFields
		{
			cod_mes,
			txt_desc,
			cnt_dias
		}
		#endregion

		#region Data Members

			int _cod_mes;
			string _txt_desc;
			double _cnt_dias;
			int _identity; 
			char _state; 
			string _connection; 

		#endregion

		#region Properties

		[DataMember]
		public int  cod_mes
		{
			 get { return _cod_mes; }
			 set {_cod_mes = value;}
		}

		[DataMember]
		public string  txt_desc
		{
			 get { return _txt_desc; }
			 set {_txt_desc = value;}
		}

		[DataMember]
		public double  cnt_dias
		{
			 get { return _cnt_dias; }
			 set {_cnt_dias = value;}
		}


		[DataMember]
		public int  Identity
		{
		  get { return _identity; }
		  set	{ _identity = value;}
		}

		[DataMember]
		public char  State
		{
		  get { return _state; }
		  set	{ _state = value;}
		}

		[DataMember]
		public string  Connection
		{
		  get { return _connection; }
		  set	{ _connection = value;}
		}

		#endregion

	}
}
