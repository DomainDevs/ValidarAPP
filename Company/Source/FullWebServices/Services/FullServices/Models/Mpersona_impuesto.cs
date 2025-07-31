using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.Serialization;

namespace Sistran.Co.Previsora.Application.FullServices.Models
{
	[DataContract]
	public class Mpersona_impuesto
	{

		#region InnerClass
		public enum Mpersona_impuestoFields
		{
			id_persona,
			cod_abona,
			cod_impuesto,
			cod_condicion,
            cod_grupo //Sergio Bautista, 02/11/2016, OT0087, Se agrega el campo cod_grupo
		}
		#endregion

		#region Data Members

			int _id_persona;
			double _cod_abona;
			double _cod_impuesto;
			double _cod_condicion;
            double _cod_grupo; //Sergio Bautista, 02/11/2016, OT0087, Se agrega el campo cod_grupo
			int _identity; 
			char _state; 
			string _connection; 

		#endregion

		#region Properties

		[DataMember]
		public int  id_persona
		{
			 get { return _id_persona; }
			 set {_id_persona = value;}
		}

		[DataMember]
		public double  cod_abona
		{
			 get { return _cod_abona; }
			 set {_cod_abona = value;}
		}

		[DataMember]
		public double  cod_impuesto
		{
			 get { return _cod_impuesto; }
			 set {_cod_impuesto = value;}
		}

		[DataMember]
		public double  cod_condicion
		{
			 get { return _cod_condicion; }
			 set {_cod_condicion = value;}
		}

        [DataMember]
        public double cod_grupo
        {
            get { return _cod_grupo; }
            set { _cod_grupo = value; }
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
