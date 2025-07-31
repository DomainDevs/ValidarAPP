// -----------------------------------------------------------------------
// <copyright file="CompanyPhoneTypeDAO.cs" company="SISTRAN">
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
    public class CompanyPhoneTypeDAO
    {
        /// <summary>
        /// Crear un nuevo tipo documento datacrédito
        /// </summary>
        /// <param name="companyPhoneType">Datos tipo documento datacrédito</param>
        /// <returns>Tipo de documento datacrédito creado</returns>
        public models.CompanyPhoneType CreateCompanyPhoneType(models.CompanyPhoneType companyPhoneType)
        {
            CompanyPhoneType companyPhoneTypeEntity = EntityAssembler.CreateCompanyPhoneType(companyPhoneType);
            DataFacadeManager.Instance.GetDataFacade().InsertObject(companyPhoneTypeEntity);
            return ModelAssembler.CreateCompanyPhoneType(companyPhoneTypeEntity);
        }

        /// <summary>
        ///  Actualizar tipo documento datacrédito
        /// </summary>
        /// <param name="companyPhoneType">Modelo tipo documento datacrédito</param>
        /// <returns>Tipo de documento datacrédito actualizado</returns>
        public models.CompanyPhoneType UpdateCompanyPhoneType(models.CompanyPhoneType companyPhoneType)
        {
            ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();
            filter.Property(CompanyPhoneType.Properties.PhoneTypeCode, typeof(CompanyPhoneType).Name);
            filter.Equal();
            filter.Constant(companyPhoneType.PhoneTypeCode);

            List<models.CompanyPhoneType> companyPhoneTypes = this.GetCompanyPhoneTypeByFilter(filter);

            if (companyPhoneTypes.Count > 0)
            {
                PrimaryKey key = CompanyPhoneType.CreatePrimaryKey(companyPhoneType.PhoneTypeCode);
                CompanyPhoneType companyPhoneTypeEntity = (CompanyPhoneType)DataFacadeManager.Instance.GetDataFacade().GetObjectByPrimaryKey(key);
                companyPhoneTypeEntity.Description = companyPhoneType.Description;
                companyPhoneTypeEntity.SmallDescription = companyPhoneType.SmallDescription;
                companyPhoneTypeEntity.IsCellphone = companyPhoneType.IsCellphone;

                DataFacadeManager.Instance.GetDataFacade().UpdateObject(companyPhoneTypeEntity);
                return ModelAssembler.CreateCompanyPhoneType(companyPhoneTypeEntity);
            }
            else
            {
                return this.CreateCompanyPhoneType(companyPhoneType);
            }
        }

        /// <summary>
        /// Obtiene el tipo de documento datacrédito por identificador del tipo de documento
        /// </summary>
        /// <param name="phoneTypeCode">identificador del tipo de documento buscado.</param>
        /// <returns>Tipo de documento datacrédito obtenido</returns>
        public List<models.CompanyPhoneType> GetCompanyPhoneTypeByPhoneTypeCode(int phoneTypeCode)
        {
            ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();
            filter.PropertyEquals(CompanyPhoneType.Properties.PhoneTypeCode, phoneTypeCode);
            BusinessCollection businessCollection = new BusinessCollection(DataFacadeManager.Instance.GetDataFacade().SelectObjects(typeof(CompanyPhoneType), filter.GetPredicate()));
            List<models.CompanyPhoneType> companyPhoneType = ModelAssembler.CreateCompanyPhoneType(businessCollection);
            return companyPhoneType;
        }

        /// <summary>
        /// Obtiene todos los tipos de documentos datacrédito
        /// </summary>
        /// <returns>Tipos de documento datacrédito creados</returns>
        public List<models.CompanyPhoneType> GetAllCompanyPhoneType()
        {
            List<models.CompanyPhoneType> companyPhoneTypes = new List<models.CompanyPhoneType>();
            BusinessCollection businessCollection = new BusinessCollection(DataFacadeManager.Instance.GetDataFacade().SelectObjects(typeof(CompanyPhoneType)));
            if (businessCollection != null && businessCollection.Count > 0)
            {
                companyPhoneTypes = ModelAssembler.CreateCompanyPhoneType(businessCollection);
                foreach (models.CompanyPhoneType model in companyPhoneTypes)
                {
                    model.IsForeing = false;
                }
            }

            return companyPhoneTypes;
        }

        /// <summary>
        /// Elimina el tipo de documento datacrédito relacionado
        /// </summary>
        /// <param name="phoneTypeCode">Id del tipo documento datacrédito</param>
        /// <returns>Resultado de la eliminación</returns>
        public bool DeleteCompanyPhoneType(int phoneTypeCode)
        {
            try
            {
                ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();
                filter.PropertyEquals(CompanyPhoneType.Properties.PhoneTypeCode, phoneTypeCode);
                DataFacadeManager.Instance.GetDataFacade().Delete<CompanyPhoneType>(filter.GetPredicate());
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
        public List<models.CompanyPhoneType> GetCompanyPhoneTypeByFilter(ObjectCriteriaBuilder filter)
        {
            BusinessCollection businessCollection = new BusinessCollection(DataFacadeManager.Instance.GetDataFacade().SelectObjects(typeof(CompanyPhoneType), filter.GetPredicate()));
            return ModelAssembler.CreateCompanyPhoneType(businessCollection);
        }

        /// <summary>
        /// Realiza las operaciones CRUD para el tipo de teléfono
        /// </summary>
        /// <param name="companyPhoneTypesAdded">tipos de teléfono para agregar</param>
        /// <param name="companyPhoneTypesEdited">tipos de teléfono para editar</param>
        /// <param name="companyPhoneTypesDeleted">tipos de teléfono para eliminar</param>
        /// <returns>Resumen de las operaciones</returns>
        public ParametrizationResponse<models.CompanyPhoneType> SaveCompanyPhoneTypes(List<models.CompanyPhoneType> companyPhoneTypesAdded, List<models.CompanyPhoneType> companyPhoneTypesEdited, List<models.CompanyPhoneType> companyPhoneTypesDeleted)
        {
            Stopwatch stopWatch = new Stopwatch();
            ParametrizationResponse<models.CompanyPhoneType> returnCompanyPhoneTypes = new ParametrizationResponse<models.CompanyPhoneType>();
            stopWatch.Start();
            using (Context.Current)
            {
                // Agregar
                if (companyPhoneTypesAdded != null)
                {
                    using (Transaction transaction = new Transaction())
                    {
                        try
                        {
                            foreach (models.CompanyPhoneType item in companyPhoneTypesAdded)
                            {
                                item.PhoneTypeCode = this.GetPhoneTypeCode();
                                CompanyPhoneType entityCompanyPhoneType = EntityAssembler.CreateCompanyPhoneType(item);
                                DataFacadeManager.Instance.GetDataFacade().InsertObject(entityCompanyPhoneType);
                            }

                            transaction.Complete();
                            returnCompanyPhoneTypes.TotalAdded = companyPhoneTypesAdded.Count;
                        }
                        catch (System.Exception)
                        {
                            transaction.Dispose();
                            returnCompanyPhoneTypes.ErrorAdded = "ErrorSaveCompanyPhoneTypesAdded";
                        }
                    }
                }

                // Modificar
                if (companyPhoneTypesEdited != null)
                {
                    using (Transaction transaction = new Transaction())
                    {
                        try
                        {
                            foreach (models.CompanyPhoneType item in companyPhoneTypesEdited)
                            {
                                PrimaryKey key = CompanyPhoneType.CreatePrimaryKey(item.PhoneTypeCode);
                                CompanyPhoneType companyPhoneTypeEntity = new CompanyPhoneType(item.PhoneTypeCode);
                                companyPhoneTypeEntity = (CompanyPhoneType)DataFacadeManager.Instance.GetDataFacade().GetObjectByPrimaryKey(key);
                                companyPhoneTypeEntity.Description = item.Description;
                                companyPhoneTypeEntity.SmallDescription = item.SmallDescription;
                                companyPhoneTypeEntity.IsCellphone = item.IsCellphone;
                                DataFacadeManager.Instance.GetDataFacade().UpdateObject(companyPhoneTypeEntity);
                            }

                            transaction.Complete();
                            returnCompanyPhoneTypes.TotalModify = companyPhoneTypesEdited.Count;
                        }
                        catch (System.Exception)
                        {
                            transaction.Dispose();
                            returnCompanyPhoneTypes.ErrorModify = "ErrorSaveCompanyPhoneTypesEdited";
                        }
                    }
                }

                // Eliminar
                if (companyPhoneTypesDeleted != null)
                {
                    using (Transaction transaction = new Transaction())
                    {
                        try
                        {
                            foreach (models.CompanyPhoneType item in companyPhoneTypesDeleted)
                            {
                                PrimaryKey key = CompanyPhoneType.CreatePrimaryKey(item.PhoneTypeCode);
                                CompanyPhoneType companyPhoneTypeEntity = new CompanyPhoneType(item.PhoneTypeCode);
                                companyPhoneTypeEntity = (CompanyPhoneType)DataFacadeManager.Instance.GetDataFacade().GetObjectByPrimaryKey(key);
                                DataFacadeManager.Instance.GetDataFacade().DeleteObject(companyPhoneTypeEntity);
                            }

                            transaction.Complete();
                            returnCompanyPhoneTypes.TotalDeleted = companyPhoneTypesDeleted.Count;
                        }
                        catch (ForeignKeyException)
                        {
                            transaction.Dispose();
                            returnCompanyPhoneTypes.ErrorDeleted = "ErrorSaveCompanyPhoneTypesRelated";
                        }
                        catch (RelatedObjectException)
                        {
                            transaction.Dispose();
                            returnCompanyPhoneTypes.ErrorDeleted = "ErrorSaveCompanyPhoneTypesRelated";
                        }
                        catch (System.Exception)
                        {
                            transaction.Dispose();
                            returnCompanyPhoneTypes.ErrorDeleted = "ErrorSaveCompanyPhoneTypesDeleted";
                        }
                    }
                }

                returnCompanyPhoneTypes.ReturnedList = this.GetAllCompanyPhoneType();
            }

            stopWatch.Stop();
            Debug.WriteLine(stopWatch.Elapsed.ToString(), "Sistran.Core.Application.UniquePersonService.DAOs.SaveCompanyPhoneTypes");
            return returnCompanyPhoneTypes;
        }

        /// <summary>
        /// Retorna el valor de la proxima key de la tabla PhoneType
        /// </summary>
        /// <returns>Key dispobible</returns>
        public int GetPhoneTypeCode()
        {
            int maxPhoneTypeCode = new BusinessCollection(DataFacadeManager.Instance.GetDataFacade()
                            .SelectObjects(typeof(CompanyPhoneType)))
                            .Cast<CompanyPhoneType>().Max(x => x.PhoneTypeCode);
            maxPhoneTypeCode++;
            return maxPhoneTypeCode;
        }

        /// <summary>
        /// Verifica si el tipo de teléfono esta relacionado en otras tablas
        /// </summary>
        /// <param name="phoneTypeCode">key del tipo de teléfono</param>
        /// <returns>Resultado de la validación</returns>
        public bool ValidateForeingCompanyPhoneType(int phoneTypeCode)
        {
            // COMM.CO_BRANCH COMMON
            ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();
            filter.PropertyEquals(CoBranch.Properties.PhoneTypeCode, phoneTypeCode);
            BusinessCollection businessCollection = new BusinessCollection(DataFacadeManager.Instance.GetDataFacade().SelectObjects(typeof(CoBranch), filter.GetPredicate()));
            if (businessCollection != null && businessCollection.Count > 0)
            {
                return true;
            }

            // UP.PHONE
            filter = new ObjectCriteriaBuilder();
            filter.PropertyEquals(Phone.Properties.PhoneTypeCode, phoneTypeCode);
            businessCollection = new BusinessCollection(DataFacadeManager.Instance.GetDataFacade().SelectObjects(typeof(Phone), filter.GetPredicate()));
            if (businessCollection != null && businessCollection.Count > 0)
            {
                return true;
            }

            return false;
        }
    }
}
