using Sistran.Core.Application.UniquePersonService.V1.Assemblers;
using Sistran.Core.Application.UniquePersonService.V1.Models;
using Sistran.Core.Application.Utilities.DataFacade;
using Sistran.Core.Framework.DAF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using COMMEN = Sistran.Core.Application.Common.Entities;

namespace Sistran.Core.Application.UniquePersonService.DAOs
{
    public class HouseTypeDAO
    {
        public List<HouseType> GetHouseTypes()
        {
            BusinessCollection businessCollection = new BusinessCollection(DataFacadeManager.Instance.GetDataFacade().SelectObjects(typeof(COMMEN.HouseType)));
            return ModelAssembler.CreateHouseTypes(businessCollection);
        }
    }
}