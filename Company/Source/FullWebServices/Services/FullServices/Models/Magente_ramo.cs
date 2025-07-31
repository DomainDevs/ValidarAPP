using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.Serialization;

namespace Sistran.Co.Previsora.Application.FullServices.Models
{
	[DataContract]
	public class Magente_ramo
	{

		#region InnerClass
		public enum Magente_ramoFields
		{
			cod_tipo_agente,
			cod_agente,
			cod_ramo
		}
		#endregion

		#region Data Members

			double _cod_tipo_agente;
			int _cod_agente;
			double _cod_ramo;
			int _identity; 
			char _state; 
			string _connection;
            int _individual_id;
            int _prefix_cd;

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
		public double  cod_ramo
		{
			 get { return _cod_ramo; }
			 set {_cod_ramo = value;}
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
        public int INDIVIDUAL_ID
        {
            get { return _individual_id; }
            set { _individual_id = value; }
        }

        [DataMember]
        public int PREFIX_CD
        {
            get { return _prefix_cd; }
            set { _prefix_cd = value; }
        }
		#endregion

	}
}
