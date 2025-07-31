using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.Serialization;

namespace Sistran.Co.Previsora.Application.FullServices.Models
{
	[DataContract]
	public class Tcpto_aseg_adic
	{

		#region InnerClass
		public enum Tcpto_aseg_adicFields
		{
			cod_aseg,
			sn_tomador,
			sn_beneficiario,
			sn_apoderado,
			sn_afianzado,
			sn_consorcio
		}
		#endregion

		#region Data Members

			int _cod_aseg;
			int _sn_tomador;
			int _sn_beneficiario;
			int _sn_apoderado;
			int _sn_afianzado;
			int _sn_consorcio;
			int _identity; 
			char _state; 
			string _connection; 

		#endregion

		#region Properties

		[DataMember]
		public int  cod_aseg
		{
			 get { return _cod_aseg; }
			 set {_cod_aseg = value;}
		}

		[DataMember]
		public int  sn_tomador
		{
			 get { return _sn_tomador; }
			 set {_sn_tomador = value;}
		}

		[DataMember]
		public int  sn_beneficiario
		{
			 get { return _sn_beneficiario; }
			 set {_sn_beneficiario = value;}
		}

		[DataMember]
		public int  sn_apoderado
		{
			 get { return _sn_apoderado; }
			 set {_sn_apoderado = value;}
		}

		[DataMember]
		public int  sn_afianzado
		{
			 get { return _sn_afianzado; }
			 set {_sn_afianzado = value;}
		}

		[DataMember]
		public int  sn_consorcio
		{
			 get { return _sn_consorcio; }
			 set {_sn_consorcio = value;}
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
