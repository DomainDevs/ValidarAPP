namespace Sistran.Core.Application.RulesScriptsServices.Models
{
    /// <summary>
    /// clase para el mapeo del excel y del XML
    /// </summary>
    public partial class DecisionTableMappingExcelPageColumn
    {
        private int _entityId, _conceptId, _commandCode;
        private bool _isDecimal;

        /// <summary>
        ///  propiedad CommandCode
        /// </summary>
        public int CommandCode
        {
            get { return _commandCode; }
            set { _commandCode = value; }
        }

        /// <summary>
        ///  propiedad ConceptId
        /// </summary>
        public int ConceptId
        {
            get { return _conceptId; }
            set { _conceptId = value; }
        }

        /// <summary>
        ///  propiedad EntityId
        /// </summary>
        public int EntityId
        {
            get { return _entityId; }
            set { _entityId = value; }
        }

        /// <summary>
        ///  propiedad entity_concept
        /// </summary>
        public string entity_concept
        {
            get { return this.entityField + "_" + this.conceptField; }
        }

        /// <summary>
        ///  propiedad IsDecimal
        /// </summary>
        public bool IsDecimal
        {
            get { return _isDecimal; }
            set { _isDecimal = value; }
        }

    }
}
