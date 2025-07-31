using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.Serialization;

namespace Sistran.Co.Previsora.Application.FullServices.Models
{
	[DataContract]
	public class Tdpto
	{

		#region InnerClass
		public enum TdptoFields
		{
			cod_pais,
			cod_dpto,
			txt_desc,
			cod_zona_sismica,
			cod_subzona_sismica
		}
		#endregion

		#region Data Members

			double _cod_pais;
			double _cod_dpto;
			string _txt_desc;
			string _cod_zona_sismica;
			string _cod_subzona_sismica;
			int _identity; 
			char _state; 
			string _connection; 

		#endregion

		#region Properties

		[DataMember]
		public double  cod_pais
		{
			 get { return _cod_pais; }
			 set {_cod_pais = value;}
		}

		[DataMember]
		public double  cod_dpto
		{
			 get { return _cod_dpto; }
			 set {_cod_dpto = value;}
		}

		[DataMember]
		public string  txt_desc
		{
			 get { return _txt_desc; }
			 set {_txt_desc = value;}
		}

		[DataMember]
		public string  cod_zona_sismica
		{
			 get { return _cod_zona_sismica; }
			 set {_cod_zona_sismica = value;}
		}

		[DataMember]
		public string  cod_subzona_sismica
		{
			 get { return _cod_subzona_sismica; }
			 set {_cod_subzona_sismica = value;}
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
