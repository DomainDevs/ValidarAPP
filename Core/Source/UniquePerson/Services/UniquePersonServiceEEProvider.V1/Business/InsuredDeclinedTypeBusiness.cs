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
    public class InsuredDeclinedTypeBusiness
    {
        public List<Models.InsuredDeclinedType> GetInsuredDeclinedTypes()
        {
            try
            {
                return ModelAssembler.CreateInsuredDeclinedTypes(DataFacadeManager.GetObjects(typeof(UniquePersonV1.Entities.InsuredDeclinedType)));
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }
    }
}