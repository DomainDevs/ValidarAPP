using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.Serialization;

namespace Sistran.Co.Previsora.Application.FullServices.Models
{
	[DataContract]
	public class Ttipo_cta_bco
	{

		#region InnerClass
		public enum Ttipo_cta_bcoFields
		{
			cod_tipo_cta_bco,
			txt_desc,
			sn_trans_bancarias
		}
		#endregion

		#region Data Members

			double _cod_tipo_cta_bco;
			string _txt_desc;
			int _sn_trans_bancarias;
			int _identity; 
			char _state; 
			string _connection; 

		#endregion

		#region Properties

		[DataMember]
		public double  cod_tipo_cta_bco
		{
			 get { return _cod_tipo_cta_bco; }
			 set {_cod_tipo_cta_bco = value;}
		}

		[DataMember]
		public string  txt_desc
		{
			 get { return _txt_desc; }
			 set {_txt_desc = value;}
		}

		[DataMember]
		public int  sn_trans_bancarias
		{
			 get { return _sn_trans_bancarias; }
			 set {_sn_trans_bancarias = value;}
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
