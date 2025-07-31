using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.Serialization;

namespace Sistran.Co.Previsora.Application.FullServices.Models
{
	[DataContract]
	public class Tcpto_pago_egreso_suc
	{

		#region InnerClass
		public enum Tcpto_pago_egreso_sucFields
		{
			cod_cpto,
			cod_suc,
			txt_desc,
			cod_cta_cble,
			cod_clase_pago,
			sn_prestador,
			sn_agente,
			sn_fondofijo,
			sn_reclamo,
			aplica_a,
			sn_descuento,
			cod_cpto_retefuente,
			sn_activo,
			id_cta_puente,
			sn_proceso_jud,
			sn_afecta_impuestos
		}
		#endregion

		#region Data Members

			decimal _cod_cpto;
			decimal _cod_suc;
			string _txt_desc;
			string _cod_cta_cble;
			decimal _cod_clase_pago;
            double _sn_prestador;
            double _sn_agente;
            double _sn_fondofijo;
            double _sn_reclamo;
			string _aplica_a;
            double? _sn_descuento;
			int? _cod_cpto_retefuente;
            double? _sn_activo;
			int? _id_cta_puente;
            double? _sn_proceso_jud;
            double? _sn_afecta_impuestos;
            bool? _enabled;
			int _identity; 
			char _state; 
			string _connection; 

		#endregion

		#region Properties

		[DataMember]
		public decimal  cod_cpto
		{
			 get { return _cod_cpto; }
			 set {_cod_cpto = value;}
		}

		[DataMember]
		public decimal  cod_suc
		{
			 get { return _cod_suc; }
			 set {_cod_suc = value;}
		}

		[DataMember]
		public string  txt_desc
		{
			 get { return _txt_desc; }
			 set {_txt_desc = value;}
		}

		[DataMember]
		public string  cod_cta_cble
		{
			 get { return _cod_cta_cble; }
			 set {_cod_cta_cble = value;}
		}

		[DataMember]
		public decimal  cod_clase_pago
		{
			 get { return _cod_clase_pago; }
			 set {_cod_clase_pago = value;}
		}

		[DataMember]
        public double sn_prestador
		{
			 get { return _sn_prestador; }
			 set {_sn_prestador = value;}
		}

		[DataMember]
        public double sn_agente
		{
			 get { return _sn_agente; }
			 set {_sn_agente = value;}
		}

		[DataMember]
        public double sn_fondofijo
		{
			 get { return _sn_fondofijo; }
			 set {_sn_fondofijo = value;}
		}

		[DataMember]
        public double sn_reclamo
		{
			 get { return _sn_reclamo; }
			 set {_sn_reclamo = value;}
		}

		[DataMember]
		public string  aplica_a
		{
			 get { return _aplica_a; }
			 set {_aplica_a = value;}
		}

		[DataMember]
        public double? sn_descuento
		{
			 get { return _sn_descuento; }
			 set {_sn_descuento = value;}
		}

		[DataMember]
		public int?  cod_cpto_retefuente
		{
			 get { return _cod_cpto_retefuente; }
			 set {_cod_cpto_retefuente = value;}
		}

		[DataMember]
        public double? sn_activo
		{
			 get { return _sn_activo; }
			 set {_sn_activo = value;}
		}

		[DataMember]
		public int?  id_cta_puente
		{
			 get { return _id_cta_puente; }
			 set {_id_cta_puente = value;}
		}

		[DataMember]
        public double? sn_proceso_jud
		{
			 get { return _sn_proceso_jud; }
			 set {_sn_proceso_jud = value;}
		}

		[DataMember]
        public double? sn_afecta_impuestos
		{
			 get { return _sn_afecta_impuestos; }
			 set {_sn_afecta_impuestos = value;}
		}

        [DataMember]
        public bool? enabled
        {
            get { return _enabled; }
            set { _enabled = value; }
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
