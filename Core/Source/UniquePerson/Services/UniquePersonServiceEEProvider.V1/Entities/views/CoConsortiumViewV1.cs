using Sistran.Core.Framework.DAF;
using Sistran.Core.Framework.Views;
using System;


namespace Sistran.Core.Application.UniquePersonService.V1.Entities.views
{
    /// <summary>
    /// Esta clase permite modelar una relación entre TempRisk y los riesgos
    /// según el tipo de riesgo cubierto de cada riesgo.
    /// </summary>
    [Serializable()]
    public class CoConsortiumViewV1 : BusinessView
    {
        public CoConsortiumViewV1()
        {
        }
        
        /// <summary>
        /// Lista de objetos de la entidad Insured.
        /// </summary>
        /// <value>
        /// Objetos de la entidad Insured.
        /// </value>
        public BusinessCollection InsuredList
        {
            get
            {
                return this["Insured"];
            }
        }
        /// <summary>
        /// Lista de objetos de la entidad CoConsortium.
        /// </summary>
        /// <value>
        /// Objetos de la entidad CoConsortium.
        /// </value>
        public BusinessCollection CoConsortiumList
        {
            get
            {
                return this["CoConsortium"];
            }
        }
            }
}
