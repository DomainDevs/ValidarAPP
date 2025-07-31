using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.Serialization;

namespace Sistran.Co.Previsora.Application.FullServices.Models
{
	[DataContract]
	public class Mtercero
	{

		#region InnerClass
		public enum MterceroFields
		{
			cod_tercero,
			id_persona,
			txt_nombre,
			txt_direccion,
			txt_telefono,
			cod_tipo_persona,
			cod_tipo_doc,
			nro_doc,
			nro_nit,
			txt_sexo,
			edad,
			fec_alta,
			fec_baja,
			cod_baja
		}
		#endregion

		#region Data Members

			int _cod_tercero;
			int _id_persona;
			string _txt_nombre;
			string _txt_direccion;
			string _txt_telefono;
			string _cod_tipo_persona;
			decimal? _cod_tipo_doc;
			string _nro_doc;
			string _nro_nit;
			string _txt_sexo;
			decimal? _edad;
            string _fec_alta;
            string _fec_baja;
			decimal? _cod_baja;
			int _identity; 
			char _state; 
			string _connection; 

		#endregion

		#region Properties

		[DataMember]
		public int  cod_tercero
		{
			 get { return _cod_tercero; }
			 set {_cod_tercero = value;}
		}

		[DataMember]
		public int  id_persona
		{
			 get { return _id_persona; }
			 set {_id_persona = value;}
		}

		[DataMember]
		public string  txt_nombre
		{
			 get { return _txt_nombre; }
			 set {_txt_nombre = value;}
		}

		[DataMember]
		public string  txt_direccion
		{
			 get { return _txt_direccion; }
			 set {_txt_direccion = value;}
		}

		[DataMember]
		public string  txt_telefono
		{
			 get { return _txt_telefono; }
			 set {_txt_telefono = value;}
		}

		[DataMember]
		public string  cod_tipo_persona
		{
			 get { return _cod_tipo_persona; }
			 set {_cod_tipo_persona = value;}
		}

		[DataMember]
		public decimal?  cod_tipo_doc
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
		public string  nro_nit
		{
			 get { return _nro_nit; }
			 set {_nro_nit = value;}
		}

		[DataMember]
		public string  txt_sexo
		{
			 get { return _txt_sexo; }
			 set {_txt_sexo = value;}
		}

		[DataMember]
		public decimal?  edad
		{
			 get { return _edad; }
			 set {_edad = value;}
		}

		[DataMember]
        public string fec_alta
		{
			 get { return _fec_alta; }
			 set {_fec_alta = value;}
		}

		[DataMember]
        public string fec_baja
		{
			 get { return _fec_baja; }
			 set {_fec_baja = value;}
		}

		[DataMember]
		public decimal?  cod_baja
		{
			 get { return _cod_baja; }
			 set {_cod_baja = value;}
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
