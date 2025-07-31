using Sistran.Core.Application.UniquePersonService.V1.Assemblers;
using Sistran.Core.Application.UniquePersonService.V1.Models;
using Sistran.Core.Application.Utilities.DataFacade;
using Sistran.Core.Framework.BAF;
using Sistran.Core.Framework.DAF;
using Sistran.Core.Framework.Queries;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using COMMEN = Sistran.Core.Application.Common.Entities;

namespace Sistran.Core.Application.UniquePersonService.V1.Business
{
    public class OthersDeclinedTypeBusiness
    {
        public List<Models.AllOthersDeclinedType> GetAllOthersDeclinedTypes()
        {
            try
            {
                return ModelAssembler.CreateAllOthersDeclinedTypes(DataFacadeManager.GetObjects(typeof(UniquePersonV1.Entities.OthersDeclinedType)));
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }
    }
}