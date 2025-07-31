using Sistran.Core.Application.UniquePersonService.V1.Assemblers;
using Sistran.Core.Application.UniquePersonService.V1.Models;
using Sistran.Core.Application.Utilities.DataFacade;
using Sistran.Core.Framework.DAF;
using System.Collections.Generic;

namespace Sistran.Core.Application.UniquePersonService.V1.Business
{
    public class RoleBusiness
    {

        internal Role CreateRole(Role Role)
        {
            UniquePersonV1.Entities.Role entityRole = EntityAssembler.CreateRole(Role);
            DataFacadeManager.Insert(entityRole);

            return ModelAssembler.CreateRole(entityRole);
        }

        internal Role UpdateRole(Role Role)
        {
            UniquePersonV1.Entities.Role entityRole = EntityAssembler.CreateRole(Role);
            DataFacadeManager.Update(entityRole);

            return ModelAssembler.CreateRole(entityRole);
        }

        internal void DeleteRole(int RoleId)
        {
            PrimaryKey primaryKey = UniquePersonV1.Entities.Role.CreatePrimaryKey(RoleId);
            DataFacadeManager.Delete(primaryKey);
        }

        internal Role GetRoleByRoleId(int RoleId)
        {
            PrimaryKey primaryKey = UniquePersonV1.Entities.Role.CreatePrimaryKey(RoleId);
            UniquePersonV1.Entities.Role entityRole = (UniquePersonV1.Entities.Role)DataFacadeManager.GetObject(primaryKey);

            return ModelAssembler.CreateRole(entityRole);
        }

        internal List<Role> GetRoles()
        {
            return ModelAssembler.CreateRoles(DataFacadeManager.GetObjects(typeof(UniquePersonV1.Entities.Role)));
        }

    }
}
