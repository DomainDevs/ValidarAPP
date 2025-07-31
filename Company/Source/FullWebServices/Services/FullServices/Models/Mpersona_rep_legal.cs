using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.Serialization;

namespace Sistran.Co.Previsora.Application.FullServices.Models
{
	[DataContract]
	public class Mpersona_rep_legal
	{

		#region InnerClass
		public enum Mpersona_rep_legalFields
		{
			id_persona,
			nro_doc_rep_legal,
			cod_tipo_doc,
			txt_nombre,
			fec_nacimiento,
			txt_lugar_nacimi,
			txt_nacionalidad,
			fec_expedicion_doc,
			txt_lugar_expedicion,
			txt_ciudad,
			txt_telefono,
			txt_celular,
			txt_email,
			txt_facultades,
			vr_facultades,
			txt_cargo,
			cod_unidad
		}
		#endregion

		#region Data Members

			int _id_persona;
			string _nro_doc_rep_legal;
			double _cod_tipo_doc;
			string _txt_nombre;
			string _fec_nacimiento;
			string _txt_lugar_nacimi;
			string _txt_nacionalidad;
			string _fec_expedicion_doc;
			string _txt_lugar_expedicion;
			string _txt_ciudad;
			string _txt_telefono;
			string _txt_celular;
			string _txt_email;
			string _txt_facultades;
			string _vr_facultades;
			string _txt_cargo;
			string _cod_unidad;
			int _identity; 
			char _state;
            char _state_3G; 
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
		public string  nro_doc_rep_legal
		{
			 get { return _nro_doc_rep_legal; }
			 set {_nro_doc_rep_legal = value;}
		}

		[DataMember]
		public double  cod_tipo_doc
		{
			 get { return _cod_tipo_doc; }
			 set {_cod_tipo_doc = value;}
		}

		[DataMember]
		public string  txt_nombre
		{
			 get { return _txt_nombre; }
			 set {_txt_nombre = value;}
		}

		[DataMember]
		public string  fec_nacimiento
		{
			 get { return _fec_nacimiento; }
			 set {_fec_nacimiento = value;}
		}

		[DataMember]
		public string  txt_lugar_nacimi
		{
			 get { return _txt_lugar_nacimi; }
			 set {_txt_lugar_nacimi = value;}
		}

		[DataMember]
		public string  txt_nacionalidad
		{
			 get { return _txt_nacionalidad; }
			 set {_txt_nacionalidad = value;}
		}

		[DataMember]
		public string  fec_expedicion_doc
		{
			 get { return _fec_expedicion_doc; }
			 set {_fec_expedicion_doc = value;}
		}

		[DataMember]
		public string  txt_lugar_expedicion
		{
			 get { return _txt_lugar_expedicion; }
			 set {_txt_lugar_expedicion = value;}
		}

		[DataMember]
		public string  txt_ciudad
		{
			 get { return _txt_ciudad; }
			 set {_txt_ciudad = value;}
		}

		[DataMember]
		public string  txt_telefono
		{
			 get { return _txt_telefono; }
			 set {_txt_telefono = value;}
		}

		[DataMember]
		public string  txt_celular
		{
			 get { return _txt_celular; }
			 set {_txt_celular = value;}
		}

		[DataMember]
		public string  txt_email
		{
			 get { return _txt_email; }
			 set {_txt_email = value;}
		}

		[DataMember]
		public string  txt_facultades
		{
			 get { return _txt_facultades; }
			 set {_txt_facultades = value;}
		}

		[DataMember]
		public string  vr_facultades
		{
			 get { return _vr_facultades; }
			 set {_vr_facultades = value;}
		}

		[DataMember]
		public string  txt_cargo
		{
			 get { return _txt_cargo; }
			 set {_txt_cargo = value;}
		}

		[DataMember]
		public string  cod_unidad
		{
			 get { return _cod_unidad; }
			 set {_cod_unidad = value;}
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

        [DataMember]
        public char State_3G
        {
            get { return _state_3G; }
            set { _state_3G = value; }
        }
		#endregion

	}
}
