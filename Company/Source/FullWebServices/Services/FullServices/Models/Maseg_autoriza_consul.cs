using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.Serialization;

namespace Sistran.Co.Previsora.Application.FullServices.Models
{
	[DataContract]
	public class Maseg_autoriza_consul
	{

		#region InnerClass
		public enum Maseg_autoriza_consulFields
		{
			cod_aseg,
			fec_sing,
			txt_nombre_autoriza,
			cod_tipo_doc,
			nro_doc,
			fec_save,
			cod_usuario,
			cod_tipo_doc_aseg,
			nro_doc_aseg
		}
		#endregion

		#region Data Members

			int _cod_aseg;
			string _fec_sing;
			string _txt_nombre_autoriza;
			double _cod_tipo_doc;
			string _nro_doc;
			string _fec_save;
			string _cod_usuario;
			double _cod_tipo_doc_aseg;
			string _nro_doc_aseg;
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
		public string  fec_sing
		{
			 get { return _fec_sing; }
			 set {_fec_sing = value;}
		}

		[DataMember]
		public string  txt_nombre_autoriza
		{
			 get { return _txt_nombre_autoriza; }
			 set {_txt_nombre_autoriza = value;}
		}

		[DataMember]
		public double  cod_tipo_doc
		{
			 get { return _cod_tipo_doc; }
			 set {_cod_tipo_doc = value;}
		}

		[DataMember]
		public string  nro_doc
		{
			 get { return _nro_doc; }
			 set {_nro_doc = value;}
		}

		[DataMember]
		public string  fec_save
		{
			 get { return _fec_save; }
			 set {_fec_save = value;}
		}

		[DataMember]
		public string  cod_usuario
		{
			 get { return _cod_usuario; }
			 set {_cod_usuario = value;}
		}

		[DataMember]
		public double  cod_tipo_doc_aseg
		{
			 get { return _cod_tipo_doc_aseg; }
			 set {_cod_tipo_doc_aseg = value;}
		}

		[DataMember]
		public string  nro_doc_aseg
		{
			 get { return _nro_doc_aseg; }
			 set {_nro_doc_aseg = value;}
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
