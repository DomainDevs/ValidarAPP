using System.Collections.Generic;
using Sistran.Core.Framework.DAF.Engine;
using Sistran.Core.Framework.Queries;
using Sistran.Company.Application.MassiveServices.EEProvider.Assemblers;
using Sistran.Company.Application.MassiveServices.EEProvider.Entities.View;
using Sistran.Core.Application.Product.Entities;
using PRODMOD = Sistran.Core.Application.ProductServices.Models;
using Sistran.Core.Application.Utilities.DataFacade;

namespace Sistran.Company.Application.MassiveServices.EEProvider.DAOs
{
    public class ProductDAO
    {
        /// <summary>
        /// Buscar los productos para un agente habilitados para solicitud agrupadora
        /// </summary>
        /// <param name="agentId">Identificador del agente</param>
        /// <param name="prefixId">Identificador del ramo</param>
        /// <param name="isRequest">Esta habilitado para solicitud agrupadora</param>
        /// <returns>Lista Model.Product</returns>        
        public List<PRODMOD.Product> GetProductsByAgentIdPrefixId(int agentId, int prefixId)
        {
            List<PRODMOD.Product> products = new List<PRODMOD.Product>();

            CoProductAgentView view = new CoProductAgentView();
            ViewBuilder builder = new ViewBuilder("CoProductAgentView");
            ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();

            filter.Property(Core.Application.Product.Entities.ProductAgent.Properties.IndividualId, typeof(Core.Application.Product.Entities.ProductAgent).Name);
            filter.Equal();
            filter.Constant(agentId);
            filter.And();
            filter.Property(Core.Application.Product.Entities.Product.Properties.PrefixCode, typeof(Core.Application.Product.Entities.Product).Name);
            filter.Equal();
            filter.Constant(prefixId);
            filter.And();
            filter.Property(CoProduct.Properties.IsCollective, typeof(CoProduct).Name);
            filter.Equal();
            filter.Constant(0);
            filter.And();
            filter.Property(Core.Application.Product.Entities.Product.Properties.CurrentTo, typeof(Core.Application.Product.Entities.Product).Name);
            filter.IsNull();

            builder.Filter = filter.GetPredicate();

            DataFacadeManager.Instance.GetDataFacade().FillView(builder, view);

            if (view.Products.Count > 0)
	        {
                products = ModelAssembler.CreateProducts(view.Products);
	        }

            return products;
        }
        /// <summary>
        /// Buscar los productos para un agente habilitados para colectivas
        /// </summary>
        /// <param name="agentId">Identificador del agente</param>
        /// <param name="prefixId">Identificador del ramo</param>
        /// <returns>Lista Model.Product</returns>        
        public List<PRODMOD.Product> GetCollectiveProductsByAgentIdPrefixId(int agentId, int prefixId)
        {
            List<PRODMOD.Product> products = new List<PRODMOD.Product>();

            CoProductAgentView view = new CoProductAgentView();
            ViewBuilder builder = new ViewBuilder("CoProductAgentView");
            ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();

            filter.Property(Core.Application.Product.Entities.ProductAgent.Properties.IndividualId, typeof(Core.Application.Product.Entities.ProductAgent).Name);
            filter.Equal();
            filter.Constant(agentId);
            filter.And();
            filter.Property(Core.Application.Product.Entities.Product.Properties.PrefixCode, typeof(Core.Application.Product.Entities.Product).Name);
            filter.Equal();
            filter.Constant(prefixId);
            filter.And();
            filter.Property(CoProduct.Properties.IsCollective, typeof(CoProduct).Name);
            filter.Equal();
            filter.Constant(1);
            builder.Filter = filter.GetPredicate();

            DataFacadeManager.Instance.GetDataFacade().FillView(builder, view);

            if (view.Products.Count > 0)
            {
                products = ModelAssembler.CreateProducts(view.Products);
            }

            return products;
        }
    }
}