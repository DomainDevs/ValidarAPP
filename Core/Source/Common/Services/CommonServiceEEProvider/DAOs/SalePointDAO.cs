using Sistran.Core.Application.CommonServices.EEProvider.Assemblers;
using Sistran.Core.Application.Utilities.DataFacade;
using Sistran.Core.Framework.DAF;
using Sistran.Core.Framework.Queries;
using System;
using System.Collections.Generic;
using COMMEN = Sistran.Core.Application.Common.Entities;
using COMMML = Sistran.Core.Application.CommonService.Models;

namespace Sistran.Core.Application.CommonServices.EEProvider.DAOs
{
    public class SalePointDAO
    {
        private static readonly Object thisLock = new Object();
        /// <summary>
        /// Obtener puntos de venta
        /// </summary>
        /// <param name="branchId">Id branch</param>
        /// <returns></returns>
        public List<COMMML.SalePoint> GetSalePoints()
        {
            BusinessCollection businessCollection = new BusinessCollection(DataFacadeManager.Instance.GetDataFacade().SelectObjects(typeof(COMMEN.SalePoint)));
            return ModelAssembler.CreateSalePoints(businessCollection);
        }
        

        /// <summary>
        /// Get all UserSalePoint 
        /// </summary>
        /// <param name="userId">userId</param>
        /// <returns> List of UserSalePoint</returns>
        public List<COMMML.SalePoint> GetSalePointByBranchId(int branchId, bool isEnabled)
        {
            List<COMMML.SalePoint> salesPoints = new List<COMMML.SalePoint>();
            ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();
            filter.Property(COMMEN.SalePoint.Properties.BranchCode);
            filter.Equal();
            filter.Constant(branchId);

            if (isEnabled)
            {
                filter.And();
                filter.Property(COMMEN.SalePoint.Properties.Enabled);
                filter.Equal();
                filter.Constant(isEnabled);
            }

            BusinessCollection businessCollection = new BusinessCollection(DataFacadeManager.Instance.GetDataFacade().SelectObjects(typeof(COMMEN.SalePoint), filter.GetPredicate()));
            return ModelAssembler.CreateSalePoints(businessCollection);
        }


        /// <summary>
        /// Obtener puntos de venta por sucursal
        /// </summary>
        /// <param name="branchId">Id Sucursal</param>
        /// <returns>Lista de puntos de venta</returns>
        public List<COMMML.SalePoint> GetSalePointsByBranchId(int branchId)
        {
            List<COMMML.SalePoint> salesPoints = new List<COMMML.SalePoint>();
            ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();
            filter.Property(COMMEN.SalePoint.Properties.BranchCode);
            filter.Equal();
            filter.Constant(branchId);            

            BusinessCollection businessCollection = new BusinessCollection(DataFacadeManager.Instance.GetDataFacade().SelectObjects(typeof(COMMEN.SalePoint), filter.GetPredicate()));
            return ModelAssembler.CreateSalePoints(businessCollection);
        }

    }
}
