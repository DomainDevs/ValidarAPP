using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.Serialization;

namespace Sistran.Co.Previsora.Application.FullServices.Models
{
	[DataContract]
	public class LOGBOOK
	{

		#region InnerClass
		public enum LOGBOOKFields
		{
			ID_LOGBOOK,
			ID_PERSONA,			
			ID_MECHANISM,
			REFERENCE,
			LIST_PURPOSES,
			DATE_LOGBOOK
		}
		#endregion

		#region Data Members

			decimal _id_logbook;
			int _id_persona;			
			int _id_mechanism;
			string _reference;
			string _list_purposes;
			string _date_loogbook;
			int _identity; 
			char _state; 
			string _connection;
            string _nameMechanism;            

		#endregion

		#region Properties

		[DataMember]
		public decimal  ID_LOGBOOK
		{
			 get { return _id_logbook; }
			 set {_id_logbook = value;}
		}

		[DataMember]
		public int  ID_PERSONA
		{
			 get { return _id_persona; }
			 set {_id_persona = value;}
		}

		[DataMember]
		public int  ID_MECHANISM
		{
			 get { return _id_mechanism; }
			 set {_id_mechanism = value;}
		}

		[DataMember]
		public string  REFERENCE
		{
			 get { return _reference; }
			 set {_reference = value;}
		}

		[DataMember]
		public string  LIST_PURPOSES
		{
			 get { return _list_purposes; }
			 set {_list_purposes = value;}
		}

		[DataMember]
        public string DATE_LOGBOOK
		{
			 get { return _date_loogbook; }
			 set {_date_loogbook = value;}
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
        public string NAME_MECHANISM
        {
            get { return _nameMechanism; }
            set { _nameMechanism = value; }
        }

		#endregion

	}
}
