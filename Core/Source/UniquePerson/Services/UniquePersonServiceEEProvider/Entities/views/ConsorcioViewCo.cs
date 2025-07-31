using Sistran.Core.Framework.DAF;
using Sistran.Core.Framework.Views;
using System;


namespace Sistran.Core.Application.UniquePersonService.Entities.views
{
    /// <summary>
    /// Esta clase permite modelar una relación entre TempRisk y los riesgos
    /// según el tipo de riesgo cubierto de cada riesgo.
    /// </summary>
    [Serializable()]
    public class ConsorcioViewCo : BusinessView
    {
        public ConsorcioViewCo()
        {
        }
        /// <summary>
        /// Lista de objetos de la entidad Company.
        /// </summary>
        /// <value>
        /// Objetos de la entidad Company.
        /// </value>
        public BusinessCollection CompanyList
        {
            get
            {
                return this["Company"];
            }
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
        /// Lista de objetos de la entidad CoCompany.
        /// </summary>
        /// <value>
        /// Objetos de la entidad CoCompany.
        /// </value>
        public BusinessCollection CoCompanyList
        {
            get
            {
                return this["CoCompany"];
            }
        }
        public BusinessCollection CoAssociationTypeList
        {
            get
            {
                return this["CoAssociationType"];
            }
        }
        public BusinessCollection CoConsortiumList
        {
            get
            {
                return this["CoConsortium"];
            }
        }
    }
}
