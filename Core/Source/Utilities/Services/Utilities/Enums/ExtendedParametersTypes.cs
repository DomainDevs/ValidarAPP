namespace Sistran.Core.Application.UtilitiesServices.Enums
{
    /// <summary>
    /// Id de valores de la tabla COMM.CO_Parameter
    /// </summary>
    public enum ExtendedParametersTypes
    {
        /// <summary>
        /// Accesorios no originales - ACC. NO ORIGINALES DE FABRICA
        /// </summary>
        NonOriginalAccessories = 1015,
        /// <summary>
        /// Accesorios originales - ACC. ORIGINALES DE FABRICA
        /// </summary>
        OriginalAccessories = 1016,

        /// <summary>
        /// Campos que permiten Id 0
        /// </summary>
        FieldAllowZero = 10031,

        /// <summary>
        /// Campos que permiten caracteres especiales
        /// </summary>
        FieldAllowSpecialCharacter = 10032,

        /// <summary>
        /// Campos de texto que permiten solo numeros
        /// </summary>
        FieldTextTypeAllowNumericOnly = 10033
    }
}
