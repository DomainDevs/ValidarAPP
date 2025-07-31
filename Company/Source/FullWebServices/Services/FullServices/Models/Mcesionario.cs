using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.Serialization;

namespace Sistran.Co.Previsora.Application.FullServices.Models
{
	[DataContract]
	public class Mcesionario
	{

		#region InnerClass
		public enum McesionarioFields
		{
			cod_cesionario,
			id_persona,
			fec_alta,
			fec_baja,
			edad,
			cod_baja,
			fec_inactivacion,
			sn_inactivacion,
			cod_usuario,
			fec_registro
		}
		#endregion

		#region Data Members

			int _cod_cesionario;
			int _id_persona;
			string _fec_alta;
			string _fec_baja;
			string _edad;
			string _cod_baja;
			string _fec_inactivacion;
			string _sn_inactivacion;
			string _cod_usuario;
			string _fec_registro;
			int _identity; 
			char _state; 
			string _connection;
            string _txt_desc;
            string _nro_doc;
            string _txt_apellido1;
            string _txt_apellido2;
            string _txt_nombre;
            string _tipo_persona;
            string _name_role;

		#endregion

		#region Properties
            [DataMember]
            public string txt_desc
            {
                get { return _txt_desc; }
                set { _txt_desc = value; }
            }

            [DataMember]
            public string nro_doc
            {
                get { return _nro_doc; }
                set { _nro_doc = value; }
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
            public string tipo_persona
            {
                get { return _tipo_persona; }
                set { _tipo_persona = value; }
            }

            [DataMember]
            public string name_role
            {
                get { return _name_role; }
                set { _name_role = value; }
            }        


		[DataMember]
		public int  cod_cesionario
		{
			 get { return _cod_cesionario; }
			 set {_cod_cesionario = value;}
		}

		[DataMember]
		public int  id_persona
		{
			 get { return _id_persona; }
			 set {_id_persona = value;}
		}

		[DataMember]
		public string  fec_alta
		{
			 get { return _fec_alta; }
			 set {_fec_alta = value;}
		}

		[DataMember]
		public string  fec_baja
		{
			 get { return _fec_baja; }
			 set {_fec_baja = value;}
		}

		[DataMember]
		public string  edad
		{
			 get { return _edad; }
			 set {_edad = value;}
		}

		[DataMember]
		public string  cod_baja
		{
			 get { return _cod_baja; }
			 set {_cod_baja = value;}
		}

		[DataMember]
		public string  fec_inactivacion
		{
			 get { return _fec_inactivacion; }
			 set {_fec_inactivacion = value;}
		}

		[DataMember]
		public string  sn_inactivacion
		{
			 get { return _sn_inactivacion; }
			 set {_sn_inactivacion = value;}
		}

		[DataMember]
		public string  cod_usuario
		{
			 get { return _cod_usuario; }
			 set {_cod_usuario = value;}
		}

		[DataMember]
		public string  fec_registro
		{
			 get { return _fec_registro; }
			 set {_fec_registro = value;}
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
