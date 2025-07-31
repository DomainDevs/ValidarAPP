using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.Serialization;

namespace Sistran.Co.Previsora.Application.FullServices.Models
{
	[DataContract]
	public class Frm_sarlaft_accionistas_asoc
	{

		#region InnerClass
		public enum Frm_sarlaft_accionistas_asocFields
		{
			id_persona,
			nro_asociacion,
			cod_tipo_doc_asoc,
			nro_doc_asoc,
			txtnombre,
			sn_estado_Asoc
		}
		#endregion

		#region Data Members

			int _id_persona;
			double _nro_asociacion;
			double _cod_tipo_doc_asoc;
			string _nro_doc_asoc;
			string _txtnombre;
			int _sn_estado_asoc;
			int _identity; 
			char _state; 
			string _connection; 

		#endregion

		#region Properties

		[DataMember]
		public int  id_persona
		{
			 get { return _id_persona; }
			 set {_id_persona = value;}
		}

		[DataMember]
		public double  nro_asociacion
		{
			 get { return _nro_asociacion; }
			 set {_nro_asociacion = value;}
		}

		[DataMember]
		public double  cod_tipo_doc_asoc
		{
			 get { return _cod_tipo_doc_asoc; }
			 set {_cod_tipo_doc_asoc = value;}
		}

		[DataMember]
		public string  nro_doc_asoc
		{
			 get { return _nro_doc_asoc; }
			 set {_nro_doc_asoc = value;}
		}

		[DataMember]
		public string  txtnombre
		{
			 get { return _txtnombre; }
			 set {_txtnombre = value;}
		}

		[DataMember]
		public int  sn_estado_Asoc
		{
			 get { return _sn_estado_asoc; }
			 set {_sn_estado_asoc = value;}
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
