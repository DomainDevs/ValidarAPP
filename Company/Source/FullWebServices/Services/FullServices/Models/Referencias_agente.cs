using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.Serialization;

namespace Sistran.Co.Previsora.Application.FullServices.Models
{
	[DataContract]
	public class Referencias_agente
	{

		#region InnerClass
		public enum Referencias_agenteFields
		{
			cod_agente,
			cod_tipo_agente,
			tipo_ref_agente,
			sn_ref1,
			sn_ref2,
			txt_nombre,
			txt_direccion,
			txt_telefono,
			txt_producto,
			num_producto
		}
		#endregion

		#region Data Members

			int _cod_agente;
			double _cod_tipo_agente;
			double _tipo_ref_agente;
			int _sn_ref1;
			int _sn_ref2;
			string _txt_nombre;
			string _txt_direccion;
			string _txt_telefono;
			string _txt_producto;
			string _num_producto;
			int _identity; 
			char _state; 
			string _connection; 

		#endregion

		#region Properties

		[DataMember]
		public int  cod_agente
		{
			 get { return _cod_agente; }
			 set {_cod_agente = value;}
		}

		[DataMember]
		public double  cod_tipo_agente
		{
			 get { return _cod_tipo_agente; }
			 set {_cod_tipo_agente = value;}
		}

		[DataMember]
		public double  tipo_ref_agente
		{
			 get { return _tipo_ref_agente; }
			 set {_tipo_ref_agente = value;}
		}

		[DataMember]
		public int  sn_ref1
		{
			 get { return _sn_ref1; }
			 set {_sn_ref1 = value;}
		}

		[DataMember]
		public int  sn_ref2
		{
			 get { return _sn_ref2; }
			 set {_sn_ref2 = value;}
		}

		[DataMember]
		public string  txt_nombre
		{
			 get { return _txt_nombre; }
			 set {_txt_nombre = value;}
		}

		[DataMember]
		public string  txt_direccion
		{
			 get { return _txt_direccion; }
			 set {_txt_direccion = value;}
		}

		[DataMember]
		public string  txt_telefono
		{
			 get { return _txt_telefono; }
			 set {_txt_telefono = value;}
		}

		[DataMember]
		public string  txt_producto
		{
			 get { return _txt_producto; }
			 set {_txt_producto = value;}
		}

		[DataMember]
		public string  num_producto
		{
			 get { return _num_producto; }
			 set {_num_producto = value;}
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
