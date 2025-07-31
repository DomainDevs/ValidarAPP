using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.Serialization;

namespace Sistran.Co.Previsora.Application.FullServices.Models
{
	[DataContract]
	public class Frm_sarlaft_detalle
	{

		#region InnerClass
		public enum Frm_sarlaft_detalleFields
		{
			id_persona,
			id_formulario,
			aaaa_formulario,
			nro_formulario,
			origen,
			cod_suc,
			fec_diligenciamiento,
			fec_verifica,
			txt_usuario_veri,
			cod_usuario_sise,
			txt_usuario_auto,
			fec_registro,
			fec_aprobacion
		}
		#endregion

		#region Data Members

			int _id_persona;
			int _id_formulario;
			int _aaaa_formulario;
			double _nro_formulario;
			string _origen;
			double _cod_suc;
			string _fec_diligenciamiento;
			string _fec_verifica;
			string _txt_usuario_veri;
			string _cod_usuario_sise;
			string _txt_usuario_auto;
			string _fec_registro;
			string _fec_aprobacion;
            double _cod_tipo_doc;
            string _documento;
            string _nombrers;
			int _identity; 
			char _state; 
			string _connection;
            char _state_3G;

		#endregion

		#region Properties

		[DataMember]
		public int  id_persona
		{
			 get { return _id_persona; }
			 set {_id_persona = value;}
		}

		[DataMember]
		public int  id_formulario
		{
			 get { return _id_formulario; }
			 set {_id_formulario = value;}
		}

		[DataMember]
		public int  aaaa_formulario
		{
			 get { return _aaaa_formulario; }
			 set {_aaaa_formulario = value;}
		}

		[DataMember]
		public double  nro_formulario
		{
			 get { return _nro_formulario; }
			 set {_nro_formulario = value;}
		}

		[DataMember]
		public string  origen
		{
			 get { return _origen; }
			 set {_origen = value;}
		}

		[DataMember]
		public double  cod_suc
		{
			 get { return _cod_suc; }
			 set {_cod_suc = value;}
		}

		[DataMember]
		public string  fec_diligenciamiento
		{
			 get { return _fec_diligenciamiento; }
			 set {_fec_diligenciamiento = value;}
		}

		[DataMember]
		public string  fec_verifica
		{
			 get { return _fec_verifica; }
			 set {_fec_verifica = value;}
		}

		[DataMember]
		public string  txt_usuario_veri
		{
			 get { return _txt_usuario_veri; }
			 set {_txt_usuario_veri = value;}
		}

		[DataMember]
		public string  cod_usuario_sise
		{
			 get { return _cod_usuario_sise; }
			 set {_cod_usuario_sise = value;}
		}

		[DataMember]
		public string  txt_usuario_auto
		{
			 get { return _txt_usuario_auto; }
			 set {_txt_usuario_auto = value;}
		}

		[DataMember]
		public string  fec_registro
		{
			 get { return _fec_registro; }
			 set {_fec_registro = value;}
		}

		[DataMember]
		public string  fec_aprobacion
		{
			 get { return _fec_aprobacion; }
			 set {_fec_aprobacion = value;}
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
        public double cod_tipo_doc
        {
            get { return _cod_tipo_doc; }
            set { _cod_tipo_doc = value; }
        }

        [DataMember]
        public string documento
        {
            get { return _documento; }
            set { _documento = value; }
        }

        [DataMember]
        public string nombrers
        {
            get { return _nombrers; }
            set { _nombrers = value; }
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
