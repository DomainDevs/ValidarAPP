using Sistran.Core.Application.UniquePersonService.V1.Enums;
using MOUP= Sistran.Core.Application.UniquePersonService.V1.Models;
using Sistran.Core.Application.Utilities.DataFacade;
using Sistran.Core.Framework.Queries;
using System.Collections.Generic;
using Sistran.Core.Application.UniquePersonV1.Entities;
using Sistran.Core.Framework.DAF;
using Sistran.Core.Application.UniquePersonService.V1.Assemblers;

namespace Sistran.Core.Application.UniquePersonService.V1.Business
{
    public class ProspectBusiness
    {
        public List<MOUP.Person> GetPersonByDocument( string documentNumber)
        {
            var filter = new ObjectCriteriaBuilder();
            filter.Property(Prospect.Properties.IdCardNo, typeof(Prospect).Name);
            filter.Equal();
            filter.Constant(documentNumber);
            var prospectCollection = DataFacadeManager.GetObjects(typeof(Prospect), filter.GetPredicate());
            return ModelAssembler.CreatePersonProspects(prospectCollection); 
        }

        public List<MOUP.Person> GetPersonAdv(MOUP.Person person)
        {
            var filter = new ObjectCriteriaBuilder();
            bool useAdd = false;
            if (!string.IsNullOrEmpty(person.IdentificationDocument.Number))
            {
                filter.Property(Prospect.Properties.IdCardNo, typeof(Prospect).Name);
                filter.Equal();
                filter.Constant(string.IsNullOrEmpty(person.IdentificationDocument.Number));
                useAdd = true;
            }
            if (!string.IsNullOrEmpty(person.SurName))
            {
                if (useAdd)
                {
                    filter.And();
                }
                filter.Property(Prospect.Properties.Surname, typeof(Prospect).Name);
                filter.Like();
                filter.Constant(person.SurName + "%");
                useAdd = true;
            }
            if (!string.IsNullOrEmpty(person.SecondSurName))
            {
                if (useAdd)
                {
                    filter.And();
                }
                filter.Property(Prospect.Properties.MotherLastName, typeof(Prospect).Name);
                filter.Like();
                filter.Constant(person.SecondSurName + "%");
                useAdd = true;
            }
            if (!string.IsNullOrEmpty(person.FullName))
            {
                if (useAdd)
                {
                    filter.And();
                }
                filter.Property(Prospect.Properties.Name, typeof(Prospect).Name);
                filter.Like();
                filter.Constant(person.FullName + "%");
            }
            var prospectCollection = DataFacadeManager.GetObjects(typeof(Prospect), filter.GetPredicate());
            return ModelAssembler.CreatePersonProspects(prospectCollection);
        }

        public List<MOUP.Company> GetCompanyByDocument(string documentNumber)
        {
            var filter = new ObjectCriteriaBuilder();
            filter.Property(Prospect.Properties.IdCardNo, typeof(Prospect).Name);
            filter.Equal();
            filter.Constant(documentNumber);
            var prospectCollection = DataFacadeManager.GetObjects(typeof(Prospect), filter.GetPredicate());
            return ModelAssembler.CreateCompanyProspects(prospectCollection);
        }
    }
}