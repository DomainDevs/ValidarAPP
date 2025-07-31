using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.Serialization;

namespace Sistran.Co.Previsora.Application.FullServices.Models
{
	[DataContract]
	public class Frm_sarlaft_info_financiera
	{

		#region InnerClass
		public enum Frm_sarlaft_info_financieraFields
		{
			id_formulario,
			imp_ingresos,
			imp_egresos,
			imp_otros_ingr,
			cod_act_secundaria,
			imp_activos,
			imp_pasivos,
			cod_act_principal,
			cod_ciiu_ppal_nuevo,
			cod_ciiu_scndria_nuevo
		}
		#endregion

		#region Data Members

			int _id_formulario;
			double _imp_ingresos;
			double _imp_egresos;
			double _imp_otros_ingr;
			double _cod_act_secundaria;
			double _imp_activos;
			double _imp_pasivos;
			double _cod_act_principal;
			double _cod_ciiu_ppal_nuevo;
			double _cod_ciiu_scndria_nuevo;
			int _identity; 
			char _state;
            char _state_3G;
			string _connection; 

		#endregion

		#region Properties

		[DataMember]
		public int  id_formulario
		{
			 get { return _id_formulario; }
			 set {_id_formulario = value;}
		}

		[DataMember]
		public double  imp_ingresos
		{
			 get { return _imp_ingresos; }
			 set {_imp_ingresos = value;}
		}

		[DataMember]
		public double  imp_egresos
		{
			 get { return _imp_egresos; }
			 set {_imp_egresos = value;}
		}

		[DataMember]
		public double  imp_otros_ingr
		{
			 get { return _imp_otros_ingr; }
			 set {_imp_otros_ingr = value;}
		}

		[DataMember]
		public double  cod_act_secundaria
		{
			 get { return _cod_act_secundaria; }
			 set {_cod_act_secundaria = value;}
		}

		[DataMember]
		public double  imp_activos
		{
			 get { return _imp_activos; }
			 set {_imp_activos = value;}
		}

		[DataMember]
		public double  imp_pasivos
		{
			 get { return _imp_pasivos; }
			 set {_imp_pasivos = value;}
		}

		[DataMember]
		public double  cod_act_principal
		{
			 get { return _cod_act_principal; }
			 set {_cod_act_principal = value;}
		}

		[DataMember]
		public double  cod_ciiu_ppal_nuevo
		{
			 get { return _cod_ciiu_ppal_nuevo; }
			 set {_cod_ciiu_ppal_nuevo = value;}
		}

		[DataMember]
		public double  cod_ciiu_scndria_nuevo
		{
			 get { return _cod_ciiu_scndria_nuevo; }
			 set {_cod_ciiu_scndria_nuevo = value;}
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
