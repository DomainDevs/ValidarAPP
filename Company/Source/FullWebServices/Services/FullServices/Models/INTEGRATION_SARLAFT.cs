using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.Serialization;

namespace Sistran.Co.Previsora.Application.FullServices.Models
{
	[DataContract]
	public class INTEGRATION_SARLAFT
	{

		#region InnerClass
		public enum INTEGRATION_SARLAFTFields
		{
			ID_PERSON,
			ID_FORM,
			NUM_FORM,
			DATE_CREATE,
			BRANCH
		}
		#endregion

		#region Data Members

			int _id_person;
			int _id_form;
			int _num_form;
			string _date_create;
			string _branch;
			int _identity; 
			char _state; 
			string _connection; 

		#endregion

		#region Properties

		[DataMember]
		public int  ID_PERSON
		{
			 get { return _id_person; }
			 set {_id_person = value;}
		}

		[DataMember]
		public int  ID_FORM
		{
			 get { return _id_form; }
			 set {_id_form = value;}
		}

		[DataMember]
		public int  NUM_FORM
		{
			 get { return _num_form; }
			 set {_num_form = value;}
		}

		[DataMember]
		public string  DATE_CREATE
		{
			 get { return _date_create; }
			 set {_date_create = value;}
		}

		[DataMember]
		public string  BRANCH
		{
			 get { return _branch; }
			 set {_branch = value;}
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
