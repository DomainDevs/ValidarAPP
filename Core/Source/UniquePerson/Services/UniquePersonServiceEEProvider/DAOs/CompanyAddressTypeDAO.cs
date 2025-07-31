// -----------------------------------------------------------------------
// <copyright file="CompanyAddressTypeDAO.cs" company="SISTRAN">
// Copyright (c) SISTRAN ANDINA. All rights reserved.
// </copyright>
// <author>Cristyan Fernando Ballesteros</author>
// -----------------------------------------------------------------------

namespace Sistran.Core.Application.UniquePersonService.DAOs
{
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Linq;
    using Sistran.Core.Application.Common.Entities;
    using Sistran.Core.Application.CommonService.Models;
    using Sistran.Core.Application.Quotation.Entities;
    using Sistran.Core.Application.UniquePerson.Entities;
    using Sistran.Core.Application.UniquePersonService.Assemblers;
    using Sistran.Core.Application.Utilities.DataFacade;
    using Sistran.Core.Framework.Contexts;
    using Sistran.Core.Framework.DAF;
    using Sistran.Core.Framework.Queries;
    using Sistran.Core.Framework.Transactions;
    using Sistran.Core.Services.UtilitiesServices.Models;
    using models = Sistran.Core.Application.UniquePersonService.Models;

    /// <summary>
    /// Contiene los procedimientos del tipo de documento datacrédito para la capa de datos 
    /// </summary>
    public class CompanyAddressTypeDAO
    {
        /// <summary>
        /// Crear un nuevo tipo documento datacrédito
        /// </summary>
        /// <param name="companyAddressType">Datos tipo documento datacrédito</param>
        /// <returns>Tipo de documento datacrédito creado</returns>
        public models.CompanyAddressType CreateCompanyAddressType(models.CompanyAddressType companyAddressType)
        {
            CompanyAddressType companyAddressTypeEntity = EntityAssembler.CreateCompanyAddressType(companyAddressType);
            DataFacadeManager.Instance.GetDataFacade().InsertObject(companyAddressTypeEntity);
            return ModelAssembler.CreateCompanyAddressType(companyAddressTypeEntity);
        }

        /// <summary>
        ///  Actualizar tipo documento datacrédito
        /// </summary>
        /// <param name="companyAddressType">Modelo tipo documento datacrédito</param>
        /// <returns>Tipo de documento datacrédito actualizado</returns>
        public models.CompanyAddressType UpdateCompanyAddressType(models.CompanyAddressType companyAddressType)
        {
            ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();
            filter.Property(CompanyAddressType.Properties.AddressTypeCode, typeof(CompanyAddressType).Name);
            filter.Equal();
            filter.Constant(companyAddressType.AddressTypeCode);

            List<models.CompanyAddressType> companyAddressTypes = this.GetCompanyAddressTypeByFilter(filter);

            if (companyAddressTypes.Count > 0)
            {
                PrimaryKey key = CompanyAddressType.CreatePrimaryKey(companyAddressType.AddressTypeCode);
                CompanyAddressType companyAddressTypeEntity = (CompanyAddressType)DataFacadeManager.Instance.GetDataFacade().GetObjectByPrimaryKey(key);
                companyAddressTypeEntity.SmallDescription = companyAddressType.SmallDescription;
                companyAddressTypeEntity.TinyDescription = companyAddressType.TinyDescription;
                companyAddressTypeEntity.IsElectronicMail = companyAddressType.IsElectronicMail;

                DataFacadeManager.Instance.GetDataFacade().UpdateObject(companyAddressTypeEntity);
                return ModelAssembler.CreateCompanyAddressType(companyAddressTypeEntity);
            }
            else
            {
                return this.CreateCompanyAddressType(companyAddressType);
            }
        }

        /// <summary>
        /// Obtiene el tipo de documento datacrédito por identificador del tipo de documento
        /// </summary>
        /// <param name="addressTypeCode">identificador del tipo de documento buscado.</param>
        /// <returns>Tipo de documento datacrédito obtenido</returns>
        public List<models.CompanyAddressType> GetCompanyAddressTypeByAddressTypeCode(int addressTypeCode)
        {
            ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();
            filter.PropertyEquals(CompanyAddressType.Properties.AddressTypeCode, addressTypeCode);
            BusinessCollection businessCollection = new BusinessCollection(DataFacadeManager.Instance.GetDataFacade().SelectObjects(typeof(CompanyAddressType), filter.GetPredicate()));
            List<models.CompanyAddressType> companyAddressType = ModelAssembler.CreateCompanyAddressType(businessCollection);
            return companyAddressType;
        }

        /// <summary>
        /// Obtiene todos los tipos de documentos datacrédito
        /// </summary>
        /// <returns>Tipos de documento datacrédito creados</returns>
        public List<models.CompanyAddressType> GetAllCompanyAddressType()
        {
            List<models.CompanyAddressType> companyAddressTypes = new List<models.CompanyAddressType>();
            BusinessCollection businessCollection = new BusinessCollection(DataFacadeManager.Instance.GetDataFacade().SelectObjects(typeof(CompanyAddressType)));
            if (businessCollection != null && businessCollection.Count > 0)
            {
                companyAddressTypes = ModelAssembler.CreateCompanyAddressType(businessCollection);

                foreach (models.CompanyAddressType model in companyAddressTypes)
                {
                    model.IsForeing = false;
                }
            }

            return companyAddressTypes;
        }

        /// <summary>
        /// Elimina el tipo de documento datacrédito relacionado
        /// </summary>
        /// <param name="addressTypeCode">Id del tipo documento datacrédito</param>
        /// <returns>Resultado de la eliminación</returns>
        public bool DeleteCompanyAddressType(int addressTypeCode)
        {
            try
            {
                ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();
                filter.PropertyEquals(CompanyAddressType.Properties.AddressTypeCode, addressTypeCode);
                DataFacadeManager.Instance.GetDataFacade().Delete<CompanyAddressType>(filter.GetPredicate());
                return true;
            }
            catch (System.Exception)
            {
                return false;
            }
        }

        /// <summary>
        /// Obtiene el tipo de documento datacrédito por filtro
        /// </summary>
        /// <param name="filter">Filtro realizado</param>
        /// <returns>Resultado del filtro aplicado</returns>
        public List<models.CompanyAddressType> GetCompanyAddressTypeByFilter(ObjectCriteriaBuilder filter)
        {
            BusinessCollection businessCollection = new BusinessCollection(DataFacadeManager.Instance.GetDataFacade().SelectObjects(typeof(CompanyAddressType), filter.GetPredicate()));
            return ModelAssembler.CreateCompanyAddressType(businessCollection);
        }

        /// <summary>
        /// Realiza las operaciones CRUD para el tipo de dirección
        /// </summary>
        /// <param name="companyAddressTypesAdded">tipos de dirección para agregar</param>
        /// <param name="companyAddressTypesEdited">tipos de dirección para editar</param>
        /// <param name="companyAddressTypesDeleted">tipos de direción para eliminar</param>
        /// <returns>Resumen de las operaciones</returns>
        public ParametrizationResponse<models.CompanyAddressType> SaveCompanyAddressTypes(List<models.CompanyAddressType> companyAddressTypesAdded, List<models.CompanyAddressType> companyAddressTypesEdited, List<models.CompanyAddressType> companyAddressTypesDeleted)
        {
            Stopwatch stopWatch = new Stopwatch();
            ParametrizationResponse<models.CompanyAddressType> returnCompanyAddressTypes = new ParametrizationResponse<models.CompanyAddressType>();
            stopWatch.Start();
            using (Context.Current)
            {
                // Agregar
                if (companyAddressTypesAdded != null)
                {
                    using (Transaction transaction = new Transaction())
                    {
                        try
                        {
                            foreach (models.CompanyAddressType item in companyAddressTypesAdded)
                            {
                                item.AddressTypeCode = this.GetAddressTypeCode();
                                CompanyAddressType entityCompanyAddressType = EntityAssembler.CreateCompanyAddressType(item);
                                DataFacadeManager.Instance.GetDataFacade().InsertObject(entityCompanyAddressType);
                            }

                            transaction.Complete();
                            returnCompanyAddressTypes.TotalAdded = companyAddressTypesAdded.Count;
                        }
                        catch (System.Exception)
                        {
                            transaction.Dispose();
                            returnCompanyAddressTypes.ErrorAdded = "ErrorSaveCompanyAddressTypesAdded";
                        }
                    }
                }

                // Modificar
                if (companyAddressTypesEdited != null)
                {
                    using (Transaction transaction = new Transaction())
                    {
                        try
                        {
                            foreach (models.CompanyAddressType item in companyAddressTypesEdited)
                            {
                                PrimaryKey key = CompanyAddressType.CreatePrimaryKey(item.AddressTypeCode);
                                CompanyAddressType companyAddressTypeEntity = new CompanyAddressType(item.AddressTypeCode);
                                companyAddressTypeEntity = (CompanyAddressType)DataFacadeManager.Instance.GetDataFacade().GetObjectByPrimaryKey(key);
                                companyAddressTypeEntity.SmallDescription = item.SmallDescription;
                                companyAddressTypeEntity.TinyDescription = item.TinyDescription;
                                companyAddressTypeEntity.IsElectronicMail = item.IsElectronicMail;
                                DataFacadeManager.Instance.GetDataFacade().UpdateObject(companyAddressTypeEntity);
                            }

                            transaction.Complete();
                            returnCompanyAddressTypes.TotalModify = companyAddressTypesEdited.Count;
                        }
                        catch (System.Exception)
                        {
                            transaction.Dispose();
                            returnCompanyAddressTypes.ErrorModify = "ErrorSaveCompanyAddressTypesEdited";
                        }
                    }
                }

                // Eliminar
                if (companyAddressTypesDeleted != null)
                {
                    using (Transaction transaction = new Transaction())
                    {
                        try
                        {
                            foreach (models.CompanyAddressType item in companyAddressTypesDeleted)
                            {
                                PrimaryKey key = CompanyAddressType.CreatePrimaryKey(item.AddressTypeCode);
                                CompanyAddressType companyAddressTypeEntity = new CompanyAddressType(item.AddressTypeCode);
                                companyAddressTypeEntity = (CompanyAddressType)DataFacadeManager.Instance.GetDataFacade().GetObjectByPrimaryKey(key);
                                DataFacadeManager.Instance.GetDataFacade().DeleteObject(companyAddressTypeEntity);
                            }

                            transaction.Complete();
                            returnCompanyAddressTypes.TotalDeleted = companyAddressTypesDeleted.Count;
                        }
                        catch (ForeignKeyException)
                        {
                            transaction.Dispose();
                            returnCompanyAddressTypes.ErrorDeleted = "ErrorSaveCompanyAddressTypesRelated";
                        }
                        catch (RelatedObjectException)
                        {
                            transaction.Dispose();
                            returnCompanyAddressTypes.ErrorDeleted = "ErrorSaveCompanyAddressTypesRelated";
                        }
                        catch (System.Exception)
                        {
                            transaction.Dispose();
                            returnCompanyAddressTypes.ErrorDeleted = "ErrorSaveCompanyAddressTypesDeleted";
                        }
                    }
                }

                returnCompanyAddressTypes.ReturnedList = this.GetAllCompanyAddressType();
            }

            stopWatch.Stop();
            Debug.WriteLine(stopWatch.Elapsed.ToString(), "Sistran.Core.Application.UniquePersonService.DAOs.SaveCompanyAddressTypes");
            return returnCompanyAddressTypes;
        }

        /// <summary>
        /// Retorna el valor de la proxima key de la tabla AddressType
        /// </summary>
        /// <returns>Key dispobible</returns>
        public int GetAddressTypeCode()
        {
            int maxAddressTypeCode = new BusinessCollection(DataFacadeManager.Instance.GetDataFacade()
                            .SelectObjects(typeof(CompanyAddressType)))
                            .Cast<CompanyAddressType>().Max(x => x.AddressTypeCode);
            maxAddressTypeCode++;
            return maxAddressTypeCode;
        }

        /// <summary>
        /// Verifica si el tipo de dirección esta relacionado en otras tablas
        /// </summary>
        /// <param name="addressTypeCode">key del tipo de dirección</param>
        /// <returns>Resultado de la validación</returns>
        public bool ValidateForeingCompanyAddressType(int addressTypeCode)
        {
            // QUO.RISK_LOCATION QUOTATION
            ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();
            filter.PropertyEquals(RiskLocation.Properties.AddressTypeCode, addressTypeCode);
            BusinessCollection businessCollection = new BusinessCollection(DataFacadeManager.Instance.GetDataFacade().SelectObjects(typeof(RiskLocation), filter.GetPredicate()));
            if (businessCollection != null && businessCollection.Count > 0)
            {
                return true;
            }

            // COMM.CO_BRANCH COMMON
            filter = new ObjectCriteriaBuilder();
            filter.PropertyEquals(CoBranch.Properties.AddressTypeCode, addressTypeCode);
            businessCollection = new BusinessCollection(DataFacadeManager.Instance.GetDataFacade().SelectObjects(typeof(CoBranch), filter.GetPredicate()));
            if (businessCollection != null && businessCollection.Count > 0)
            {
                return true;
            }

            // UP.ADDRESS UNIQUE PERSON
            filter = new ObjectCriteriaBuilder();
            filter.PropertyEquals(Address.Properties.AddressTypeCode, addressTypeCode);
            businessCollection = new BusinessCollection(DataFacadeManager.Instance.GetDataFacade().SelectObjects(typeof(Address), filter.GetPredicate()));
            if (businessCollection != null && businessCollection.Count > 0)
            {
                return true;
            }

            // UP.PROSPECT UNIQUE PERSON
            filter = new ObjectCriteriaBuilder();
            filter.PropertyEquals(Prospect.Properties.AddressTypeCode, addressTypeCode);
            businessCollection = new BusinessCollection(DataFacadeManager.Instance.GetDataFacade().SelectObjects(typeof(Prospect), filter.GetPredicate()));
            if (businessCollection != null && businessCollection.Count > 0)
            {
                return true;
            }

            // INS.INSPECTION UNIQUE PERSON
            filter = new ObjectCriteriaBuilder();
            filter.PropertyEquals(CompanyInspection.Properties.AddressTypeCode, addressTypeCode);
            businessCollection = new BusinessCollection(DataFacadeManager.Instance.GetDataFacade().SelectObjects(typeof(CompanyInspection), filter.GetPredicate()));
            if (businessCollection != null && businessCollection.Count > 0)
            {
                return true;
            }

            return false;
        }
    }
}
