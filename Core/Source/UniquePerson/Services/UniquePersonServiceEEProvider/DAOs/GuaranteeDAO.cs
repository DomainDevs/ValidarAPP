
using Sistran.Core.Application.Common.Entities;
using Sistran.Core.Application.Parameters.Entities;
using Sistran.Core.Application.UniquePerson.Entities;
using Sistran.Core.Application.UniquePersonService.Assemblers;
using Sistran.Core.Application.UniquePersonService.Entities.views;
using Sistran.Core.Application.Utilities.DataFacade;
using Sistran.Core.Framework.BAF;
using Sistran.Core.Framework.Contexts;
using Sistran.Core.Framework.DAF;
using Sistran.Core.Framework.DAF.Engine;
using Sistran.Core.Framework.Queries;
using Sistran.Core.Framework.Transactions;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;

namespace Sistran.Core.Application.UniquePersonService.DAOs
{
    /// <summary>
    /// contragarantías
    /// </summary>
    public class GuaranteeDAO
    {
        /// <summary>
        /// Consulta lista de contragarantías dado un individual Id
        /// </summary>
        /// <param name="individualId"> Id del afianzado</param>
        /// <returns> Contragarantíass </returns>
        public List<Models.Guarantee> GetInsuredGuaranteesByIndividualId(int individualId)
        {
            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();
            ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();
            filter.Property(InsuredGuarantee.Properties.IndividualId, "ig");
            filter.Equal();
            filter.Constant(individualId);

            List<Models.Guarantee> guarantee = GetInsuredGuarantees(filter);

            stopWatch.Stop();
            Debug.WriteLine(stopWatch.Elapsed.ToString(), "Sistran.Core.Application.UniquePersonService.DAOs.GetInsuredGuaranteesByIndividualId");
            return guarantee;
        }

        /// <summary>
        /// Consulta una contragarantía dado su id
        /// </summary>
        /// <param name="guaranteeId"> Id de la contragarantía</param>
        /// <param name="userId"> userId</param>
        /// <returns> Contragarantía según el parámetro Id </returns>
        public Models.Guarantee GetInsuredGuaranteeByIdGuarantee(int guaranteeId)
        {
            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();
            ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();
            filter.Property(InsuredGuarantee.Properties.GuaranteeId, "ig");
            filter.Equal();
            filter.Constant(guaranteeId);

            List<Models.Guarantee> guarantee = GetInsuredGuarantees(filter);
            if (guarantee != null && guarantee.Count > 0)
            {
                stopWatch.Stop();
                Debug.WriteLine(stopWatch.Elapsed.ToString(), "Sistran.Core.Application.UniquePersonService.DAOs.GetInsuredGuaranteeByIdGuarantee");
                return guarantee.FirstOrDefault();
            }

            stopWatch.Stop();
            Debug.WriteLine(stopWatch.Elapsed.ToString(), "Sistran.Core.Application.UniquePersonService.DAOs.GetInsuredGuaranteeByIdGuarantee");
            return null;
        }

        /// <summary>
        /// Obtener lista contragarantias del asegurado
        /// </summary>
        /// <param name="individualId">individualId Asegurado</param>
        /// <returns></returns>
        public List<Models.Guarantee> GetInsuredGuaranteeByIndividualId(int individualId)
        {
            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();
            InsuredGuaranteeView view = new InsuredGuaranteeView();
            ViewBuilder builder = new ViewBuilder("InsuredGuaranteeView");
            ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();
            filter.Property(Insured.Properties.IndividualId, typeof(Insured).Name);
            filter.Equal();
            filter.Constant(individualId);
            filter.And();
            filter.Property(GuaranteeStatus.Properties.EnabledInd, typeof(GuaranteeStatus).Name);
            filter.Equal();

            builder.Filter = filter.GetPredicate();
            DataFacadeManager.Instance.GetDataFacade().FillView(builder, view);
            List<Models.Guarantee> guarantees = ModelAssembler.CreateGuarantees(view);

            stopWatch.Stop();
            Debug.WriteLine(stopWatch.Elapsed.ToString(), "Sistran.Core.Application.UniquePersonService.DAOs.GetInsuredGuaranteeByIndividualId");
            return guarantees;
        }

        /// <summary>
        /// Guarda contragarantias de un afianzado
        /// </summary>
        /// <param name="listGuarantee"> Lista Contragarantías</param>
        /// <returns> Contragarantías </returns>
        public List<Models.Guarantee> SaveInsuredGuarantees(List<Models.Guarantee> listGuarantee)
        {
            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();
            Transaction.Created += delegate (object sender, TransactionEventArgs e) { };
            using (Context.Current)
            {
                using (Transaction transaction = new Transaction())
                {
                    transaction.Completed += delegate (object sender, TransactionEventArgs e) { };
                    transaction.Disposed += delegate (object sender, TransactionEventArgs e) { };
                    try
                    {
                        int individualId = 0;
                        foreach (var guarantee in listGuarantee)
                        {
                            Models.InsuredGuarantee insuredGuarantee = guarantee.InsuredGuarantee;
                            //Guarantors
                            GuarantorDAO guarantorDAO = new GuarantorDAO();
                            guarantorDAO.SaveGuarantors(insuredGuarantee.Guarantors, insuredGuarantee.Id);
                            //Documentación
                            InsuredGuaranteeDocumentationDAO insuredGuaranteeDocumentationDAO = new InsuredGuaranteeDocumentationDAO();
                            insuredGuaranteeDocumentationDAO.SaveGuaranteeDocumentation(insuredGuarantee.listDocumentation, insuredGuarantee.Id);
                            //Prefix
                            InsuredGuaranteePrefixDAO insuredGuaranteePrefixDAO = new InsuredGuaranteePrefixDAO();
                            insuredGuaranteePrefixDAO.SaveGuaranteePrefix(insuredGuarantee.listPrefix, insuredGuarantee.Id);
                            //Bitacora
                            InsuredGuaranteeLogDAO insuredGuaranteeLogDAO = new InsuredGuaranteeLogDAO();
                            insuredGuaranteeLogDAO.SaveInsuredGuaranteLog(insuredGuarantee.InsuredGuaranteeLog.First(), insuredGuarantee.Id);
                        }

                        stopWatch.Stop();
                        Debug.WriteLine(stopWatch.Elapsed.ToString(), "Sistran.Core.Application.UniquePersonService.DAOs.SaveInsuredGuarantees");
                        transaction.Complete();
                        return GetInsuredGuaranteesByIndividualId(individualId);
                    }
                    catch (Exception ex)
                    {

                        stopWatch.Stop();
                        Debug.WriteLine(stopWatch.Elapsed.ToString(), "Sistran.Core.Application.UniquePersonService.DAOs.SaveInsuredGuarantees");
                        transaction.Dispose();
                        throw new BusinessException("Error in SaveInsuredGuarantees", ex);
                    }
                }
            }
        }

        /// <summary>
        /// Guarda contragarantia de un afianzado
        /// </summary>
        /// <param name="guarantee">Contragarantía</param>
        /// <param name="userId"> userId</param>
        /// <returns> Contragarantía </returns>
        public Models.Guarantee SaveInsuredGuarantee(Models.Guarantee guarantee)
        {
            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();
            Transaction.Created += delegate (object sender, TransactionEventArgs e) { };
            using (Context.Current)
            {
                using (Transaction transaction = new Transaction())
                {
                    transaction.Completed += delegate (object sender, TransactionEventArgs e) { };
                    transaction.Disposed += delegate (object sender, TransactionEventArgs e) { };
                    try
                    {
                        int individualId = 0;
                        Models.InsuredGuarantee insuredGuarantee = guarantee.InsuredGuarantee;
                        individualId = insuredGuarantee.IndividualId;
                        //Guarantee
                        InsuredGuaranteeDAO insuredGuaranteeDAO = new InsuredGuaranteeDAO();
                        if (insuredGuarantee.Id == 0)
                            insuredGuarantee = insuredGuaranteeDAO.CreateNewGuarantee(insuredGuarantee);
                        else
                            insuredGuarantee = insuredGuaranteeDAO.UpdateGuarantee(insuredGuarantee);

                        //Guarantors
                        GuarantorDAO guarantorDAO = new GuarantorDAO();
                        guarantorDAO.SaveGuarantors(insuredGuarantee.Guarantors, insuredGuarantee.Id);
                        //Documentación
                        InsuredGuaranteeDocumentationDAO insuredGuaranteeDocumentationDAO = new InsuredGuaranteeDocumentationDAO();
                        insuredGuaranteeDocumentationDAO.SaveGuaranteeDocumentation(insuredGuarantee.listDocumentation, insuredGuarantee.Id);
                        //Prefix
                        InsuredGuaranteePrefixDAO insuredGuaranteePrefixDAO = new InsuredGuaranteePrefixDAO();
                        insuredGuaranteePrefixDAO.SaveGuaranteePrefix(insuredGuarantee.listPrefix, insuredGuarantee.Id);
                        //Bitacora
                        InsuredGuaranteeLogDAO insuredGuaranteeLogDAO = new InsuredGuaranteeLogDAO();
                        //insuredGuaranteeLogDAO.SaveInsuredGuaranteLog(insuredGuarantee.InsuredGuaranteeLog.First(), insuredGuarantee.Id);
                        insuredGuaranteeLogDAO.SaveInsuredGuaranteLog(insuredGuarantee.InsuredGuaranteeLogObject, insuredGuarantee.Id);
                        transaction.Complete();

                        stopWatch.Stop();
                        Debug.WriteLine(stopWatch.Elapsed.ToString(), "Sistran.Core.Application.UniquePersonService.DAOs.SaveInsuredGuarantee");
                        return GetInsuredGuaranteeByIdGuarantee(insuredGuarantee.Id);
                    }
                    catch (Exception ex)
                    {

                        stopWatch.Stop();
                        Debug.WriteLine(stopWatch.Elapsed.ToString(), "Sistran.Core.Application.UniquePersonService.DAOs.SaveInsuredGuarantee");
                        transaction.Dispose();
                        throw new BusinessException("Error in SaveInsuredGuarantee", ex);
                    }
                }
            }
        }

        /// <summary>
        /// Gets the insured guarantees.
        /// </summary>
        /// <param name="filter">The filter.</param>
        /// <returns></returns>
        public List<Models.Guarantee> GetInsuredGuarantees(ObjectCriteriaBuilder filter)
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

            select.AddSelectValue(new SelectValue(new Column(MeasurementType.Properties.MeasurementTypeCode, "mety"), "MeasurementTypeCode"));
            select.AddSelectValue(new SelectValue(new Column(MeasurementType.Properties.SmallDescription, "mety"), "SmallDescriptionMeasurementType"));

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

            join = new Join(join, new ClassNameTable(typeof(MeasurementType), "mety"), JoinType.Left);
            join.Criteria = (new ObjectCriteriaBuilder()
                .Property(MeasurementType.Properties.MeasurementTypeCode, "mety")
                .Equal()
                .Property(InsuredGuarantee.Properties.MeasurementTypeCode, "ig")
                .GetPredicate());
            #endregion

            select.Table = join;
            select.Where = filter.GetPredicate();

            List<Models.Guarantee> guarantee = new List<Models.Guarantee>();

            #region EntityToModel
            using (IDataReader reader = DataFacadeManager.Instance.GetDataFacade().Select(select))
            {
                Models.Guarantee tmpGuarantee = new Models.Guarantee();
                Models.InsuredGuarantee model = new Models.InsuredGuarantee();

                while (reader.Read())
                {
                    model = new Models.InsuredGuarantee();
                    tmpGuarantee = new Models.Guarantee();

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
                        model.GuaranteeStatus = new Models.GuaranteeStatus();
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

                    if (reader["PromissoryNoteTypeCode"] != null)
                    {
                        model.PromissoryNoteType = new Models.PromissoryNoteType();
                        model.PromissoryNoteType.Id = (int)reader["PromissoryNoteTypeCode"];
                        model.PromissoryNoteType.Description = (string)reader["DescriptionPromissoryNoteType"];
                    }

                    if (reader["MeasurementTypeCode"] != null)
                    {
                        model.MeasurementType = new Models.MeasurementType();
                        model.MeasurementType.Code = (int)reader["MeasurementTypeCode"];
                        model.MeasurementType.Description = (string)reader["SmallDescriptionMeasurementType"];
                    }

                    if (reader["CurrencyCode"] != null)
                    {
                        model.Currency = new CommonService.Models.Currency();
                        model.Currency.Id = (int)reader["CurrencyCode"];
                        model.Currency.Description = (string)reader["DescriptionCurrency"];
                    }

                    if (reader["InsuranceCompanyId"] != null)
                    {
                        model.InsuranceCompany = new Models.CoInsuranceCompany();
                        model.InsuranceCompany.Description = (string)reader["DescriptionCoInsuranceCompany"];
                        model.InsuranceCompany.Id = Convert.ToInt32(reader["InsuranceCompanyId"]);
                    }
                    //var instance = new InsuredGuaranteeLogDAO();
                    //var guaranteeLogs = instance.GetInsuredGuaranteeLogs(model.IndividualId, model.Id);
                    //model.InsuredGuaranteeLog = guaranteeLogs;

                    tmpGuarantee.Apostille = (bool)reader["Apostille"];
                    tmpGuarantee.Code = (int)reader["GuaranteeCode"];
                    tmpGuarantee.Description = (string)reader["DescriptionGuarantee"];
                    tmpGuarantee.GuaranteeType = new Models.GuaranteeType();
                    tmpGuarantee.GuaranteeType.Code = (int)reader["GuaranteeTypeCodeGuaranteeType"];
                    tmpGuarantee.GuaranteeType.Description = (string)reader["DescriptionGuaranteeType"];
                    tmpGuarantee.InsuredGuarantee = model;
                    guarantee.Add(tmpGuarantee);
                }
                foreach (Models.Guarantee item in guarantee)
                {
                    var instance = new InsuredGuaranteeLogDAO();
                    var guaranteeLogs = instance.GetInsuredGuaranteeLogs(model.IndividualId, item.InsuredGuarantee.Id);
                    item.InsuredGuarantee.InsuredGuaranteeLog = guaranteeLogs;
                }
            }


            #endregion


            foreach (Models.Guarantee g in guarantee)
            {
                InsuredGuaranteeDocumentationDAO insuredGuaranteeDocumentationDAO = new InsuredGuaranteeDocumentationDAO();
                g.InsuredGuarantee.listDocumentation = insuredGuaranteeDocumentationDAO.GetInsuredGuaranteeDocumentation(g.InsuredGuarantee.IndividualId, g.InsuredGuarantee.Id);
                InsuredGuaranteePrefixDAO insuredGuaranteePrefixDAO = new InsuredGuaranteePrefixDAO();
                g.InsuredGuarantee.listPrefix = insuredGuaranteePrefixDAO.GetInsuredGuaranteePrefix(g.InsuredGuarantee.IndividualId, g.InsuredGuarantee.Id);
                if (g.InsuredGuarantee.SignatoriesNumber != null && g.InsuredGuarantee.SignatoriesNumber > 0)
                {
                    GuarantorDAO guarantorDao = new GuarantorDAO();
                    g.InsuredGuarantee.Guarantors = guarantorDao.GetGuarantorsByGuaranteeId(g.InsuredGuarantee.Id);
                }
            }

            return guarantee;
        }
        
        /// <summary>
        /// Obtener Tipos de Contragarantias
        /// </summary>
        public List<Models.GuaranteeType> GetGuaranteesTypes()
        {
            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();
            Framework.DAF.BusinessCollection businessCollection = new Framework.DAF.BusinessCollection(DataFacadeManager.Instance.GetDataFacade().SelectObjects(typeof(GuaranteeType)));

            stopWatch.Stop();
            Debug.WriteLine(stopWatch.Elapsed.ToString(), "Sistran.Core.Application.UniquePersonService.DAOs.GetGuaranteesTypes");
            return ModelAssembler.CreateGuaranteeTypes(businessCollection);
        }

        /// <summary>
        /// Obtener lista contragarantias 
        /// </summary>
        public List<Models.Guarantee> GetGuarantees()
        {
            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();
            GuaranteeView view = new GuaranteeView();
            ViewBuilder builder = new ViewBuilder("GuaranteeView");
            DataFacadeManager.Instance.GetDataFacade().FillView(builder, view);
            List<Models.Guarantee> guarantees = ModelAssembler.CreateGuarantees(view);

            stopWatch.Stop();
            Debug.WriteLine(stopWatch.Elapsed.ToString(), "Sistran.Core.Application.UniquePersonService.DAOs.GetGuarantees");
            return guarantees;
        }
    }
}

