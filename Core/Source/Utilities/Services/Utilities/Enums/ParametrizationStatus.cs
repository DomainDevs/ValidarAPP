namespace Sistran.Core.Services.UtilitiesServices.Enums
{
    /// <summary>
    /// Enum para los estados de los registros de los ABM
    /// </summary>
    public enum ParametrizationStatus
    {
        /// <summary>
        /// Estado inicial 
        /// </summary>
        Original = 1,
        /// <summary>
        /// Crear Registro 
        /// </summary>
        Create = 2,
        /// <summary>
        /// Registro actualizado
        /// </summary>
        Update = 3,
        /// <summary>
        /// Registro eliminado
        /// </summary>
        Delete = 4,
        /// <summary>
        /// Error al realizar alguna de las operaciones
        /// </summary>
        Error = 5
    }
}
