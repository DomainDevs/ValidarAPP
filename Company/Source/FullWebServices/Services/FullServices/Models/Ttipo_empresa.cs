using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.Serialization;

namespace Sistran.Co.Previsora.Application.FullServices.Models
{
	[DataContract]
	public class Ttipo_empresa
	{

		#region InnerClass
		public enum Ttipo_empresaFields
		{
			cod_ttipo_empresa,
			txt_desc,
			cod_super
		}
		#endregion

		#region Data Members

			string _cod_ttipo_empresa;
			string _txt_desc;
			int _cod_super;
			int _identity; 
			char _state; 
			string _connection; 

		#endregion

		#region Properties

		[DataMember]
		public string  cod_ttipo_empresa
		{
			 get { return _cod_ttipo_empresa; }
			 set {_cod_ttipo_empresa = value;}
		}

		[DataMember]
		public string  txt_desc
		{
			 get { return _txt_desc; }
			 set {_txt_desc = value;}
		}

		[DataMember]
		public int  cod_super
		{
			 get { return _cod_super; }
			 set {_cod_super = value;}
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
