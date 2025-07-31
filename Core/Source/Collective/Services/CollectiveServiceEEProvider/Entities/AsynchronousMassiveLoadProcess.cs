
using System;
using System.Collections;
using System.Collections.Generic;
using Sistran.Core.Framework;
using Sistran.Core.Framework.DAF;

namespace Sistran.Core.Application.CollectiveServices.EEProvider.Entities
{
    /// <summary>
    /// Definición de entidad AsynchronousMassiveLoadProcess.
    /// </summary>
    [
    PersistentClass("Sistran.Core.Application.CollectiveServices.EEProvider.AsynchronousMassiveLoadProcess.dict"),
    Serializable(),
    DescriptionKey("ASYNCHRONOUS_MASSIVE_LOAD_PROCESS_ENTITY"),
    TableMap(TableName="ASYNCHRONOUS_MASSIVE_LOAD_PROCESS", Schema="QUO"),
    ]
    public partial class AsynchronousMassiveLoadProcess :
        BusinessObject2
    {
#region static
		/// <summary>
		/// Propiedades públicas de la entidad.
		/// </summary>
		public static class Properties
		{
			public static readonly string ProcessId = "ProcessId";
			public static readonly string MassiveLoadId = "MassiveLoadId";
		}

		/// <summary>
		/// Crea una clave primaria para una clase concreta.
		/// </summary>
		/// <param name="concreteClass">Clase concreta.</param>
		/// <param name="processId">Propiedad clave ProcessId.</param>
		/// <returns>Clave primaria.</returns>
	    protected static PrimaryKey InternalCreatePrimaryKey<T>(int processId)
	    {
		    Dictionary<string, object> keys = new Dictionary<string, object>();
		    keys.Add(Properties.ProcessId, processId);

		    return new PrimaryKey<T>(keys);
	    }

		/// <summary>
		/// Crea una clave primaria para esta clase.
		/// </summary>
		/// <param name="processId">Propiedad clave ProcessId.</param>
		public static PrimaryKey CreatePrimaryKey(int processId)
	    {
			return InternalCreatePrimaryKey<AsynchronousMassiveLoadProcess>(processId);
		}
#endregion

	    //*** Object Attributes ********************************

		/// <summary>
		/// Atributo para la propiedad MassiveLoadId.
		/// </summary>
        private int _massiveLoadId;
	    //*** Object Constructors ********************************

		/// <summary>
		/// Constructor de instancia de la clase en base a las propiedades claves.
		/// </summary>
		/// <param name="processId">ProcessId key property.</param>
	    public AsynchronousMassiveLoadProcess(int processId):
			this(AsynchronousMassiveLoadProcess.CreatePrimaryKey(processId), null)
	    {
	    }

		/// <summary>
		/// Constructor de instancia de la clase en base a una clave primaria y a valores iniciales.
		/// </summary>
		/// <param name="key">
		/// Identificador de la instancia de la entidad.
		/// </param>
		/// <param name="initialValues">
		/// Valores para establecer el estado de la instancia.
		/// </param>
	    public AsynchronousMassiveLoadProcess(PrimaryKey key, IDictionary initialValues):
			base(key, initialValues)
	    {
	    }

	    /*** Object Properties ********************************/
		/// <summary>
		/// Devuelve o setea el valor de la propiedad ProcessId.
		/// </summary>
		/// <value>Propiedad ProcessId.</value>
	    [
		DescriptionKey("PROCESS_ID_PROPERTY"),
		PersistentProperty(IsKey = true),
        ColumnMap(ColumnName="PROCESS_ID", DbType=System.Data.DbType.String),
	    ]
        public int ProcessId
	    {
		    get
		    {
				return (int)this._primaryKey[Properties.ProcessId];
		    }
		    set
		    {
				this._primaryKey[Properties.ProcessId] = value;
			}
	    }

		/// <summary>
		/// Devuelve o setea el valor de la propiedad MassiveLoadId.
		/// </summary>
		/// <value>Propiedad MassiveLoadId.</value>
	    [
		DescriptionKey("MASSIVE_LOAD_ID_PROPERTY"),
		PersistentProperty(),
        ColumnMap(ColumnName="MASSIVE_LOAD_ID", DbType=System.Data.DbType.String),
	    ]
        public int MassiveLoadId
	    {
		    get
		    {
			    return this._massiveLoadId;
		    }
		    set
		    {
				this._massiveLoadId = value;
			}
	    }

    }
}