using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.Serialization;

namespace Sistran.Co.Previsora.Application.FullServices.Models
{
    [DataContract]
    public class CesionarioDe
    {
        #region InnerClass
        public enum CesionarioDeFields
        {
            id_persona,
            cod_abona,
            sn_activo,
            txt_desc,
            txt_desc_redu,
            num_doc,
            txt_apellido1,
            txt_apellido2,
            txt_nombre,
            cod_tipo_persona
        }
        #endregion

        #region Data Members

        int _id_persona;
        int _cod_abona;
        string _sn_activo;
        string _txt_desc;
        string _txt_desc_redu;
        string _num_doc;
        string _txt_apellido1;
        string _txt_apellido2;
        string _txt_nombre;
        string _cod_tipo_persona;
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
        public int cod_abona
        {
            get { return _cod_abona; }
            set { _cod_abona = value; }
        }

        [DataMember]
        public string sn_activo
        {
            get { return _sn_activo; }
            set { _sn_activo = value; }
        }

        [DataMember]
        public string txt_desc
        {
            get { return _txt_desc; }
            set { _txt_desc = value; }
        }

        [DataMember]
        public string txt_desc_redu
        {
            get { return _txt_desc_redu; }
            set { _txt_desc_redu = value; }
        }

        [DataMember]
        public string num_doc
        {
            get { return _num_doc; }
            set { _num_doc = value; }
        }

        [DataMember]
        public string txt_apellido1
        {
            get { return _txt_apellido1; }
            set { _txt_apellido1 = value; }
        }

        [DataMember]
        public string txt_apellido2
        {
            get { return _txt_apellido2; }
            set { _txt_apellido2 = value; }
        }

        [DataMember]
        public string txt_nombre
        {
            get { return _txt_nombre; }
            set { _txt_nombre = value; }
        }

        [DataMember]
        public string cod_tipo_persona
        {
            get { return _cod_tipo_persona; }
            set { _cod_tipo_persona = value; }
        }


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
