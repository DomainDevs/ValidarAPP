using Sistran.Core.Framework.DAF;
using Sistran.Core.Framework.Views;
using System;

namespace Sistran.Core.Application.ProductServices.EEProvider.Entities.Views
{
    /// <summary>
    /// Producto por Agencia
    /// </summary>
    /// <seealso cref="Sistran.Core.Framework.Views.BusinessView" />
    [Serializable()]
    public class ProductAgentView : BusinessView
    {
        public BusinessCollection Products
        {
            get
            {
                return this["Product"];
            }
        }
        public BusinessCollection ProductAgents
        {
            get
            {
                return this["ProductAgent"];
            }
        }

        public BusinessCollection Agents
        {
            get
            {
                return this["Agent"];
            }
        }

        public BusinessCollection AgentTypes
        {
            get
            {
                return this["AgentType"];
            }
        }

    }
}
