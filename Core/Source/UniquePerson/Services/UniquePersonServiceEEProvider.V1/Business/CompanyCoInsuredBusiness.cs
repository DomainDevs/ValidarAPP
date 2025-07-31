using MOUP = Sistran.Core.Application.UniquePersonService.V1.Models;
using Sistran.Core.Application.Utilities.DataFacade;
using Sistran.Core.Framework.Queries;
using System.Collections.Generic;
using AUPE = Sistran.Core.Application.UniquePersonV1.Entities;
using SCD = Sistran.Core.Framework.DAF;
using Sistran.Core.Application.UniquePersonService.V1.Assemblers;
using System.Linq;
using System.Diagnostics;
using COMMEN = Sistran.Core.Application.Common.Entities;
using Sistran.Core.Framework.Transactions;
using System;
using Sistran.Core.Framework.DAF;
using Sistran.Core.Application.UniquePersonV1.Entities;
using System.Data;

namespace Sistran.Core.Application.UniquePersonService.V1.Business
{
    public class CompanyCoInsuredBusiness
    {
        public MOUP.CompanyCoInsured GetCompanyCoInsuredIndividualId(int IndividualId)
        {
            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();
            ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();
            
            if (IndividualId != 0)
            {
                filter.Property(COMMEN.CoInsuranceCompany.Properties.IndividualId, typeof(COMMEN.CoInsuranceCompany).Name);
                filter.Equal();
                filter.Constant(IndividualId);
            }
            var coInsuranceCompany = (COMMEN.CoInsuranceCompany)DataFacadeManager.GetObjects(typeof(COMMEN.CoInsuranceCompany), filter.GetPredicate()).FirstOrDefault();
            stopWatch.Stop();
            Debug.WriteLine(stopWatch.Elapsed.ToString(), "Sistran.Core.Application.UniquePersonService.V1.Business.GetCompanyCoInsuredIndividualId");
            if (coInsuranceCompany != null)
            {
                return ModelAssembler.CreateCoInsured(coInsuranceCompany);
            }
            else
            {
                return null;
            }                      
        }

        public MOUP.CompanyCoInsured UpdateCompanyCoInsured(MOUP.CompanyCoInsured companyCoInsured)
        {
            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();
            PrimaryKey primaryKey = COMMEN.CoInsuranceCompany.CreatePrimaryKey(companyCoInsured.InsuraceCompanyId);
            var companyInsured = (COMMEN.CoInsuranceCompany)DataFacadeManager.GetObject(primaryKey);
            
            companyInsured.Description = companyCoInsured.Description;
            companyInsured.AddressTypeCode = companyCoInsured.AddressTypeCode;
            companyInsured.Street = companyCoInsured.Street;
            companyInsured.CityCode = companyCoInsured.CityCode;
            companyInsured.StateCode = companyCoInsured.StateCode;
            companyInsured.CountryCode = companyCoInsured.CountryCode;
            companyInsured.PhoneTypeCode = companyCoInsured.PhoneTypeCode;
            companyInsured.PhoneNumber = companyCoInsured.PhoneNumber;
            companyInsured.TributaryIdNo = companyCoInsured.TributaryIdNo;
            companyInsured.EnsureInd = companyCoInsured.EnsureInd;
            companyInsured.EnteredDate = companyCoInsured.EnteredDate;
            companyInsured.ModifyDate = companyCoInsured.ModifyDate;
            companyInsured.DeclinedDate = companyCoInsured.DeclinedDate;
            companyInsured.ComDeclinedTypeCode = companyCoInsured.ComDeclinedTypeCode;
            companyInsured.Annotations = companyCoInsured.Annotations;
            
            DataFacadeManager.Update(companyInsured);
            var result = ModelAssembler.CreateCoInsured(companyInsured);
            return result;
        }

        public MOUP.CompanyCoInsured CreateCompanyCoInsured(MOUP.CompanyCoInsured companyCoInsured)
        {
            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();
            COMMEN.CoInsuranceCompany companyInsured = null;
            if (companyCoInsured != null)
            {
                companyInsured = EntityAssembler.CreateCompanyCoInsured(companyCoInsured);
                SelectQuery selectQuery = new SelectQuery();
                Function funtion = new Function(FunctionType.Max);
                funtion.AddParameter(new Column(COMMEN.CoInsuranceCompany.Properties.InsuranceCompanyId));
                selectQuery.Table = new ClassNameTable(typeof(COMMEN.CoInsuranceCompany), "InsuranceCompanyId");
                selectQuery.AddSelectValue(new SelectValue(funtion));
                using (IDataReader reader = DataFacadeManager.Instance.GetDataFacade().Select(selectQuery))
                {
                    while (reader.Read())
                    {
                        companyInsured.InsuranceCompanyId = (Convert.ToInt32(reader[0]) + 1);
                    }
                }
                DataFacadeManager.Insert(companyInsured);
                
            }
            var result = ModelAssembler.CreateCoInsured(companyInsured);
            return result;
        }
    }
}
