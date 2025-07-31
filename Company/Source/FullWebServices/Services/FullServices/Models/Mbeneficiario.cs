using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.Serialization;

namespace Sistran.Co.Previsora.Application.FullServices.Models
{
	[DataContract]
	public class Mbeneficiario
	{

		#region InnerClass
		public enum MbeneficiarioFields
		{
			cod_beneficiario,
			id_persona,
			fec_alta,
			fec_baja,
			cod_baja,
			fec_inactivacion,
			edad,
			cod_justificacion
		}
		#endregion

		#region Data Members

			int _cod_beneficiario;
			int _id_persona;
			string _fec_alta;
			string _fec_baja;
			string _cod_baja;
			string _fec_inactivacion;
			string _edad;
			string _cod_justificacion;
			int _identity; 
			char _state; 
			string _connection; 

		#endregion

		#region Properties

		[DataMember]
		public int  cod_beneficiario
		{
			 get { return _cod_beneficiario; }
			 set {_cod_beneficiario = value;}
		}

		[DataMember]
		public int  id_persona
		{
			 get { return _id_persona; }
			 set {_id_persona = value;}
		}

		[DataMember]
		public string  fec_alta
		{
			 get { return _fec_alta; }
			 set {_fec_alta = value;}
		}

		[DataMember]
		public string  fec_baja
		{
			 get { return _fec_baja; }
			 set {_fec_baja = value;}
		}

		[DataMember]
		public string  cod_baja
		{
			 get { return _cod_baja; }
			 set {_cod_baja = value;}
		}

		[DataMember]
		public string  fec_inactivacion
		{
			 get { return _fec_inactivacion; }
			 set {_fec_inactivacion = value;}
		}

		[DataMember]
		public string  edad
		{
			 get { return _edad; }
			 set {_edad = value;}
		}

		[DataMember]
		public string  cod_justificacion
		{
			 get { return _cod_justificacion; }
			 set {_cod_justificacion = value;}
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
