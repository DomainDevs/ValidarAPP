using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.Serialization;

namespace Sistran.Co.Previsora.Application.FullServices.Models
{
	[DataContract]
	public class Ttipo_doc
	{

		#region InnerClass
		public enum Ttipo_docFields
		{
			cod_tipo_doc,
			txt_desc_redu,
			txt_desc,
			sn_activo
		}
		#endregion

		#region Data Members

			double _cod_tipo_doc;
			string _txt_desc_redu;
			string _txt_desc;
			string _sn_activo;
			int _identity; 
			char _state; 
			string _connection; 

		#endregion

		#region Properties

		[DataMember]
		public double  cod_tipo_doc
		{
			 get { return _cod_tipo_doc; }
			 set {_cod_tipo_doc = value;}
		}

		[DataMember]
		public string  txt_desc_redu
		{
			 get { return _txt_desc_redu; }
			 set {_txt_desc_redu = value;}
		}

		[DataMember]
		public string  txt_desc
		{
			 get { return _txt_desc; }
			 set {_txt_desc = value;}
		}

		[DataMember]
		public string  sn_activo
		{
			 get { return _sn_activo; }
			 set {_sn_activo = value;}
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
