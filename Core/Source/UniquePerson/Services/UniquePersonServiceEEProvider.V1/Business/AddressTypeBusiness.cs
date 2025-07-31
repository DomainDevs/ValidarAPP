using Sistran.Core.Application.UniquePersonService.V1.Enums;
using MOUP= Sistran.Core.Application.UniquePersonService.V1.Models;
using Sistran.Core.Application.Utilities.DataFacade;
using Sistran.Core.Framework.Queries;
using System.Collections.Generic;
using Sistran.Core.Application.UniquePersonV1.Entities;
using Sistran.Core.Framework.DAF;
using Sistran.Core.Application.UniquePersonService.V1.Assemblers;
using System.Diagnostics;
using System.Linq;
using System;
using Sistran.Core.Framework.BAF;

namespace Sistran.Core.Application.UniquePersonService.V1.Business
{
    public class AddressTypeBusiness
    {

        /// <summary>
        /// GetMaritalStatus
        /// </summary>
        /// <returns></returns>
        public List<Models.AddressType> GetAddressesTypes()
        {
            try
            {
                return ModelAssembler.CreateAddressTypes(DataFacadeManager.GetObjects(typeof(UniquePersonV1.Entities.AddressType)));
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
            
        }
    }
}