using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.Serialization;

namespace Sistran.Co.Previsora.Application.FullServices.Models
{
	[DataContract]
	public class Tplaza_trans_banc
	{

		#region InnerClass
		public enum Tplaza_trans_bancFields
		{
			cod_plaza,
			cod_tipo_red,
			cod_dpto,
			cod_municipio,
			txt_plaza,
			sn_habilitado
		}
		#endregion

		#region Data Members

			string _cod_plaza;
			int _cod_tipo_red;
			double _cod_dpto;
			double _cod_municipio;
			string _txt_plaza;
			int _sn_habilitado;
			int _identity; 
			char _state; 
			string _connection; 

		#endregion

		#region Properties

		[DataMember]
		public string  cod_plaza
		{
			 get { return _cod_plaza; }
			 set {_cod_plaza = value;}
		}

		[DataMember]
		public int  cod_tipo_red
		{
			 get { return _cod_tipo_red; }
			 set {_cod_tipo_red = value;}
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
		public string  txt_plaza
		{
			 get { return _txt_plaza; }
			 set {_txt_plaza = value;}
		}

		[DataMember]
		public int  sn_habilitado
		{
			 get { return _sn_habilitado; }
			 set {_sn_habilitado = value;}
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
