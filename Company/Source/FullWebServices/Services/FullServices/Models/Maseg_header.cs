using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.Serialization;

namespace Sistran.Co.Previsora.Application.FullServices.Models
{
	[DataContract]
	public class Maseg_header
	{

		#region InnerClass
		public enum Maseg_headerFields
		{
			cod_aseg,
			id_persona,
			cod_figura_aseg,
			cod_tipo_aseg,
			cod_imp_aseg,
			txt_vincula,
			cod_tipo_agente,
			cod_agente,
			fec_alta,
			fec_baja,
			cod_ocupacion,
			sn_aviso_vto,
			txt_aseg_viejo,
			cod_aseg_vinc,
			fec_ult_modif,
			cod_usuario,
			txt_nom_factura,
			cod_seg_ocupacion,
			imp_sueldo,
			imp_otros_ingresos,
			cod_ciiu,
			imp_niveloperativo,
			txt_garantia,
			cod_baja,
			cod_calif_cart,
			cod_ttipo_empresa,
			cod_tasociacion,
			sn_valida_cgarantia,
			sn_aseg_especial,
			sn_consorcio
		}
		#endregion

		#region Data Members

			int _cod_aseg;
			int _id_persona;
			string _cod_figura_aseg;
			double _cod_tipo_aseg;
			double _cod_imp_aseg;
			string _txt_vincula;
			double _cod_tipo_agente;
			int _cod_agente;
			string _fec_alta;
			string _fec_baja;
			string _cod_ocupacion;
			string _sn_aviso_vto;
			string _txt_aseg_viejo;
			string _cod_aseg_vinc;
			string _fec_ult_modif;
			string _cod_usuario;
			string _txt_nom_factura;
			string _cod_seg_ocupacion;
			string _imp_sueldo;
			string _imp_otros_ingresos;
			double _cod_ciiu;
			string _imp_niveloperativo;
			string _txt_garantia;
			string _cod_baja;
			string _cod_calif_cart;
			string _cod_ttipo_empresa;
			string _cod_tasociacion;
			int _sn_valida_cgarantia;
			int _sn_aseg_especial;
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
		public int  id_persona
		{
			 get { return _id_persona; }
			 set {_id_persona = value;}
		}

		[DataMember]
		public string  cod_figura_aseg
		{
			 get { return _cod_figura_aseg; }
			 set {_cod_figura_aseg = value;}
		}

		[DataMember]
		public double  cod_tipo_aseg
		{
			 get { return _cod_tipo_aseg; }
			 set {_cod_tipo_aseg = value;}
		}

		[DataMember]
		public double  cod_imp_aseg
		{
			 get { return _cod_imp_aseg; }
			 set {_cod_imp_aseg = value;}
		}

		[DataMember]
		public string  txt_vincula
		{
			 get { return _txt_vincula; }
			 set {_txt_vincula = value;}
		}

		[DataMember]
		public double  cod_tipo_agente
		{
			 get { return _cod_tipo_agente; }
			 set {_cod_tipo_agente = value;}
		}

		[DataMember]
		public int  cod_agente
		{
			 get { return _cod_agente; }
			 set {_cod_agente = value;}
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
		public string  cod_ocupacion
		{
			 get { return _cod_ocupacion; }
			 set {_cod_ocupacion = value;}
		}

		[DataMember]
		public string  sn_aviso_vto
		{
			 get { return _sn_aviso_vto; }
			 set {_sn_aviso_vto = value;}
		}

		[DataMember]
		public string  txt_aseg_viejo
		{
			 get { return _txt_aseg_viejo; }
			 set {_txt_aseg_viejo = value;}
		}

		[DataMember]
		public string  cod_aseg_vinc
		{
			 get { return _cod_aseg_vinc; }
			 set {_cod_aseg_vinc = value;}
		}

		[DataMember]
		public string  fec_ult_modif
		{
			 get { return _fec_ult_modif; }
			 set {_fec_ult_modif = value;}
		}

		[DataMember]
		public string  cod_usuario
		{
			 get { return _cod_usuario; }
			 set {_cod_usuario = value;}
		}

		[DataMember]
		public string  txt_nom_factura
		{
			 get { return _txt_nom_factura; }
			 set {_txt_nom_factura = value;}
		}

		[DataMember]
		public string  cod_seg_ocupacion
		{
			 get { return _cod_seg_ocupacion; }
			 set {_cod_seg_ocupacion = value;}
		}

		[DataMember]
		public string  imp_sueldo
		{
			 get { return _imp_sueldo; }
			 set {_imp_sueldo = value;}
		}

		[DataMember]
		public string  imp_otros_ingresos
		{
			 get { return _imp_otros_ingresos; }
			 set {_imp_otros_ingresos = value;}
		}

		[DataMember]
		public double  cod_ciiu
		{
			 get { return _cod_ciiu; }
			 set {_cod_ciiu = value;}
		}

		[DataMember]
		public string  imp_niveloperativo
		{
			 get { return _imp_niveloperativo; }
			 set {_imp_niveloperativo = value;}
		}

		[DataMember]
		public string  txt_garantia
		{
			 get { return _txt_garantia; }
			 set {_txt_garantia = value;}
		}

		[DataMember]
		public string  cod_baja
		{
			 get { return _cod_baja; }
			 set {_cod_baja = value;}
		}

		[DataMember]
		public string  cod_calif_cart
		{
			 get { return _cod_calif_cart; }
			 set {_cod_calif_cart = value;}
		}

		[DataMember]
		public string  cod_ttipo_empresa
		{
			 get { return _cod_ttipo_empresa; }
			 set {_cod_ttipo_empresa = value;}
		}

		[DataMember]
		public string  cod_tasociacion
		{
			 get { return _cod_tasociacion; }
			 set {_cod_tasociacion = value;}
		}

		[DataMember]
		public int  sn_valida_cgarantia
		{
			 get { return _sn_valida_cgarantia; }
			 set {_sn_valida_cgarantia = value;}
		}

		[DataMember]
		public int  sn_aseg_especial
		{
			 get { return _sn_aseg_especial; }
			 set {_sn_aseg_especial = value;}
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

        [DataMember]
        public int AGENT_AGENCY_ID
        {
            get;
            set;
        }

        [DataMember]
        public int AGENT_INDIVIDUAL_ID
        {
            get;
            set;
        }

        [DataMember]
        public string ANNOTATIONS
        {
            get;
            set;
        }

        [DataMember]
        public string BRANCH_ID
        {
            get;
            set;
        }

        #endregion
	}
}
