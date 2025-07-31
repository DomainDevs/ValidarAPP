namespace Sistran.Core.Application.SecurityServices.EEProvider.Models
{

    /// <summary>
    /// Accesos Menu
    /// </summary>
    public class Operation
    {
        /// <summary>
        /// Constructor<see cref="Operation"/> class.
        /// </summary>
        public Operation()
        {
            IsParentOperationIdNull = true;
            IsOperationObjectIdNull = true;
        }
        public int ParentOperationId { get; set; }
        /// <summary>
        /// Indicate if ParentOperationId property value is null.
        /// </summary>

        public bool IsParentOperationIdNull { get; set; }
        /// <summary>
        /// Enabled property attribute.
        /// </summary>
        public bool Enabled { get; set; }
        /// <summary>
        /// OperationId property attribute.
        /// </summary>
        public int OperationId { get; set; }
        /// <summary>
        /// ModuleId property attribute.
        /// </summary>
        public int ModuleId { get; set; }
        /// <summary>
        /// OperationObjectId property attribute.
        /// </summary>
        public int OperationObjectId { get; set; }
        /// <summary>
        /// Indicate if OperationObjectId property value is null.
        /// </summary>
        public bool IsOperationObjectIdNull { get; set; }
        /// <summary>
        /// SubmoduleId property attribute.
        /// </summary>
        public int SubmoduleId { get; set; }
        /// <summary>
        /// Url property attribute.
        /// </summary>
        public string Route { get; set; }
        /// <summary>
        /// Description property attribute.
        /// </summary>
        public string Description { get; set; }

    }
}