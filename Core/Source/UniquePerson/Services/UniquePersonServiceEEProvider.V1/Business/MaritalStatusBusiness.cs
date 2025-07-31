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
    public class MaritalStatusBusiness
    {
        public List<Models.MaritalStatus> GetMaritalStatus()
        {
            try
            {
                return ModelAssembler.CreateMaritalStatus(DataFacadeManager.GetObjects(typeof(UniquePersonV1.Entities.MaritalStatus)));
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }
    }
}