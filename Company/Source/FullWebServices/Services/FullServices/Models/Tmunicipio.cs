using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.Serialization;

namespace Sistran.Co.Previsora.Application.FullServices.Models
{
	[DataContract]
	public class Tmunicipio
	{

		#region InnerClass
		public enum TmunicipioFields
		{
			cod_pais,
			cod_dpto,
			cod_municipio,
			txt_desc,
			cod_divipola
		}
		#endregion

		#region Data Members

			double _cod_pais;
			double _cod_dpto;
			double _cod_municipio;
			string _txt_desc;
			string _cod_divipola;
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
		public double  cod_municipio
		{
			 get { return _cod_municipio; }
			 set {_cod_municipio = value;}
		}

		[DataMember]
		public string  txt_desc
		{
			 get { return _txt_desc; }
			 set {_txt_desc = value;}
		}

		[DataMember]
		public string  cod_divipola
		{
			 get { return _cod_divipola; }
			 set {_cod_divipola = value;}
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
