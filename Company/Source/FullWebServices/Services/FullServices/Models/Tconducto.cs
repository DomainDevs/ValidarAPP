using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.Serialization;

namespace Sistran.Co.Previsora.Application.FullServices.Models
{
	[DataContract]
	public class Tconducto
	{

		#region InnerClass
		public enum TconductoFields
		{
			cod_conducto,
			txt_desc_cond,
			cod_tipo_conducto,
			cnt_digitos_tarj,
			dd_rend_tarj,
			imp_limite,
			id_banco,
			txt_pgma_asoc,
			pje_comision,
			cod_valor_asociado,
			id_bco_receptor_default,
			cod_cta_cble_comision,
			cod_cta_cble_cuenta_cobrar,
			sn_emision,
			sn_ingresos,
			sn_datafono
		}
		#endregion

		#region Data Members

			double _cod_conducto;
			string _txt_desc_cond;
			double _cod_tipo_conducto;
			string _cnt_digitos_tarj;
			string _dd_rend_tarj;
			double _imp_limite;
			string _id_banco;
			string _txt_pgma_asoc;
			string _pje_comision;
			string _cod_valor_asociado;
			string _id_bco_receptor_default;
			string _cod_cta_cble_comision;
			string _cod_cta_cble_cuenta_cobrar;
			string _sn_emision;
			string _sn_ingresos;
			string _sn_datafono;
			int _identity; 
			char _state; 
			string _connection; 

		#endregion

		#region Properties

		[DataMember]
		public double  cod_conducto
		{
			 get { return _cod_conducto; }
			 set {_cod_conducto = value;}
		}

		[DataMember]
		public string  txt_desc_cond
		{
			 get { return _txt_desc_cond; }
			 set {_txt_desc_cond = value;}
		}

		[DataMember]
		public double  cod_tipo_conducto
		{
			 get { return _cod_tipo_conducto; }
			 set {_cod_tipo_conducto = value;}
		}

		[DataMember]
		public string  cnt_digitos_tarj
		{
			 get { return _cnt_digitos_tarj; }
			 set {_cnt_digitos_tarj = value;}
		}

		[DataMember]
		public string  dd_rend_tarj
		{
			 get { return _dd_rend_tarj; }
			 set {_dd_rend_tarj = value;}
		}

		[DataMember]
		public double  imp_limite
		{
			 get { return _imp_limite; }
			 set {_imp_limite = value;}
		}

		[DataMember]
		public string  id_banco
		{
			 get { return _id_banco; }
			 set {_id_banco = value;}
		}

		[DataMember]
		public string  txt_pgma_asoc
		{
			 get { return _txt_pgma_asoc; }
			 set {_txt_pgma_asoc = value;}
		}

		[DataMember]
		public string  pje_comision
		{
			 get { return _pje_comision; }
			 set {_pje_comision = value;}
		}

		[DataMember]
		public string  cod_valor_asociado
		{
			 get { return _cod_valor_asociado; }
			 set {_cod_valor_asociado = value;}
		}

		[DataMember]
		public string  id_bco_receptor_default
		{
			 get { return _id_bco_receptor_default; }
			 set {_id_bco_receptor_default = value;}
		}

		[DataMember]
		public string  cod_cta_cble_comision
		{
			 get { return _cod_cta_cble_comision; }
			 set {_cod_cta_cble_comision = value;}
		}

		[DataMember]
		public string  cod_cta_cble_cuenta_cobrar
		{
			 get { return _cod_cta_cble_cuenta_cobrar; }
			 set {_cod_cta_cble_cuenta_cobrar = value;}
		}

		[DataMember]
		public string  sn_emision
		{
			 get { return _sn_emision; }
			 set {_sn_emision = value;}
		}

		[DataMember]
		public string  sn_ingresos
		{
			 get { return _sn_ingresos; }
			 set {_sn_ingresos = value;}
		}

		[DataMember]
		public string  sn_datafono
		{
			 get { return _sn_datafono; }
			 set {_sn_datafono = value;}
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
