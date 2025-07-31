using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.Serialization;

namespace Sistran.Co.Previsora.Application.FullServices.Models
{
	[DataContract]
	public class Tbanco
	{

		#region InnerClass
		public enum TbancoFields
		{
			cod_banco,
			txt_nombre,
			cod_banco_debito,
			nro_nit,
			sn_ach
		}
		#endregion

		#region Data Members

			double _cod_banco;
			string _txt_nombre;
			string _cod_banco_debito;
			string _nro_nit;
			int _sn_ach;
			int _identity; 
			char _state; 
			string _connection; 

		#endregion

		#region Properties

		[DataMember]
		public double  cod_banco
		{
			 get { return _cod_banco; }
			 set {_cod_banco = value;}
		}

		[DataMember]
		public string  txt_nombre
		{
			 get { return _txt_nombre; }
			 set {_txt_nombre = value;}
		}

		[DataMember]
		public string  cod_banco_debito
		{
			 get { return _cod_banco_debito; }
			 set {_cod_banco_debito = value;}
		}

		[DataMember]
		public string  nro_nit
		{
			 get { return _nro_nit; }
			 set {_nro_nit = value;}
		}

		[DataMember]
		public int  sn_ach
		{
			 get { return _sn_ach; }
			 set {_sn_ach = value;}
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
