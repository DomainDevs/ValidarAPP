// -----------------------------------------------------------------------
// <copyright file="EntityAssembler.cs" company="SISTRAN">
// Copyright (c) SISTRAN ANDINA. All rights reserved.
// </copyright>
// <author>Jaime Trujillo</author>
// -----------------------------------------------------------------------
namespace Sistran.Company.Application.ParametrizationParamBusinessServiceProvider.Assemblers
{  
    using Sistran.Company.Application.Common.Entities;
    using Sistran.Company.Application.ParametrizationParamBusinessService.Model;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    /// <summary>
    /// convierte de modelo company a entity
    /// </summary>
    public class EntityAssembler
    {
        #region ciudades
        public static City CreateParamCity(CompanyParamCity companyParamCity, int maxId)
        {
            return new City(companyParamCity.Country.Id, companyParamCity.Id, companyParamCity.State.Id)
            {
                CityCode = maxId,
                CountryCode= companyParamCity.Country.Id,
                StateCode= companyParamCity.State.Id, 
                SmallDescription= companyParamCity.SmallDescription,
                Description= companyParamCity.Description
            };
        }
        #endregion
    }
}
