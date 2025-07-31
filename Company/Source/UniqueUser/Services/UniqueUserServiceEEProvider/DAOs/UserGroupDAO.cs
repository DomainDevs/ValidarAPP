using Sistran.Company.Application.UniqueUserServices.EEProvider.Entities.Views;
using Sistran.Core.Framework.DAF.Engine;
using Sistran.Core.Framework.Queries;
using System;
using System.Collections.Generic;
using UUEN = Sistran.Core.Application.UniqueUser.Entities;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sistran.Core.Application.Utilities.DataFacade;
using Sistran.Company.Application.UniqueUserServices.Models;
using Sistran.Core.Framework.DAF;

namespace Sistran.Company.Application.UniqueUserServices.EEProvider.DAOs
{
    public class UserGroupDAO
    {
        internal List<CompanyUserGroup> GetListUserGroupsByUserId(int userId)
        {
            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();
            //UserGroupView view = new UserGroupView();
            //ViewBuilder builder = new ViewBuilder("UserGroupView");

            ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();
            filter.Property(UUEN.UserGroup.Properties.UserId, typeof(UUEN.UniqueUsers).Name);
            filter.Equal();
            filter.Constant(userId);

            //builder.Filter = filter.GetPredicate();
            BusinessCollection businessCollection = new BusinessCollection(
                DataFacadeManager.Instance.GetDataFacade().
                SelectObjects(typeof(UUEN.UserGroup), filter.GetPredicate()));

            //DataFacadeManager.Instance.GetDataFacade().FillView(builder, view);
            stopWatch.Stop();
            return Assemblers.ModelAssembler.CreateUserGroups(businessCollection);
        }
    }
}
