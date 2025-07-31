using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.Serialization;

namespace Sistran.Co.Previsora.Application.FullServices.Models
{
    [DataContract]
    public class Mpersona_telef
    {

        #region InnerClass
        public enum Mpersona_telefFields
        {
            id_persona,
            cod_tipo_telef,
            cod_tipo_telef_old, //Autor: Edward Rubiano; Fecha: 18/04/2016; Asunto: C11226; Descripcion: Tipo de telefono antiguo para modificacion de este en 2g
            txt_telefono, //SUPDB
            cod_pais, //SUPDB
            cod_dpto, //SUPDB
            cod_municipio, //SUPDB
            sn_domicilio //SUPDB
        }
        #endregion

        #region Data Members

        int _id_persona;
        double _cod_tipo_telef;
        double _cod_tipo_telef_old; //Autor: Edward Rubiano; Fecha: 18/04/2016; Asunto: C11226; Descripcion: Tipo de telefono antiguo para modificacion de este en 2g
        string _txt_telefono;
        string _cod_pais; //SUPDB
        string _cod_dpto; //SUPDB
        string _cod_municipio; //SUPDB
        int _sn_domicilio; //SUPDB
        int _identity;
        char _state;
        string _connection;

        #endregion

        #region Properties

        [DataMember]
        public int id_persona
        {
            get { return _id_persona; }
            set { _id_persona = value; }
        }

        [DataMember]
        public double cod_tipo_telef
        {
            get { return _cod_tipo_telef; }
            set { _cod_tipo_telef = value; }
        }

        //Autor: Edward Rubiano; Fecha: 18/04/2016; Asunto: C11226; Descripcion: Tipo de telefono antiguo para modificacion de este en 2g
        [DataMember]
        public double cod_tipo_telef_old
        {
            get { return _cod_tipo_telef_old; }
            set { _cod_tipo_telef_old = value; }
        }
        //Autor: Edward Rubiano; Fecha: 18/04/2016; Asunto: C11226; Descripcion: Tipo de telefono antiguo para modificacion de este en 2g

        [DataMember]
        public string txt_telefono
        {
            get { return _txt_telefono; }
            set { _txt_telefono = value; }
        }

		//SUPDB - INICIO
        [DataMember]
        public string cod_pais
        {
            get { return _cod_pais; }
            set { _cod_pais = value; }
        }

        [DataMember]
        public string cod_dpto
        {
            get { return _cod_dpto; }
            set { _cod_dpto = value; }
        }

        [DataMember]
        public string cod_municipio
        {
            get { return _cod_municipio; }
            set { _cod_municipio = value; }
        }

        [DataMember]
        public int sn_domicilio
        {
            get { return _sn_domicilio; }
            set { _sn_domicilio = value; }
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

        [DataMember]
        public string EXTENSION
        {
            get;
            set;
        }

        [DataMember]
        public string SCHEDULE_AVAILABILITY
        {
            get;
            set;
        }

        [DataMember]
        public int DATA_ID
        {
            get;
            set;
        }

        [DataMember]
        public char State_3g
        {
            get;
            set;
        }

		//SUPDB - INICIO
        [DataMember]
        public bool IS_HOME
        {
            get;
            set;
        }
		//SUPDB - FIN

        #endregion

    }
}
