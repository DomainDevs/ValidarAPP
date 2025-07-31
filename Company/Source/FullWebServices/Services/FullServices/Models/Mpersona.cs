using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.Serialization;

namespace Sistran.Co.Previsora.Application.FullServices.Models
{
	[DataContract]
	public class Mpersona
	{

		#region InnerClass
		public enum MpersonaFields
		{
			id_persona,
			txt_apellido1,
			txt_apellido2,
			txt_nombre,
			cod_tipo_doc,
			nro_doc,
			cod_tipo_iva,
			nro_nit,
			txt_cia_tra,
			txt_dpto_tra,
			txt_puesto_tra,
			txt_asistente_tra,
			fec_nac,
			txt_lugar_nac,
			txt_sexo,
			cod_est_civil,
			txt_notas,
			cod_tipo_persona,
			txt_origen,
			cod_ent_oficial
		}
		#endregion

		#region Data Members

			int _id_persona;
			string _txt_apellido1;
			string _txt_apellido2;
			string _txt_nombre;
			string _cod_tipo_doc;
			string _nro_doc;
			double _cod_tipo_iva;
			string _nro_nit;
			string _txt_cia_tra;
			string _txt_dpto_tra;
			string _txt_puesto_tra;
			string _txt_asistente_tra;
			string _fec_nac;
			string _txt_lugar_nac;
			string _txt_sexo;
			string _cod_est_civil;
			string _txt_notas;
			string _cod_tipo_persona;
			string _txt_origen;
			string _cod_ent_oficial;
            
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
		public string  txt_apellido1
		{
			 get { return _txt_apellido1; }
			 set {_txt_apellido1 = value;}
		}

		[DataMember]
		public string  txt_apellido2
		{
			 get { return _txt_apellido2; }
			 set {_txt_apellido2 = value;}
		}

		[DataMember]
		public string  txt_nombre
		{
			 get { return _txt_nombre; }
			 set {_txt_nombre = value;}
		}

		[DataMember]
		public string  cod_tipo_doc
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
		public double  cod_tipo_iva
		{
			 get { return _cod_tipo_iva; }
			 set {_cod_tipo_iva = value;}
		}

		[DataMember]
		public string  nro_nit
		{
			 get { return _nro_nit; }
			 set {_nro_nit = value;}
		}

		[DataMember]
		public string  txt_cia_tra
		{
			 get { return _txt_cia_tra; }
			 set {_txt_cia_tra = value;}
		}

		[DataMember]
		public string  txt_dpto_tra
		{
			 get { return _txt_dpto_tra; }
			 set {_txt_dpto_tra = value;}
		}

		[DataMember]
		public string  txt_puesto_tra
		{
			 get { return _txt_puesto_tra; }
			 set {_txt_puesto_tra = value;}
		}

		[DataMember]
		public string  txt_asistente_tra
		{
			 get { return _txt_asistente_tra; }
			 set {_txt_asistente_tra = value;}
		}

		[DataMember]
		public string  fec_nac
		{
			 get { return _fec_nac; }
			 set {_fec_nac = value;}
		}

		[DataMember]
		public string  txt_lugar_nac
		{
			 get { return _txt_lugar_nac; }
			 set {_txt_lugar_nac = value;}
		}

		[DataMember]
		public string  txt_sexo
		{
			 get { return _txt_sexo; }
			 set {_txt_sexo = value;}
		}

		[DataMember]
		public string  cod_est_civil
		{
			 get { return _cod_est_civil; }
			 set {_cod_est_civil = value;}
		}

		[DataMember]
		public string  txt_notas
		{
			 get { return _txt_notas; }
			 set {_txt_notas = value;}
		}

		[DataMember]
		public string  cod_tipo_persona
		{
			 get { return _cod_tipo_persona; }
			 set {_cod_tipo_persona = value;}
		}

		[DataMember]
		public string  txt_origen
		{
			 get { return _txt_origen; }
			 set {_txt_origen = value;}
		}

		[DataMember]
		public string  cod_ent_oficial
		{
			 get { return _cod_ent_oficial; }
			 set {_cod_ent_oficial = value;}
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
        public string SPOUSE_NAME
        {
            get;
            set;
        }

        [DataMember]
        public string EDUCATIVE_LEVEL_CD
        {
            get;
            set;
        }

        [DataMember]
        public string CHILDREN
        {
            get;
            set;
        }

        [DataMember]
        public string SOCIAL_LAYER_CD
        {
            get;
            set;
        }

        [DataMember]
        public string HOUSE_TYPE_CD
        {
            get;
            set;
        }

        [DataMember]
        public string INCOME_LEVEL_CD
        {
            get;
            set;
        }

        [DataMember]
        public string SPECIALITY_CD
        {
            get;
            set;
        }

        [DataMember]
        public string COMPANY_PHONE
        {
            get;
            set;
        }

        [DataMember]
        public string OCCUPATION_CD
        {
            get;
            set;
        }

        [DataMember]
        public string OTHER_OCCUPATION_CD
        {
            get;
            set;
        }
        
        [DataMember]
        public string COMPANY_TYPE_CD
        {
            get;
            set;
        }

        [DataMember]
        public string ASSOCIATION_TYPE_CD
        {
            get;
            set;
        }

        [DataMember]
        public string VERIFY_DIGIT
        {
            get;
            set;
        }

        [DataMember]
        public string CATEGORY_CD
        {
            get;
            set;
        }
		#endregion
	}
}