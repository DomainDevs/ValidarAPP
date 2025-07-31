// -----------------------------------------------------------------------
// <copyright file="CompanyAplicationAssembler.cs" company="SISTRAN">
// Copyright (c) SISTRAN ANDINA. All rights reserved.
// </copyright>
// <author>Jaime Trujillo</author>
// -----------------------------------------------------------------------

namespace Sistran.Company.Application.ParametrizationAplicationServiceProvider.Assemblers
{
    using Sistran.Company.Application.ParametrizationAplicationServices.DTO;
    using Sistran.Company.Application.ParametrizationParamBusinessService.Model;
    using Sistran.Company.Application.Utilities.DTO;
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    


    /// <summary>
    /// CompanyAplicationAssembler. Ensamblado de Company a DTO 
    /// </summary>
    public class CompanyAplicationAssembler
    {
        #region"combo"
        /// <summary>
        /// CreateCombo. Retorna un objeto de modelo DTO a partir de un objeto modelo Company.
        /// </summary>
        /// <param name="companyCombo">Modelo Company</param>
        /// <returns>ListaComboDTO. Modelo DTO</returns>
        public static ListaComboDTO CreateCombo(ParamCompanyListCombo companyCombo)
        {
            ListaComboDTO listcomboDTO = new ListaComboDTO();

            listcomboDTO.PkColumn = companyCombo.PkColumn;
            listcomboDTO.DescriptionColumn = companyCombo.DescriptionColumn;
            listcomboDTO.Table = companyCombo.Table;
            listcomboDTO.Filter = listcomboDTO.Filter;
            listcomboDTO.Order = listcomboDTO.Order;

            List<ElementDTO> elementsDTO = new List<ElementDTO>();
            foreach (ParamCompanyElement element in companyCombo.CompanyElements)
            {
                ElementDTO elementDTO = new ElementDTO();
                elementDTO.Id = element.Pk;
                elementDTO.Description = element.Description;
                elementsDTO.Add(elementDTO);
            }

            listcomboDTO.Elements = elementsDTO;

            return listcomboDTO;
        }
        #endregion

        #region BillingPeriod

        /// <summary>
        ///  CreateBillingPeriod. realiza la conversión de objeto tipo BillingPeriod del modelo Company al modelo DTO.
        /// </summary>
        /// <param name="entityBillingPeriod"></param>
        /// <returns>BillingPeriodDTO</returns>
        public static BillingPeriodDTO CreateBillingPeriod(CompanyParamBillingPeriod entityBillingPeriod)
        {
            return new BillingPeriodDTO
            {
                BILLING_PERIOD_CD = entityBillingPeriod.BILLING_PERIOD_CD,
                Description = entityBillingPeriod.Description

            };
        }

        /// <summary>
        /// CreateBillingPeriods. realiza la conversión de objeto tipo Lista BillingPeriod del modelo Company al modelo DTO.
        /// </summary>
        /// <param name="businessCollection"></param>
        /// <returns>BillingPeriodQueryDTO</returns>
        public static BillingPeriodQueryDTO CreateBillingPeriods(List<CompanyParamBillingPeriod> businessCollection)
        {
            var sync = new object();
            BillingPeriodQueryDTO BillingPeriod = new BillingPeriodQueryDTO();

                Parallel.ForEach(businessCollection, (entity) =>
                {
                    lock (sync)
                    {
                        BillingPeriod.BillingPeriodQueryDTOs.Add(CompanyAplicationAssembler.CreateBillingPeriod(entity));
                    }
                }
                );
            return BillingPeriod;
        }
        #endregion

        #region BusinessType

        /// <summary>
        /// CreateBusinessType. realiza la conversión de objeto tipo BusinessType del modelo Company al modelo DTO.
        /// </summary>
        /// <param name="entityBusinessType"></param>
        /// <returns>BusinessTypeDTO</returns>
        public static BusinessTypeDTO CreateBusinessType(CompanyParamBusinessType entityBusinessType)
        {
            return new BusinessTypeDTO
            {
                BUSINESS_TYPE_CD = entityBusinessType.BUSINESS_TYPE_CD,
                SMALL_DESCRIPTION = entityBusinessType.SMALL_DESCRIPTION

            };
        }

        /// <summary>
        /// CreateBusinessTypes. realiza la conversión de objeto tipo Lista BusinessType del modelo Company al modelo DTO.
        /// </summary>
        /// <param name="businessCollection"></param>
        /// <returns>BusinessTypeQueryDTO</returns>
        public static BusinessTypeQueryDTO CreateBusinessTypes(List<CompanyParamBusinessType> businessCollection)
        {
            var sync = new object();
            BusinessTypeQueryDTO BusinessType = new BusinessTypeQueryDTO();

              Parallel.ForEach(businessCollection, (entity) =>
                {
                    lock (sync)
                    {
                        BusinessType.BusinessTypeQueryDTOs.Add(CompanyAplicationAssembler.CreateBusinessType(entity));
                    }
                }
                );
           
           return BusinessType;
        }
        #endregion

        #region ciudades

        public static List<CityDTO> MappParamCities(List<CompanyParamCity> city)
        {
            List<CityDTO> cities = new List<CityDTO>();
            foreach (var item in city)
            {
                cities.Add(MappParamCity(item));
            }
            return cities;
        }


        public static CityDTO MappParamCity(CompanyParamCity city)
        {
            CityDTO cityDTO = new CityDTO
            {
                Id = city.Id,
                Description = city.Description,
                SmallDescription = city.SmallDescription,
                Country = new CountryDTO { Id = city.Country.Id, Description = city.Country.Description },
                State = new StateDTO { Id = city.State.Id, Description = city.State.Description }
            };

            return cityDTO;
        }

        public static ExcelFileDTO MappExcelFile(CompanyExcel companyExcel)
        {
            ExcelFileDTO excel = new ExcelFileDTO
            {
                File = companyExcel.FileData,
            };

            return excel;
        }
        #endregion
    }
}