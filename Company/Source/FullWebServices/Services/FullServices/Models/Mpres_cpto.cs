using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.Serialization;

namespace Sistran.Co.Previsora.Application.FullServices.Models
{
	[DataContract]
	public class Mpres_cpto
	{

		#region InnerClass
		public enum Mpres_cptoFields
		{
			cod_pres,
			cod_cpto,
			cod_suc
		}
		#endregion

		#region Data Members

			double _cod_pres;
			double _cod_cpto;
			double _cod_suc;
			int _identity; 
			char _state; 
			string _connection; 

		#endregion

		#region Properties

		[DataMember]
		public double  cod_pres
		{
			 get { return _cod_pres; }
			 set {_cod_pres = value;}
		}

		[DataMember]
		public double  cod_cpto
		{
			 get { return _cod_cpto; }
			 set {_cod_cpto = value;}
		}

		[DataMember]
		public double  cod_suc
		{
			 get { return _cod_suc; }
			 set {_cod_suc = value;}
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
