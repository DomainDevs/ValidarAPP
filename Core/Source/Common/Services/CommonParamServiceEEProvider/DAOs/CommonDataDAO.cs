// -----------------------------------------------------------------------
// <copyright file="CommonDataDAO.cs" company="SISTRAN">
// Copyright (c) SISTRAN ANDINA. All rights reserved.
// </copyright>
// <author>Oscar Camacho</author>

namespace Sistran.Core.Application.CommonParamService.DAOs
{
    using System;
    using System.Collections.Generic;
    using System.Linq;    
    using Framework.DAF;
    using UniquePerson.Entities;
    using Utilities.DataFacade;
    using ENUMUT = Utilities.Enums;
    using UNIEN = Sistran.Core.Application.UniquePerson.Entities;
    using UTMO = Utilities.Error;  

    /// <summary>
    /// datos comunes 
    /// </summary>
    public class CommonDataDAO
    {
        /// <summary>
        /// Obtiene los tipo de teléfono
        /// </summary>
        /// <returns>Lista de teléfonos</returns>
        public UTMO.Result<List<PhoneType>, UTMO.ErrorModel> GetPhoneType()
        {
            try
            {
                List<UNIEN.PhoneType> entityPhoneType = new BusinessCollection(DataFacadeManager.Instance.GetDataFacade().SelectObjects(typeof(UNIEN.PhoneType))).Cast<UNIEN.PhoneType>().ToList();
                List<PhoneType> phoneType = entityPhoneType.OrderBy(x => x.Description).Select(x => new PhoneType(x.PhoneTypeCode)
                {
                    PhoneTypeCode = x.PhoneTypeCode,
                    Description = x.Description
                }).ToList();

                return new UTMO.ResultValue<List<PhoneType>, UTMO.ErrorModel>(phoneType);
            }
            catch (Exception ex)
            {
                List<string> listErrors = new List<string>();
                listErrors.Add(Resources.Errors.ErrorGetCountries);
                return new UTMO.ResultError<List<PhoneType>, UTMO.ErrorModel>(UTMO.ErrorModel.CreateErrorModel(listErrors, ENUMUT.ErrorType.TechnicalFault, ex));
            }
        }

        /// <summary>
        /// Obtiene los tipo de teléfono
        /// </summary>
        /// <returns>Lista de teléfonos</returns>
        public UTMO.Result<List<AddressType>, UTMO.ErrorModel> GetAddressType()
        {
            try
            {
                List<UNIEN.AddressType> entityPhoneType = new BusinessCollection(DataFacadeManager.Instance.GetDataFacade().SelectObjects(typeof(UNIEN.AddressType))).Cast<UNIEN.AddressType>().ToList();
                List<AddressType> addressTypeCode = entityPhoneType.OrderBy(x => x.SmallDescription).Select(x => new AddressType(x.AddressTypeCode)
                {
                    AddressTypeCode = x.AddressTypeCode,
                    SmallDescription = x.SmallDescription
                }).ToList();

                return new UTMO.ResultValue<List<AddressType>, UTMO.ErrorModel>(addressTypeCode);
            }
            catch (Exception ex)
            {
                List<string> listErrors = new List<string>();
                listErrors.Add(Resources.Errors.ErrorGetCountries);
                return new UTMO.ResultError<List<AddressType>, UTMO.ErrorModel>(UTMO.ErrorModel.CreateErrorModel(listErrors, ENUMUT.ErrorType.TechnicalFault, ex));
            }
        }
    }
}
