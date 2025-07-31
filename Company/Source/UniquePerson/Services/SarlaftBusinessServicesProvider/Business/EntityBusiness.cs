using Sistran.Company.Application.SarlaftBusinessServices.Models;
using Sistran.Company.Application.SarlaftBusinessServicesProvider.Assemblers;
using Sistran.Company.Application.UniquePerson.Entities;
using Sistran.Core.Application.Utilities.DataFacade;
using Sistran.Core.Framework.DAF;
using Sistran.Core.Framework.Queries;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using UPCEN = Sistran.Company.Application.UniquePerson.Entities;
using UCCEN = Sistran.Core.Application.UniquePerson.Entities;
using UPCENV = Sistran.Core.Application.UniquePersonV1.Entities;


namespace Sistran.Company.Application.SarlaftBusinessServicesProvider.Business
{
    public class EntityBusiness
    {
        public List<CompanyEntity> GetCategory()
        {

            List<CompanyEntity> CompanyEntityCategorys = new List<CompanyEntity>();

             BusinessCollection businessCollection = DataFacadeManager.GetObjects(typeof(UPCENV.CategoryType));
           

            if (businessCollection.Count > 0)
                CompanyEntityCategorys = ModelAssembler.CreateEntity(businessCollection);
            else
                CompanyEntityCategorys = new List<CompanyEntity>();

            return CompanyEntityCategorys;

        }

        public List<CompanyEntity> GetAffinity()
        {

            List<CompanyEntity> CompanyEntityAffinitys = new List<CompanyEntity>();

            BusinessCollection businessCollection = DataFacadeManager.GetObjects(typeof(UPCENV.ConsanguinityType));
            
            if (businessCollection.Count > 0)
                CompanyEntityAffinitys = ModelAssembler.CreateEntity(businessCollection);
            else
                CompanyEntityAffinitys = new List<CompanyEntity>();

            return CompanyEntityAffinitys;

        }

        public List<CompanyEntity> GetRelation()
        {

            List<CompanyEntity> CompanyEntityRelations = new List<CompanyEntity>();

            BusinessCollection businessCollection = DataFacadeManager.GetObjects(typeof(UPCENV.RelationType));
           
            if (businessCollection.Count > 0)
                CompanyEntityRelations = ModelAssembler.CreateEntity(businessCollection);
            else
                CompanyEntityRelations = new List<CompanyEntity>();

            return CompanyEntityRelations;

        }

       
        public List<CompanyEntity> GetOppositor()
        {

            List<CompanyEntity> CompanyEntity = new List<CompanyEntity>();

            BusinessCollection businessCollection = DataFacadeManager.GetObjects(typeof(UPCENV.OppositorType));

            if (businessCollection.Count > 0) { 
                CompanyEntity = ModelAssembler.CreateEntity(businessCollection);
                CompanyEntity.RemoveAll(x => x.Id == 1);
            }
            else { 
                CompanyEntity = new List<CompanyEntity>();
            }

            return CompanyEntity;

        }

        public List<CompanyEntity> GetSociety()
        {

            List<CompanyEntity> CompanyEntity = new List<CompanyEntity>();

            BusinessCollection businessCollection = DataFacadeManager.GetObjects(typeof(UPCENV.SocietyType));

            if (businessCollection.Count > 0)
                CompanyEntity = ModelAssembler.CreateEntity(businessCollection);
            else
                CompanyEntity = new List<CompanyEntity>();

            return CompanyEntity;

        }

        public List<CompanyEntity> GetNationality()
        {

            List<CompanyEntity> CompanyEntity = new List<CompanyEntity>();

            BusinessCollection businessCollection = DataFacadeManager.GetObjects(typeof(UPCENV.NationalityType));

            if (businessCollection.Count > 0)
                CompanyEntity = ModelAssembler.CreateEntity(businessCollection);
            else
                CompanyEntity = new List<CompanyEntity>();

            return CompanyEntity;

        }


    }
}
