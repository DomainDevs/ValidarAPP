using Sistran.Core.Framework;
using Sistran.Core.Framework.DAF;
using System;
using System.Collections;
using System.Collections.Specialized;

namespace Sistran.Core.Application.UniquePersonV1.Entities
{
    [PersistentClass("Sistran.Core.Application.UniquePersonV1.PhoneType.dict"),
    Serializable()]
    public class PhoneType : BusinessObject
    {
        #region static

        public static class Properties
        {
            public static readonly string PhoneTypeCode = "PhoneTypeCode";
            public static readonly string Description = "Description";
            public static readonly string SmallDescription = "SmallDescription";
        }

        /// <summary>
        /// Create primary key for concrete class.
        /// </summary>
        /// <param name="concreteClass">Concrete class.</param>
        /// <param name="phoneTypeCode">PhoneTypeCode key property.</param>
        /// <returns>Primary key.</returns>
        protected static PrimaryKey InternalCreatePrimaryKey(Type concreteClass, int phoneTypeCode)
        {
            ListDictionary keys = new ListDictionary();
            keys.Add("PhoneTypeCode", phoneTypeCode);

            return new PrimaryKey(concreteClass, keys);
        }

        /// <summary>
        /// Create primary key for this class.
        /// </summary>
        /// <param name="phoneTypeCode">PhoneTypeCode key property.</param>
        public static PrimaryKey CreatePrimaryKey(int phoneTypeCode)
        {
            return InternalCreatePrimaryKey(typeof(PhoneType), phoneTypeCode);
        }
        #endregion

        //*** Object Attributes ********************************

        /// <summary>
        /// PhoneTypeCode property attribute.
        /// </summary>
        private int _phoneTypeCode;
        /// <summary>
        /// SmallDescription property attribute.
        /// </summary>
        private string _smallDescription = null;
        /// <summary>
        /// Description property attribute.
        /// </summary>
        private string _description = null;
        //*** Object Constructors ********************************

        /// <summary>
        /// Construct an instance of the class with the specified 
        /// primary key and object engine.
        /// </summary>
        /// <param name="key">Primary key.</param>
        public PhoneType(PrimaryKey key, IDictionary initialValues) :
            base(key, initialValues)
        { }

        /// <summary>
        /// Constructor basado en el Identificador de la entidad.
        /// </summary>
        /// <param name="phoneTypeCode">
        /// Identificador utilizado para generar la Primary Key
        /// </param>
        public PhoneType(int phoneTypeCode) :
            base(PhoneType.CreatePrimaryKey(phoneTypeCode),
            null)
        { }



        /// <summary>
        /// Return or set PhoneTypeCode property.
        /// </summary>
        /// <value>PhoneTypeCode property.</value>
        [PersistentProperty(IsKey = true)]
        public int PhoneTypeCode
        {
            get
            {
                return this._phoneTypeCode;
            }
            set
            {
                this._phoneTypeCode = value;
            }
        }

        /// <summary>
        /// Return or set SmallDescription property.
        /// </summary>
        /// <value>SmallDescription property.</value>
        [PersistentProperty()]
        public string SmallDescription
        {
            get
            {
                if (this._smallDescription == null)
                {
                    throw new InvalidPropertyValueException(
                        this.GetType().FullName,
                        "SmallDescription",
                        "<NULL>");
                }
                return this._smallDescription;
            }
            set
            {
                if (value == null)
                {
                    throw new PropertyNotNullableException(
                        this.GetType().FullName,
                        "SmallDescription");
                }
                this._smallDescription = value;
            }
        }

        /// <summary>
        /// Return or set Description property.
        /// </summary>
        /// <value>Description property.</value>
        [PersistentProperty()]
        public string Description
        {
            get
            {
                if (this._description == null)
                {
                    throw new InvalidPropertyValueException(
                        this.GetType().FullName,
                        "Description",
                        "<NULL>");
                }
                return this._description;
            }
            set
            {
                if (value == null)
                {
                    throw new PropertyNotNullableException(
                        this.GetType().FullName,
                        "Description");
                }
                this._description = value;
            }
        }

        /// <summary>
        /// Set all property values.
        /// </summary>
        protected override void SetKeyPropertyValues(IDictionary keys)
        {
            this._phoneTypeCode = (int)keys["PhoneTypeCode"];
        }

        /// <summary>
        /// Set all property values.
        /// </summary>
        public override void SetPropertyValues(IDictionary values)
        {
            this.SmallDescription = (string)values["SmallDescription"];
            this.Description = (string)values["Description"];
        }

        /// <summary>
        /// Get all property values.
        /// </summary>
        public override IDictionary GetPropertyValues()
        {
            ListDictionary values = new ListDictionary();

            values.Add("SmallDescription", this.SmallDescription);
            values.Add("Description", this.Description);

            return values;
        }

        public override void Validate()
        {
            StringCollection propertyNames = new StringCollection();
            if (this._smallDescription == null)
            {
                propertyNames.Add("SmallDescription");
            }
            if (this._description == null)
            {
                propertyNames.Add("Description");
            }
            if (propertyNames.Count > 0)
            {
                throw new PropertyNotNullableException(
                    this.GetType().FullName,
                    propertyNames);
            }
        }
    }
}