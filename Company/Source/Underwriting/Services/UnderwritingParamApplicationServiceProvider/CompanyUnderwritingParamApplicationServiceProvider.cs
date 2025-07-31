// -----------------------------------------------------------------------
// <copyright file="CompanyUnderwritingParamApplicationServiceProvider.cs" company="SISTRAN">
// Copyright (c) SISTRAN ANDINA. All rights reserved.
// </copyright>
// <author>William Martin</author>
// -----------------------------------------------------------------------


namespace Sistran.Company.Application.UnderwritingParamApplicationServiceProvider
{
    using Sistran.Company.Application.UnderwritingParamApplicationService.DTOs;
    using Sistran.Company.Application.UnderwritingParamApplicationServiceProvider.Assemblers;
    using Sistran.Company.Application.UnderwritingParamBusinessServiceProvider;
    using Sistran.Company.Application.UnderwritingParamApplicationService;
    using Sistran.Company.Application.UnderwritingParamBusinessService.Model;
    using Sistran.Company.Application.Utilities.DTO;
    using Sistran.Core.Framework.BAF;
    using System;
    using System.Collections.Generic;
    using Sistran.Company.Application.UnderwritingParamBusinessServiceProvider.Services;

    /// <summary>
    /// CompanyUnderwritingParamApplicationServiceProvider.Proveedor del servicio de aplicación.
    /// </summary>
    public class CompanyUnderwritingParamApplicationServiceProvider : ICompanyUnderwritingParamApplicationService
    {

        #region VehicleType_Previsora

        public List<VehicleTypeDTO> ExecuteOperationsApplicationVehicleType(List<VehicleTypeDTO> vehicleTypesDTO)
        {
            try
            {
                UnderwritingParamBusinessServiceProvider.CompanyUnderwritingParamBusinessServiceProvider providerBusiness = new UnderwritingParamBusinessServiceProvider.CompanyUnderwritingParamBusinessServiceProvider();
                return AplicationCompanyAssembler.CreateVehicleTypes(providerBusiness.ExecuteOperationsBusinessVehicleType(CompanyAplicationAssembler.CreateCompanyVehicleTypes(vehicleTypesDTO)));
            }
            catch (Exception ex)
            {

                throw new Exception("Error in ExecuteOperationsVehicleType", ex);
            }
        }

        public string GenerateFileToApplicationVehicleBody(VehicleTypeDTO vehicleTypeDTO, string fileName)
        {
            try
            {
                UnderwritingParamBusinessServiceProvider.CompanyUnderwritingParamBusinessServiceProvider providerBusiness = new UnderwritingParamBusinessServiceProvider.CompanyUnderwritingParamBusinessServiceProvider();
                return providerBusiness.GenerateFileToBusinessVehicleBody(CompanyAplicationAssembler.CreateCompanyVehicleType(vehicleTypeDTO), fileName);
            }
            catch (Exception ex)
            {

                throw new Exception("Error in GenerateFileToVehicleBody", ex);
            }
        }

        public string GenerateFileToApplicationVehicleType(string fileName)
        {
            try
            {
                UnderwritingParamBusinessServiceProvider.CompanyUnderwritingParamBusinessServiceProvider providerBusiness = new UnderwritingParamBusinessServiceProvider.CompanyUnderwritingParamBusinessServiceProvider();
                return providerBusiness.GenerateFileToBusinessVehicleType(fileName);
            }
            catch (Exception ex)
            {

                throw new Exception("Error in GenerateFileToVehicleType", ex);
            }
        }

        public List<VehicleTypeDTO> GetApplicationVehicleTypes()
        {
            try
            {
                UnderwritingParamBusinessServiceProvider.CompanyUnderwritingParamBusinessServiceProvider providerBusiness = new UnderwritingParamBusinessServiceProvider.CompanyUnderwritingParamBusinessServiceProvider();
                return AplicationCompanyAssembler.CreateVehicleTypes(providerBusiness.GetBusinessVehicleTypes());
            }
            catch (Exception ex)
            {

                throw new Exception("Error in GetVehicleTypes", ex);
            }
        }
        #endregion

        #region MinPremiunRelation
        public MinPremiunRelationDTO CreateApplicationMinPremiunRelation(MinPremiunRelationDTO dto)
        {
            var business = new CompanyUnderwritingParamBusinessServiceProvider();
            var companyParam = Assemblers.AplicationCompanyAssembler.Mapper(dto);
            var result = business.CreateApplicationMinPremiunRelation(companyParam);
            return Assemblers.CompanyAplicationAssembler.Mapper(result);
        }

        public MinPremiunRelationDTO DeleteApplicationMinPremiunRelation(MinPremiunRelationDTO dto)
        {
            var business = new CompanyUnderwritingParamBusinessServiceProvider();
            var companyParam = Assemblers.AplicationCompanyAssembler.Mapper(dto);
            var result = business.DeleteApplicationMinPremiunRelation(companyParam);

            if (!bool.Parse(result))
            {
                dto.ErrorDTO = new ErrorDTO { ErrorType = Utilities.Enums.ErrorType.TechnicalFault };
            }
            else
            {
                dto.ErrorDTO = new ErrorDTO { ErrorType = Utilities.Enums.ErrorType.Ok };
            }

            return dto;
        }

        public MinPremiunRelationQueryDTO GetApplicationMinPremiunRelation()
        {
            var business = new CompanyUnderwritingParamBusinessServiceProvider();
            var result = new List<CompanyParamMinPremiunRelation>();
            var response = new MinPremiunRelationQueryDTO();
            try
            {
                result = business.GetApplicationMinPremiunRelation();
                response = Assemblers.CompanyAplicationAssembler.Mapper(result);
            }
            catch (Exception e)
            {
                response.ErrorDTO = new ErrorDTO() { ErrorDescription = new List<string>(), ErrorType = Utilities.Enums.ErrorType.BusinessFault };
                response.ErrorDTO.ErrorDescription.Add(e.Message);
                if (e.InnerException != null)
                {
                    response.ErrorDTO.ErrorDescription.Add(e.InnerException.ToString());
                }
            }
            return response;
        }

        public MinPremiunRelationQueryDTO GetApplicationMinPremiunRelationByPrefixIdAndProductName(int PrefixId, string ProductName)
        {
            var business = new CompanyUnderwritingParamBusinessServiceProvider();
            var result = new List<CompanyParamMinPremiunRelation>();
            var response = new MinPremiunRelationQueryDTO();
            try
            {
                result = business.GetApplicationMinPremiunRelationByPrefixIdAndProductName(PrefixId, ProductName);
                response = Assemblers.CompanyAplicationAssembler.Mapper(result);
            }
            catch (Exception e)
            {
                response.ErrorDTO = new ErrorDTO() { ErrorDescription = new List<string>(), ErrorType = Utilities.Enums.ErrorType.BusinessFault };
                response.ErrorDTO.ErrorDescription.Add(e.Message);
                if (e.InnerException != null)
                {
                    response.ErrorDTO.ErrorDescription.Add(e.InnerException.ToString());
                }
            }
            return response;
        }

        public MinPremiunRelationDTO UpdateApplicationMinPremiunRelation(MinPremiunRelationDTO MinPremiunRelationDTO)
        {
            var business = new CompanyUnderwritingParamBusinessServiceProvider();
            var companyParam = Assemblers.AplicationCompanyAssembler.Mapper(MinPremiunRelationDTO);
            var result = business.UpdateApplicationMinPremiunRelation(companyParam);
            return Assemblers.CompanyAplicationAssembler.Mapper(result);
        }

        public string GenerateFileToMinPremiunRelation(string fileName)
        {
            var business = new CompanyUnderwritingParamBusinessServiceProvider();
            return business.GenerateFileToMinPremiumRelation(fileName);
        }

        public List<CoverageDTO> GetCoverageByPrefixId(int PrefixId)
        {
            var business = new CompanyUnderwritingParamBusinessServiceProvider();
            var result = business.GetCoverageByPrefixId(PrefixId);
            return Assemblers.CompanyAplicationAssembler.MappCoverages(result);
        }
        public List<CoverageDTO> GetAllMinRange ()
        {
            var business = new CompanyUnderwritingParamBusinessServiceProvider();
            var result = business.GetAllMinRange();
            return Assemblers.CompanyAplicationAssembler.MappCoverages(result);
        }
        #endregion

        #region Coverage

        /// <summary>
        /// CreateApplicationCoCoverageValue: metodo que inserta una cobertura nueva
        /// </summary>
        /// <param name="coCoverageValueDTO"></param>
        /// <returns></returns>
        public CoCoverageValueDTO CreateApplicationCoCoverageValue(CoCoverageValueDTO coCoverageValueDTO)
        {
            try
            {
                CompanyParamCoCoverageValue companyParamCoCoverageValue = Assemblers.AplicationCompanyAssembler.MappCompanyParamCoCoverageValue(coCoverageValueDTO);
                var companyBusinessProvider = new UnderwritingParamBusinessServiceProvider.CompanyUnderwritingParamBusinessServiceProvider();
                companyBusinessProvider.CreateBusinessCoCoverageValue(companyParamCoCoverageValue);
                coCoverageValueDTO = Assemblers.CompanyAplicationAssembler.MappCoCoverageValueQuery(companyParamCoCoverageValue);
                coCoverageValueDTO.Error = new ErrorDTO { ErrorType = Utilities.Enums.ErrorType.Ok };
                return coCoverageValueDTO;
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message);
            }
        }

        /// <summary>
        /// DeleteApplicationCoCoverageValue: Metodo que elimina el registro de una cobertura
        /// </summary>
        /// <param name="coCoverageValueDTO"></param>
        /// <returns></returns>
        public CoCoverageValueDTO DeleteApplicationCoCoverageValue(CoCoverageValueDTO coCoverageValueDTO)
        {
            try
            {
                CompanyParamCoCoverageValue companyParamCoCoverageValue = Assemblers.AplicationCompanyAssembler.MappCompanyParamCoCoverageValue(coCoverageValueDTO);
                var companyBusinessProvider = new UnderwritingParamBusinessServiceProvider.CompanyUnderwritingParamBusinessServiceProvider();
                var resutDel = companyBusinessProvider.DeleteBusinessCocoVerageValue(companyParamCoCoverageValue);
                if (resutDel.Equals("Ok"))
                    coCoverageValueDTO.Error = new ErrorDTO { ErrorType = Utilities.Enums.ErrorType.Ok };
                else
                    coCoverageValueDTO.Error = new ErrorDTO { ErrorType = Utilities.Enums.ErrorType.BusinessFault };
                return coCoverageValueDTO;
            }
            catch (Exception)
            {
                coCoverageValueDTO.Error = new ErrorDTO { ErrorType = Utilities.Enums.ErrorType.BusinessFault };
                return coCoverageValueDTO;
            }
        }

        /// <summary>
        /// GenerateFileApplicationToCoCoverage: netodo que genera el archivo excel del listado de coberturas
        /// </summary>
        /// <returns></returns>
        public ExcelFileDTO GenerateFileApplicationToCoCoverage(string fileName)
        {
            var companyBusinessProvider = new UnderwritingParamBusinessServiceProvider.CompanyUnderwritingParamBusinessServiceProvider();
            var result = companyBusinessProvider.GenerateFileBusinessToCoCoverageValue(fileName);
            return Assemblers.CompanyAplicationAssembler.MappExcelFile(result);

        }


        /// <summary>
        /// GetApplicationCoCoverageValue: metodo que consulta el listado completo de las coberturas
        /// </summary>
        /// <param name="coCoverageValueDTO"></param>
        /// <returns></returns>
        public CoCoverageValueQueryDTO GetApplicationCoCoverageValue()
        {
            try
            {
                CoCoverageValueQueryDTO coCoverageValueQueryDTO = new CoCoverageValueQueryDTO();
                CompanyParamCoCoverageValue companyParamCoCoverageValue = new CompanyParamCoCoverageValue();

                var companyBusinessProvider = new UnderwritingParamBusinessServiceProvider.CompanyUnderwritingParamBusinessServiceProvider();
                var result = companyBusinessProvider.GetBusinessCoCoverageValue();
                coCoverageValueQueryDTO.CoCoverageValue = Assemblers.CompanyAplicationAssembler.MappCoCoverageValues(result);
                return coCoverageValueQueryDTO;
            }
            catch (Exception e)
            {
                throw new BusinessException(e.Message);
            }
        }

        /// <summary>
        /// GetApplicationCoCoverageValueAdv: metodo que consulta el listado de coberturas a partir de los filtros ingresados en la busqueda avanzada
        /// </summary>
        /// <param name="coCoverageValueDTO"></param>
        /// <returns></returns>
        public CoCoverageValueQueryDTO GetApplicationCoCoverageValueAdv(CoCoverageValueDTO coCoverageValueDTO)
        {
            try
            {
                CoCoverageValueQueryDTO coCoverageValueQueryDTO = new CoCoverageValueQueryDTO();
                CompanyParamCoCoverageValue companyParamCoCoverageValue = new CompanyParamCoCoverageValue();
                companyParamCoCoverageValue = Assemblers.AplicationCompanyAssembler.MappCompanyParamCoCoverageValue(coCoverageValueDTO);
                var companyBusinessProvider = new UnderwritingParamBusinessServiceProvider.CompanyUnderwritingParamBusinessServiceProvider();
                coCoverageValueQueryDTO.CoCoverageValue = Assemblers.CompanyAplicationAssembler.MappCoCoverageValues(companyBusinessProvider.GetBusinessCoverageValueAdv(companyParamCoCoverageValue));
                return coCoverageValueQueryDTO;
            }
            catch (Exception e)
            {
                throw new BusinessException(e.Message);
            }
        }

        /// <summary>
        /// GetApplicationCoCoverageValueByDescription: metodo que consulta el listado de coberturas a partir de la descripcion 
        /// </summary>
        /// <param name="Description"></param>
        /// <returns></returns>
        public CoCoverageValueQueryDTO GetApplicationCoCoverageValueByPrefixId(int prefixId)
        {
            try
            {
                CoCoverageValueQueryDTO coCoverageValueQueryDTO = new CoCoverageValueQueryDTO();

                var companyBusinessProvider = new UnderwritingParamBusinessServiceProvider.CompanyUnderwritingParamBusinessServiceProvider();
                coCoverageValueQueryDTO.CoCoverageValue = Assemblers.CompanyAplicationAssembler.MappCoCoverageValues(companyBusinessProvider.GetBusinessCoverageValueByPrefixId(prefixId));
                return coCoverageValueQueryDTO;
            }
            catch (Exception e)
            {
                throw new BusinessException(e.Message);
            }

        }

        /// <summary>
        /// UpdateApplicationCoCoverageValue: metodo que actualiza la informacion de una cobertura
        /// </summary>
        /// <param name="coCoverageValueDTO"></param>
        /// <returns></returns>
        public CoCoverageValueDTO UpdateApplicationCoCoverageValue(CoCoverageValueDTO coCoverageValueDTO)
        {
            CompanyParamCoCoverageValue companyParamCoCoverageValue = Assemblers.AplicationCompanyAssembler.MappCompanyParamCoCoverageValue(coCoverageValueDTO);
            var companyBusinessProvider = new UnderwritingParamBusinessServiceProvider.CompanyUnderwritingParamBusinessServiceProvider();
            companyBusinessProvider.UpdateBusinessCocoVerageValue(companyParamCoCoverageValue);
            coCoverageValueDTO = Assemblers.CompanyAplicationAssembler.MappCoCoverageValueQuery(companyParamCoCoverageValue);
            coCoverageValueDTO.Error = new ErrorDTO { ErrorType = Utilities.Enums.ErrorType.Ok };
            return coCoverageValueDTO;
        }

        /// <summary>
        /// GetApplicationCoverageByPrefixId: Metodo que consulta listado de coberturas por prefixId
        /// </summary>
        /// <param name="prefixId"></param>
        /// <returns></returns>
        public List<CoverageDTO> GetApplicationCoverageByPrefixId(int prefixId)
        {
            try
            {
                List<CoverageDTO> CoverageDTOs = new List<CoverageDTO>();

                var companyBusinessProvider = new UnderwritingParamBusinessServiceProvider.CompanyUnderwritingParamBusinessServiceProvider();
                CoverageDTOs = Assemblers.CompanyAplicationAssembler.MappCoverages(companyBusinessProvider.GetBusinessCoverageByPrefixId(prefixId));
                return CoverageDTOs;
            }
            catch (Exception e)
            {
                throw new BusinessException(e.Message);
            }

        }

        #endregion

        #region AllyCoverage

        public AllyCoverageQueryDTO GetAplicationAllyCoverage()
        {
            try
            {

                CompanyUnderwritingParamBusinessServiceProvider providerBusiness = new CompanyUnderwritingParamBusinessServiceProvider();


                return null;//providerBusiness.Get
                            //AplicationCompanyAssembler.CreateVehicleTypes(
                            //    providerBusiness.ExecuteOperationsBusinessVehicleType(
                            //        CompanyAplicationAssembler.CreateCompanyVehicleTypes(vehicleTypesDTO))); 

            }
            catch (Exception)
            {
                throw;
            }
            //AllyCoverageQueryDTO allyCoverageQueryDTO = new AllyCoverageQueryDTO();
            //List<string> errorDescriptions = new List<string>();
            //try
            //{
            //    UnderwritingParamBusinessService providerBusiness = new UnderwritingParamBusinessService();
            //    List<CompanyParamAllyCoverage> coverages = providerBusiness.GetBusinessAllyCoverage();
            //    List<AllyCoverageDTO> coveragesDTO = Assemblers.CompanyAplicationAssembler.MappParamAllyCoverages(coverages);
            //    allyCoverageQueryDTO.AllyCoverageDTO = coveragesDTO;
            //    allyCoverageQueryDTO.ErrorDTO = new ErrorDTO { ErrorType = Utilities.Enums.ErrorType.Ok };
            //    sreturn allyCoverageQueryDTO;
            //}
            //catch (Exception ex)
            //{
            //    throw new Exception(string.Format("Application Service Error. {0}", ex.Message));
            //}
        }

        public AllyCoverageQueryDTO GetAplicationAllyCoverageAdv(AllyCoverageQueryDTO allyCoverageQueryDTO)
        {
            throw new System.NotImplementedException();
        }

        //public AllyCoverageQueryDTO GetAplicationAllyCoverageAdv(AllyCoverageDTO allyCoverage)
        //{
        //    throw new Exception();
        //}

        public AllyCoverageQueryDTO GetAplicationAllyCoverageByDescription(string data, int num)
        {
            throw new Exception();
        }

        public ExcelFileDTO GenerateFileAplicationToAllyCoverage(string fileName)
        {
            var companyBusinessProvider = new UnderwritingParamBusinessServiceProvider.CompanyUnderwritingParamBusinessServiceProvider();
            var result = companyBusinessProvider.GenerateFileToAllyCoverage(fileName);//GenerateFileBusinessToCoCoverageValue(fileName);
            return Assemblers.CompanyAplicationAssembler.MappExcelFile(result);

            //CompanyUnderwritingParamBusinessServiceProvider providerBusiness = new CompanyUnderwritingParamBusinessServiceProvider();
            //throw new Exception();
            //try
            //{
            //    var companyExcel = DelegateService.provider.GenerateFileToCity(name);
            //    return CompanyAplicationAssembler.MappExcelFile(companyExcel);
            //}
            //catch (Exception ex)
            //{
            //    throw new BusinessException(ex.Message);
            //}
        }

        public ExcelFileDTO GenerateFileAplicationToAllyCoverageList(List<QueryAllyCoverageDTO> li_allyCoverage, string fileName)
        {
            var companyBusinessProvider = new UnderwritingParamBusinessServiceProvider.CompanyUnderwritingParamBusinessServiceProvider();
            var companyParamQueryAllyCoverage = Assemblers.AplicationCompanyAssembler.MappParamQueryAllyCoverage(li_allyCoverage);
            var result = companyBusinessProvider.GenerateFileToAllyCoverageList(companyParamQueryAllyCoverage, fileName);
            return Assemblers.CompanyAplicationAssembler.MappExcelFile(result);
        }

        public AllyCoverageDTO CreateAplicationAllyCoverage(AllyCoverageDTO allyCoverage)
        {
            CompanyParamAllyCoverage companyParamAllyCoverage = Assemblers.AplicationCompanyAssembler.MappParamAllyCoverage(allyCoverage);
            var companyBusinessProvider = new UnderwritingParamBusinessServiceProvider.CompanyUnderwritingParamBusinessServiceProvider();

            companyBusinessProvider.CreateBusinessAllyCoverage(companyParamAllyCoverage);//UpdateBusinessAllyCoverage(companyParamAllyCoverage, companyParamAllyCoverageOld);
            allyCoverage = Assemblers.CompanyAplicationAssembler.MappParamAllyCoverage(companyParamAllyCoverage);
            allyCoverage.errorDTO = new ErrorDTO { ErrorType = Utilities.Enums.ErrorType.Ok };
            return allyCoverage;
        }

        public AllyCoverageDTO UpdateAplicationAllyCoverage(AllyCoverageDTO allyCoverage, AllyCoverageDTO allyCoverageOld)
        {

            CompanyParamAllyCoverage companyParamAllyCoverage = Assemblers.AplicationCompanyAssembler.MappParamAllyCoverage(allyCoverage);
            CompanyParamAllyCoverage companyParamAllyCoverageOld = Assemblers.AplicationCompanyAssembler.MappParamAllyCoverage(allyCoverageOld);
            var companyBusinessProvider = new UnderwritingParamBusinessServiceProvider.CompanyUnderwritingParamBusinessServiceProvider();

            companyBusinessProvider.UpdateBusinessAllyCoverage(companyParamAllyCoverage, companyParamAllyCoverageOld);
            allyCoverage = Assemblers.CompanyAplicationAssembler.MappParamAllyCoverage(companyParamAllyCoverage);
            allyCoverage.errorDTO = new ErrorDTO { ErrorType = Utilities.Enums.ErrorType.Ok };
            return allyCoverage;
        }

        public AllyCoverageDTO DeleteAplicationAllyCoverage(AllyCoverageDTO allyCoverage)
        {
            CompanyParamAllyCoverage companyParamAllyCoverage = Assemblers.AplicationCompanyAssembler.MappParamAllyCoverage(allyCoverage);
            var companyBusinessProvider = new UnderwritingParamBusinessServiceProvider.CompanyUnderwritingParamBusinessServiceProvider();

            companyBusinessProvider.DeleteBusinessAllyCoverage(companyParamAllyCoverage);//UpdateBusinessAllyCoverage(companyParamAllyCoverage);
            allyCoverage = Assemblers.CompanyAplicationAssembler.MappParamAllyCoverage(companyParamAllyCoverage);
            allyCoverage.errorDTO = new ErrorDTO { ErrorType = Utilities.Enums.ErrorType.Ok };
            return allyCoverage;
        }

        public AllyCoverageQueryDTO GetAplicationAllyCoveragePrincipal()
        {
            throw new Exception();
        }

        public AllyCoverageQueryDTO GetAplicationCoverageAlly(int position)
        {
            throw new Exception();
        }

        #endregion

        #region "Condition Text"
        public ConditionTextDTO CreateApplicationConditiontext(ConditionTextDTO conditionTextdto)
        {
            CompanyUnderwritingParamBusinessServiceProvider providerBusiness = new CompanyUnderwritingParamBusinessServiceProvider();
            CompanyParamConditionText conditionText = new CompanyParamConditionText();
            conditionText = AplicationCompanyAssembler.MappCompanyConditionalText(conditionTextdto);
            providerBusiness.CreateBusinessConditiontext(conditionText);
            ConditionTextDTO conditionTextDTO = new ConditionTextDTO();
            conditionTextDTO = CompanyAplicationAssembler.MappConditionTextDTO(conditionText);
            return conditionTextDTO;
        }

        public ConditionTextDTO UpdateApplicationConditiontext(ConditionTextDTO conditionTextDTO)
        {
            CompanyUnderwritingParamBusinessServiceProvider providerBusiness = new CompanyUnderwritingParamBusinessServiceProvider();
            CompanyParamConditionText conditionText = new CompanyParamConditionText();
            conditionText = AplicationCompanyAssembler.MappCompanyConditionalText(conditionTextDTO);
            providerBusiness.UpdateBusinessConditiontext(conditionText);
            ConditionTextDTO conditionTextDTOnew = new ConditionTextDTO();
            conditionTextDTO = CompanyAplicationAssembler.MappConditionTextDTO(conditionText);
            return conditionTextDTOnew;
        }

        public string DeleteApplicationConditiontext(ConditionTextDTO conditionTextDTO)
        {
            CompanyUnderwritingParamBusinessServiceProvider providerBusiness = new CompanyUnderwritingParamBusinessServiceProvider();
            CompanyParamConditionText conditionText = new CompanyParamConditionText();
            conditionText = AplicationCompanyAssembler.MappCompanyConditionalText(conditionTextDTO);
            providerBusiness.DeleteBusinessConditiontext(conditionText);
            return "";
        }

        public ConditionTextQueryDTO GetApplicationConditiontext()
        {
            ConditionTextQueryDTO ConditionTextQueryDTO = new ConditionTextQueryDTO();
            List<string> errorDescriptions = new List<string>();
            CompanyUnderwritingParamBusinessServiceProvider providerBusiness = new CompanyUnderwritingParamBusinessServiceProvider();
            try
            {
                List<CompanyParamConditionText> ConditionTexts = providerBusiness.GetBusinessConditiontext();
                ConditionTextQueryDTO.ConditionText = Assemblers.CompanyAplicationAssembler.MappConditionalTextsDTO(ConditionTexts);
                ConditionTextQueryDTO.ErrorDto = new ErrorDTO { ErrorType = Utilities.Enums.ErrorType.Ok };
                return ConditionTextQueryDTO;
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message);
            }
        }


        public ConditionTextQueryDTO GetApplicationConditiontextByDescription(string description = "")
        {
            ConditionTextQueryDTO ConditionTextQueryDTO = new ConditionTextQueryDTO();
            CompanyUnderwritingParamBusinessServiceProvider providerBusiness = new CompanyUnderwritingParamBusinessServiceProvider();
            List<CompanyParamConditionText> ConditionTexts = providerBusiness.GetBusinessConditiontextByDescription(description);
            ConditionTextQueryDTO.ConditionText = Assemblers.CompanyAplicationAssembler.MappConditionalTextsDTO(ConditionTexts);
            ConditionTextQueryDTO.ErrorDto = new ErrorDTO { ErrorType = Utilities.Enums.ErrorType.Ok };
            return ConditionTextQueryDTO;
        }

        public ExcelFileDTO GenerateFileApplicationToConditiontext(string fileName)
        {
            ExcelFileDTO excelFileDTO = new ExcelFileDTO();

            try
            {
                CompanyUnderwritingParamBusinessServiceProvider providerBusiness = new CompanyUnderwritingParamBusinessServiceProvider();
                var companyExcel = providerBusiness.GenerateFileBusinessToConditiontext(fileName);
                return CompanyAplicationAssembler.MappExcelFile(companyExcel);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message);
            }
        }
        #endregion

        #region Tax

        #region TaxMethods
        public TaxDTO CreateApplicationTax(TaxDTO TaxDTO)
        {
            try
            {
                CompanyParamTax companyParamTax = Assemblers.AplicationCompanyAssembler.MappCompanyDTOtoParamTax(TaxDTO);

                CompanyUnderwritingParamBusinessServiceProvider companyBusinessProvider = new CompanyUnderwritingParamBusinessServiceProvider();
                CompanyParamTax companyParamTaxResult = companyBusinessProvider.CreateBusinessTax(companyParamTax);

                TaxDTO mappedTaxDTO = Assemblers.CompanyAplicationAssembler.MappTaxCompanytoDTO(companyParamTaxResult);
                mappedTaxDTO.errorDTO = new ErrorDTO { ErrorType = Utilities.Enums.ErrorType.Ok };
                return mappedTaxDTO;
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message);
            }
        }

        public TaxDTO UpdateApplicationTax(TaxDTO TaxDTO)
        {
            try
            {
                CompanyParamTax companyParamTax = Assemblers.AplicationCompanyAssembler.MappCompanyDTOtoParamTax(TaxDTO);
                CompanyUnderwritingParamBusinessServiceProvider companyBusinessProvider = new CompanyUnderwritingParamBusinessServiceProvider();
                CompanyParamTax companyParamTaxResult = companyBusinessProvider.UpdateBusinessTax(companyParamTax);

                TaxDTO mappedTaxDTO = Assemblers.CompanyAplicationAssembler.MappTaxCompanytoDTO(companyParamTaxResult);
                mappedTaxDTO.errorDTO = new ErrorDTO { ErrorType = Utilities.Enums.ErrorType.Ok };
                return mappedTaxDTO;
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message);
            }
        }

        public TaxQueryDTO GetApplicationTaxByDescription(string TaxDescription)
        {
            try
            {
                TaxQueryDTO taxQueryDTO = new TaxQueryDTO();
                CompanyUnderwritingParamBusinessServiceProvider companyBusinessProvider = new CompanyUnderwritingParamBusinessServiceProvider();

                taxQueryDTO.TaxDTOlist = CompanyAplicationAssembler.MappTaxesCompanytoDTOs(companyBusinessProvider.GetBusinessTaxByDescription(TaxDescription));

                return taxQueryDTO;
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message);
            }
        }

        public TaxQueryDTO GetApplicationTaxByIdAndDescription(int taxId, string taxDescription)
        {
            try
            {
                TaxQueryDTO taxQueryDTO = new TaxQueryDTO();
                CompanyUnderwritingParamBusinessServiceProvider companyBusinessProvider = new CompanyUnderwritingParamBusinessServiceProvider();

                taxQueryDTO.TaxDTOlist = CompanyAplicationAssembler.MappTaxesCompanytoDTOs(companyBusinessProvider.GetBusinessTaxByIdAndDescription(taxId, taxDescription));

                return taxQueryDTO;
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message);
            }
        }


        public ExcelFileDTO GenerateFileApplicationToTax()
        {
            throw new Exception();
        }

        #endregion

        #region TaxRateMethods
        public TaxRateDTO CreateApplicationTaxRate(TaxRateDTO taxRateDTO)
        {
            try
            {
                CompanyParamTaxRate companyParamTaxRate = Assemblers.AplicationCompanyAssembler.MappCompanyDTOtoParamTaxRate(taxRateDTO);

                CompanyUnderwritingParamBusinessServiceProvider companyBusinessProvider = new CompanyUnderwritingParamBusinessServiceProvider();
                CompanyParamTaxRate companyParamTaxRateResult = companyBusinessProvider.CreateBusinessTaxRate(companyParamTaxRate);

                TaxRateDTO mappedTaxRateDTO = Assemblers.CompanyAplicationAssembler.MappTaxRateCompanytoDTO(companyParamTaxRateResult);
                mappedTaxRateDTO.errorDTO = new ErrorDTO { ErrorType = Utilities.Enums.ErrorType.Ok };
                return mappedTaxRateDTO;
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message);
            }
        }

        public TaxRateDTO UpdateApplicationTaxRate(TaxRateDTO taxRateDTO)
        {
            try
            {
                CompanyParamTaxRate companyParamTaxRate = Assemblers.AplicationCompanyAssembler.MappCompanyDTOtoParamTaxRate(taxRateDTO);

                CompanyUnderwritingParamBusinessServiceProvider companyBusinessProvider = new CompanyUnderwritingParamBusinessServiceProvider();
                CompanyParamTaxRate companyParamTaxRateResult = companyBusinessProvider.UpdateBusinessTaxRate(companyParamTaxRate);

                TaxRateDTO mappedTaxRateDTO = Assemblers.CompanyAplicationAssembler.MappTaxRateCompanytoDTO(companyParamTaxRateResult);
                mappedTaxRateDTO.errorDTO = new ErrorDTO { ErrorType = Utilities.Enums.ErrorType.Ok };
                return mappedTaxRateDTO;
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message);
            }
        }

        public TaxRateQueryDTO GetApplicationTaxRateByTaxId(int TaxId)
        {
            try
            {
                TaxRateQueryDTO taxQueryDTO = new TaxRateQueryDTO();
                CompanyUnderwritingParamBusinessServiceProvider companyBusinessProvider = new CompanyUnderwritingParamBusinessServiceProvider();

                taxQueryDTO.TaxRateDTOlist = CompanyAplicationAssembler.MappTaxRatesCompanytoDTOs(companyBusinessProvider.GetBusinessTaxRateByTaxId(TaxId));

                return taxQueryDTO;
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message);
            }
        }

        public TaxRateDTO getApplicationTaxRateByTaxIdByAttributes(int taxId, int? taxConditionId, int? taxCategoryId, int? countryCode, int? stateCode, int? cityCode, int? economicActivityCode, int? prefixId, int? coverageId, int? technicalBranchId)
        {
            try
            {
                TaxRateDTO taxRateDTO = new TaxRateDTO();
                CompanyUnderwritingParamBusinessServiceProvider companyBusinessProvider = new CompanyUnderwritingParamBusinessServiceProvider();
                CompanyParamTaxRate companyParamTaxRate = companyBusinessProvider.GetBusinessTaxRateByTaxIdbyAttributes(taxId, taxConditionId, taxCategoryId, countryCode, stateCode, cityCode, economicActivityCode, prefixId, coverageId, technicalBranchId);
                if (companyParamTaxRate.Id > 0)
                {
                    taxRateDTO = Assemblers.CompanyAplicationAssembler.MappTaxRateCompanytoDTO(companyParamTaxRate);
                }
                return taxRateDTO;
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message);
            }
        }

        public TaxRateDTO GetApplicationTaxRateById(int taxRateId)
        {
            try
            {
                TaxRateDTO taxRateDTO = new TaxRateDTO();
                CompanyUnderwritingParamBusinessServiceProvider companyBusinessProvider = new CompanyUnderwritingParamBusinessServiceProvider();
                CompanyParamTaxRate companyParamTaxRate = companyBusinessProvider.GetBusinessTaxRateById(taxRateId);
                if (companyParamTaxRate.Id > 0)
                {
                    taxRateDTO = Assemblers.CompanyAplicationAssembler.MappTaxRateCompanytoDTO(companyParamTaxRate);
                }
                return taxRateDTO;
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message);
            }
        }

        #endregion

        #region TaxCategoryMethods
        public TaxCategoryDTO CreateApplicationTaxCategory(TaxCategoryDTO taxCategoryDTO)
        {
            try
            {
                CompanyParamTaxCategory companyParamTaxCategory = Assemblers.AplicationCompanyAssembler.MappCompanyDTOtoParamTaxCategory(taxCategoryDTO);

                CompanyUnderwritingParamBusinessServiceProvider companyBusinessProvider = new CompanyUnderwritingParamBusinessServiceProvider();
                CompanyParamTaxCategory companyParamTaxCategoryResult = companyBusinessProvider.CreateBusinessTaxCategory(companyParamTaxCategory);

                TaxCategoryDTO mappedTaxCategoryDTO = Assemblers.CompanyAplicationAssembler.MappTaxCategoryCompanytoDTO(companyParamTaxCategoryResult);
                mappedTaxCategoryDTO.errorDTO = new ErrorDTO { ErrorType = Utilities.Enums.ErrorType.Ok };
                return mappedTaxCategoryDTO;
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message);
            }
        }

        public TaxCategoryDTO UpdateApplicationTaxCategory(TaxCategoryDTO taxCategoryDTO)
        {
            try
            {
                CompanyParamTaxCategory companyParamTaxCategory = Assemblers.AplicationCompanyAssembler.MappCompanyDTOtoParamTaxCategory(taxCategoryDTO);

                CompanyUnderwritingParamBusinessServiceProvider companyBusinessProvider = new CompanyUnderwritingParamBusinessServiceProvider();
                CompanyParamTaxCategory companyParamTaxCategoryResult = companyBusinessProvider.UpdateBusinessTaxCategory(companyParamTaxCategory);

                TaxCategoryDTO mappedTaxCategoryDTO = Assemblers.CompanyAplicationAssembler.MappTaxCategoryCompanytoDTO(companyParamTaxCategoryResult);
                mappedTaxCategoryDTO.errorDTO = new ErrorDTO { ErrorType = Utilities.Enums.ErrorType.Ok };
                return mappedTaxCategoryDTO;
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message);
            }
        }

        public TaxCategoryQueryDTO GetApplicationTaxCategoriesByTaxId(int TaxId)
        {
            try
            {
                TaxCategoryQueryDTO taxCategoryQueryDTO = new TaxCategoryQueryDTO();
                CompanyUnderwritingParamBusinessServiceProvider companyBusinessProvider = new CompanyUnderwritingParamBusinessServiceProvider();

                taxCategoryQueryDTO.TaxCategoryDTOlist = CompanyAplicationAssembler.MappTaxCategoriesCompanytoDTOs(companyBusinessProvider.GetBusinessTaxCategoriesByTaxId(TaxId));
                taxCategoryQueryDTO.errorDTO = new ErrorDTO { ErrorType = Utilities.Enums.ErrorType.Ok };

                return taxCategoryQueryDTO;
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message);
            }
        }

        public bool DeleteApplicationTaxCategoriesByTaxId(int categoryId, int taxId)
        {
            try
            {
                CompanyUnderwritingParamBusinessServiceProvider companyBusinessProvider = new CompanyUnderwritingParamBusinessServiceProvider();
                bool CategoriesDeleted = companyBusinessProvider.DeleteBusinessTaxCategoriesByTaxId(categoryId, taxId);

                return CategoriesDeleted;
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message);
            }

        }
        #endregion

        #region TaxConditionMethods
        public TaxConditionDTO CreateApplicationTaxCondition(TaxConditionDTO taxConditionDTO)
        {
            try
            {
                CompanyParamTaxCondition companyParamTaxCondition = Assemblers.AplicationCompanyAssembler.MappCompanyDTOtoParamTaxCondition(taxConditionDTO);

                CompanyUnderwritingParamBusinessServiceProvider companyBusinessProvider = new CompanyUnderwritingParamBusinessServiceProvider();
                CompanyParamTaxCondition companyParamTaxConditionResult = companyBusinessProvider.CreateBusinessTaxCondition(companyParamTaxCondition);

                TaxConditionDTO mappedTaxConditionDTO = Assemblers.CompanyAplicationAssembler.MappTaxConditionCompanytoDTO(companyParamTaxConditionResult);
                mappedTaxConditionDTO.errorDTO = new ErrorDTO { ErrorType = Utilities.Enums.ErrorType.Ok };
                return mappedTaxConditionDTO;
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message);
            }
        }

        public TaxConditionDTO UpdateApplicationTaxCondition(TaxConditionDTO taxConditionDTO)
        {
            try
            {
                CompanyParamTaxCondition companyParamTaxCondition = Assemblers.AplicationCompanyAssembler.MappCompanyDTOtoParamTaxCondition(taxConditionDTO);

                CompanyUnderwritingParamBusinessServiceProvider companyBusinessProvider = new CompanyUnderwritingParamBusinessServiceProvider();
                CompanyParamTaxCondition companyParamTaxConditionResult = companyBusinessProvider.UpdateBusinessTaxCondition(companyParamTaxCondition);

                TaxConditionDTO mappedTaxConditionDTO = Assemblers.CompanyAplicationAssembler.MappTaxConditionCompanytoDTO(companyParamTaxConditionResult);
                mappedTaxConditionDTO.errorDTO = new ErrorDTO { ErrorType = Utilities.Enums.ErrorType.Ok };
                return mappedTaxConditionDTO;
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message);
            }
        }

        public TaxConditionQueryDTO GetApplicationTaxConditionsByTaxId(int TaxId)
        {
            try
            {
                TaxConditionQueryDTO taxConditionQueryDTO = new TaxConditionQueryDTO();
                CompanyUnderwritingParamBusinessServiceProvider companyBusinessProvider = new CompanyUnderwritingParamBusinessServiceProvider();

                taxConditionQueryDTO.TaxConditionDTOlist = CompanyAplicationAssembler.MappTaxConditionsCompanytoDTOs(companyBusinessProvider.GetBusinessTaxConditionsByTaxId(TaxId));
                taxConditionQueryDTO.errorDTO = new ErrorDTO { ErrorType = Utilities.Enums.ErrorType.Ok };


                return taxConditionQueryDTO;
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message);
            }
        }

        public bool DeleteApplicationTaxConditionsByTaxId(int conditionId, int taxId)
        {
            try
            {
                CompanyUnderwritingParamBusinessServiceProvider companyBusinessProvider = new CompanyUnderwritingParamBusinessServiceProvider();
                bool CategoriesDeleted = companyBusinessProvider.DeleteBusinessTaxConditionsByTaxId(conditionId, taxId);

                return CategoriesDeleted;
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message);
            }

        }
        #endregion

        #endregion
    }
}