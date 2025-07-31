using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.Serialization;

namespace Sistran.Co.Previsora.Application.FullServices.Models
{
	[DataContract]
	public class Maseg_ficha_tec_financ
	{

		#region InnerClass
		public enum Maseg_ficha_tec_financFields
		{
			cod_aseg,
			fec_informacion,
			imp_inventarios,
			imp_cuentas_cobrar,
			imp_activo_cte,
			imp_equipos,
			imp_activo_fijo,
			imp_activo_total,
			imp_pasivo_cte,
			imp_pasivo_lplazo,
			imp_pasivo_total,
			imp_patrimonio,
			imp_costo_vtas,
			imp_ventas,
			imp_util_bruta,
			imp_util_neta,
			imp_util_oper,
			imp_terrenos,
			imp_edificios,
			imp_ctas_pagar,
			imp_obl_bancos,
			imp_obl_lplazo,
			imp_cap_social,
			imp_util_acum,
			imp_reval_patrim,
			imp_invers_temp,
			imp_activ_fijos_br,
			imp_superavit_valoriz,
			imp_otros_superavit,
			imp_reservas,
			imp_primas,
			imp_otros_ingr_nopera,
			imp_ajustes_infl,
			imp_intereses,
			imp_otros_gastos_nopera,
			imp_valorizaciones,
			fec_creacion,
			fec_modif,
			cod_usuario_crea,
			cod_usuario_modif
		}
		#endregion

		#region Data Members

			int _cod_aseg;
			string _fec_informacion;
			double _imp_inventarios;
			double _imp_cuentas_cobrar;
			double _imp_activo_cte;
			double _imp_equipos;
			double _imp_activo_fijo;
			double _imp_activo_total;
			double _imp_pasivo_cte;
			double _imp_pasivo_lplazo;
			double _imp_pasivo_total;
			double _imp_patrimonio;
			double _imp_costo_vtas;
			double _imp_ventas;
			double _imp_util_bruta;
			double _imp_util_neta;
			double _imp_util_oper;
			double _imp_terrenos;
			double _imp_edificios;
			double _imp_ctas_pagar;
			double _imp_obl_bancos;
			double _imp_obl_lplazo;
			double _imp_cap_social;
			double _imp_util_acum;
			double _imp_reval_patrim;
			double _imp_invers_temp;
			double _imp_activ_fijos_br;
			double _imp_superavit_valoriz;
			double _imp_otros_superavit;
			double _imp_reservas;
			double _imp_primas;
			double _imp_otros_ingr_nopera;
			double _imp_ajustes_infl;
			double _imp_intereses;
			double _imp_otros_gastos_nopera;
			double _imp_valorizaciones;
			string _fec_creacion;
			string _fec_modif;
			string _cod_usuario_crea;
			string _cod_usuario_modif;
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
		public string  fec_informacion
		{
			 get { return _fec_informacion; }
			 set {_fec_informacion = value;}
		}

		[DataMember]
		public double  imp_inventarios
		{
			 get { return _imp_inventarios; }
			 set {_imp_inventarios = value;}
		}

		[DataMember]
		public double  imp_cuentas_cobrar
		{
			 get { return _imp_cuentas_cobrar; }
			 set {_imp_cuentas_cobrar = value;}
		}

		[DataMember]
		public double  imp_activo_cte
		{
			 get { return _imp_activo_cte; }
			 set {_imp_activo_cte = value;}
		}

		[DataMember]
		public double  imp_equipos
		{
			 get { return _imp_equipos; }
			 set {_imp_equipos = value;}
		}

		[DataMember]
		public double  imp_activo_fijo
		{
			 get { return _imp_activo_fijo; }
			 set {_imp_activo_fijo = value;}
		}

		[DataMember]
		public double  imp_activo_total
		{
			 get { return _imp_activo_total; }
			 set {_imp_activo_total = value;}
		}

		[DataMember]
		public double  imp_pasivo_cte
		{
			 get { return _imp_pasivo_cte; }
			 set {_imp_pasivo_cte = value;}
		}

		[DataMember]
		public double  imp_pasivo_lplazo
		{
			 get { return _imp_pasivo_lplazo; }
			 set {_imp_pasivo_lplazo = value;}
		}

		[DataMember]
		public double  imp_pasivo_total
		{
			 get { return _imp_pasivo_total; }
			 set {_imp_pasivo_total = value;}
		}

		[DataMember]
		public double  imp_patrimonio
		{
			 get { return _imp_patrimonio; }
			 set {_imp_patrimonio = value;}
		}

		[DataMember]
		public double  imp_costo_vtas
		{
			 get { return _imp_costo_vtas; }
			 set {_imp_costo_vtas = value;}
		}

		[DataMember]
		public double  imp_ventas
		{
			 get { return _imp_ventas; }
			 set {_imp_ventas = value;}
		}

		[DataMember]
		public double  imp_util_bruta
		{
			 get { return _imp_util_bruta; }
			 set {_imp_util_bruta = value;}
		}

		[DataMember]
		public double  imp_util_neta
		{
			 get { return _imp_util_neta; }
			 set {_imp_util_neta = value;}
		}

		[DataMember]
		public double  imp_util_oper
		{
			 get { return _imp_util_oper; }
			 set {_imp_util_oper = value;}
		}

		[DataMember]
		public double  imp_terrenos
		{
			 get { return _imp_terrenos; }
			 set {_imp_terrenos = value;}
		}

		[DataMember]
		public double  imp_edificios
		{
			 get { return _imp_edificios; }
			 set {_imp_edificios = value;}
		}

		[DataMember]
		public double  imp_ctas_pagar
		{
			 get { return _imp_ctas_pagar; }
			 set {_imp_ctas_pagar = value;}
		}

		[DataMember]
		public double  imp_obl_bancos
		{
			 get { return _imp_obl_bancos; }
			 set {_imp_obl_bancos = value;}
		}

		[DataMember]
		public double  imp_obl_lplazo
		{
			 get { return _imp_obl_lplazo; }
			 set {_imp_obl_lplazo = value;}
		}

		[DataMember]
		public double  imp_cap_social
		{
			 get { return _imp_cap_social; }
			 set {_imp_cap_social = value;}
		}

		[DataMember]
		public double  imp_util_acum
		{
			 get { return _imp_util_acum; }
			 set {_imp_util_acum = value;}
		}

		[DataMember]
		public double  imp_reval_patrim
		{
			 get { return _imp_reval_patrim; }
			 set {_imp_reval_patrim = value;}
		}

		[DataMember]
		public double  imp_invers_temp
		{
			 get { return _imp_invers_temp; }
			 set {_imp_invers_temp = value;}
		}

		[DataMember]
		public double  imp_activ_fijos_br
		{
			 get { return _imp_activ_fijos_br; }
			 set {_imp_activ_fijos_br = value;}
		}

		[DataMember]
		public double  imp_superavit_valoriz
		{
			 get { return _imp_superavit_valoriz; }
			 set {_imp_superavit_valoriz = value;}
		}

		[DataMember]
		public double  imp_otros_superavit
		{
			 get { return _imp_otros_superavit; }
			 set {_imp_otros_superavit = value;}
		}

		[DataMember]
		public double  imp_reservas
		{
			 get { return _imp_reservas; }
			 set {_imp_reservas = value;}
		}

		[DataMember]
		public double  imp_primas
		{
			 get { return _imp_primas; }
			 set {_imp_primas = value;}
		}

		[DataMember]
		public double  imp_otros_ingr_nopera
		{
			 get { return _imp_otros_ingr_nopera; }
			 set {_imp_otros_ingr_nopera = value;}
		}

		[DataMember]
		public double  imp_ajustes_infl
		{
			 get { return _imp_ajustes_infl; }
			 set {_imp_ajustes_infl = value;}
		}

		[DataMember]
		public double  imp_intereses
		{
			 get { return _imp_intereses; }
			 set {_imp_intereses = value;}
		}

		[DataMember]
		public double  imp_otros_gastos_nopera
		{
			 get { return _imp_otros_gastos_nopera; }
			 set {_imp_otros_gastos_nopera = value;}
		}

		[DataMember]
		public double  imp_valorizaciones
		{
			 get { return _imp_valorizaciones; }
			 set {_imp_valorizaciones = value;}
		}

		[DataMember]
		public string  fec_creacion
		{
			 get { return _fec_creacion; }
			 set {_fec_creacion = value;}
		}

		[DataMember]
		public string  fec_modif
		{
			 get { return _fec_modif; }
			 set {_fec_modif = value;}
		}

		[DataMember]
		public string  cod_usuario_crea
		{
			 get { return _cod_usuario_crea; }
			 set {_cod_usuario_crea = value;}
		}

		[DataMember]
		public string  cod_usuario_modif
		{
			 get { return _cod_usuario_modif; }
			 set {_cod_usuario_modif = value;}
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
