
using Sistran.Core.Application.UniquePerson.Entities;
using Sistran.Core.Application.UniquePersonService.Assemblers;
using Sistran.Core.Application.Utilities.DataFacade;
using Sistran.Core.Framework.DAF;
using Sistran.Core.Framework.Queries;
using System.Collections.Generic;

namespace Sistran.Core.Application.UniquePersonService.DAOs
{
    /// <summary>
    /// Inidividual Rol
    /// </summary>
    public class IndividualRolDAO
    {
        /// <summary>
        /// Eliminar Roles desasociados
        /// </summary>
        /// <param name="rolesAssociated">Modelo de Rol</param>
        /// <param name="individualId">Individuo</param>
        /// <returns></returns>
        public List<Models.Rol> RemoveRolesAssociated(List<Models.Rol> rolesAssociated, int individualId)
        {

            ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();
            filter.Property(IndividualRole.Properties.IndividualId, typeof(IndividualRole).Name);
            filter.Equal();
            filter.Constant(individualId);
            BusinessCollection businessCollection = new BusinessCollection();
            businessCollection = new BusinessCollection(DataFacadeManager.Instance.GetDataFacade().SelectObjects(typeof(IndividualRole), filter.GetPredicate()));
            List<Models.Rol> rolesAssociatedEntity = ModelAssembler.CreateIndividualRoles(businessCollection);

            foreach (Models.Rol rol in rolesAssociated)
            {
                if (rolesAssociatedEntity.Find(x => x.Id == rol.Id) == null)
                {
                    IndividualRole individualRoleEntity = null;
                    PrimaryKey keyIndividualRole = IndividualRole.CreatePrimaryKey(individualId, rol.Id);
                    individualRoleEntity = (IndividualRole)DataFacadeManager.Instance.GetDataFacade().GetObjectByPrimaryKey(keyIndividualRole);
                    if (individualRoleEntity != null)
                    {
                        DataFacadeManager.Instance.GetDataFacade().DeleteObject(individualRoleEntity);
                    }
                }
            }

            return rolesAssociated;
        }

        /// <summary>
        /// Registrar Roles asociados
        /// </summary>
        /// <param name="rolesAssociated">Modelo de Rol</param>
        /// <param name="individualId">Individuo</param>
        /// <returns></returns>
        public List<Models.Rol> CreateIndividualRoles(List<Models.Rol> rolesAssociated, int individualId)
        {
            ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();
            filter.Property(IndividualRole.Properties.IndividualId, typeof(IndividualRole).Name);
            filter.Equal();
            filter.Constant(individualId);
            BusinessCollection businessCollection = new BusinessCollection();
            businessCollection = new BusinessCollection(DataFacadeManager.Instance.GetDataFacade().SelectObjects(typeof(IndividualRole), filter.GetPredicate()));
            List<Models.Rol> rolesAssociatedEntity = ModelAssembler.CreateIndividualRoles(businessCollection);

            foreach (Models.Rol rol in rolesAssociated)
            {
                if (rolesAssociatedEntity.Find(x => x.Id == rol.Id) == null)
                {
                    IndividualRole individualRoleEntity = ModelAssembler.CreateIndividualRol(rol, individualId);
                    DataFacadeManager.Instance.GetDataFacade().InsertObject(individualRoleEntity);
                }
            }

            return rolesAssociated;
        }

    }
}
