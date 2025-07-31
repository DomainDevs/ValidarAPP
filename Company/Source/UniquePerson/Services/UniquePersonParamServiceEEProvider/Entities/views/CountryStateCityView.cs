
namespace Sistran.Company.Application.UniquePersonParamService.EEProvider.Entities.views
{
    using System;
    using Sistran.Core.Framework.DAF;
    using Sistran.Core.Framework.Views;

    /// <summary>
    /// Vista de zonas de tarifacion y ciudades
    /// </summary>
    [Serializable]
    public class CountryStateCityView : BusinessView
    {
        /// <summary>
        /// Obtiene colección ciudades
        /// </summary>
        public BusinessCollection City => this["City"];

        /// <summary>
        /// Obtiene colección estados
        /// </summary>
        public BusinessCollection State => this["State"];

        /// <summary>
        /// Obtiene colección paises
        /// </summary>
        public BusinessCollection Country => this["Country"];
    }
}
