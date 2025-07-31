using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.Serialization;

namespace Sistran.Co.Previsora.Application.FullServices.Models
{
	[DataContract]
	public class Magente_organizador
	{

		#region InnerClass
		public enum Magente_organizadorFields
		{
			cod_tipo_agente_ppal,
			cod_agente_ppal,
			cod_tipo_agente,
			cod_agente
		}
		#endregion

		#region Data Members

			double _cod_tipo_agente_ppal;
			double _cod_agente_ppal;
			double _cod_tipo_agente;
			int _cod_agente;
			int _identity; 
			char _state; 
			string _connection;
            string _nombre;

          

		#endregion

		#region Properties

		[DataMember]
		public double  cod_tipo_agente_ppal
		{
			 get { return _cod_tipo_agente_ppal; }
			 set {_cod_tipo_agente_ppal = value;}
		}

		[DataMember]
		public double  cod_agente_ppal
		{
			 get { return _cod_agente_ppal; }
			 set {_cod_agente_ppal = value;}
		}

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
        public string nombre
        {
            get { return _nombre; }
            set { _nombre = value; }
        }
		#endregion

	}
}
