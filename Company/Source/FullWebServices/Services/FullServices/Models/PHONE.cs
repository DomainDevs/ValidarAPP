using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.Serialization;

namespace Sistran.Co.Previsora.Application.FullServices.Models
{
    [DataContract]
    public class PHONE
    {

        #region InnerClass
        public enum PHONEFields
        {
            INDIVIDUAL_ID,
            DATA_ID,
            PHONE_TYPE_CD,
            PHONE_NUMBER,
            EXTENSION,
            COUNTRY_CODE,
            CITY_CODE,
            SCHEDULE_AVAILABILITY, //SUPDB
            COUNTRY_CD, //SUPDB
            STATE_CD, //SUPDB
            CITY_CD, //SUPDB
            IS_HOME //SUPDB
        }
        #endregion

        #region Data Members

        int _individual_id;
        int _data_id;
        int _phone_type_cd;
        long _phone_number;
        string _extension;
        string _country_code;
        string _city_code;
        string _schedule_availability;
        int _country_cd; //SUPDB
        int _state_cd; //SUPDB
        int _city_cd; //SUPDB
        bool _is_home; //SUPDB
        int _identity;
        char _state;
        string _connection;

        #endregion

        #region Properties

        [DataMember]
        public int INDIVIDUAL_ID
        {
            get { return _individual_id; }
            set { _individual_id = value; }
        }

        [DataMember]
        public int DATA_ID
        {
            get { return _data_id; }
            set { _data_id = value; }
        }

        [DataMember]
        public int PHONE_TYPE_CD
        {
            get { return _phone_type_cd; }
            set { _phone_type_cd = value; }
        }

        [DataMember]
        public long PHONE_NUMBER
        {
            get { return _phone_number; }
            set { _phone_number = value; }
        }

        [DataMember]
        public string EXTENSION
        {
            get { return _extension; }
            set { _extension = value; }
        }

        [DataMember]
        public string COUNTRY_CODE
        {
            get { return _country_code; }
            set { _country_code = value; }
        }

        [DataMember]
        public string CITY_CODE
        {
            get { return _city_code; }
            set { _city_code = value; }
        }

        [DataMember]
        public string SCHEDULE_AVAILABILITY
        {
            get { return _schedule_availability; }
            set { _schedule_availability = value; }
        }

		//SUPDB - INICIO
        [DataMember]
        public int COUNTRY_CD
        {
            get { return _country_cd; }
            set { _country_cd = value; }
        }

        [DataMember]
        public int STATE_CD
        {
            get { return _state_cd; }
            set { _state_cd = value; }
        }

        [DataMember]
        public int CITY_CD
        {
            get { return _city_cd; }
            set { _city_cd = value; }
        }

        [DataMember]
        public bool IS_HOME
        {
            get { return _is_home; }
            set { _is_home = value; }
        }
		//SUPDB - FIN


        [DataMember]
        public int Identity
        {
            get { return _identity; }
            set { _identity = value; }
        }

        [DataMember]
        public char State
        {
            get { return _state; }
            set { _state = value; }
        }

        [DataMember]
        public string Connection
        {
            get { return _connection; }
            set { _connection = value; }
        }

        #endregion

    }
}
