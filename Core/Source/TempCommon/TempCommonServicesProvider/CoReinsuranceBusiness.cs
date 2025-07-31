using Sistran.Core.Application.TempCommonServices.DTOs;
using Sistran.Core.Framework.Queries;
using System.Collections.Generic;
using UPEN = Sistran.Core.Application.UniquePerson.Entities;
using COMMEN = Sistran.Core.Application.Common.Entities;
using System.Data;
using Sistran.Core.Application.Utilities.DataFacade;
using System;

namespace Sistran.Core.Application.TempCommonServices.Provider
{
    public class CoReinsuranceBusiness
    {
        public List<IndividualDTO> GetReinsurerByDocumentNumber(CoReinsuranceSearch filter)
        {
            List<IndividualDTO> reinsurers = new List<IndividualDTO>();
            
            ObjectCriteriaBuilder criteriaBuilder = new ObjectCriteriaBuilder();
            if (filter.DocumentNumber != null && filter.DocumentNumber != "")
            {
                //criteriaBuilder.PropertyEquals(UPEN.Company.Properties.CompanyTypeCode, filter.CompanyTypeCode).And();
                criteriaBuilder.Property(UPEN.Company.Properties.TributaryIdNo, typeof(UPEN.Company).Name);
                criteriaBuilder.Like();
                criteriaBuilder.Constant(filter.DocumentNumber + "%");
            }
            else
            {
                Int32 id = 0;
                Int32.TryParse(filter.Name, out id);

                //criteriaBuilder.PropertyEquals(UPEN.Company.Properties.CompanyTypeCode, filter.CompanyTypeCode).Or();
                //criteriaBuilder.PropertyEquals(UPEN.Company.Properties.CompanyTypeCode, filter.ForeignReinsurance).And();
                if (id > 0)
                {
                    criteriaBuilder.OpenParenthesis();
                    criteriaBuilder.Property(UPEN.Company.Properties.IndividualId, typeof(UPEN.Company).Name);
                    criteriaBuilder.Like();
                    criteriaBuilder.Constant(id + "%");
                    criteriaBuilder.Or();
                    criteriaBuilder.Property(UPEN.Company.Properties.TributaryIdNo, typeof(UPEN.Company).Name);
                    criteriaBuilder.Like();
                    criteriaBuilder.Constant(id + "%");
                    criteriaBuilder.CloseParenthesis();
                }
                else
                {
                    criteriaBuilder.Property(UPEN.Company.Properties.TradeName, typeof(UPEN.Company).Name);
                    criteriaBuilder.Like();
                    criteriaBuilder.Constant(filter.Name + "%");
                }
            }
            criteriaBuilder.And();
            criteriaBuilder.OpenParenthesis();
            criteriaBuilder.Property(UPEN.Reinsurer.Properties.ReinsurerCode, typeof(UPEN.Reinsurer).Name);
            criteriaBuilder.IsNotNull();
            criteriaBuilder.Or();
            criteriaBuilder.Property(COMMEN.CoInsuranceCompany.Properties.InsuranceCompanyId, typeof(COMMEN.CoInsuranceCompany).Name);
            criteriaBuilder.IsNotNull();
            criteriaBuilder.CloseParenthesis();

            SelectQuery selectQuery = new SelectQuery();
            selectQuery.AddSelectValue(new SelectValue(new Column(UPEN.Company.Properties.IndividualId, typeof(UPEN.Company).Name), UPEN.Company.Properties.IndividualId));
            selectQuery.AddSelectValue(new SelectValue(new Column(UPEN.Company.Properties.TributaryIdNo, typeof(UPEN.Company).Name), UPEN.Company.Properties.TributaryIdNo));
            selectQuery.AddSelectValue(new SelectValue(new Column(UPEN.Company.Properties.TradeName, typeof(UPEN.Company).Name), UPEN.Company.Properties.TradeName));
            selectQuery.AddSelectValue(new SelectValue(new Column(UPEN.Company.Properties.CompanyTypeCode, typeof(UPEN.Company).Name), UPEN.Company.Properties.CompanyTypeCode));
            selectQuery.AddSelectValue(new SelectValue(new Column(UPEN.Company.Properties.TributaryIdTypeCode, typeof(UPEN.Company).Name), UPEN.Company.Properties.TributaryIdTypeCode));
            selectQuery.AddSelectValue(new SelectValue(new Column(UPEN.Individual.Properties.IndividualTypeCode, typeof(UPEN.Individual).Name), UPEN.Individual.Properties.IndividualTypeCode));

            Join join = new Join(new ClassNameTable(typeof(UPEN.Company), typeof(UPEN.Company).Name),
                new ClassNameTable(typeof(UPEN.Individual), typeof(UPEN.Individual).Name), JoinType.Inner);

            join.Criteria = (new ObjectCriteriaBuilder()
                .Property(UPEN.Company.Properties.IndividualId, typeof(UPEN.Company).Name)
                .Equal()
                .Property(UPEN.Individual.Properties.IndividualId, typeof(UPEN.Individual).Name)
                .GetPredicate());

            join = new Join(join, new ClassNameTable(typeof(UPEN.Reinsurer), typeof(UPEN.Reinsurer).Name), JoinType.Left);
            join.Criteria = (new ObjectCriteriaBuilder()
                .Property(UPEN.Individual.Properties.IndividualId, typeof(UPEN.Individual).Name)
                .Equal()
                .Property(UPEN.Reinsurer.Properties.IndividualId, typeof(UPEN.Reinsurer).Name)
                .GetPredicate());
            
            join = new Join(join, new ClassNameTable(typeof(COMMEN.CoInsuranceCompany), typeof(COMMEN.CoInsuranceCompany).Name), JoinType.Left);
            join.Criteria = (new ObjectCriteriaBuilder()
                .Property(UPEN.Individual.Properties.IndividualId, typeof(UPEN.Individual).Name)
                .Equal()
                .Property(COMMEN.CoInsuranceCompany.Properties.IndividualId, typeof(COMMEN.CoInsuranceCompany).Name)
                .GetPredicate());

            selectQuery.Table = join;
            selectQuery.Where = criteriaBuilder.GetPredicate();
            selectQuery.MaxRows = 10;

            using (IDataReader reader = DataFacadeManager.Instance.GetDataFacade().Select(selectQuery))
            {
                while (reader.Read())
                {
                    reinsurers.Add(new IndividualDTO()
                    {
                        DocumentNumber = reader[UPEN.Company.Properties.TributaryIdNo] == null ? "" : Convert.ToString(reader[UPEN.Company.Properties.TributaryIdNo]),
                        DocumentTypeId = reader[UPEN.Company.Properties.TributaryIdTypeCode] == null ? -1 : Convert.ToInt32(reader[UPEN.Company.Properties.TributaryIdTypeCode]),
                        IndividualId = reader[UPEN.Company.Properties.IndividualId] == null ? -1 : Convert.ToInt32(reader[UPEN.Company.Properties.IndividualId]),
                        IndividualTypeId = reader[UPEN.Individual.Properties.IndividualTypeCode] == null ? -1 : Convert.ToInt32(reader[UPEN.Individual.Properties.IndividualTypeCode]),
                        Name = reader[UPEN.Company.Properties.TradeName] == null ? "" : Convert.ToString(reader[UPEN.Company.Properties.TradeName])
                    });
                }
            }
            return reinsurers;
        }
    }
}
