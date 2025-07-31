using Sistran.Core.Application.Common.Entities;
using Sistran.Core.Application.UniquePersonService.V1.Assemblers;
using Sistran.Core.Application.Utilities.DataFacade;
using Sistran.Core.Framework.DAF;
using System.Collections.Generic;
using Models = Sistran.Core.Application.UniquePersonService.V1.Models;


namespace Sistran.Core.Application.UniquePersonService.V1.Business
{
    public class GuaranteeBusiness
    {

        internal Models.Guarantee CreateGuarantee(Models.Guarantee guarantee)
        {
            Guarantee entityGuarantee = EntityAssembler.CreateGuarantee(guarantee);
            DataFacadeManager.Insert(entityGuarantee);
            return ModelAssembler.CreateGuarantee(entityGuarantee);
        }

        internal Models.Guarantee UpdateGuarantee(Models.Guarantee guarantee)
        {
            Guarantee entityGuarantee = EntityAssembler.CreateGuarantee(guarantee);
            DataFacadeManager.Update(entityGuarantee);
            return ModelAssembler.CreateGuarantee(entityGuarantee);
        }

        internal void DeleteGuarantee(int guaranteeId)
        {
            PrimaryKey primaryKey = Guarantee.CreatePrimaryKey(guaranteeId);
            DataFacadeManager.Delete(primaryKey);
        }

        internal Models.Guarantee GetGuaranteeByGuaranteeId(int guaranteeId)
        {
            PrimaryKey primaryKey = Guarantee.CreatePrimaryKey(guaranteeId);
            Guarantee entityGuarantee = (Guarantee)DataFacadeManager.GetObject(primaryKey);
            return ModelAssembler.CreateGuarantee(entityGuarantee);
        }

        internal List<Models.Guarantee> GetGuarantees()
        {
            return ModelAssembler.CreateGuarantees(DataFacadeManager.GetObjects(typeof(Guarantee)));
        }


    }
}
