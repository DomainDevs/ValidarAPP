using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.Serialization;

namespace Sistran.Co.Previsora.Application.FullServices.Models
{
	[DataContract]
	public class Magente
	{

		#region InnerClass
		public enum MagenteFields
		{
			cod_tipo_agente,
			cod_agente,
			id_persona,
			fec_alta,
			fec_baja,
			cod_baja,
			txt_cheque_a_nom,
			txt_obs,
			cod_grupo,
			nro_carnet,
			cnt_frentes,
			cnt_facturas,
			txt_casillero,
			txt_leyenda_fact,
			cod_zona,
			cod_estado,
			cod_gerente,
			cod_dependencia,
			nro_igss,
			sn_insc_agaps,
			txt_referencia,
			txt_patrocinador,
			nro_resolucion,
			cod_impresion,
			sn_comision,
			pje_comision_gm,
			sn_op_automatica,
			cod_cond_isr,
			fec_ult_modif,
			cod_usuario,
			sn_descuenta_comision,
			txt_auxiliar,
			cod_suc,
			cod_calif_cart,
			sn_corte_cuenta,
			sn_envio_aviso_cobro,
			fec_inactivacion,
			cnt_dias_anul,
			sn_timbre
		}
		#endregion

		#region Data Members

			double _cod_tipo_agente;
			int _cod_agente;
			int _id_persona;
			string _fec_alta;
			string _fec_baja;
			string _cod_baja;
			string _txt_cheque_a_nom;
			string _txt_obs;
			double _cod_grupo;
			string _nro_carnet;
			double _cnt_frentes;
			double _cnt_facturas;
			string _txt_casillero;
			string _txt_leyenda_fact;
			string _cod_zona;
			string _cod_estado;
			string _cod_gerente;
			string _cod_dependencia;
			string _nro_igss;
			string _sn_insc_agaps;
			string _txt_referencia;
			string _txt_patrocinador;
			string _nro_resolucion;
			string _cod_impresion;
			string _sn_comision;
			string _pje_comision_gm;
			string _sn_op_automatica;
			string _cod_cond_isr;
			string _fec_ult_modif;
			string _cod_usuario;
			string _sn_descuenta_comision;
			string _txt_auxiliar;
			string _cod_suc;
			string _cod_calif_cart;
			string _sn_corte_cuenta;
			string _sn_envio_aviso_cobro;
			string _fec_inactivacion;
			string _cnt_dias_anul;
			string _sn_timbre;
			int _identity; 
			char _state; 
			string _connection; 

		#endregion

		#region Properties

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
		public string  txt_cheque_a_nom
		{
			 get { return _txt_cheque_a_nom; }
			 set {_txt_cheque_a_nom = value;}
		}

		[DataMember]
		public string  txt_obs
		{
			 get { return _txt_obs; }
			 set {_txt_obs = value;}
		}

		[DataMember]
		public double  cod_grupo
		{
			 get { return _cod_grupo; }
			 set {_cod_grupo = value;}
		}

		[DataMember]
		public string  nro_carnet
		{
			 get { return _nro_carnet; }
			 set {_nro_carnet = value;}
		}

		[DataMember]
		public double  cnt_frentes
		{
			 get { return _cnt_frentes; }
			 set {_cnt_frentes = value;}
		}

		[DataMember]
		public double  cnt_facturas
		{
			 get { return _cnt_facturas; }
			 set {_cnt_facturas = value;}
		}

		[DataMember]
		public string  txt_casillero
		{
			 get { return _txt_casillero; }
			 set {_txt_casillero = value;}
		}

		[DataMember]
		public string  txt_leyenda_fact
		{
			 get { return _txt_leyenda_fact; }
			 set {_txt_leyenda_fact = value;}
		}

		[DataMember]
		public string  cod_zona
		{
			 get { return _cod_zona; }
			 set {_cod_zona = value;}
		}

		[DataMember]
		public string  cod_estado
		{
			 get { return _cod_estado; }
			 set {_cod_estado = value;}
		}

		[DataMember]
		public string  cod_gerente
		{
			 get { return _cod_gerente; }
			 set {_cod_gerente = value;}
		}

		[DataMember]
		public string  cod_dependencia
		{
			 get { return _cod_dependencia; }
			 set {_cod_dependencia = value;}
		}

		[DataMember]
		public string  nro_igss
		{
			 get { return _nro_igss; }
			 set {_nro_igss = value;}
		}

		[DataMember]
		public string  sn_insc_agaps
		{
			 get { return _sn_insc_agaps; }
			 set {_sn_insc_agaps = value;}
		}

		[DataMember]
		public string  txt_referencia
		{
			 get { return _txt_referencia; }
			 set {_txt_referencia = value;}
		}

		[DataMember]
		public string  txt_patrocinador
		{
			 get { return _txt_patrocinador; }
			 set {_txt_patrocinador = value;}
		}

		[DataMember]
		public string  nro_resolucion
		{
			 get { return _nro_resolucion; }
			 set {_nro_resolucion = value;}
		}

		[DataMember]
		public string  cod_impresion
		{
			 get { return _cod_impresion; }
			 set {_cod_impresion = value;}
		}

		[DataMember]
		public string  sn_comision
		{
			 get { return _sn_comision; }
			 set {_sn_comision = value;}
		}

		[DataMember]
		public string  pje_comision_gm
		{
			 get { return _pje_comision_gm; }
			 set {_pje_comision_gm = value;}
		}

		[DataMember]
		public string  sn_op_automatica
		{
			 get { return _sn_op_automatica; }
			 set {_sn_op_automatica = value;}
		}

		[DataMember]
		public string  cod_cond_isr
		{
			 get { return _cod_cond_isr; }
			 set {_cod_cond_isr = value;}
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
		public string  sn_descuenta_comision
		{
			 get { return _sn_descuenta_comision; }
			 set {_sn_descuenta_comision = value;}
		}

		[DataMember]
		public string  txt_auxiliar
		{
			 get { return _txt_auxiliar; }
			 set {_txt_auxiliar = value;}
		}

		[DataMember]
		public string  cod_suc
		{
			 get { return _cod_suc; }
			 set {_cod_suc = value;}
		}

		[DataMember]
		public string  cod_calif_cart
		{
			 get { return _cod_calif_cart; }
			 set {_cod_calif_cart = value;}
		}

		[DataMember]
		public string  sn_corte_cuenta
		{
			 get { return _sn_corte_cuenta; }
			 set {_sn_corte_cuenta = value;}
		}

		[DataMember]
		public string  sn_envio_aviso_cobro
		{
			 get { return _sn_envio_aviso_cobro; }
			 set {_sn_envio_aviso_cobro = value;}
		}

		[DataMember]
		public string  fec_inactivacion
		{
			 get { return _fec_inactivacion; }
			 set {_fec_inactivacion = value;}
		}

		[DataMember]
		public string  cnt_dias_anul
		{
			 get { return _cnt_dias_anul; }
			 set {_cnt_dias_anul = value;}
		}

		[DataMember]
		public string  sn_timbre
		{
			 get { return _sn_timbre; }
			 set {_sn_timbre = value;}
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
