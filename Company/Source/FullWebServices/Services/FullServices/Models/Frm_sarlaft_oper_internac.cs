using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.Serialization;

namespace Sistran.Co.Previsora.Application.FullServices.Models
{
	[DataContract]
	public class Frm_sarlaft_oper_internac
	{

		#region InnerClass
		public enum Frm_sarlaft_oper_internacFields
		{
			id_formulario,
			consecutivo_oper,
			cod_tipo_operacion,
			cod_tipo_producto,
			imp_producto,
			cod_moneda,
			cod_pais_origen,
			txt_entidad,
			nro_producto,
			cod_dpto,
			cod_municipio
		}
		#endregion

		#region Data Members

			int _id_formulario;
			double _consecutivo_oper;
			double _cod_tipo_operacion;
			double _cod_tipo_producto;
			double _imp_producto;
			double _cod_moneda;
			double _cod_pais_origen;
			string _txt_entidad;
			string _nro_producto;
			double _cod_dpto;
			double _cod_municipio;
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
		public double  consecutivo_oper
		{
			 get { return _consecutivo_oper; }
			 set {_consecutivo_oper = value;}
		}

		[DataMember]
		public double  cod_tipo_operacion
		{
			 get { return _cod_tipo_operacion; }
			 set {_cod_tipo_operacion = value;}
		}

		[DataMember]
		public double  cod_tipo_producto
		{
			 get { return _cod_tipo_producto; }
			 set {_cod_tipo_producto = value;}
		}

		[DataMember]
		public double  imp_producto
		{
			 get { return _imp_producto; }
			 set {_imp_producto = value;}
		}

		[DataMember]
		public double  cod_moneda
		{
			 get { return _cod_moneda; }
			 set {_cod_moneda = value;}
		}

		[DataMember]
		public double  cod_pais_origen
		{
			 get { return _cod_pais_origen; }
			 set {_cod_pais_origen = value;}
		}

		[DataMember]
		public string  txt_entidad
		{
			 get { return _txt_entidad; }
			 set {_txt_entidad = value;}
		}

		[DataMember]
		public string  nro_producto
		{
			 get { return _nro_producto; }
			 set {_nro_producto = value;}
		}

		[DataMember]
		public double  cod_dpto
		{
			 get { return _cod_dpto; }
			 set {_cod_dpto = value;}
		}

		[DataMember]
		public double  cod_municipio
		{
			 get { return _cod_municipio; }
			 set {_cod_municipio = value;}
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
