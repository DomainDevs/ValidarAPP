// -----------------------------------------------------------------------
// <copyright file="PersonBasicDAO.cs" company="SISTRAN">
// Copyright (c) SISTRAN ANDINA. All rights reserved.
// </copyright>
// <author>David S. Niño T.</author>
// -----------------------------------------------------------------------
namespace Sistran.Company.Application.UniquePersonParamService.EEProvider.DAOs
{
    using Sistran.Company.Application.UniquePersonParamService.EEProvider.Assemblers;
    using Sistran.Company.Application.UniquePersonParamService.EEProvider.Resources;
    using Sistran.Company.Application.UniquePersonParamService.Models;
    using Sistran.Core.Application.UniquePerson.Entities;
    using Sistran.Core.Application.Issuance.Entities;
    using Sistran.Core.Application.Utilities.DataFacade;
    using Sistran.Core.Application.Utilities.Enums;
    using Sistran.Core.Application.Utilities.Error;
    using Sistran.Core.Framework.DAF;
    using Sistran.Core.Framework.Queries;
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using Sistran.Company.Application.ModelServices.Models;
    using Sistran.Core.Application.ModelServices.Enums;
    using System.Data;

    /// <summary>
    /// Clase DAO del objeto PersonBasicDAO.
    /// </summary>
    public class PersonBasicDAO
    {

        /// <summary>
        /// Buscar datos basicos de persona
        /// </summary>
        /// <param name="codePerson">Codigo de Persona</param>
        /// <param name="firstName">Primer Apellido</param>
        ///  <param name="lastName">Segundo Apellido</param>
        /// <param name="name">Nombre(s)</param>
        ///  <param name="documentNumber">número de documento</param>
        /// <returns></returns>
        public Result<List<ParamBasicPerson>, ErrorModel> GetPersonBasicByCodePersonByFirtsNameByLastNameByNameByTypeDocumentByDocumentNumber(string codePerson, string firstName, string lastName, string name, string documentNumber, string typeDocument)
        {
            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();
            List<string> errorModelListDescription = new List<string>();
            try
            {
                List<ParamBasicPerson> basicPersons = new List<ParamBasicPerson>();
                ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();
                bool filterAdd = false;
                filter.Property(Person.Properties.IdCardTypeCode, "per");
                filter.Equal();
                filter.Constant(typeDocument);
                filter.And();
                filter.Property(Person.Properties.Surname, "per");
                filter.Like();
                filter.Constant(firstName + "%");
                filter.And();
                filter.Property(Person.Properties.MotherLastName, "per");
                filter.Like();
                filter.Constant(lastName + "%");
                filter.And();
                filter.Property(Person.Properties.Name, "per");
                filter.Like();
                filter.Constant(name + "%");
                filterAdd = true;
                if (documentNumber != "")
                {
                    if (filterAdd)
                    {
                        filter.And();
                    }
                    filter.Property(Person.Properties.IdCardNo, "per");
                    filter.Equal();
                    filter.Constant(documentNumber);
                    filterAdd = true;
                }
                if (codePerson != "")
                {
                    if (filterAdd)
                    {
                        filter.And();
                    }
                    filter.Property(Person.Properties.IndividualId, "per");
                    filter.Equal();
                    filter.Constant(codePerson);
                }
                BusinessCollection businessObjects = new BusinessCollection(DataFacadeManager.Instance.GetDataFacade().SelectObjects(typeof(Person), filter.GetPredicate(), null, 1, 20, false));
                Result<List<ParamBasicPerson>, ErrorModel> lstPersonBasic = ModelAssembler.CreatePersonsBasic(businessObjects);
                if (lstPersonBasic is ResultError<List<ParamBasicPerson>, ErrorModel>)
                {
                    return lstPersonBasic;
                }
                else
                {
                    List<ParamBasicPerson> resultValue = (lstPersonBasic as ResultValue<List<ParamBasicPerson>, ErrorModel>).Value;

                    if (resultValue.Count == 0)
                    {
                        errorModelListDescription.Add(Errors.NoPersonBasicWasFound);
                        return new ResultError<List<ParamBasicPerson>, ErrorModel>(ErrorModel.CreateErrorModel(errorModelListDescription, ErrorType.NotFound, null));
                    }
                    else
                    {
                        foreach (ParamBasicPerson person in resultValue)
                        {
                            person.Beneficiary = ValidateIndividualRiskBeneficiaryByIndividualId(person.IndividualId);
                            person.Insured = ValidateIndividualRiskByIndividualId(person.IndividualId);
                            person.Policy = ValidateIndividualPolicyByIndividualId(person.IndividualId);
                            var individual = GetIndividualByIndividualId(person.IndividualId);
                        }
                        return lstPersonBasic;
                    }
                }
            }
            catch (Exception ex)
            {
                errorModelListDescription.Add(Errors.ErrorQueryPersonBasic);
                return new ResultError<List<ParamBasicPerson>, ErrorModel>(ErrorModel.CreateErrorModel(errorModelListDescription, ErrorType.TechnicalFault, ex));
            }
            finally
            {
                stopWatch.Stop();
                Debug.WriteLine(stopWatch.Elapsed.ToString(), "Sistran.Core.Application.UniquePersonParamService.DAOs");
            }
        }

        /// <summary>
        /// Buscar datos basicos de la Compañia
        /// </summary>
        /// <param name="codeCompany">Codigo de la Compañia</param>
        /// <param name="tradeName">Razon social</param>
        ///  <param name="documentNumber">número de documento</param>
        /// <returns></returns>
        public Result<List<ParamBasicCompany>, ErrorModel> GetCompanyBasicByCodeCompanyByTradeNameByTypeDocumentByTypeDocumentByDocumentNumber(string codeCompany, string tradeName, string documentNumber, string typeDocument)
        {
            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();
            List<string> errorModelListDescription = new List<string>();
            try
            {
                ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();
                bool filterAdd = false;
                filter.Property(Company.Properties.TributaryIdTypeCode, typeof(Company).Name);
                filter.Equal();
                filter.Constant(typeDocument);
                filter.And();
                filter.Property(Company.Properties.TradeName, typeof(Company).Name);
                filter.Like();
                filter.Constant("%" + tradeName + "%");
                filterAdd = true;
                if (documentNumber != "")
                {
                    if (filterAdd)
                    {
                        filter.And();
                    }
                    filter.Property(Company.Properties.TributaryIdNo, typeof(Company).Name);
                    filter.Equal();
                    filter.Constant(documentNumber);
                    filterAdd = true;
                }
                if (codeCompany != "")
                {
                    if (filterAdd)
                    {
                        filter.And();
                    }
                    filter.Property(Company.Properties.IndividualId, typeof(Company).Name);
                    filter.Equal();
                    filter.Constant(codeCompany);
                }
                BusinessCollection businessObjects = new BusinessCollection(DataFacadeManager.Instance.GetDataFacade().SelectObjects(typeof(Company), filter.GetPredicate(), null, 1, 20, false));
                Result<List<ParamBasicCompany>, ErrorModel> lstCopmanyBasic = ModelAssembler.CreateCompanyBasic(businessObjects);
                if (lstCopmanyBasic is ResultError<List<ParamBasicCompany>, ErrorModel>)
                {
                    return lstCopmanyBasic;
                }
                else
                {
                    List<ParamBasicCompany> resultValue = (lstCopmanyBasic as ResultValue<List<ParamBasicCompany>, ErrorModel>).Value;

                    if (resultValue.Count == 0)
                    {
                        errorModelListDescription.Add(Errors.NoCompanyBasicWasFound);
                        return new ResultError<List<ParamBasicCompany>, ErrorModel>(ErrorModel.CreateErrorModel(errorModelListDescription, ErrorType.NotFound, null));
                    }
                    else
                    {
                        foreach (ParamBasicCompany company in resultValue)
                        {
                            company.Beneficiary = ValidateIndividualRiskBeneficiaryByIndividualId(company.IndividualId);
                            company.Insured = ValidateIndividualRiskByIndividualId(company.IndividualId);
                            company.Policy = ValidateIndividualPolicyByIndividualId(company.IndividualId);
                            var individual = GetIndividualByIndividualId(company.IndividualId);
                            var coCompany = GetCoCompanyByIndividualId(company.IndividualId);
                            if (coCompany != null)
                            {
                                company.TypePartnership = coCompany.AssociationTypeCode;
                            }

                        }
                        return lstCopmanyBasic;
                    }
                }
            }
            catch (Exception ex)
            {
                errorModelListDescription.Add(Errors.ErrorQueryCompanyBasic);
                return new ResultError<List<ParamBasicCompany>, ErrorModel>(ErrorModel.CreateErrorModel(errorModelListDescription, ErrorType.TechnicalFault, ex));
            }
            finally
            {
                stopWatch.Stop();
                Debug.WriteLine(stopWatch.Elapsed.ToString(), "Sistran.Core.Application.UniquePersonParamService.DAOs");
            }
        }

        /// <summary>
        /// Buscar datos basicos de persona
        /// </summary>
        ///  <param name="documentNumber">número de documento</param>
        /// <returns>Modelo Result<List<ParamBasicPerson>, ErrorModel></returns>
        public Result<List<ParamBasicPerson>, ErrorModel> GetPersonBasicByByDocumentNumber(string documentNumber)
        {
            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();
            List<string> errorModelListDescription = new List<string>();
            try
            {
                ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();
                filter.Property(Person.Properties.IdCardNo, typeof(Person).Name);
                filter.Like();
                filter.Constant(documentNumber + "%");
                BusinessCollection businessCollection = new BusinessCollection();
                using (var daf = DataFacadeManager.Instance.GetDataFacade())
                {
                    businessCollection = new BusinessCollection(daf.SelectObjects(typeof(Person), filter.GetPredicate()));
                }
                Result<List<ParamBasicPerson>, ErrorModel> lstPersonBasic = ModelAssembler.CreatePersonsBasic(businessCollection);
                if (lstPersonBasic is ResultError<List<ParamBasicPerson>, ErrorModel>)
                {
                    return lstPersonBasic;
                }
                else
                {
                    List<ParamBasicPerson> resultValue = (lstPersonBasic as ResultValue<List<ParamBasicPerson>, ErrorModel>).Value;

                    if (resultValue.Count == 0)
                    {
                        errorModelListDescription.Add(Errors.NoPersonBasicWasFound);
                        return new ResultError<List<ParamBasicPerson>, ErrorModel>(ErrorModel.CreateErrorModel(errorModelListDescription, ErrorType.NotFound, null));
                    }
                    else
                    {
                        foreach (ParamBasicPerson person in resultValue)
                        {
                            person.Beneficiary = ValidateIndividualRiskBeneficiaryByIndividualId(person.IndividualId);
                            person.Insured = ValidateIndividualRiskByIndividualId(person.IndividualId);
                            person.Policy = ValidateIndividualPolicyByIndividualId(person.IndividualId);
                            var individual = GetIndividualByIndividualId(person.IndividualId);
                        }
                        return lstPersonBasic;
                    }
                }
            }
            catch (Exception ex)
            {
                errorModelListDescription.Add(Errors.ErrorQueryPersonBasic);
                return new ResultError<List<ParamBasicPerson>, ErrorModel>(ErrorModel.CreateErrorModel(errorModelListDescription, ErrorType.TechnicalFault, ex));
            }
            finally
            {
                stopWatch.Stop();
                Debug.WriteLine(stopWatch.Elapsed.ToString(), "Sistran.Core.Application.UniquePersonParamService.DAOs");
            }
        }

        /// <summary>
        /// Buscar datos basicos de la Compañia
        /// </summary>
        /// <param name="documentNumber">No. Documento de la Compañia</param>
        /// <returns>Modelo Result<List<ParamBasicCompany>, ErrorModel></returns>
        public Result<List<ParamBasicCompany>, ErrorModel> GetCompanyBasicByDocumentNumber(string documentNumber)
        {
            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();
            List<string> errorModelListDescription = new List<string>();
            try
            {
                ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();
                filter.Property(Company.Properties.TributaryIdNo, typeof(Company).Name);
                filter.Like();
                filter.Constant(documentNumber + "%");
                BusinessCollection businessCollection = new BusinessCollection();
                using (var daf = DataFacadeManager.Instance.GetDataFacade())
                {
                    businessCollection = new BusinessCollection(daf.SelectObjects(typeof(Company), filter.GetPredicate()));
                }
                Result<List<ParamBasicCompany>, ErrorModel> lstCopmanyBasic = ModelAssembler.CreateCompanyBasic(businessCollection);
                if (lstCopmanyBasic is ResultError<List<ParamBasicCompany>, ErrorModel>)
                {
                    return lstCopmanyBasic;
                }
                else
                {
                    List<ParamBasicCompany> resultValue = (lstCopmanyBasic as ResultValue<List<ParamBasicCompany>, ErrorModel>).Value;

                    if (resultValue.Count == 0)
                    {
                        errorModelListDescription.Add(Errors.NoPersonBasicWasFound);
                        return new ResultError<List<ParamBasicCompany>, ErrorModel>(ErrorModel.CreateErrorModel(errorModelListDescription, ErrorType.NotFound, null));
                    }
                    else
                    {
                        foreach (ParamBasicCompany company in resultValue)
                        {
                            company.Beneficiary = ValidateIndividualRiskBeneficiaryByIndividualId(company.IndividualId);
                            company.Insured = ValidateIndividualRiskByIndividualId(company.IndividualId);
                            company.Policy = ValidateIndividualPolicyByIndividualId(company.IndividualId);
                            var individual = GetIndividualByIndividualId(company.IndividualId);
                            var coCompany = GetCoCompanyByIndividualId(company.IndividualId);
                            if (coCompany != null)
                            {
                                company.TypePartnership = coCompany.AssociationTypeCode;
                            }
                        }
                        return lstCopmanyBasic;
                    }
                }
            }
            catch (Exception ex)
            {
                errorModelListDescription.Add(Errors.ErrorQueryCompanyBasic);
                return new ResultError<List<ParamBasicCompany>, ErrorModel>(ErrorModel.CreateErrorModel(errorModelListDescription, ErrorType.TechnicalFault, ex));
            }
            finally
            {
                stopWatch.Stop();
                Debug.WriteLine(stopWatch.Elapsed.ToString(), "Sistran.Core.Application.UniquePersonParamService.DAOs");
            }
        }

        /// <summary>
        /// Actualizar datos basicos de una Compañia
        /// </summary>
        /// <param name="paramBasicCompany">Datos basicos de compañia</param>
        /// <returns>Modelo de Servicio BasicCompanysServiceModel</returns>

        public BasicCompanysServiceModel SaveCompanyBasic(ParamBasicCompany paramBasicCompany)
        {
            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();
            List<string> errorModelListDescription = new List<string>();
            BasicCompanysServiceModel resultCompanyBasic = new BasicCompanysServiceModel();
            try
            {
                PrimaryKey key = Company.CreatePrimaryKey(paramBasicCompany.IndividualId);
                Company companyEntity = null;
                using (var daf = DataFacadeManager.Instance.GetDataFacade())
                {
                    companyEntity = (Company)daf.GetObjectByPrimaryKey(key);
                }
                if (companyEntity != null)
                {
                    companyEntity.TributaryIdNo = paramBasicCompany.DocumentNumber;
                    companyEntity.TradeName = paramBasicCompany.TradeName;

                    using (var daf = DataFacadeManager.Instance.GetDataFacade())
                    {
                        daf.UpdateObject(companyEntity);
                    }

                    PrimaryKey keyIndividual = Individual.CreatePrimaryKey(paramBasicCompany.IndividualId);
                    Individual individualEntity = null;
                    using (var daf = DataFacadeManager.Instance.GetDataFacade())
                    {
                        individualEntity = (Individual)daf.GetObjectByPrimaryKey(keyIndividual);
                    }
                    if (individualEntity != null)
                    {
                        using (var daf = DataFacadeManager.Instance.GetDataFacade())
                        {
                            daf.UpdateObject(individualEntity);
                        }
                    }
                    resultCompanyBasic.ErrorTypeService = ErrorTypeService.Ok;
                    return resultCompanyBasic;
                }
            }
            catch (Exception ex)
            {
                errorModelListDescription.Add(Errors.ErrorUpdatePerson);
                resultCompanyBasic.ErrorTypeService = ErrorTypeService.TechnicalFault;
                return resultCompanyBasic;
            }
            resultCompanyBasic.ErrorDescription = errorModelListDescription;
            resultCompanyBasic.ErrorTypeService = ErrorTypeService.TechnicalFault;
            stopWatch.Stop();
            Debug.WriteLine(stopWatch.Elapsed.ToString(), "Sistran.Core.Application.UniquePersonParamService.DAOs");
            return resultCompanyBasic;
        }

        /// <summary>
        /// Actualizar datos basicos de una Persona
        /// </summary>
        /// <param name="paramBasicPerson">Datos basicos de persona</param>
        /// <returns>Modelo de Servicio BasicCompanysServiceModel</returns>

        public BasicPersonsServiceModel SavePersonBasic(ParamBasicPerson paramBasicPerson)
        {
            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();
            List<string> errorModelListDescription = new List<string>();
            BasicPersonsServiceModel resultPersonBasic = new BasicPersonsServiceModel();
            try
            {
                PrimaryKey key = Person.CreatePrimaryKey(paramBasicPerson.IndividualId);
                Person personEntity = null;
                using (var daf = DataFacadeManager.Instance.GetDataFacade())
                {
                    personEntity = (Person)daf.GetObjectByPrimaryKey(key);
                }
                if (personEntity != null)
                {
                    personEntity.IdCardNo = paramBasicPerson.DocumentNumber;
                    personEntity.Surname = paramBasicPerson.FirstName;
                    personEntity.MotherLastName = paramBasicPerson.LastName;
                    personEntity.Name = paramBasicPerson.Name;
                    using (var daf = DataFacadeManager.Instance.GetDataFacade())
                    {
                        daf.UpdateObject(personEntity);
                    }
                    PrimaryKey keyIndividual = Individual.CreatePrimaryKey(paramBasicPerson.IndividualId);
                    Individual individualEntity = null;
                    using (var daf = DataFacadeManager.Instance.GetDataFacade())
                    {
                        individualEntity = (Individual)daf.GetObjectByPrimaryKey(keyIndividual);
                    }
                    if (individualEntity != null)
                    {
                        using (var daf = DataFacadeManager.Instance.GetDataFacade())
                        {
                            daf.UpdateObject(individualEntity);
                        }
                    }
                    resultPersonBasic.ErrorTypeService = ErrorTypeService.Ok;
                    return resultPersonBasic;
                }
            }
            catch (Exception ex)
            {
                errorModelListDescription.Add(Errors.ErrorUpdatePerson);
                resultPersonBasic.ErrorTypeService = ErrorTypeService.TechnicalFault;
                return resultPersonBasic;
            }
            resultPersonBasic.ErrorDescription = errorModelListDescription;
            resultPersonBasic.ErrorTypeService = ErrorTypeService.TechnicalFault;
            stopWatch.Stop();
            Debug.WriteLine(stopWatch.Elapsed.ToString(), "Sistran.Core.Application.UniquePersonParamService.DAOs");
            return resultPersonBasic;
        }

        /// <summary>
        /// Validar si la Persona/Compañia es actualmente un tomador de una poliza
        /// </summary>
        /// <param name="individualId">Codigo de la Persona/Compañia</param>
        /// <returns>Boolean</returns>
        public Boolean ValidateIndividualPolicyByIndividualId(int individualId)
        {
            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();
            try
            {
                ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();
                filter.Property(Policy.Properties.PolicyholderId, typeof(Policy).Name);
                filter.Equal();
                filter.Constant(individualId);
                BusinessCollection businessCollection = new BusinessCollection();
                using (var daf = DataFacadeManager.Instance.GetDataFacade())
                {
                    businessCollection = new BusinessCollection(daf.SelectObjects(typeof(Policy), filter.GetPredicate()));
                }
                if (businessCollection.Count > 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }

            }
            catch (Exception ex)
            {
                return false;
            }
            finally
            {
                stopWatch.Stop();
                Debug.WriteLine(stopWatch.Elapsed.ToString(), "Sistran.Core.Application.UniquePersonParamService.DAOs");
            }
        }

        /// <summary>
        /// Validar si la Persona/Compañia es actualmente un asegurado de una poliza
        /// </summary>
        /// <param name="individualId">Codigo de la Persona/Compañia</param>
        /// <returns>Boolean</returns>
        public Boolean ValidateIndividualRiskByIndividualId(int individualId)
        {
            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();
            try
            {
                ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();
                filter.Property(Risk.Properties.InsuredId, typeof(Risk).Name);
                filter.Equal();
                filter.Constant(individualId);
                BusinessCollection businessCollection = new BusinessCollection();
                using (var daf = DataFacadeManager.Instance.GetDataFacade())
                {
                    businessCollection = new BusinessCollection(daf.SelectObjects(typeof(Risk), filter.GetPredicate()));
                }
                if (businessCollection.Count > 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }

            }
            catch (Exception ex)
            {
                return false;
            }
            finally
            {
                stopWatch.Stop();
                Debug.WriteLine(stopWatch.Elapsed.ToString(), "Sistran.Core.Application.UniquePersonParamService.DAOs");
            }
        }

        /// <summary>
        /// Validar si la Persona/Compañia es actualmente un beneficiario de una poliza
        /// </summary>
        /// <param name="individualId">Codigo de la Persona/Compañia</param>
        /// <returns>Boolean</returns>
        public Boolean ValidateIndividualRiskBeneficiaryByIndividualId(int individualId)
        {
            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();
            try
            {
                ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();
                filter.Property(RiskBeneficiary.Properties.BeneficiaryId, typeof(RiskBeneficiary).Name);
                filter.Equal();
                filter.Constant(individualId);
                BusinessCollection businessCollection = new BusinessCollection();
                using (var daf = DataFacadeManager.Instance.GetDataFacade())
                {
                    businessCollection = new BusinessCollection(daf.SelectObjects(typeof(RiskBeneficiary), filter.GetPredicate()));
                }
                if (businessCollection.Count > 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }

            }
            catch (Exception ex)
            {
                return false;
            }
            finally
            {
                stopWatch.Stop();
                Debug.WriteLine(stopWatch.Elapsed.ToString(), "Sistran.Core.Application.UniquePersonParamService.DAOs");
            }
        }

        /// <summary>
        /// Obtener Individual
        /// </summary>
        /// <param name="individualId">Codigo del Individuo</param>
        /// <returns>Entiddad Individual</returns>
        public Individual GetIndividualByIndividualId(int individualId)
        {
            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();
            try
            {
                ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();
                filter.Property(Individual.Properties.IndividualId, typeof(Individual).Name);
                filter.Equal();
                filter.Constant(individualId);
                BusinessCollection businessCollection = new BusinessCollection();
                using (var daf = DataFacadeManager.Instance.GetDataFacade())
                {
                    businessCollection = new BusinessCollection(daf.SelectObjects(typeof(Individual), filter.GetPredicate()));
                }
                if (businessCollection.Count == 1)
                {
                    return (Individual)businessCollection[0];
                }
                return null;
            }
            catch (Exception ex)
            {
                return null;
            }
            finally
            {
                stopWatch.Stop();
                Debug.WriteLine(stopWatch.Elapsed.ToString(), "Sistran.Core.Application.UniquePersonParamService.DAOs");
            }
        }

        public CoCompany GetCoCompanyByIndividualId(int individualId)
        {
            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();
            try
            {
                ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();
                filter.Property(CoCompany.Properties.IndividualId, typeof(CoCompany).Name);
                filter.Equal();
                filter.Constant(individualId);
                BusinessCollection businessCollection = new BusinessCollection();
                using (var daf = DataFacadeManager.Instance.GetDataFacade())
                {
                    businessCollection = new BusinessCollection(daf.SelectObjects(typeof(CoCompany), filter.GetPredicate()));
                }
                if (businessCollection.Count == 1)
                {
                    return (CoCompany)businessCollection[0];
                }
                return null;
            }
            catch (Exception ex)
            {
                return null;
            }
            finally
            {
                stopWatch.Stop();
                Debug.WriteLine(stopWatch.Elapsed.ToString(), "Sistran.Core.Application.UniquePersonParamService.DAOs");
            }
        }
    }
}
