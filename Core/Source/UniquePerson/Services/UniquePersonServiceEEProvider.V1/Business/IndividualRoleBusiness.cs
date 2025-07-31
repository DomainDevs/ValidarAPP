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

namespace Sistran.Core.Application.UniquePersonService.V1.Business
{
    public class IndividualRoleBusiness
    {
        /// <summary>
        /// GetIndividualRoleByIndividualId
        /// </summary>
        /// <param name="individualId">The individual identifier.</param>
        /// <returns></returns>
        public List<Models.IndividualRole> GetIndividualRoleByIndividualId(int individualId)
        {
            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();
            List<Models.IndividualRole> listIndividualRole = new List<Models.IndividualRole>();

            ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();
            filter.Property(IndividualRole.Properties.IndividualId, typeof(IndividualRole).Name);
            filter.Equal();
            filter.Constant(individualId);

            var business = DataFacadeManager.GetObjects(typeof(IndividualRole), filter.GetPredicate());
            listIndividualRole = ModelAssembler.CreateIndividualRoles(business);

            if (listIndividualRole.Count > 0)
            {
                List<MOUP.Role> roles = new RoleBusiness().GetRoles();
                foreach(MOUP.IndividualRole individualRole in listIndividualRole)
                {
                    individualRole.Description = roles.Where(x => x.Id == individualRole.RoleId).FirstOrDefault().Description;
                }
            }
            

          

            stopWatch.Stop();
            Debug.WriteLine(stopWatch.Elapsed.ToString(), "Sistran.Core.Application.UniquePersonService.V1.Business.GetIndividualRoleByIndividualId");
            return listIndividualRole;
        }

       
    }
}