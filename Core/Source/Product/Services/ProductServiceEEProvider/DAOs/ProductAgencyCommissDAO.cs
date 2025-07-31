using System.Collections;
using System.Collections.Generic;
using Sistran.Core.Framework.DAF;
using Model = Sistran.Core.Application.ProductServices.Models;
using System.Diagnostics;
using PRODEN = Sistran.Core.Application.Product.Entities;
using Sistran.Core.Application.Utilities.DataFacade;
using Sistran.Core.Framework.Queries;
using System.Linq;

namespace Sistran.Core.Application.ProductServices.EEProvider.DAOs
{
    /// <summary>
    /// Comision asociadas al propducto
    /// </summary>
    public class ProductAgencyCommissDAO
    {
        /// <summary>
        /// mapea un Model.ProductAgencyCommiss a apartir de un Entity.ProductAgencyCommiss
        /// </summary>
        /// <param name="productAgencyCommiss"></param>
        /// <returns></returns>
        public virtual Model.ProductAgencyCommiss CreateProductAgencyCommiss(PRODEN.ProductAgencyCommiss productAgencyCommiss)
        {
            return new Model.ProductAgencyCommiss
            {
                IndividualId = productAgencyCommiss.IndividualId,
                AgencyId = productAgencyCommiss.AgentAgencyId,
                CommissPercentage = productAgencyCommiss.StCommissPercentage,
                ProductId = productAgencyCommiss.ProductId
            };
        }

        /// <summary>
        /// mapea una lista de Model.ProductAgencyCommiss a apartir de una lista de Entity.ProductAgencyCommiss
        /// </summary>
        /// <param name="productAgencyCommiss"></param>
        /// <returns></returns>
        public virtual List<Model.ProductAgencyCommiss> CreateProductAgencyCommissions(IList productAgencyCommissionList)
        {
            List<Model.ProductAgencyCommiss> productAgencyCommissions = new List<Model.ProductAgencyCommiss>();
            ProductAgencyCommissDAO productAgencyCommissDAO = new ProductAgencyCommissDAO();
            foreach (PRODEN.ProductAgencyCommiss productAgencyCommiss in productAgencyCommissionList)
            {
                productAgencyCommissions.Add(productAgencyCommissDAO.CreateProductAgencyCommiss(productAgencyCommiss));
            }
            return productAgencyCommissions;
        }

        /// <summary>
        /// Obtener comisión de agente por producto
        /// </summary>
        /// <param name="agentId">Id agente</param>
        /// <param name="agencyId">Id agencia</param>
        /// <param name="productId">Id producto</param>
        /// <returns>Comission agente</returns>
        public virtual Model.ProductAgencyCommiss GetCommissByAgentIdAgencyIdProductId(int agentId, int agencyId, int productId)
        {
            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();

            PrimaryKey key = PRODEN.ProductAgencyCommiss.CreatePrimaryKey(agentId, agencyId, productId);
            PRODEN.ProductAgencyCommiss productAgencyCommissEntity = (PRODEN.ProductAgencyCommiss)DataFacadeManager.Instance.GetDataFacade().GetObjectByPrimaryKey(key);

            Model.ProductAgencyCommiss productAgencyCommiss = new Model.ProductAgencyCommiss();
            ProductAgencyCommissDAO productAgencyCommissDAO = new ProductAgencyCommissDAO();
            if (productAgencyCommissEntity != null)
            {
                productAgencyCommiss = productAgencyCommissDAO.CreateProductAgencyCommiss(productAgencyCommissEntity);
            }
            else
            {
                PRODEN.Product productEntity = null;

                ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();
                filter.Property(PRODEN.Product.Properties.ProductId, typeof(PRODEN.Product).Name)
                .Equal()
                .Constant(productId);
                using (var daf = DataFacadeManager.Instance.GetDataFacade())
                {
                    productEntity = daf.List(typeof(PRODEN.Product), filter.GetPredicate()).Cast<PRODEN.Product>().FirstOrDefault();
                }

                if (productEntity != null)
                {
                    productAgencyCommiss.CommissPercentage = productEntity.StandardCommissionPercentage.Value;
                }
            }

            stopWatch.Stop();
            Debug.WriteLine(stopWatch.Elapsed.ToString(), "Sistran.Core.Application.ProductServices.EEProvider.DAOs.GetCommissByAgentIdAgencyIdProductId");
            return productAgencyCommiss;
        }
    }
}
