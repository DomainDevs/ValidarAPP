using System;
using System.Collections;
using Sistran.Core.Framework;
using Sistran.Core.Framework.DAF;
using System.Collections.Specialized;

namespace Sistran.Core.Application.RulesScriptsServices.EEProvider.Entities
{
    /// <summary>
    /// Definición de entidad RuleBase.
    /// </summary>
    [
    PersistentClass("Sistran.Core.Application.RulesScriptsServices.EEProvider.RuleFunction.dict"),
    Serializable,
    DescriptionKey("RULE_FUNCTION_ENTITY")
    ]
    public class RuleFunction:BusinessObject
    {
        #region static
		/// <summary>
		/// Propiedades públicas de la entidad.
		/// </summary>
		public sealed class Properties
		{
            public static readonly string RuleFunctionId = "RuleFunctionId";
            public static readonly string PackageId = "PackageId";
            public static readonly string LevelId = "LevelId";
			public static readonly string FunctionName = "FunctionName";
			public static readonly string Description = "Description";


			private Properties()
			{
			}
		}

		/// <summary>
		/// Crea una clave primaria para una clase concreta.
		/// </summary>
		/// <param name="concreteClass">Clase concreta.</param>
		/// <returns>Clave primaria.</returns>
		protected static PrimaryKey InternalCreatePrimaryKey(Type concreteClass)
		{
			ListDictionary keys = new ListDictionary();
			keys.Add(Properties.RuleFunctionId, null);

			return new PrimaryKey(concreteClass, keys);
		}

		/// <summary>
		/// Crea una clave primaria para una clase concreta.
		/// </summary>
		/// <param name="concreteClass">Clase concreta.</param>
		/// <param name="ruleFunctionId">Propiedad clave RuleFunctionId.</param>
		/// <returns>Clave primaria.</returns>
	    protected static PrimaryKey InternalCreatePrimaryKey(Type concreteClass, int ruleFunctionId)
	    {
		    ListDictionary keys = new ListDictionary();
		    keys.Add(Properties.RuleFunctionId, ruleFunctionId);

		    return new PrimaryKey(concreteClass, keys);
	    }

		/// <summary>
		/// Crea una clave primaria para esta clase.
		/// </summary>
		public static PrimaryKey CreatePrimaryKey()
		{
			return InternalCreatePrimaryKey(typeof(RuleFunction));
		}

		/// <summary>
		/// Crea una clave primaria para esta clase.
		/// </summary>
		/// <param name="ruleFunctionId">Propiedad clave rRuleFunctionId.</param>
		public static PrimaryKey CreatePrimaryKey(int ruleFunctionId)
	    {
			return InternalCreatePrimaryKey(typeof(RuleFunction), ruleFunctionId);
		}
#endregion

	    //*** Object Attributes ********************************
		
        /// <summary>
		/// Atributo para la propiedad PackageId.
		/// </summary>
        private int _packageId;
		/// <summary>
		/// Atributo para la propiedad LevelId.
		/// </summary>
        private int _levelId;
        /// <summary>
		/// Atributo para la propiedad Description.
		/// </summary>
        private string _functionName ;
		/// <summary>
		/// Atributo para la propiedad Description.
		/// </summary>
        private string _description ;

		//*** Object Constructors ********************************

		/// <summary>
		/// Constructor de instancia de la clase en base a las propiedades claves.
		/// </summary>
		public RuleFunction(): this(CreatePrimaryKey(), null)
		{}

		/// <summary>
		/// Constructor de instancia de la clase en base a las propiedades claves.
		/// </summary>
		/// <param name="ruleFunctionId">ruleFunctionId key property.</param>
	    public RuleFunction(int ruleFunctionId):
			this(CreatePrimaryKey(ruleFunctionId), null)
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
        public RuleFunction(PrimaryKey key, IDictionary initialValues) :
			base(key, initialValues)
	    {
	    }


        /// <summary>
        /// Devuelve o setea el valor de la propiedad RuleFunctionId.
        /// </summary>
        /// <value>Propiedad RuleFunctionId.</value>
        [
            DescriptionKey("RULE_FUNCTION_ID_PROPERTY"),
            PersistentProperty(IsKey = true)
        ]
        public int RuleFunctionId
        {
            get
            {
                return (int)_primaryKey[Properties.RuleFunctionId];
            }
            set
            {
                _primaryKey[Properties.RuleFunctionId] = value;
            }
        }

        /// <summary>
        /// Devuelve o setea el valor de la propiedad PackageId.
        /// </summary>
        /// <value>Propiedad PackageId.</value>
        [
            DescriptionKey("PACKAGE_ID_PROPERTY"),
            PersistentProperty
        ]
        public int PackageId
        {
            get
            {
                return _packageId;
            }
            set
            {
                _packageId = value;
            }
        }
	    /*** Object Properties ********************************/
		/// <summary>
		/// Devuelve o setea el valor de la propiedad LevelId.
		/// </summary>
		/// <value>Propiedad LevelId.</value>
	    [
			DescriptionKey("LEVEL_ID_PROPERTY"),
			PersistentProperty
	    ]
        public int LevelId
	    {
		    get
		    {
			    return _levelId;
		    }
		    set
		    {
				_levelId = value;
			}
	    }


        /// <summary>
        /// Devuelve o setea el valor de la propiedad Description.
        /// </summary>
        /// <value>Propiedad Description.</value>
        [
            DescriptionKey("FUNCTION_NAME_PROPERTY"),
            PersistentProperty
        ]
        public string FunctionName
        {
            get
            {
                return _functionName;
            }
            set
            {
                if (value == null)
                {
                    throw new PropertyNotNullableException(GetType().FullName, Properties.FunctionName);
                }
                _functionName = value;
            }
        }

		/// <summary>
		/// Devuelve o setea el valor de la propiedad Description.
		/// </summary>
		/// <value>Propiedad Description.</value>
	    [
			DescriptionKey("DESCRIPTION_PROPERTY"),
			PersistentProperty
	    ]
        public string Description
	    {
		    get
		    {
			    return _description;
		    }
		    set
		    {
				if(value == null)
				{
					throw new PropertyNotNullableException(GetType().FullName, Properties.Description);
				}
				_description = value;
			}
	    }


		/// <summary>
		/// Establecer los valores de las propiedades que no forman parte de la clave o identificador de la instancia.
		/// </summary>
		/// <param name="values">
		/// Valores para establecer el estado de la instancia.
		/// </param>
		public override void SetPropertyValues(IDictionary values)
		{
			// Set base properties
			base.SetPropertyValues(values);

			object value;

			value = values[Properties.PackageId];
			if(value == null)
			{
				throw new PropertyNotNullableException(GetType().FullName, Properties.PackageId);
			}
			PackageId = (int)value;		
            
            
            value = values[Properties.LevelId];
			if(value == null)
			{
				throw new PropertyNotNullableException(GetType().FullName, Properties.LevelId);
			}
			LevelId = (int)value;


            value = values[Properties.FunctionName];
            if (value == null)
            {
                throw new PropertyNotNullableException(GetType().FullName, Properties.FunctionName);
            }
            FunctionName = (string)values[Properties.FunctionName];

            value = values[Properties.Description];
            if (value == null)
            {
                throw new PropertyNotNullableException(GetType().FullName, Properties.Description);
            } 
            Description = (string)values[Properties.Description];

		}

		/// <summary>
		/// Obtiene el estado a partir de los valores de la instancia de la entidad.
		/// </summary>
		/// <returns>
		/// Diccionario con los valores de las propiedades.
		/// </returns>
		public override IDictionary GetPropertyValues()
		{
			// Get base properties
			IDictionary values = base.GetPropertyValues();

			values.Add(Properties.PackageId, PackageId);
			values.Add(Properties.LevelId, LevelId);
            values.Add(Properties.FunctionName, FunctionName);
			values.Add(Properties.Description, Description);


			return values;
		}
		
		/// <summary>
		/// Validación de propiedades obligatorias para la persistencia de 
		/// instancias de de la entidad. Se valida que las propiedades 
		/// obligatorias no estén sin valor asignado (null).
		/// </summary>
		/// <exception cref = "Sistran.Core.Framework.PropertyNotNullableException">
		/// En caso que alguna propiedad obligatoria no tenga valor asignado o tenga el valor null.
		/// </exception>
		public override void Validate()
		{
			StringCollection propertyNames = new StringCollection();
			if(_description == null)
			{
				propertyNames.Add(Properties.Description);
			}
			if(propertyNames.Count > 0)
			{
				throw new PropertyNotNullableException(GetType().FullName, propertyNames);
			}
		}
    }
}
