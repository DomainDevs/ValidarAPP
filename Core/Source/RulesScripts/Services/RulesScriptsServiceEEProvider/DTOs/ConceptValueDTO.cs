using System;
using Sistran.Core.Framework.DAF;

namespace Sistran.Core.Application.RulesScriptsServices.EEProvider.DTOs
{
    /// <summary>
    /// Representa un Concepto
    /// </summary>
    [Serializable()]
    public class ConceptValueDTO
    {
        #region Internal Values
        private PrimaryKey _primaryKey;
        private int _entityCode;
        private int _conceptCode;
        private Object _value;
        #endregion

        /// <summary>
        /// Inicializa una nueva instancia de la clase 
        /// definiendole la entidad y concepto.
        /// </summary>
        /// <param name="entityId">Identificación de la entidad</param>
        /// <param name="conceptId">Identificacion del concepto</param>
        public ConceptValueDTO(int entityId, int conceptId)
        {
            this.ConceptCode = conceptId;
            this.EntityCode = entityId;
        }
        /// <summary>
        /// Inicializa una nueva instancia de la clase 
        /// definiendole la entidad, concepto y la instancia del objeto.
        /// </summary>
        /// <param name="key">Identifiación de instancia del objeto</param>
        /// <param name="entityId">Identificación de la entidad</param>
        /// <param name="conceptId">Identificacion del concepto</param>
        public ConceptValueDTO(PrimaryKey key, int entityId, int conceptId)
        {
            this.ConceptCode = conceptId;
            this.EntityCode = entityId;
            this.PrimaryKey = key;
        }

        /// <summary>
        /// Devuelve o establece un key de intancia del objeto.
        /// </summary>
        public PrimaryKey PrimaryKey
        {
            get { return _primaryKey; }
            set { _primaryKey = value; }
        }
        /// <summary>
        /// Devuelve o establece el codigo de la entidad 
        /// de la que depende el concepto.
        /// </summary>
        public int EntityCode
        {
            get { return _entityCode; }
            set { _entityCode = value; }
        }

        /// <summary>
        /// Devuelve o establece el codigo de identificacion del concepto.
        /// </summary>
        public int ConceptCode
        {
            get { return _conceptCode; }
            set { _conceptCode = value; }
        }

        /// <summary>
        /// Devuelve o establece el valor.
        /// </summary>
        public Object Value
        {
            get { return _value; }
            set { _value = value; }
        }
    }
}
