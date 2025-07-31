using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.Serialization;

namespace Sistran.Co.Previsora.Application.FullServices.Models
{
	[DataContract]
	public class Tramo
	{

		#region InnerClass
		public enum TramoFields
		{
			cod_ramo,
			txt_desc_abrev,
			txt_desc_redu,
			txt_desc,
			sn_iva,
			pje_bomberos,
			cod_ttipo_ramo,
			pje_rrc,
			ult_nro_poliza,
			ult_nro_stro,
			sn_det_cta_cte,
			cod_grupo,
			cod_ramo_super,
			cod_ramo_fasecolda,
			sn_emision,
			cnt_ptos_comision,
			cod_rolfil,
			cod_ramo_cumulo_reas,
			cod_tipo_ejer_reas,
			cod_estado,
			fecha_modificacion
		}
		#endregion

		#region Data Members

			double _cod_ramo;
			string _txt_desc_abrev;
			string _txt_desc_redu;
			string _txt_desc;
			string _sn_iva;
			string _pje_bomberos;
			double _cod_ttipo_ramo;
			string _pje_rrc;
			string _ult_nro_poliza;
			double _ult_nro_stro;
			string _sn_det_cta_cte;
			string _cod_grupo;
			string _cod_ramo_super;
			string _cod_ramo_fasecolda;
			string _sn_emision;
			string _cnt_ptos_comision;
			string _cod_rolfil;
			string _cod_ramo_cumulo_reas;
			string _cod_tipo_ejer_reas;
			double _cod_estado;
			string _fecha_modificacion;
			int _identity; 
			char _state; 
			string _connection; 

		#endregion

		#region Properties

		[DataMember]
		public double  cod_ramo
		{
			 get { return _cod_ramo; }
			 set {_cod_ramo = value;}
		}

		[DataMember]
		public string  txt_desc_abrev
		{
			 get { return _txt_desc_abrev; }
			 set {_txt_desc_abrev = value;}
		}

		[DataMember]
		public string  txt_desc_redu
		{
			 get { return _txt_desc_redu; }
			 set {_txt_desc_redu = value;}
		}

		[DataMember]
		public string  txt_desc
		{
			 get { return _txt_desc; }
			 set {_txt_desc = value;}
		}

		[DataMember]
		public string  sn_iva
		{
			 get { return _sn_iva; }
			 set {_sn_iva = value;}
		}

		[DataMember]
		public string  pje_bomberos
		{
			 get { return _pje_bomberos; }
			 set {_pje_bomberos = value;}
		}

		[DataMember]
		public double  cod_ttipo_ramo
		{
			 get { return _cod_ttipo_ramo; }
			 set {_cod_ttipo_ramo = value;}
		}

		[DataMember]
		public string  pje_rrc
		{
			 get { return _pje_rrc; }
			 set {_pje_rrc = value;}
		}

		[DataMember]
		public string  ult_nro_poliza
		{
			 get { return _ult_nro_poliza; }
			 set {_ult_nro_poliza = value;}
		}

		[DataMember]
		public double  ult_nro_stro
		{
			 get { return _ult_nro_stro; }
			 set {_ult_nro_stro = value;}
		}

		[DataMember]
		public string  sn_det_cta_cte
		{
			 get { return _sn_det_cta_cte; }
			 set {_sn_det_cta_cte = value;}
		}

		[DataMember]
		public string  cod_grupo
		{
			 get { return _cod_grupo; }
			 set {_cod_grupo = value;}
		}

		[DataMember]
		public string  cod_ramo_super
		{
			 get { return _cod_ramo_super; }
			 set {_cod_ramo_super = value;}
		}

		[DataMember]
		public string  cod_ramo_fasecolda
		{
			 get { return _cod_ramo_fasecolda; }
			 set {_cod_ramo_fasecolda = value;}
		}

		[DataMember]
		public string  sn_emision
		{
			 get { return _sn_emision; }
			 set {_sn_emision = value;}
		}

		[DataMember]
		public string  cnt_ptos_comision
		{
			 get { return _cnt_ptos_comision; }
			 set {_cnt_ptos_comision = value;}
		}

		[DataMember]
		public string  cod_rolfil
		{
			 get { return _cod_rolfil; }
			 set {_cod_rolfil = value;}
		}

		[DataMember]
		public string  cod_ramo_cumulo_reas
		{
			 get { return _cod_ramo_cumulo_reas; }
			 set {_cod_ramo_cumulo_reas = value;}
		}

		[DataMember]
		public string  cod_tipo_ejer_reas
		{
			 get { return _cod_tipo_ejer_reas; }
			 set {_cod_tipo_ejer_reas = value;}
		}

		[DataMember]
		public double  cod_estado
		{
			 get { return _cod_estado; }
			 set {_cod_estado = value;}
		}

		[DataMember]
		public string  fecha_modificacion
		{
			 get { return _fecha_modificacion; }
			 set {_fecha_modificacion = value;}
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
