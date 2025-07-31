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
    public class CompanyAddressTypeDAO
    {

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
        /// Realiza las operaciones CRUD para el tipo de dirección
        /// </summary>
        /// <param name="companyAddressTypesAdded">tipos de dirección para agregar</param>
        /// <param name="companyAddressTypesEdited">tipos de dirección para editar</param>
        /// <param name="companyAddressTypesDeleted">tipos de direción para eliminar</param>
        /// <returns>Resumen de las operaciones</returns>
        public ParametrizationResponse<CompanyAddressType> SaveCompanyAddressTypes(List<CompanyAddressType> companyAddressTypesAdded, List<CompanyAddressType> companyAddressTypesEdited, List<CompanyAddressType> companyAddressTypesDeleted)
        {
            Stopwatch stopWatch = new Stopwatch();
            ParametrizationResponse<CompanyAddressType> returnCompanyAddressTypes = new ParametrizationResponse<CompanyAddressType>();
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
                            foreach (CompanyAddressType item in companyAddressTypesAdded)
                            {
                                item.AddressTypeCode = this.GetAddressTypeCode();
                                entities.CompanyAddressType entityCompanyAddressType = EntityAssembler.CreateCompanyAddressType(item);
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
                            foreach (CompanyAddressType item in companyAddressTypesEdited)
                            {
                                PrimaryKey key = entities.CompanyAddressType.CreatePrimaryKey(item.AddressTypeCode);
                                entities.CompanyAddressType companyAddressTypeEntity = new entities.CompanyAddressType(item.AddressTypeCode);
                                companyAddressTypeEntity = (entities.CompanyAddressType)DataFacadeManager.Instance.GetDataFacade().GetObjectByPrimaryKey(key);
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
                            foreach (CompanyAddressType item in companyAddressTypesDeleted)
                            {
                                PrimaryKey key = entities.CompanyAddressType.CreatePrimaryKey(item.AddressTypeCode);
                                entities.CompanyAddressType companyAddressTypeEntity = new entities.CompanyAddressType(item.AddressTypeCode);
                                companyAddressTypeEntity = (entities.CompanyAddressType)DataFacadeManager.Instance.GetDataFacade().GetObjectByPrimaryKey(key);
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
        /// Obtiene todos los tipos de documentos datacrédito
        /// </summary>
        /// <returns>Tipos de documento datacrédito creados</returns>
        public List<CompanyAddressType> GetAllCompanyAddressType()
        {
            List<CompanyAddressType> companyAddressTypes = new List<CompanyAddressType>();
            BusinessCollection businessCollection = new BusinessCollection(DataFacadeManager.Instance.GetDataFacade().SelectObjects(typeof(entities.CompanyAddressType)));
            if (businessCollection != null && businessCollection.Count > 0)
            {
                companyAddressTypes = ModelAssembler.CreateCompanyAddressType(businessCollection);

                foreach (CompanyAddressType model in companyAddressTypes)
                {
                    model.IsForeing = false;
                }
            }

            return companyAddressTypes;
        }
    }
}
