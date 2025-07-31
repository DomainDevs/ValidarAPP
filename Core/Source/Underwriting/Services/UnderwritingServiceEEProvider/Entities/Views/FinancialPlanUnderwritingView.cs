using System;
using Sistran.Core.Framework.DAF;
using Sistran.Core.Framework.Views;
namespace Sistran.Core.Application.UnderwritingServices.EEProvider.Entities.Views
{
    /// <summary>
    /// Esta clase permite modelar una relación entre Sistran.Core.Aplication
    /// .Products.Entities.ProductFinancialPlan, Sistran.Core.Aplication  
    /// .Products.Entities.FinancialPlan y Sistran.Core.Aplication   
    /// </summary>
    [Serializable()]
    public class FinancialPlanUnderwritingView : BusinessView
    {

        /// <summary>
        /// Constructor de instancias de la vista.
        /// </summary>
        public FinancialPlanUnderwritingView()
        { }
        /// <summary>
        /// Colección de elementos de la entidad
        /// </summary>
        /// <value>
        /// Colección de FinancialPlan.
        /// </value>
        public BusinessCollection FinancialPlanList
        {
            get
            {
                return this["FinancialPlan"];
            }
        }

        /// <summary>
        /// Colección de elementos de la entidad
        /// </summary>
        /// <value>
        /// Coleccion de ProductFinancialPlan
        /// </value>
        public BusinessCollection ProductFinancialPlanList
        {
            get
            {
                return this["ProductFinancialPlan"];
            }
        }

    }
}
