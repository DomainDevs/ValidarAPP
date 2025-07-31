
using Sistran.Core.Application.Common.Entities;
using Sistran.Core.Application.UnderwritingServices.EEProvider.Assemblers;
using Sistran.Core.Application.UniquePerson.Entities;
using Sistran.Core.Application.Utilities.DataFacade;
using Sistran.Core.Application.UnderwritingServices.Models;
using ISS = Sistran.Core.Application.Issuance.Entities;
using Sistran.Core.Framework.DAF.Engine;
using Sistran.Core.Framework.Queries;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;
using ISSEN = Sistran.Core.Application.Issuance.Entities;

namespace Sistran.Core.Application.UnderwritingServices.EEProvider.DAOs
{
    /// <summary>
    /// contragarantías
    /// </summary>
    public class GuaranteeDAO
    {
        public List<IssuanceGuarantee> GetInsuredGuaranteesByIndividualId(int individualId)
        {
            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();
            ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();
            filter.Property(InsuredGuarantee.Properties.IndividualId, "ig");
            filter.Equal();
            filter.Constant(individualId);

            List<Models.IssuanceGuarantee> guarantee = GetInsuredGuarantees(filter);

            stopWatch.Stop();
            Debug.WriteLine(stopWatch.Elapsed.ToString(), "Sistran.Core.Application.UniquePersonService.DAOs.GetInsuredGuaranteesByIndividualId");
            return guarantee;
        }

        public List<IssuanceGuarantee> GetCounterGuaranteesByIndividualId(int individualId)
        {
            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();
            ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();
            filter.Property(InsuredGuarantee.Properties.IndividualId, "ig");
            filter.Equal();
            filter.Constant(individualId);
            SelectQuery select = new SelectQuery();
            #region Select

            //RiskSuretyGuarantee
            //select.AddSelectValue(new SelectValue(new Column(ISS.RiskSuretyGuarantee.Properties.RiskId, "rsg"), "RiskId"));
            //select.AddSelectValue(new SelectValue(new Column(ISS.RiskSuretyGuarantee.Properties.GuaranteeId, "rsg"), "GuaranteeId"));

            //InsuredGuarantee
            select.AddSelectValue(new SelectValue(new Column(InsuredGuarantee.Properties.GuaranteeId, "ig"), "GuaranteeId"));
            select.AddSelectValue(new SelectValue(new Column(InsuredGuarantee.Properties.IndividualId, "ig"), "IndividualId"));
            select.AddSelectValue(new SelectValue(new Column(InsuredGuarantee.Properties.Description, "ig"), "DescriptionInsuredGuarantee"));
            select.AddSelectValue(new SelectValue(new Column(InsuredGuarantee.Properties.CountryCode, "ig"), "CountryCode"));
            select.AddSelectValue(new SelectValue(new Column(InsuredGuarantee.Properties.StateCode, "ig"), "StateCode"));
            select.AddSelectValue(new SelectValue(new Column(InsuredGuarantee.Properties.CityCode, "ig"), "CityCode"));
            select.AddSelectValue(new SelectValue(new Column(InsuredGuarantee.Properties.Address, "ig"), "Address"));
            select.AddSelectValue(new SelectValue(new Column(InsuredGuarantee.Properties.InsuranceCompany, "ig"), "InsuranceCompany"));
            select.AddSelectValue(new SelectValue(new Column(InsuredGuarantee.Properties.InsuranceValueAmount, "ig"), "InsuranceValueAmount"));
            select.AddSelectValue(new SelectValue(new Column(InsuredGuarantee.Properties.GuaranteePolicyNumber, "ig"), "GuaranteePolicyNumber"));
            select.AddSelectValue(new SelectValue(new Column(InsuredGuarantee.Properties.GuaranteeStatusCode, "ig"), "GuaranteeStatusCode"));
            select.AddSelectValue(new SelectValue(new Column(InsuredGuarantee.Properties.RegistrationNumber, "ig"), "RegistrationNumber"));
            select.AddSelectValue(new SelectValue(new Column(InsuredGuarantee.Properties.BranchCode, "ig"), "BranchCode"));


            //Risk
            //select.AddSelectValue(new SelectValue(new Column(ISS.Risk.Properties.RiskId, "r"), "RiskId"));
            //select.AddSelectValue(new SelectValue(new Column(ISS.Risk.Properties.InsuredId, "r"), "InsuredId"));
            //select.AddSelectValue(new SelectValue(new Column(ISS.Risk.Properties.CoveredRiskTypeCode, "r"), "CoveredRiskTypeCode"));

            //Guarantee
            select.AddSelectValue(new SelectValue(new Column(Guarantee.Properties.Description, "g"), "DescriptionGuarantee"));
            select.AddSelectValue(new SelectValue(new Column(Guarantee.Properties.GuaranteeCode, "g"), "GuaranteeCode"));

            // GuaranteeType
            select.AddSelectValue(new SelectValue(new Column(GuaranteeType.Properties.Description, "gt"), "DescriptionGuaranteeType"));
            select.AddSelectValue(new SelectValue(new Column(GuaranteeType.Properties.GuaranteeTypeCode, "gt"), "GuaranteeTypeCode"));
            #endregion

            #region Join
            //Join join = new Join(new ClassNameTable(typeof(ISS.RiskSuretyGuarantee), "rsg"), new ClassNameTable(typeof(InsuredGuarantee), "ig"), JoinType.Inner);
            //join.Criteria = (new ObjectCriteriaBuilder()
            //    .Property(ISS.RiskSuretyGuarantee.Properties.GuaranteeId, "rsg")
            //    .Equal()
            //    .Property(InsuredGuarantee.Properties.GuaranteeId, "ig")
            //    .GetPredicate());

            //join = new Join(join, new ClassNameTable(typeof(ISS.Risk), "r"), JoinType.Inner);
            //join.Criteria = (new ObjectCriteriaBuilder()
            //    .Property(ISS.RiskSuretyGuarantee.Properties.RiskId, "rsg")
            //    .Equal()
            //    .Property(ISS.Risk.Properties.RiskId, "r")
            //    .GetPredicate());

            Join join = new Join(new ClassNameTable(typeof(InsuredGuarantee), "ig"), new ClassNameTable(typeof(Guarantee), "g"), JoinType.Inner);
            //join = new Join(join, new ClassNameTable(typeof(Guarantee), "g"), JoinType.Inner);
            join.Criteria = (new ObjectCriteriaBuilder()
                .Property(Guarantee.Properties.GuaranteeCode, "g")
                .Equal()
                .Property(InsuredGuarantee.Properties.GuaranteeCode, "ig")
                .GetPredicate());

            join = new Join(join, new ClassNameTable(typeof(GuaranteeType), "gt"), JoinType.Inner);
            join.Criteria = (new ObjectCriteriaBuilder()
                .Property(GuaranteeType.Properties.GuaranteeTypeCode, "gt")
                .Equal()
                .Property(Guarantee.Properties.GuaranteeTypeCode, "g")
                .GetPredicate());
            #endregion

            select.Table = join;
            select.Where = filter.GetPredicate();

            List<Models.IssuanceGuarantee> guarantee = new List<Models.IssuanceGuarantee>();

            #region EntityToModel
            using (IDataReader reader = DataFacadeManager.Instance.GetDataFacade().Select(select))
            {
                Models.IssuanceGuarantee tmpGuarantee = new Models.IssuanceGuarantee();
                Models.IssuanceInsuredGuarantee model = new Models.IssuanceInsuredGuarantee();
                while (reader.Read()){
                    model = new Models.IssuanceInsuredGuarantee();
                    tmpGuarantee = new Models.IssuanceGuarantee();

                    model.Code = (int)reader["GuaranteeCode"];
                    model.Description = (string)reader["DescriptionInsuredGuarantee"];
                    model.Id = (int)reader["GuaranteeId"];

                    tmpGuarantee.Code = (int)reader["GuaranteeCode"];
                    tmpGuarantee.Description = (string)reader["DescriptionGuarantee"] + "/" + (string)reader["DescriptionGuaranteeType"] + "/" + (int)reader["GuaranteeId"];
                    tmpGuarantee.GuaranteeType = new Models.IssuanceGuaranteeType();
                    tmpGuarantee.GuaranteeType.Code = (int)reader["GuaranteeTypeCode"];
                    tmpGuarantee.GuaranteeType.Description = (string)reader["DescriptionGuaranteeType"];
                    tmpGuarantee.InsuredGuarantee = model;
                    guarantee.Add(tmpGuarantee);
                }
                var groupGuarante = guarantee.GroupBy(x => x.InsuredGuarantee.Id).ToList();
                guarantee = new List<Models.IssuanceGuarantee>();
                foreach (var issuanceGuarantee in groupGuarante)
                {
                    guarantee.Add(issuanceGuarantee.FirstOrDefault());
                }
            }
         
            #endregion
            return guarantee;

        }
        /// <summary>
        /// Gets the insured guarantees.
        /// </summary>
        /// <param name="filter">The filter.</param>
        /// <returns></returns>
        public List<Models.IssuanceGuarantee> GetInsuredGuarantees(ObjectCriteriaBuilder filter)
        {
            SelectQuery select = new SelectQuery();

            #region Select

            select.AddSelectValue(new SelectValue(new Column(InsuredGuarantee.Properties.Address, "ig"), "Address"));
            select.AddSelectValue(new SelectValue(new Column(InsuredGuarantee.Properties.GuaranteeAmount, "ig"), "GuaranteeAmount"));
            select.AddSelectValue(new SelectValue(new Column(InsuredGuarantee.Properties.Apostille, "ig"), "Apostille"));
            select.AddSelectValue(new SelectValue(new Column(InsuredGuarantee.Properties.AppraisalAmount, "ig"), "AppraisalAmount"));
            select.AddSelectValue(new SelectValue(new Column(InsuredGuarantee.Properties.BuiltAreaQuantity, "ig"), "BuiltAreaQuantity"));
            select.AddSelectValue(new SelectValue(new Column(InsuredGuarantee.Properties.DeedNumber, "ig"), "DeedNumber"));
            select.AddSelectValue(new SelectValue(new Column(InsuredGuarantee.Properties.GuaranteeDescriptionOthers, "ig"), "GuaranteeDescriptionOthers"));
            select.AddSelectValue(new SelectValue(new Column(InsuredGuarantee.Properties.DocumentValueAmount, "ig"), "DocumentValueAmount"));
            select.AddSelectValue(new SelectValue(new Column(InsuredGuarantee.Properties.MeasureAreaQuantity, "ig"), "MeasureAreaQuantity"));
            select.AddSelectValue(new SelectValue(new Column(InsuredGuarantee.Properties.RegistrationDate, "ig"), "RegistrationDate"));
            select.AddSelectValue(new SelectValue(new Column(InsuredGuarantee.Properties.MortgagerName, "ig"), "MortgagerName"));
            select.AddSelectValue(new SelectValue(new Column(InsuredGuarantee.Properties.DepositEntity, "ig"), "DepositEntity"));
            select.AddSelectValue(new SelectValue(new Column(InsuredGuarantee.Properties.DepositDate, "ig"), "DepositDate"));
            select.AddSelectValue(new SelectValue(new Column(InsuredGuarantee.Properties.Depositor, "ig"), "Depositor"));
            select.AddSelectValue(new SelectValue(new Column(InsuredGuarantee.Properties.Constituent, "ig"), "Constituent"));
            select.AddSelectValue(new SelectValue(new Column(InsuredGuarantee.Properties.GuaranteeId, "ig"), "GuaranteeId"));
            select.AddSelectValue(new SelectValue(new Column(InsuredGuarantee.Properties.Description, "ig"), "Description"));
            select.AddSelectValue(new SelectValue(new Column(InsuredGuarantee.Properties.ClosedInd, "ig"), "ClosedInd"));
            select.AddSelectValue(new SelectValue(new Column(InsuredGuarantee.Properties.IndividualId, "ig"), "IndividualId"));
            select.AddSelectValue(new SelectValue(new Column(InsuredGuarantee.Properties.AppraisalDate, "ig"), "AppraisalDate"));
            select.AddSelectValue(new SelectValue(new Column(InsuredGuarantee.Properties.ExpertName, "ig"), "ExpertName"));
            select.AddSelectValue(new SelectValue(new Column(InsuredGuarantee.Properties.InsuranceValueAmount, "ig"), "InsuranceValueAmount"));
            select.AddSelectValue(new SelectValue(new Column(InsuredGuarantee.Properties.GuaranteePolicyNumber, "ig"), "GuaranteePolicyNumber"));
            select.AddSelectValue(new SelectValue(new Column(InsuredGuarantee.Properties.IssuerName, "ig"), "IssuerName"));
            select.AddSelectValue(new SelectValue(new Column(InsuredGuarantee.Properties.DocumentNumber, "ig"), "DocumentNumber"));
            select.AddSelectValue(new SelectValue(new Column(InsuredGuarantee.Properties.ExpDate, "ig"), "ExpDate"));
            select.AddSelectValue(new SelectValue(new Column(InsuredGuarantee.Properties.LineBusinessCode, "ig"), "LineBusinessCode"));
            select.AddSelectValue(new SelectValue(new Column(InsuredGuarantee.Properties.RegistrationNumber, "ig"), "RegistrationNumber"));
            select.AddSelectValue(new SelectValue(new Column(InsuredGuarantee.Properties.LicensePlate, "ig"), "LicensePlate"));
            select.AddSelectValue(new SelectValue(new Column(InsuredGuarantee.Properties.EngineSerNro, "ig"), "EngineSerNro"));
            select.AddSelectValue(new SelectValue(new Column(InsuredGuarantee.Properties.ChassisSerNo, "ig"), "ChassisSerNo"));
            select.AddSelectValue(new SelectValue(new Column(InsuredGuarantee.Properties.SignatoriesNum, "ig"), "SignatoriesNum"));
            select.AddSelectValue(new SelectValue(new Column(InsuredGuarantee.Properties.AssetTypeCode, "ig"), "AssetTypeCode"));

            select.AddSelectValue(new SelectValue(new Column(Guarantee.Properties.Description, "g"), "DescriptionGuarantee"));
            select.AddSelectValue(new SelectValue(new Column(Guarantee.Properties.GuaranteeCode, "g"), "GuaranteeCode"));
            select.AddSelectValue(new SelectValue(new Column(Guarantee.Properties.GuaranteeTypeCode, "g"), "GuaranteeTypeCodeGuarantee"));

            select.AddSelectValue(new SelectValue(new Column(GuaranteeType.Properties.Description, "gt"), "DescriptionGuaranteeType"));
            select.AddSelectValue(new SelectValue(new Column(GuaranteeType.Properties.GuaranteeTypeCode, "gt"), "GuaranteeTypeCodeGuaranteeType"));
            select.AddSelectValue(new SelectValue(new Column(GuaranteeType.Properties.GuaranteeClassCode, "gt"), "GuaranteeClassCode"));

            select.AddSelectValue(new SelectValue(new Column(Country.Properties.CountryCode, "co"), "CountryCode"));
            select.AddSelectValue(new SelectValue(new Column(Country.Properties.Description, "co"), "DescriptionCountry"));

            select.AddSelectValue(new SelectValue(new Column(State.Properties.StateCode, "st"), "StateCode"));
            select.AddSelectValue(new SelectValue(new Column(State.Properties.Description, "st"), "DescriptionState"));

            select.AddSelectValue(new SelectValue(new Column(City.Properties.CityCode, "ci"), "CityCode"));
            select.AddSelectValue(new SelectValue(new Column(City.Properties.Description, "ci"), "DescriptionCity"));

            select.AddSelectValue(new SelectValue(new Column(GuaranteeStatus.Properties.GuaranteeStatusCode, "grst"), "GuaranteeStatusCode"));
            select.AddSelectValue(new SelectValue(new Column(GuaranteeStatus.Properties.Description, "grst"), "DescriptionGuaranteeStatus"));
            select.AddSelectValue(new SelectValue(new Column(GuaranteeStatus.Properties.EnabledInd, "grst"), "EnabledInd"));

            select.AddSelectValue(new SelectValue(new Column(Branch.Properties.BranchCode, "br"), "BranchCode"));
            select.AddSelectValue(new SelectValue(new Column(Branch.Properties.Description, "br"), "DescriptionBranch"));

            select.AddSelectValue(new SelectValue(new Column(PromissoryNoteType.Properties.PromissoryNoteTypeCode, "pr"), "PromissoryNoteTypeCode"));
            select.AddSelectValue(new SelectValue(new Column(PromissoryNoteType.Properties.Description, "pr"), "DescriptionPromissoryNoteType"));

            //select.AddSelectValue(new SelectValue(new Column(MeasurementType.Properties.MeasurementTypeCode, "mety"), "MeasurementTypeCode"));
            //select.AddSelectValue(new SelectValue(new Column(MeasurementType.Properties.SmallDescription, "mety"), "SmallDescriptionMeasurementType"));

            select.AddSelectValue(new SelectValue(new Column(Currency.Properties.CurrencyCode, "cu"), "CurrencyCode"));
            select.AddSelectValue(new SelectValue(new Column(Currency.Properties.Description, "cu"), "DescriptionCurrency"));

            select.AddSelectValue(new SelectValue(new Column(CoInsuranceCompany.Properties.InsuranceCompanyId, "coIn"), "InsuranceCompanyId"));
            select.AddSelectValue(new SelectValue(new Column(CoInsuranceCompany.Properties.Description, "coIn"), "DescriptionCoInsuranceCompany"));
            select.AddSelectValue(new SelectValue(new Column(CoInsuranceCompany.Properties.SmallDescription, "coIn"), "SmallDescriptionCoInsuranceCompany"));
            #endregion  

            #region Join
            Join join = new Join(new ClassNameTable(typeof(InsuredGuarantee), "ig"), new ClassNameTable(typeof(Guarantee), "g"), JoinType.Inner);
            join.Criteria = (new ObjectCriteriaBuilder()
                .Property(InsuredGuarantee.Properties.GuaranteeCode, "ig")
                .Equal()
                .Property(Guarantee.Properties.GuaranteeCode, "g")
                .GetPredicate());

            join = new Join(join, new ClassNameTable(typeof(GuaranteeType), "gt"), JoinType.Inner);
            join.Criteria = (new ObjectCriteriaBuilder()
                .Property(Guarantee.Properties.GuaranteeTypeCode, "g")
                .Equal()
                .Property(GuaranteeType.Properties.GuaranteeTypeCode, "gt")
                .GetPredicate());

            join = new Join(join, new ClassNameTable(typeof(Currency), "cu"), JoinType.Left);
            join.Criteria = (new ObjectCriteriaBuilder()
                .Property(Currency.Properties.CurrencyCode, "cu")
                .Equal()
                .Property(InsuredGuarantee.Properties.CurrencyCode, "ig")
                .GetPredicate());

            join = new Join(join, new ClassNameTable(typeof(CoInsuranceCompany), "coIn"), JoinType.Left);
            join.Criteria = (new ObjectCriteriaBuilder()
                .Property(CoInsuranceCompany.Properties.InsuranceCompanyId, "coIn")
                .Equal()
                .Property(InsuredGuarantee.Properties.InsuranceCompanyId, "ig")
                .GetPredicate());

            join = new Join(join, new ClassNameTable(typeof(Country), "co"), JoinType.Left);
            join.Criteria = (new ObjectCriteriaBuilder()
                .Property(Country.Properties.CountryCode, "co")
                .Equal()
                .Property(InsuredGuarantee.Properties.CountryCode, "ig")
                .GetPredicate());

            join = new Join(join, new ClassNameTable(typeof(State), "st"), JoinType.Left);
            join.Criteria = (new ObjectCriteriaBuilder()

                .Property(State.Properties.StateCode, "st")
                .Equal()
                .Property(InsuredGuarantee.Properties.StateCode, "ig")
                .And()

                .Property(Country.Properties.CountryCode, "co")
                .Equal()
                .Property(InsuredGuarantee.Properties.CountryCode, "st")
                .GetPredicate());


            join = new Join(join, new ClassNameTable(typeof(City), "ci"), JoinType.Left);
            join.Criteria = (new ObjectCriteriaBuilder()

                .Property(City.Properties.CityCode, "ci")
                .Equal()
                .Property(InsuredGuarantee.Properties.CityCode, "ig")
                .And()

                .Property(State.Properties.StateCode, "st")
                .Equal()
                .Property(City.Properties.StateCode, "ci")
                .And()

                .Property(Country.Properties.CountryCode, "co")
                .Equal()
                .Property(City.Properties.CountryCode, "ci")
                .GetPredicate());

            join = new Join(join, new ClassNameTable(typeof(GuaranteeStatus), "grst"), JoinType.Left);
            join.Criteria = (new ObjectCriteriaBuilder()
                .Property(GuaranteeStatus.Properties.GuaranteeStatusCode, "grst")
                .Equal()
                .Property(InsuredGuarantee.Properties.GuaranteeStatusCode, "ig")
                .GetPredicate());

            join = new Join(join, new ClassNameTable(typeof(Branch), "br"), JoinType.Inner);
            join.Criteria = (new ObjectCriteriaBuilder()
                .Property(Branch.Properties.BranchCode, "br")
                .Equal()
                .Property(InsuredGuarantee.Properties.BranchCode, "ig")
                .GetPredicate());

            join = new Join(join, new ClassNameTable(typeof(PromissoryNoteType), "pr"), JoinType.Left);
            join.Criteria = (new ObjectCriteriaBuilder()
                .Property(PromissoryNoteType.Properties.PromissoryNoteTypeCode, "pr")
                .Equal()
                .Property(InsuredGuarantee.Properties.PromissoryNoteTypeCode, "ig")
                .GetPredicate());

            //join = new Join(join, new ClassNameTable(typeof(MeasurementType), "mety"), JoinType.Left);
            //join.Criteria = (new ObjectCriteriaBuilder()
            //    .Property(MeasurementType.Properties.MeasurementTypeCode, "mety")
            //    .Equal()
            //    .Property(InsuredGuarantee.Properties.MeasurementTypeCode, "ig")
            //    .GetPredicate());
            #endregion

            select.Table = join;
            select.Where = filter.GetPredicate();

            List<Models.IssuanceGuarantee> guarantee = new List<Models.IssuanceGuarantee>();

            #region EntityToModel
            using (IDataReader reader = DataFacadeManager.Instance.GetDataFacade().Select(select))
            {
                Models.IssuanceGuarantee tmpGuarantee = new Models.IssuanceGuarantee();
                Models.IssuanceInsuredGuarantee model = new Models.IssuanceInsuredGuarantee();

                while (reader.Read())
                {
                    model = new Models.IssuanceInsuredGuarantee();
                    tmpGuarantee = new Models.IssuanceGuarantee();

                    model.Code = (int)reader["GuaranteeCode"];
                    model.GuaranteeAmount = (decimal?)reader["GuaranteeAmount"];
                    model.Address = (string)reader["Address"];
                    model.AppraisalAmount = (decimal?)reader["AppraisalAmount"];
                    model.BuiltArea = (decimal?)reader["BuiltAreaQuantity"];
                    model.DeedNumber = (string)reader["DeedNumber"];
                    model.DescriptionOthers = (string)reader["GuaranteeDescriptionOthers"];
                    model.DocumentValueAmount = (decimal?)reader["DocumentValueAmount"];
                    model.MeasureArea = (decimal?)reader["MeasureAreaQuantity"];
                    model.RegistrationDate = (DateTime?)reader["RegistrationDate"];
                    model.MortgagerName = (string)reader["MortgagerName"];
                    model.DepositEntity = (string)reader["DepositEntity"];
                    model.DepositDate = (DateTime?)reader["DepositDate"];
                    model.Depositor = (string)reader["Depositor"];
                    model.Constituent = (string)reader["Constituent"];
                    model.Id = (int)reader["GuaranteeId"];
                    model.Description = (string)reader["Description"];
                    model.IsCloseInd = (bool)reader["ClosedInd"];
                    model.IndividualId = (int)reader["IndividualId"];
                    model.AppraisalDate = (DateTime?)reader["AppraisalDate"];
                    model.ExpertName = (string)reader["ExpertName"];
                    model.InsuranceAmount = (decimal?)reader["InsuranceValueAmount"];
                    model.PolicyNumber = (string)reader["GuaranteePolicyNumber"];
                    model.AssetTypeCode = (int?)reader["AssetTypeCode"];
                    model.IssuerName = (string)reader["IssuerName"];
                    model.DocumentNumber = (string)reader["DocumentNumber"];
                    model.ExpirationDate = (DateTime?)reader["ExpDate"];
                    model.BusinessLineCode = (int?)reader["LineBusinessCode"];
                    model.RegistrationNumber = (string)reader["RegistrationNumber"];
                    model.LicensePlate = (string)reader["LicensePlate"];
                    model.EngineNro = (string)reader["EngineSerNro"];
                    model.ChassisNro = (string)reader["ChassisSerNo"];
                    model.SignatoriesNumber = (int?)reader["SignatoriesNum"];

                    if (reader["CountryCode"] != null)
                    {
                        model.Country = new Sistran.Core.Application.CommonService.Models.Country();
                        model.Country.Id = (int)reader["CountryCode"];
                        model.Country.Description = (string)reader["DescriptionCountry"];
                    }


                    if (reader["StateCode"] != null)
                    {
                        model.State = new Sistran.Core.Application.CommonService.Models.State();
                        model.State.Id = (int)reader["StateCode"];
                        model.State.Description = (string)reader["DescriptionState"];
                    }

                    if (reader["CityCode"] != null)
                    {

                        model.City = new Sistran.Core.Application.CommonService.Models.City();
                        model.City.Id = (int)reader["CityCode"];
                        model.City.Description = (string)reader["DescriptionCity"];
                    }

                    if (reader["GuaranteeStatusCode"] != null)
                    {
                        model.GuaranteeStatus = new Models.IssuanceGuaranteeStatus();
                        model.GuaranteeStatus.Code = (int)reader["GuaranteeStatusCode"];
                        model.GuaranteeStatus.Description = (string)reader["DescriptionGuaranteeStatus"];
                        model.GuaranteeStatus.IsEnabledInd = (bool)reader["EnabledInd"];
                    }

                    if (reader["BranchCode"] != null)
                    {
                        model.Branch = new CommonService.Models.Branch();
                        model.Branch.Id = (int)reader["BranchCode"];
                        model.Branch.Description = (string)reader["DescriptionBranch"];
                    }

                   
                    if (reader["CurrencyCode"] != null)
                    {
                        model.Currency = new CommonService.Models.Currency();
                        model.Currency.Id = (int)reader["CurrencyCode"];
                        model.Currency.Description = (string)reader["DescriptionCurrency"];
                    }

                  

                    tmpGuarantee.Apostille = (bool)reader["Apostille"];
                    tmpGuarantee.Code = (int)reader["GuaranteeCode"];
                    tmpGuarantee.Description = (string)reader["DescriptionGuarantee"];
                    tmpGuarantee.GuaranteeType = new Models.IssuanceGuaranteeType();
                    tmpGuarantee.GuaranteeType.Code = (int)reader["GuaranteeTypeCodeGuaranteeType"];
                    tmpGuarantee.GuaranteeType.Description = (string)reader["DescriptionGuaranteeType"];
                    tmpGuarantee.InsuredGuarantee = model;
                    guarantee.Add(tmpGuarantee);
                }

            }
            #endregion


            foreach (Models.IssuanceGuarantee g in guarantee)
            {
                //g.InsuredGuarantee = g.InsuredGuarantee;
                //InsuredGuaranteeDocumentationDAO insuredGuaranteeDocumentationDAO = new InsuredGuaranteeDocumentationDAO();
                //g.InsuredGuarantee.listDocumentation = insuredGuaranteeDocumentationDAO.GetInsuredGuaranteeDocumentation(g.InsuredGuarantee.IndividualId, g.InsuredGuarantee.Id);
                //InsuredGuaranteePrefixDAO insuredGuaranteePrefixDAO = new InsuredGuaranteePrefixDAO();
                //g.InsuredGuarantee.listPrefix = insuredGuaranteePrefixDAO.GetInsuredGuaranteePrefix(g.InsuredGuarantee.IndividualId, g.InsuredGuarantee.Id);
                //if (g.InsuredGuarantee.SignatoriesNumber != null && g.InsuredGuarantee.SignatoriesNumber > 0)
                //{
                //    GuarantorDAO guarantorDao = new GuarantorDAO();
                //    g.InsuredGuarantee.Guarantors = guarantorDao.GetGuarantorsByGuaranteeId(g.InsuredGuarantee.Id);
                //}
            }

            return guarantee;
        }

        public List<Endorsement> GetPoliciesByGuaranteeId(int guaranteeId)
        {
            SelectQuery select = new SelectQuery();
            select.AddSelectValue(new SelectValue(new Column(ISSEN.Endorsement.Properties.EndorsementId, "e")));

            Join join = new Join(new ClassNameTable(typeof(ISSEN.Policy), "p"), new ClassNameTable(typeof(ISSEN.Endorsement), "e"), JoinType.Inner)
            {
                Criteria = new ObjectCriteriaBuilder()
                    .Property(ISSEN.Policy.Properties.PolicyId, "p").Equal()
                    .Property(ISSEN.Endorsement.Properties.PolicyId, "e").GetPredicate()
            };

            join = new Join(join, new ClassNameTable(typeof(ISSEN.EndorsementRisk), "er"), JoinType.Inner)
            {
                Criteria = new ObjectCriteriaBuilder()
                    .Property(ISSEN.Endorsement.Properties.EndorsementId, "e").Equal()
                    .Property(ISSEN.EndorsementRisk.Properties.EndorsementId, "er").And()

                    .Property(ISSEN.Endorsement.Properties.PolicyId, "e").Equal()
                    .Property(ISSEN.EndorsementRisk.Properties.PolicyId, "er").GetPredicate()
            };

            join = new Join(join, new ClassNameTable(typeof(ISSEN.RiskSuretyGuarantee), "rsg"), JoinType.Inner)
            {
                Criteria = new ObjectCriteriaBuilder()
                    .Property(ISSEN.RiskSuretyGuarantee.Properties.RiskId, "rsg").Equal()
                    .Property(ISSEN.EndorsementRisk.Properties.RiskId, "er").GetPredicate()
            };

            ObjectCriteriaBuilder where = new ObjectCriteriaBuilder();

            DateTime nowDateTime = DateTime.Now;

            where.Property(ISSEN.RiskSuretyGuarantee.Properties.GuaranteeId, "rsg").Equal().Constant(guaranteeId);
            where.And().Property(ISSEN.EndorsementRisk.Properties.IsCurrent, "er").Equal().Constant(1);
            where.And().Property(ISSEN.Policy.Properties.CurrentFrom, "p").LessEqual().Constant(nowDateTime);
            where.And().Property(ISSEN.Policy.Properties.CurrentTo, "p").GreaterEqual().Constant(nowDateTime);

            select.Table = join;
            select.Where = where.GetPredicate();

            List<Endorsement> listEndorsement = new List<Endorsement>();
            using (IDataReader reader = DataFacadeManager.Instance.GetDataFacade().Select(select))
            {
                while (reader.Read())
                {
                    listEndorsement.Add(new Endorsement
                    {
                        Id = (int)reader["EndorsementId"]
                    });
                }
            }

            return listEndorsement;
        }
    }
}

