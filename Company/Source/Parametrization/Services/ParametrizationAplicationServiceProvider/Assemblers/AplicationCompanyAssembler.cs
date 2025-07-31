// -----------------------------------------------------------------------
// <copyright file="AplicationCompanyAssembler.cs" company="SISTRAN">
// Copyright (c) SISTRAN ANDINA. All rights reserved.
// </copyright>
// <author>Jaime Trujillo</author>
// -----------------------------------------------------------------------
namespace Sistran.Company.Application.ParametrizationAplicationServiceProvider.Assemblers
{
    using Sistran.Company.Application.ParametrizationAplicationServices.DTO;
    using Sistran.Company.Application.ParametrizationParamBusinessService.Model;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using ModelCompany = Sistran.Company.Application.ParametrizationParamBusinessService.Model;
    using ModelDTO = Sistran.Company.Application.ParametrizationAplicationServices.DTO;

    /// <summary>
    /// AplicationCompanyAssembler. Ensamblado de DTO a Company 
    /// </summary>
    public class AplicationCompanyAssembler
    {
        #region"combo"
        /// <summary>
        /// CreateCombo. Retorna un modelo Company a partir de un modelo DTO.
        /// </summary>
        /// <param name="listacomboDTO">Modelo Dto</param>
        /// <returns>ParamCompanyListCombo. Modelo Company</returns>
        public static ParamCompanyListCombo CreateCombo(ListaComboDTO listacomboDTO)
        {
            ParamCompanyListCombo listaCompanyCombo = new ParamCompanyListCombo();
            listaCompanyCombo.PkColumn = listacomboDTO.PkColumn;
            listaCompanyCombo.DescriptionColumn = listacomboDTO.DescriptionColumn;
            listaCompanyCombo.Table = listacomboDTO.Table;
            listaCompanyCombo.Filter = listacomboDTO.Filter;
            listaCompanyCombo.Order = listacomboDTO.Order;

            return listaCompanyCombo;
        }
        #endregion

        #region ciudades
        /// <summary>
        /// mapea de cityDTO a model company CompanyParamCity
        /// </summary>
        /// <param name="city"></param>
        /// <returns></returns>
        public static ModelCompany.CompanyParamCity MappCity(ModelDTO.CityDTO city)
        {
            ModelCompany.CompanyParamCity companyParamCity = new ModelCompany.CompanyParamCity();
            companyParamCity.Id = city.Id;
            companyParamCity.Description = city.Description;
            companyParamCity.SmallDescription = city.SmallDescription;
            companyParamCity.Country = new ModelCompany.CompanyParamCountry { Id = city.Country.Id, Description = city.Description };
            companyParamCity.State = new ModelCompany.CompanyParamState { Id = city.State.Id, Description = city.Description };

            return companyParamCity;
        }
    }

    #endregion
}

