using Sistran.Company.Application.UniquePersonServices.V1.EEProvider.Assemblers;
using Sistran.Company.Application.UniquePersonServices.V1.Models;
using Sistran.Core.Application.Utilities.DataFacade;
using Sistran.Core.Framework.Contexts;
using Sistran.Core.Framework.DAF;
using Sistran.Core.Framework.Queries;
using Sistran.Core.Framework.Transactions;
using Sistran.Core.Services.UtilitiesServices.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using entities = Sistran.Core.Application.UniquePersonV1.Entities;

namespace Sistran.Company.Application.UniquePersonServices.V1.EEProvider.DAOs
{
    public class CompanyPhoneTypeDAO
    {
        /// <summary>
        /// Obtiene todos los tipos de documentos datacrédito
        /// </summary>
        /// <returns>Tipos de documento datacrédito creados</returns>
        public List<CompanyPhoneType> GetAllCompanyPhoneType()
        {
            List<CompanyPhoneType> companyPhoneTypes = new List<CompanyPhoneType>();
            BusinessCollection businessCollection = new BusinessCollection(DataFacadeManager.Instance.GetDataFacade().SelectObjects(typeof(entities.CompanyPhoneType)));
            if (businessCollection != null && businessCollection.Count > 0)
            {
                companyPhoneTypes = ModelAssembler.CreateCompanyPhoneType(businessCollection);
                foreach (CompanyPhoneType model in companyPhoneTypes)
                {
                    model.IsForeing = false;
                }
            }

            return companyPhoneTypes;
        }


        /// <summary>
        /// Realiza las operaciones CRUD para el tipo de teléfono
        /// </summary>
        /// <param name="companyPhoneTypesAdded">tipos de teléfono para agregar</param>
        /// <param name="companyPhoneTypesEdited">tipos de teléfono para editar</param>
        /// <param name="companyPhoneTypesDeleted">tipos de teléfono para eliminar</param>
        /// <returns>Resumen de las operaciones</returns>
        public ParametrizationResponse<CompanyPhoneType> SaveCompanyPhoneTypes(List<CompanyPhoneType> companyPhoneTypesAdded, List<CompanyPhoneType> companyPhoneTypesEdited, List<CompanyPhoneType> companyPhoneTypesDeleted)
        {
            Stopwatch stopWatch = new Stopwatch();
            ParametrizationResponse<CompanyPhoneType> returnCompanyPhoneTypes = new ParametrizationResponse<CompanyPhoneType>();
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
                            foreach (CompanyPhoneType item in companyPhoneTypesAdded)
                            {
                                item.PhoneTypeCode = this.GetPhoneTypeCode();
                                entities.CompanyPhoneType entityCompanyPhoneType = EntityAssembler.CreateCompanyPhoneType(item);
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
                            foreach (CompanyPhoneType item in companyPhoneTypesEdited)
                            {
                                PrimaryKey key = entities.CompanyPhoneType.CreatePrimaryKey(item.PhoneTypeCode);
                                entities.CompanyPhoneType companyPhoneTypeEntity = new entities.CompanyPhoneType(item.PhoneTypeCode);
                                companyPhoneTypeEntity = (entities.CompanyPhoneType)DataFacadeManager.Instance.GetDataFacade().GetObjectByPrimaryKey(key);
                                companyPhoneTypeEntity.Description = item.Description;
                                companyPhoneTypeEntity.SmallDescription = item.SmallDescription;
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
                            foreach (CompanyPhoneType item in companyPhoneTypesDeleted)
                            {
                                PrimaryKey key = entities.CompanyPhoneType.CreatePrimaryKey(item.PhoneTypeCode);
                                entities.CompanyPhoneType companyPhoneTypeEntity = new entities.CompanyPhoneType(item.PhoneTypeCode);
                                companyPhoneTypeEntity = (entities.CompanyPhoneType)DataFacadeManager.Instance.GetDataFacade().GetObjectByPrimaryKey(key);
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
    }
}
