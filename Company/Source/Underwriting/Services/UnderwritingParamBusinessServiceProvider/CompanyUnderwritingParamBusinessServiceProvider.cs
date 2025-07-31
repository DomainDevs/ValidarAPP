// -----------------------------------------------------------------------
// <copyright file="CompanyUnderwritingParamBusinessServiceProvider.cs" company="SISTRAN">
// Copyright (c) SISTRAN ANDINA. All rights reserved.
// </copyright>
// <author></author>
// -----------------------------------------------------------------------

namespace Sistran.Company.Application.UnderwritingParamBusinessServiceProvider
{
    using Sistran.Company.Application.UnderwritingParamBusinessService;
    using Sistran.Company.Application.UnderwritingParamBusinessService.Model;
    using Sistran.Company.Application.UnderwritingParamBusinessServiceProvider.Assemblers;
    using Sistran.Company.Application.UnderwritingParamBusinessServiceProvider.DAOs;
    using Sistran.Core.Application.underwritingService.Enums;
    using Sistran.Core.Framework.Transactions;
    using Sistran.Company.Application.UnderwritingParamBusinessServiceProvider.Services;
    using Sistran.Core.Application.UnderwritingServices.Models;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using Sistran.Core.Application.UnderwritingParamService.Models;
    using Sistran.Core.Application.UnderwritingServices.Models.Base;
    using Sistran.Core.Framework.BAF;
    using Sistran.Company.Application.UnderwritingParamApplicationService.DTOs;
    using AutoMapper;

    /// <summary>
    /// CompanyUnderwritingParamBusinessServiceProvider. Proveedor del servicio de aplicación.
    /// </summary>
    public class CompanyUnderwritingParamBusinessServiceProvider : ICompanyUnderwritingParamBusinessService
    {
        #region VehicleType_Previsora

        public List<CompanyVehicleType> ExecuteOperationsBusinessVehicleType(List<CompanyVehicleType> vehicleTypes)
        {
            using (Transaction transaction = new Transaction())
            {
                try
                {
                    CoVehicleTypeDAO coVehicleTypeDAO = new CoVehicleTypeDAO();

                    List<CompanyVehicleType> deleteVehicleTypes = new List<CompanyVehicleType>();
                    //Core no retorna eliminados. Tomamos los Id de la lista inicial
                    deleteVehicleTypes = vehicleTypes.Where(x => x.State == (int)Status.Delete).ToList();
                    if (deleteVehicleTypes.Count > 0)
                    {
                        foreach (CompanyVehicleType item in deleteVehicleTypes)
                        {
                            coVehicleTypeDAO.DeleteCoVehicleType(item.Id);
                        }
                    }

                    //Obtener el retorno y capturar los VehicleTypes de state=insert
                    var vehicleTypesResults = ModelAssembler.CreateCompanyVehicleTypes(DelegateService.UnderwritingServices.ExecuteOperationsVehicleType(ModelAssembler.CreateVehicleTypes(vehicleTypes)));
                    List<CompanyVehicleType> createVehicleTypes = new List<CompanyVehicleType>();
                    createVehicleTypes = vehicleTypesResults.Where(x => x.State == (int)Status.Create).ToList();

                    if (createVehicleTypes.Count > 0)
                    {
                        foreach (CompanyVehicleType item in createVehicleTypes)
                        {
                            coVehicleTypeDAO.CreateCoVehicleType(item.Id);
                        }
                    }

                    transaction.Complete();
                    return vehicleTypesResults;
                }
                catch (Exception ex)
                {
                    transaction.Dispose();
                    throw new Exception("Error in ExecuteOperationsCompanyVehicleType", ex);
                }
            }
        }

        public List<CompanyVehicleType> GetBusinessVehicleTypes()
        {
            try
            {
                return ModelAssembler.CreateCompanyVehicleTypes(DelegateService.UnderwritingServices.GetVehicleTypes());
            }
            catch (Exception ex)
            {
                throw new Exception("Error in GetCompanyVehicleTypes", ex);
            }
        }

        public string GenerateFileToBusinessVehicleType(string fileName)
        {
            try
            {
                return DelegateService.UnderwritingServices.GenerateFileToVehicleType(fileName);
            }
            catch (Exception ex)
            {

                throw new Exception("Error in GenerateFileToCompanyVehicleType", ex);
            }
        }

        public string GenerateFileToBusinessVehicleBody(CompanyVehicleType vehicleTypeDTO, string fileName)
        {
            try
            {
                return DelegateService.UnderwritingServices.GenerateFileToVehicleBody(ModelAssembler.CreateVehicleType(vehicleTypeDTO), fileName);
            }
            catch (Exception ex)
            {
                throw new Exception("Error in GenerateFileToCompanyVehicleBody", ex);
            }
        }

        #endregion

        public CompanyParamMinPremiunRelation CreateApplicationMinPremiunRelation(CompanyParamMinPremiunRelation param)
        {
            var business = new Business.MinPremiunRelationBusiness();
            return business.Create(param);
        }

        public string DeleteApplicationMinPremiunRelation(CompanyParamMinPremiunRelation param)
        {
            var business = new Business.MinPremiunRelationBusiness();
            return business.Delete(param);
        }

        public List<CompanyParamMinPremiunRelation> GetApplicationMinPremiunRelation()
        {
            var business = new Business.MinPremiunRelationBusiness();
            return business.GetAll();
        }

        public List<CompanyParamMinPremiunRelation> GetApplicationMinPremiunRelationByPrefixIdAndProductName(int PrefixId, string ProductName)
        {
            var business = new Business.MinPremiunRelationBusiness();
            return business.GetByPrefixIdAndProductName(PrefixId, ProductName);
        }

        public CompanyParamMinPremiunRelation UpdateApplicationMinPremiunRelation(CompanyParamMinPremiunRelation param)
        {
            var business = new Business.MinPremiunRelationBusiness();
            return business.Update(param);
        }

        public string GenerateFileToMinPremiumRelation(string fileName)
        {
            var business = new Business.MinPremiunRelationBusiness();
            return business.GenerateExcel(fileName);
        }

        public List<CompanyParamCoverage> GetCoverageByPrefixId(int PrefixId)
        {
            var business = new Business.MinPremiunRelationBusiness();
            return business.GetCoverageByPrefixId(PrefixId);
        }

        public List<CompanyParamCoverage> GetAllMinRange()
        {
            var business = new Business.MinPremiunRelationBusiness();
            return business.GetAllMinRange();
        }
        #region Coverage

        /// <summary>
        /// CreateBusinessCoCoverageValue: metodo que inserta una cobertura
        /// </summary>
        /// <param name="companyParamCoCoverageValue"></param>
        /// <returns></returns>
        public CompanyParamCoCoverageValue CreateBusinessCoCoverageValue(CompanyParamCoCoverageValue companyParamCoCoverageValue)
        {            
            var config = Assemblers.CompanyCoreAssembler.MappcoreCoCoverageValue();
            var coreParamCoCoverageValue = Assemblers.CompanyCoreAssembler.MappCompanyCoCoverageValue(companyParamCoCoverageValue);
            var paramCoCoverageValue = DelegateService.UnderwritingServices.CreateCoCoverageValue(coreParamCoCoverageValue);

            return Assemblers.CoreCompanyAssembler.MappcoreCoCoverageValue(paramCoCoverageValue);
        }

        /// <summary>
        /// GetBusinessCoverageValueAdv: Metodo que consulta el listado de coberturas a partir de los filtros ingresados en la busqueda avanzada
        /// </summary>
        /// <param name="companyParamCoCoverageValue"></param>
        /// <returns></returns>
        public List<CompanyParamCoCoverageValue> GetBusinessCoverageValueAdv(CompanyParamCoCoverageValue companyParamCoCoverageValue)
        {
            List<CompanyParamCoCoverageValue> LisCoCoverageValue = new List<CompanyParamCoCoverageValue>();
            var coreParamCoCoverageValue = Assemblers.CompanyCoreAssembler.MappCompanyCoCoverageValue(companyParamCoCoverageValue);
            var paramCoCoverageValue = DelegateService.UnderwritingServices.GetCoCoverageValueAdv(coreParamCoCoverageValue);
            LisCoCoverageValue = Assemblers.CoreCompanyAssembler.MappCompanyCoCoverageValues(paramCoCoverageValue);
            return LisCoCoverageValue;
        }

        /// <summary>
        /// GetBusinessCoverageValueByDescription: metodo que consulta el listado de coberturas a partir de la descripcion ingresada en la busqueda simple
        /// </summary>
        /// <param name="Description"></param>
        /// <returns></returns>
        public List<CompanyParamCoCoverageValue> GetBusinessCoverageValueByPrefixId(int prefixId)
        {
            var mapper = Assemblers.CoreCompanyAssembler.MappCompanyCoCoverageValue();
            var paramCoCoverageValue = DelegateService.UnderwritingServices.GetCoCoverageValueByPrefixId(prefixId);
            return mapper.Map<List<ParamCoCoverageValue>, List<CompanyParamCoCoverageValue>>(paramCoCoverageValue);
        }

        /// <summary>
        /// GenerateFileBusinessToCoCoverageValue: Metodo que genera el archivo excel del listado de coberturas
        /// </summary>
        /// <returns></returns>
        public CompanyExcel GenerateFileBusinessToCoCoverageValue(string fileName)
        {
            var paramCoCoverageValue = DelegateService.UnderwritingServices.GenerateFileToCoCoverageValue(fileName);
            return new CompanyExcel { FileData = paramCoCoverageValue};
            
        }

        /// <summary>
        /// GetBusinessCoCoverageValue: metodo que consulta el listado completo de las coberturas
        /// </summary>
        /// <returns></returns>
        public List<CompanyParamCoCoverageValue> GetBusinessCoCoverageValue()
        {
           
            var paramCoCoverageValue = DelegateService.UnderwritingServices.GetCoCoverageValue();
            List<CompanyParamCoCoverageValue> companyParamCoCoverageValues = Assemblers.CoreCompanyAssembler.MappCompanyCoCoverageValues(paramCoCoverageValue);
            return companyParamCoCoverageValues;
        }

        /// <summary>
        /// UpdateBusinessCocoVerageValue: Metodo que actualiza la informacion de una cobertura
        /// </summary>
        /// <param name="companyParamCoCoverageValue"></param>
        /// <returns></returns>
        public CompanyParamCoCoverageValue UpdateBusinessCocoVerageValue(CompanyParamCoCoverageValue companyParamCoCoverageValue)
        {            
            var coreParamCoCoverageValue = Assemblers.CompanyCoreAssembler.MappCompanyCoCoverageValue(companyParamCoCoverageValue);
              
            var paramCoCoverageValue = DelegateService.UnderwritingServices.UpdateCoCoverageValue(coreParamCoCoverageValue);
            companyParamCoCoverageValue = Assemblers.CoreCompanyAssembler.MappcoreCoCoverageValue(paramCoCoverageValue);
            return companyParamCoCoverageValue;
        }

        /// <summary>
        /// DeleteBusinessCocoVerageValue: metodo que elimina una cobertura
        /// </summary>
        /// <param name="companyParamCoCoverageValue"></param>
        /// <returns></returns>
        public string DeleteBusinessCocoVerageValue(CompanyParamCoCoverageValue companyParamCoCoverageValue)
        {
            var coreParamCoCoverageValue = Assemblers.CompanyCoreAssembler.MappCompanyCoCoverageValue(companyParamCoCoverageValue);
            return DelegateService.UnderwritingServices.DeleteCoCoverageValue(coreParamCoCoverageValue);

        }

        public  List<CompanyParamCoverage> GetBusinessCoverageByPrefixId(int prefixId)
        {
           
            var paramCoCoverageValue = DelegateService.UnderwritingServices.GetCoverageByPrefixId(prefixId);
            return Assemblers.CoreCompanyAssembler.MappBaseCoverages(paramCoCoverageValue);
        }
        #endregion

        
        #region AllyCoverage
        public CompanyParamAllyCoverage GetBusinessAllyCoverage()
        {
            //return ModelAssembler.MappParamAllyCoverages();
            throw new Exception();
        }
        #endregion

        #region ConditionText
        public CompanyParamConditionText CreateBusinessConditiontext(CompanyParamConditionText companyConditionText)
        {
            ParamConditionText coreConditionText = new ParamConditionText();
            coreConditionText = Assemblers.CompanyCoreAssembler.MappConditionTextCore(companyConditionText);
            coreConditionText = DelegateService.UnderwritingServices.CreateBusinessConditiontext(coreConditionText);
            CompanyParamConditionText company = new CompanyParamConditionText();
            company = Assemblers.CoreCompanyAssembler.MappConditionTextCore(coreConditionText);
            return company;
        }

        public CompanyParamConditionText UpdateBusinessConditiontext(CompanyParamConditionText companyConditionText)
        {

            ParamConditionText coreConditionText = new ParamConditionText();
            coreConditionText = Assemblers.CompanyCoreAssembler.MappConditionTextCore(companyConditionText);
            coreConditionText = DelegateService.UnderwritingServices.UpdateBusinessConditiontext(coreConditionText);
            CompanyParamConditionText company = new CompanyParamConditionText();
            company= Assemblers.CoreCompanyAssembler.MappConditionTextCore(coreConditionText);
            return company;
        }

        public string DeleteBusinessConditiontext(CompanyParamConditionText companyConditionText)
        {
            ParamConditionText coreConditionText = new ParamConditionText();
            coreConditionText = Assemblers.CompanyCoreAssembler.MappConditionTextCore(companyConditionText);
            return DelegateService.UnderwritingServices.DeleteBusinessConditiontext(coreConditionText);
        }

        public List<CompanyParamConditionText> GetBusinessConditiontext()
        {
            List<ParamConditionText> listCoreConditionText = new List<ParamConditionText>();
            listCoreConditionText = DelegateService.UnderwritingServices.GetBusinessConditiontext();
            List<CompanyParamConditionText> ListCompanyParamConditionText = new List<CompanyParamConditionText>();
            foreach (ParamConditionText coreConditionText in listCoreConditionText)
            {

                CompanyParamConditionText company = new CompanyParamConditionText()
                {
                    Id = coreConditionText.Id,
                    Body = coreConditionText.Body,
                    Title = coreConditionText.Title,
                    ConditionTextLevel = coreConditionText.ConditionTextLevel,
                    ConditionTextLevelType = coreConditionText.ConditionTextLevelType
                };
                ListCompanyParamConditionText.Add(company);
            }
            return ListCompanyParamConditionText;
        }

        public List<CompanyParamConditionText> GetBusinessConditiontextByDescription( string description = "")
        {
            List<ParamConditionText> listCoreConditionText = new List<ParamConditionText>();
            listCoreConditionText = DelegateService.UnderwritingServices.GetBusinessConditiontextByDescription(0,description);
            List<CompanyParamConditionText> ListCompanyParamConditionText = new List<CompanyParamConditionText>();
            foreach (ParamConditionText coreConditionText in listCoreConditionText)
            {
                CompanyParamConditionText company = new CompanyParamConditionText()
                {
                    Id = coreConditionText.Id,
                    Body = coreConditionText.Body,
                    Title = coreConditionText.Title,
                    ConditionTextLevel = coreConditionText.ConditionTextLevel,
                    ConditionTextLevelType = coreConditionText.ConditionTextLevelType
                };
                ListCompanyParamConditionText.Add(company);
            }
            return ListCompanyParamConditionText;
        }

        public CompanyExcel GenerateFileBusinessToConditiontext(string fileName)
        {
            try
            {
                CompanyExcel companyExcel = new CompanyExcel();
                companyExcel.FileData = DelegateService.UnderwritingServices.GenerateFileBusinessToConditiontext(fileName);

                return companyExcel;
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }

        public List<CompanyParamMinPremiunRelation> GetApplicationMinPremiunRelationByDescription(string description, int id)
        {
            throw new NotImplementedException();
        }
        /// <summary>
        /// Crear una nueva cobertura aliada
        /// </summary>
        /// <param name="companyParamCoCoverageValue"></param>
        /// <returns></returns>
        public CompanyParamAllyCoverage CreateBusinessAllyCoverage(CompanyParamAllyCoverage companyParamAllyCoverage)
        {
            //Se convierte de CompanyParamAllyCoverage a ParamQueryCoverage
            var coreParamQueryCoverage = Assemblers.CompanyCoreAssembler.MappCompanyAllyCoverage(companyParamAllyCoverage);

            var paramQueryCoverage = DelegateService.UnderwritingParamServices.CreateBusinessAllyCoverage(coreParamQueryCoverage);//UpdateBusinessAllyCoverage(coreParamQueryCoverage);
            //Se convierte de ParamQuery a CompanyParam
            companyParamAllyCoverage = Assemblers.CoreCompanyAssembler.MappcoreAllyCoverage(paramQueryCoverage);
            return companyParamAllyCoverage;
        }

        /// <summary>
        /// UpdateBusinessAllycoVerage: Metodo que actualiza la informacion de una cobertura aliada
        /// </summary>
        /// <param name="companyParamCoCoverageValue"></param>
        /// <returns></returns>
        public CompanyParamAllyCoverage UpdateBusinessAllyCoverage(CompanyParamAllyCoverage companyParamAllyCoverage, CompanyParamAllyCoverage companyParamAllyCoverageold)
        {
            //Se convierte de CompanyParamAllyCoverage a ParamQueryCoverage
            var coreParamQueryCoverage = Assemblers.CompanyCoreAssembler.MappCompanyAllyCoverage(companyParamAllyCoverage);
            var coreParamQueryCoverageOld = Assemblers.CompanyCoreAssembler.MappCompanyAllyCoverage(companyParamAllyCoverageold);
            
            var paramQueryCoverage = DelegateService.UnderwritingParamServices.UpdateBusinessAllyCoverage(coreParamQueryCoverage, coreParamQueryCoverageOld);
            //Se convierte de 
            companyParamAllyCoverage = Assemblers.CoreCompanyAssembler.MappcoreAllyCoverage(paramQueryCoverage);
            return companyParamAllyCoverage;
        }

        public CompanyParamAllyCoverage DeleteBusinessAllyCoverage(CompanyParamAllyCoverage companyParamAllyCoverage)
        {
            //Se convierte de CompanyParamAllyCoverage a ParamQueryCoverage
            var coreParamQueryCoverage = Assemblers.CompanyCoreAssembler.MappCompanyAllyCoverage(companyParamAllyCoverage);

            var paramQueryCoverage = DelegateService.UnderwritingParamServices.DeleteBusinessAllyCoverage(coreParamQueryCoverage);//CreateBusinessAllyCoverage(coreParamQueryCoverage);//UpdateBusinessAllyCoverage(coreParamQueryCoverage);
            //Se convierte de ParamQuery a CompanyParam
            companyParamAllyCoverage = Assemblers.CoreCompanyAssembler.MappcoreAllyCoverage(paramQueryCoverage);
            return companyParamAllyCoverage;
        }

        public CompanyExcel GenerateFileToAllyCoverage(string fileName)
        {
            var paramCoCoverageValue = DelegateService.UnderwritingParamServices.GenerateFileBusinessToAllyCoverage(fileName);//GenerateFileToCoCoverageValue(fileName);
            return new CompanyExcel { FileData = paramCoCoverageValue };
        }

        public CompanyExcel GenerateFileToAllyCoverageList(List<CompanyParamQueryAllyCoverage> li_allyCoverage, string fileName)
        {
            var coreParamQueryAllyCoverage = Assemblers.CompanyCoreAssembler.MappCompanyQueryAllyCoverage(li_allyCoverage);

            var paramAllyCoverage = DelegateService.UnderwritingParamServices.GenerateFileBusinessToAllyCoverageList(coreParamQueryAllyCoverage, fileName);//GenerateFileToCoCoverageValue(fileName);
            return new CompanyExcel { FileData = paramAllyCoverage };
        }

        #endregion

        #region Tax

        #region TaxMethods
        public CompanyParamTax CreateBusinessTax(CompanyParamTax CompanyTax)
        {
            ParamTax coreMappedTax = Assemblers.CompanyCoreAssembler.MapTaxCompanyToCore(CompanyTax);
            ParamTax coreDelegateTax = DelegateService.UnderwritingServices.CreateTax(coreMappedTax);
            CompanyParamTax companyMapedTax = Assemblers.CoreCompanyAssembler.MapTaxCoreToCompany(coreDelegateTax);
            return companyMapedTax;
        }

        public CompanyParamTax UpdateBusinessTax(CompanyParamTax CompanyTax)
        {
           
            ParamTax coreMappedTax = Assemblers.CompanyCoreAssembler.MapTaxCompanyToCore(CompanyTax);
            ParamTax coreDelegateTax = DelegateService.UnderwritingServices.UpdateTax(coreMappedTax);
            CompanyParamTax companyMapedTax = Assemblers.CoreCompanyAssembler.MapTaxCoreToCompany(coreDelegateTax);
            return companyMapedTax;
        }

        public List<CompanyParamTax> GetBusinessTaxByDescription(string TaxDescription)
        {
            List<CompanyParamTax> companyParamTaxes =
                CoreCompanyAssembler.MapTaxesCoreTocompanyParamTaxes(DelegateService.UnderwritingServices.GetAplicationTaxByDescription(TaxDescription));
            return companyParamTaxes;
        }

        public List<CompanyParamTax> GetBusinessTaxByIdAndDescription(int taxId, string taxDescription)
        {
            List<CompanyParamTax> companyParamTaxes =
                CoreCompanyAssembler.MapTaxesCoreTocompanyParamTaxes(DelegateService.UnderwritingServices.GetTaxByIdAndDescription(taxId, taxDescription));
            return companyParamTaxes;
        }

        public string GenerateFileBusinessToTax()
        {
            throw new Exception();
        }
        #endregion

        #region TaxRate Methods

        public CompanyParamTaxRate CreateBusinessTaxRate(CompanyParamTaxRate companyTaxRate)
        {
            var mapper =  Assemblers.CompanyCoreAssembler.MappCompanyToCoreTaxRate();
            ParamTaxRate coreMappedTaxRate = Assemblers.CompanyCoreAssembler.MapTaxRateCompanyToCore(companyTaxRate);
            ParamTaxRate coreDelegateTaxRate = DelegateService.UnderwritingServices.CreateTaxRate(coreMappedTaxRate);
            CompanyParamTaxRate companyMapedTaxRate = Assemblers.CoreCompanyAssembler.MapTaxRateCoreToCompany(coreDelegateTaxRate);
            return companyMapedTaxRate;
        }

        public CompanyParamTaxRate UpdateBusinessTaxRate(CompanyParamTaxRate companyTaxRate)
        {
            ParamTaxRate coreMappedTaxRate = Assemblers.CompanyCoreAssembler.MapTaxRateCompanyToCore(companyTaxRate);
            ParamTaxRate coreDelegateTaxRate = DelegateService.UnderwritingServices.UpdateTaxRate(coreMappedTaxRate);
            CompanyParamTaxRate companyMapedTaxRate = Assemblers.CoreCompanyAssembler.MapTaxRateCoreToCompany(coreDelegateTaxRate);
            return companyMapedTaxRate;
        }

        public List<CompanyParamTaxRate> GetBusinessTaxRateByTaxId(int TaxId)
        {
            List<CompanyParamTaxRate> companyParamTaxRates =
                CoreCompanyAssembler.MapTaxRatesCoreToCompanyParamTaxRates(DelegateService.UnderwritingServices.GetTaxRatesByTaxId(TaxId));
            return companyParamTaxRates;
        }

        public CompanyParamTaxRate GetBusinessTaxRateByTaxIdbyAttributes(int taxId, int? taxConditionId, int? taxCategoryId, int? countryCode, int? stateCode, int? cityCode, int? economicActivityCode, int? prefixId, int? coverageId, int? technicalBranchId)
        {
            CompanyParamTaxRate companyParamTaxRates = new CompanyParamTaxRate();
            ParamTaxRate paramTaxRate = DelegateService.UnderwritingServices.GetBusinessTaxRateByTaxIdbyAttributes(taxId, taxConditionId, taxCategoryId, countryCode, stateCode, cityCode, economicActivityCode, prefixId, coverageId, technicalBranchId);
            if (paramTaxRate != null)
            {
                companyParamTaxRates = Assemblers.CoreCompanyAssembler.MapTaxRateCoreToCompany(paramTaxRate);
            }
            return companyParamTaxRates;
        }
        public CompanyParamTaxRate GetBusinessTaxRateById(int taxRateId)
        {
            CompanyParamTaxRate companyParamTaxRates = new CompanyParamTaxRate();
            ParamTaxRate paramTaxRate = DelegateService.UnderwritingServices.GetBusinessTaxRateById(taxRateId);
            if (paramTaxRate != null)
            {
                companyParamTaxRates = Assemblers.CoreCompanyAssembler.MapTaxRateCoreToCompany(paramTaxRate);
            }
            return companyParamTaxRates;
        }
        
        #endregion

        #region TaxCategory Methods

        public CompanyParamTaxCategory CreateBusinessTaxCategory(CompanyParamTaxCategory companyTaxCategory)
        {
            ParamTaxCategory coreMappedTaxCategory = Assemblers.CompanyCoreAssembler.MapTaxCategoryCompanyToCore(companyTaxCategory);
            ParamTaxCategory coreDelegateTaxCategory = DelegateService.UnderwritingServices.CreateTaxCategory(coreMappedTaxCategory);
            CompanyParamTaxCategory companyMapedTaxCategory = Assemblers.CoreCompanyAssembler.MapTaxCategoryCoreToCompany(coreDelegateTaxCategory);
            return companyMapedTaxCategory;
        }

        public CompanyParamTaxCategory UpdateBusinessTaxCategory(CompanyParamTaxCategory companyTaxCategory)
        {
            ParamTaxCategory coreMappedTaxCategory = Assemblers.CompanyCoreAssembler.MapTaxCategoryCompanyToCore(companyTaxCategory);
            ParamTaxCategory coreDelegateTaxCategory = DelegateService.UnderwritingServices.UpdateTaxCategory(coreMappedTaxCategory);
            CompanyParamTaxCategory companyMapedTaxCategory = Assemblers.CoreCompanyAssembler.MapTaxCategoryCoreToCompany(coreDelegateTaxCategory);
            return companyMapedTaxCategory;
        }

        public List<CompanyParamTaxCategory> GetBusinessTaxCategoriesByTaxId(int TaxId)
        {
            List<CompanyParamTaxCategory> companyParamTaxCategories =
                CoreCompanyAssembler.MapTaxCategoriesCoreToCompanyParamTaxCategories(DelegateService.UnderwritingServices.GetTaxCategoriesByTaxId(TaxId));
            return companyParamTaxCategories;
        }

        public bool DeleteBusinessTaxCategoriesByTaxId(int categoryId, int TaxId)
        {
            bool CategoriesDeleted = DelegateService.UnderwritingServices.DeleteTaxCategoriesByTaxId(categoryId, TaxId);
            return CategoriesDeleted;
        }
        #endregion

        #region TaxCondition Methods

        public CompanyParamTaxCondition CreateBusinessTaxCondition(CompanyParamTaxCondition companyTaxCondition)
        {
            ParamTaxCondition coreMappedTaxCondition = Assemblers.CompanyCoreAssembler.MapTaxConditionCompanyToCore(companyTaxCondition);
            ParamTaxCondition coreDelegateTaxCondition = DelegateService.UnderwritingServices.CreateTaxCondition(coreMappedTaxCondition);
            CompanyParamTaxCondition companyMapedTaxCondition = Assemblers.CoreCompanyAssembler.MapTaxConditionCoreToCompany(coreDelegateTaxCondition);
            return companyMapedTaxCondition;
        }

        public CompanyParamTaxCondition UpdateBusinessTaxCondition(CompanyParamTaxCondition companyTaxCondition)
        {
            ParamTaxCondition coreMappedTaxCondition = Assemblers.CompanyCoreAssembler.MapTaxConditionCompanyToCore(companyTaxCondition);
            ParamTaxCondition coreDelegateTaxCondition = DelegateService.UnderwritingServices.UpdateTaxCondition(coreMappedTaxCondition);
            CompanyParamTaxCondition companyMapedTaxCategory = Assemblers.CoreCompanyAssembler.MapTaxConditionCoreToCompany(coreDelegateTaxCondition);
            return companyMapedTaxCategory;
        }

        public List<CompanyParamTaxCondition> GetBusinessTaxConditionsByTaxId(int TaxId)
        {
            List<CompanyParamTaxCondition> companyParamTaxConditions =
                CoreCompanyAssembler.MapTaxConditionsCoreToCompanyParamTaxConditions(DelegateService.UnderwritingServices.GetTaxConditionsByTaxId(TaxId));
            return companyParamTaxConditions;
        }

        public bool DeleteBusinessTaxConditionsByTaxId(int conditionId, int TaxId)
        {
            bool ConditionsDeleted =  DelegateService.UnderwritingServices.DeleteTaxConditionsByTaxId(conditionId, TaxId);
            return ConditionsDeleted;
        }
        #endregion

        #endregion
    }
}
