using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.Serialization;

namespace Sistran.Co.Previsora.Application.FullServices.Models
{
	[DataContract]
	public class Ttarifa
	{

		#region InnerClass
		public enum TtarifaFields
		{
			cod_ramo,
			cod_subramo,
			cod_tarifa,
			txt_desc,
			cod_deduc,
			cod_item_deduc_defa,
			cod_categ,
			cod_item_categ_defa,
			sn_principal,
			sn_ast,
			sn_rc,
			txt_lim_comun,
			txt_lim_agreg,
			cod_riesgo,
			col_cresta,
			sn_pje,
			sn_imprime,
			sn_adicional,
			sn_acumsum_reas,
			sn_habilita,
			fec_habilita,
			fec_deshabilita
		}
		#endregion

		#region Data Members

			double _cod_ramo;
			double _cod_subramo;
			double _cod_tarifa;
			string _txt_desc;
			double _cod_deduc;
			double _cod_item_deduc_defa;
			double _cod_categ;
			double _cod_item_categ_defa;
			string _sn_principal;
			string _sn_ast;
			string _sn_rc;
			string _txt_lim_comun;
			string _txt_lim_agreg;
			string _cod_riesgo;
			string _col_cresta;
			string _sn_pje;
			string _sn_imprime;
			string _sn_adicional;
			string _sn_acumsum_reas;
			string _sn_habilita;
			string _fec_habilita;
			string _fec_deshabilita;
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
		public double  cod_subramo
		{
			 get { return _cod_subramo; }
			 set {_cod_subramo = value;}
		}

		[DataMember]
		public double  cod_tarifa
		{
			 get { return _cod_tarifa; }
			 set {_cod_tarifa = value;}
		}

		[DataMember]
		public string  txt_desc
		{
			 get { return _txt_desc; }
			 set {_txt_desc = value;}
		}

		[DataMember]
		public double  cod_deduc
		{
			 get { return _cod_deduc; }
			 set {_cod_deduc = value;}
		}

		[DataMember]
		public double  cod_item_deduc_defa
		{
			 get { return _cod_item_deduc_defa; }
			 set {_cod_item_deduc_defa = value;}
		}

		[DataMember]
		public double  cod_categ
		{
			 get { return _cod_categ; }
			 set {_cod_categ = value;}
		}

		[DataMember]
		public double  cod_item_categ_defa
		{
			 get { return _cod_item_categ_defa; }
			 set {_cod_item_categ_defa = value;}
		}

		[DataMember]
		public string  sn_principal
		{
			 get { return _sn_principal; }
			 set {_sn_principal = value;}
		}

		[DataMember]
		public string  sn_ast
		{
			 get { return _sn_ast; }
			 set {_sn_ast = value;}
		}

		[DataMember]
		public string  sn_rc
		{
			 get { return _sn_rc; }
			 set {_sn_rc = value;}
		}

		[DataMember]
		public string  txt_lim_comun
		{
			 get { return _txt_lim_comun; }
			 set {_txt_lim_comun = value;}
		}

		[DataMember]
		public string  txt_lim_agreg
		{
			 get { return _txt_lim_agreg; }
			 set {_txt_lim_agreg = value;}
		}

		[DataMember]
		public string  cod_riesgo
		{
			 get { return _cod_riesgo; }
			 set {_cod_riesgo = value;}
		}

		[DataMember]
		public string  col_cresta
		{
			 get { return _col_cresta; }
			 set {_col_cresta = value;}
		}

		[DataMember]
		public string  sn_pje
		{
			 get { return _sn_pje; }
			 set {_sn_pje = value;}
		}

		[DataMember]
		public string  sn_imprime
		{
			 get { return _sn_imprime; }
			 set {_sn_imprime = value;}
		}

		[DataMember]
		public string  sn_adicional
		{
			 get { return _sn_adicional; }
			 set {_sn_adicional = value;}
		}

		[DataMember]
		public string  sn_acumsum_reas
		{
			 get { return _sn_acumsum_reas; }
			 set {_sn_acumsum_reas = value;}
		}

		[DataMember]
		public string  sn_habilita
		{
			 get { return _sn_habilita; }
			 set {_sn_habilita = value;}
		}

		[DataMember]
		public string  fec_habilita
		{
			 get { return _fec_habilita; }
			 set {_fec_habilita = value;}
		}

		[DataMember]
		public string  fec_deshabilita
		{
			 get { return _fec_deshabilita; }
			 set {_fec_deshabilita = value;}
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
