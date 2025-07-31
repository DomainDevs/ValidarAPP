using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.Serialization;

namespace Sistran.Co.Previsora.Application.FullServices.Models
{
	[DataContract]
	public class CO_CONSORTIUM
	{

		#region InnerClass
		public enum CO_CONSORTIUMFields
		{
			INSURED_CD,
			INDIVIDUAL_ID,
			CONSORTIUM_ID,
			IS_MAIN,
			PARTICIPATION_RATE,
			START_DATE,
			ENABLED
		}
		#endregion

		#region Data Members

			int _iNSURED_CD;
			int _iNDIVIDUAL_ID;
			int _cONSORTIUM_ID;
			bool _iS_MAIN;
			double _pARTICIPATION_RATE;
			string _sTART_DATE;
			bool _eNABLED;
			int _identity; 
			char _state; 
			string _connection; 

		#endregion

		#region Properties

		[DataMember]
		public int  INSURED_CD
		{
			 get { return _iNSURED_CD; }
			 set {_iNSURED_CD = value;}
		}

		[DataMember]
		public int  INDIVIDUAL_ID
		{
			 get { return _iNDIVIDUAL_ID; }
			 set {_iNDIVIDUAL_ID = value;}
		}

		[DataMember]
		public int  CONSORTIUM_ID
		{
			 get { return _cONSORTIUM_ID; }
			 set {_cONSORTIUM_ID = value;}
		}

		[DataMember]
		public bool  IS_MAIN
		{
			 get { return _iS_MAIN; }
			 set {_iS_MAIN = value;}
		}

		[DataMember]
		public double  PARTICIPATION_RATE
		{
			 get { return _pARTICIPATION_RATE; }
			 set {_pARTICIPATION_RATE = value;}
		}

		[DataMember]
		public string  START_DATE
		{
			 get { return _sTART_DATE; }
			 set {_sTART_DATE = value;}
		}

		[DataMember]
		public bool  ENABLED
		{
			 get { return _eNABLED; }
			 set {_eNABLED = value;}
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
