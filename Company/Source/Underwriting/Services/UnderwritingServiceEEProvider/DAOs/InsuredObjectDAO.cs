using Sistran.Company.Application.UnderwritingServices.EEProvider.Assemblers;
using Sistran.Company.Application.UnderwritingServices.EEProvider.Entities.Views;
using Sistran.Company.Application.UnderwritingServices.Models;
using Sistran.Core.Application.Utilities.DataFacade;
using Sistran.Core.Framework.DAF;
using Sistran.Core.Framework.DAF.Engine;
using Sistran.Core.Framework.Queries;
using Sistran.Core.Services.UtilitiesServices.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using COMMEN = Sistran.Core.Application.Common.Entities;
using QUOEN = Sistran.Core.Application.Quotation.Entities;

namespace Sistran.Company.Application.UnderwritingServices.EEProvider.DAOs
{
    public class InsuredObjectDAO
    {
        /// <summary>
        /// Obtener Objetos Del Seguro Por Ramo Técnico
        /// </summary>
        /// <param name="lineBusinessId"></param>
        /// <returns>Objetos Del Seguro</returns>
        public List<CompanyInsuredObject> GetCompanyInsuredObjectsByLineBusinessId(int lineBusinessId)
        {
            List<CompanyInsuredObject> companyInsuredObjects = new List<CompanyInsuredObject>();
            ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();
            filter.PropertyEquals(COMMEN.LineBusiness.Properties.LineBusinessCode, typeof(COMMEN.LineBusiness).Name, lineBusinessId);

            CompanyInsuredObjectLineBusinessView insuredObjectLineBusinessView = new CompanyInsuredObjectLineBusinessView();
            ViewBuilder viewBuilder = new ViewBuilder("CompanyInsuredObjectLineBusinessView");            
            viewBuilder.Filter = filter.GetPredicate();
            DataFacadeManager.Instance.GetDataFacade().FillView(viewBuilder, insuredObjectLineBusinessView);

            if (insuredObjectLineBusinessView.InsuredObjects.Count > 0)
            {
                companyInsuredObjects = ModelAssembler.CreateCompanyInsuredObjects(insuredObjectLineBusinessView.InsuredObjects);
            }
            return companyInsuredObjects;
        }

        /// <summary>
        /// Obtener Objetos Del Seguro Por Descripción
        /// </summary>
        /// <param name="description"></param>
        /// <returns>Objetos Del Seguro</returns>
        public List<CompanyInsuredObject> GetCompanyInsuredObjectsByDescription(string description)
        {
            List<CompanyInsuredObject> companyInsuredObjects = new List<CompanyInsuredObject>();
            ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();

            filter.Property(QUOEN.InsuredObject.Properties.Description, typeof(QUOEN.InsuredObject).Name).Like().Constant("%" + description + "%");
            filter.Or();
            filter.Property(QUOEN.InsuredObject.Properties.SmallDescription, typeof(QUOEN.InsuredObject).Name).Like().Constant("%" + description + "%");

            BusinessCollection businessCollection = new BusinessCollection(DataFacadeManager.Instance.GetDataFacade().SelectObjects(typeof(QUOEN.InsuredObject), filter.GetPredicate()));

            if (businessCollection.Count > 0)
            {
                companyInsuredObjects = ModelAssembler.CreateCompanyInsuredObjects(businessCollection);
            }
            return companyInsuredObjects;
        }

        /// <summary>
        /// Crear, Actualizar Y Eliminar Objetos Del Seguro
        /// </summary>
        /// <param name="companyInsuredObjects">Objetos Del Seguro</param>
        /// <returns>Objetos Del Seguro</returns>
        public List<CompanyInsuredObject> CreateCompanyInsuredObjects(List<CompanyInsuredObject> companyInsuredObjects)
        {
            CreateCompanyInsuredObject(companyInsuredObjects.Where(x => x.ParametrizationStatus == ParametrizationStatus.Create).ToList());
            UpdateCompanyInsuredObject(companyInsuredObjects.Where(x => x.ParametrizationStatus == ParametrizationStatus.Update).ToList());
            DeleteCompanyInsuredObject(companyInsuredObjects.Where(x => x.ParametrizationStatus == ParametrizationStatus.Delete).ToList());
            
            return companyInsuredObjects;
        }

        /// <summary>
        /// Crear Objetos Del Seguro
        /// </summary>
        /// <param name="companyInsuredObjects">Objetos Del Seguro</param>
        private void CreateCompanyInsuredObject(List<CompanyInsuredObject> companyInsuredObjects)
        {
            if (companyInsuredObjects != null)
            {
                foreach (CompanyInsuredObject companyInsuredObject in companyInsuredObjects)
                {
                    try
                    {
                        QUOEN.InsuredObject entityInsuredObject = EntityAssembler.CreateInsuredObject(companyInsuredObject);
                        DataFacadeManager.Instance.GetDataFacade().InsertObject(entityInsuredObject);
                        companyInsuredObject.Id = entityInsuredObject.InsuredObjectId;
                    }
                    catch (Exception)
                    {
                        companyInsuredObject.ParametrizationStatus = ParametrizationStatus.Error;
                    }                    
                }
            }
        }

        /// <summary>
        /// Actualizar Objetos Del Seguro
        /// </summary>
        /// <param name="companyInsuredObjects">Objetos Del Seguro</param>
        private void UpdateCompanyInsuredObject(List<CompanyInsuredObject> companyInsuredObjects)
        {
            if (companyInsuredObjects != null)
            {
                foreach (CompanyInsuredObject companyInsuredObject in companyInsuredObjects)
                {
                    try
                    {
                        PrimaryKey primaryKey = QUOEN.InsuredObject.CreatePrimaryKey(companyInsuredObject.Id);
                        QUOEN.InsuredObject entityInsuredObject = (QUOEN.InsuredObject)DataFacadeManager.Instance.GetDataFacade().GetObjectByPrimaryKey(primaryKey);

                        if (entityInsuredObject != null)
                        {
                            entityInsuredObject.Description = companyInsuredObject.Description;
                            entityInsuredObject.SmallDescription = companyInsuredObject.SmallDescription;
                            entityInsuredObject.IsDeclarative = companyInsuredObject.IsDeclarative;

                            DataFacadeManager.Instance.GetDataFacade().UpdateObject(entityInsuredObject);
                        }
                    }
                    catch (Exception)
                    {
                        companyInsuredObject.ParametrizationStatus = ParametrizationStatus.Error;
                    }
                }
            }
        }

        /// <summary>
        /// Eliminar Objetos Del Seguro
        /// </summary>
        /// <param name="companyInsuredObjects">Objetos Del Seguro</param>
        private void DeleteCompanyInsuredObject(List<CompanyInsuredObject> companyInsuredObjects)
        {
            if (companyInsuredObjects != null)
            {
                foreach (CompanyInsuredObject companyInsuredObject in companyInsuredObjects)
                {
                    try
                    {
                        PrimaryKey primaryKey = QUOEN.InsuredObject.CreatePrimaryKey(companyInsuredObject.Id);
                        QUOEN.InsuredObject entityInsuredObject = (QUOEN.InsuredObject)DataFacadeManager.Instance.GetDataFacade().GetObjectByPrimaryKey(primaryKey);

                        if (entityInsuredObject != null)
                        {
                            DataFacadeManager.Instance.GetDataFacade().DeleteObject(entityInsuredObject);
                        }
                    }
                    catch (Exception)
                    {
                        companyInsuredObject.ParametrizationStatus = ParametrizationStatus.Error;
                    }
                }
            }
        }
    }
}