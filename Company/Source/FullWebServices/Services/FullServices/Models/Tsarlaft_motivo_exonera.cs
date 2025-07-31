using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.Serialization;

namespace Sistran.Co.Previsora.Application.FullServices.Models
{
	[DataContract]
	public class Tsarlaft_motivo_exonera
	{

		#region InnerClass
		public enum Tsarlaft_motivo_exoneraFields
		{
			cod_motivo_exonera,
			txt_desc,
			sn_habilitado,
			fec_modificacion,
			cod_usuario,
			cod_tipo_persona
		}
		#endregion

		#region Data Members

			double _cod_motivo_exonera;
			string _txt_desc;
			int _sn_habilitado;
			string _fec_modificacion;
			string _cod_usuario;
			string _cod_tipo_persona;
			int _identity; 
			char _state; 
			string _connection; 

		#endregion

		#region Properties

		[DataMember]
		public double  cod_motivo_exonera
		{
			 get { return _cod_motivo_exonera; }
			 set {_cod_motivo_exonera = value;}
		}

		[DataMember]
		public string  txt_desc
		{
			 get { return _txt_desc; }
			 set {_txt_desc = value;}
		}

		[DataMember]
		public int  sn_habilitado
		{
			 get { return _sn_habilitado; }
			 set {_sn_habilitado = value;}
		}

		[DataMember]
		public string  fec_modificacion
		{
			 get { return _fec_modificacion; }
			 set {_fec_modificacion = value;}
		}

		[DataMember]
		public string  cod_usuario
		{
			 get { return _cod_usuario; }
			 set {_cod_usuario = value;}
		}

		[DataMember]
		public string  cod_tipo_persona
		{
			 get { return _cod_tipo_persona; }
			 set {_cod_tipo_persona = value;}
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
