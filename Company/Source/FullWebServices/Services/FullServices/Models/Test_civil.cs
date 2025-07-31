using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.Serialization;

namespace Sistran.Co.Previsora.Application.FullServices.Models
{
	[DataContract]
	public class Test_civil
	{

		#region InnerClass
		public enum Test_civilFields
		{
			cod_est_civil,
			txt_desc
		}
		#endregion

		#region Data Members

			double _cod_est_civil;
			string _txt_desc;
			int _identity; 
			char _state; 
			string _connection; 

		#endregion

		#region Properties

		[DataMember]
		public double  cod_est_civil
		{
			 get { return _cod_est_civil; }
			 set {_cod_est_civil = value;}
		}

		[DataMember]
		public string  txt_desc
		{
			 get { return _txt_desc; }
			 set {_txt_desc = value;}
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
