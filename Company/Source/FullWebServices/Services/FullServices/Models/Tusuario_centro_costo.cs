using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.Serialization;

namespace Sistran.Co.Previsora.Application.FullServices.Models
{
	[DataContract]
	public class Tusuario_centro_costo
	{

		#region InnerClass
		public enum Tusuario_centro_costoFields
		{
			cod_usuario,
			cod_cencosto,
			sn_activo
		}
		#endregion

		#region Data Members

			string _cod_usuario;
			double _cod_cencosto;
			int _sn_activo;
			int _identity; 
			char _state; 
			string _connection; 

		#endregion

		#region Properties

		[DataMember]
		public string  cod_usuario
		{
			 get { return _cod_usuario; }
			 set {_cod_usuario = value;}
		}

		[DataMember]
		public double  cod_cencosto
		{
			 get { return _cod_cencosto; }
			 set {_cod_cencosto = value;}
		}

		[DataMember]
		public int  sn_activo
		{
			 get { return _sn_activo; }
			 set {_sn_activo = value;}
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
