// -----------------------------------------------------------------------
// <copyright file="UniqueUsersProductDAO.cs" company="SISTRAN">
// Copyright (c) SISTRAN ANDINA. All rights reserved.
// </copyright>
// <author>Carlos Eduardo Rodríguez</author>
// -----------------------------------------------------------------------
namespace Sistran.Core.Application.UniqueUserServices.EEProvider.DAOs
{
    using System.Collections.Generic;
    using System.Diagnostics;
    using Sistran.Core.Application.UniqueUserServices.EEProvider.Assemblers;
    using Sistran.Core.Application.UniqueUserServices.Models;
    using Sistran.Core.Application.Utilities.DataFacade;
    using Sistran.Core.Framework.DAF;
    using Sistran.Core.Framework.Queries;
    
    using UniqueUserEntities = Sistran.Core.Application.UniqueUser.Entities;
    using UniqueProductEntities = Sistran.Core.Application.Product.Entities;

    /// <summary>
    /// Clase de acceso a datos Unique User Product.
    /// </summary>
    public class UniqueUsersProductDAO
    {
        /// <summary>
        /// Obtiene el listado de productos del usuario por ramo comercial.
        /// </summary>
        /// <param name="prefixCode">Código del ramo comercial.</param>
        /// <returns>Listado de productos del usuario por ramo comercial.</returns>  
        public List<UniqueUsersProduct> GetUniqueUsersProductByPrefixCd(int prefixCode)
        {
            ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();
            filter.Property(UniqueUserEntities.UniqueUsersProduct.Properties.PrefixCode, typeof(UniqueUserEntities.UniqueUsersProduct).Name);
            filter.Equal();
            filter.Constant(prefixCode);
            BusinessCollection businessCollection = new BusinessCollection(DataFacadeManager.Instance.GetDataFacade().SelectObjects(typeof(UniqueUserEntities.UniqueUsersProduct), filter.GetPredicate()));
            List<UniqueUsersProduct> uniqueUsersProduct = Assemblers.ModelAssembler.MappUniqueUsersProductList(businessCollection);

            return uniqueUsersProduct;
        }

        /// <summary>
        /// Obtiene el listado de productos del usuario por ramo comercial.
        /// </summary>
        /// <param name="userId">Identificador del usuario.</param>
        /// <param name="productId">Identificador del producto.</param>
        /// <param name="prefixCode">Código del ramo comercial.</param>
        /// <returns>Listado de productos del usuario por ramo comercial.</returns>  
        public UniqueUserEntities.UniqueUsersProduct GetUniqueUsersProductByPrimaryKey(int userId, int productId, int prefixCode)
        {
            PrimaryKey key = UniqueUserEntities.UniqueUsersProduct.CreatePrimaryKey(userId, productId, prefixCode);
            UniqueUserEntities.UniqueUsersProduct uniqueUsersProductEntity = (UniqueUserEntities.UniqueUsersProduct)DataFacadeManager.Instance.GetDataFacade().GetObjectByPrimaryKey(key);            
            return uniqueUsersProductEntity;
        }

        /// <summary>
        /// Actualiza y crea los puntos de venta de aliado del usuario. 
        /// </summary>
        /// <param name="uniqueUsersProductModelList">Lista de puntos de venta de aliado del usuario</param>        
        public void SaveUniqueUsersProduct(List<UniqueUsersProduct> uniqueUsersProductModelList, int userId)
        {
            
            ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();
            filter.PropertyEquals(UniqueUserEntities.UniqueUsersProduct.Properties.UserId, userId);
            DataFacadeManager.Instance.GetDataFacade().Delete<UniqueUserEntities.UniqueUsersProduct>(filter.GetPredicate());
            if (uniqueUsersProductModelList!=null && uniqueUsersProductModelList.Count>0)
            {
                List<UniqueUserEntities.UniqueUsersProduct> uniqueUsersProductEntityList = EntityAssembler.MappUniqueUsersProductList(uniqueUsersProductModelList);
                foreach (UniqueUserEntities.UniqueUsersProduct itemUniqueUsersProductEntity in uniqueUsersProductEntityList)
                {
                    UniqueUserEntities.UniqueUsersProduct uniqueUsersProductEntity = this.GetUniqueUsersProductByPrimaryKey(itemUniqueUsersProductEntity.UserId, itemUniqueUsersProductEntity.ProductId, itemUniqueUsersProductEntity.PrefixCode);
                    if (uniqueUsersProductEntity == null)
                    {
                        DataFacadeManager.Instance.GetDataFacade().InsertObject(itemUniqueUsersProductEntity);
                    }
                    else
                    {
                        DataFacadeManager.Instance.GetDataFacade().UpdateObject(itemUniqueUsersProductEntity);
                    }
                }
            }
        
        }

        /// <summary>
        /// Obtiene el listado de productos del usuario por ramo comercial.
        /// </summary>
        /// <param name="userId">Id del usuario.</param>
        /// <returns>Listado de productos del usuario por ramo comercial.</returns>  
        public List<UniqueUsersProduct> GetUniqueUsersProductsStatusByUserId(int userId)
        {
            ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();
            filter.Property(UniqueUserEntities.UniqueUsersProduct.Properties.UserId, typeof(UniqueUserEntities.UniqueUsersProduct).Name);
            filter.Equal();
            filter.Constant(userId);
            BusinessCollection businessCollection = new BusinessCollection(DataFacadeManager.Instance.GetDataFacade().SelectObjects(typeof(UniqueUserEntities.UniqueUsersProduct), filter.GetPredicate()));
            List<UniqueUsersProduct> listUniqueUsersProduct = Assemblers.ModelAssembler.MappUniqueUsersProductList(businessCollection);
            businessCollection = new BusinessCollection(DataFacadeManager.Instance.GetDataFacade().SelectObjects(typeof(UniqueProductEntities.Product)));
            List<UniqueUsersProduct> result = new List<UniqueUsersProduct>();
            foreach (UniqueProductEntities.Product itemProduct in businessCollection)
            {
                UniqueUsersProduct uniqueUsersProduct = new UniqueUsersProduct();
                uniqueUsersProduct.UserId = userId;
                uniqueUsersProduct.ProductId = itemProduct.ProductId;
                uniqueUsersProduct.ProductDescription = itemProduct.Description;
                uniqueUsersProduct.PrefixCode = itemProduct.PrefixCode;
                if (itemProduct.CurrentTo == null || itemProduct.CurrentTo >= System.DateTime.Now)
                {
                    uniqueUsersProduct.Enabled = true;
                }
                else
                {
                    uniqueUsersProduct.Enabled = false;
                }
                if (listUniqueUsersProduct != null)
                {
                    UniqueUsersProduct exist = listUniqueUsersProduct.Find(x => x.ProductId == itemProduct.ProductId);
                    if (exist != null )
                    {
                        uniqueUsersProduct.Assign = true;  
                    }
                    else
                    {
                        uniqueUsersProduct.Assign = false;
                    }
                }
                else
                {
                    uniqueUsersProduct.Assign = false;
                }
                result.Add(uniqueUsersProduct);
            }
            return result;
        }
    }
}
