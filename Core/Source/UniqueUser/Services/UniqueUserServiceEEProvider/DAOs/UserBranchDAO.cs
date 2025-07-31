using Sistran.Core.Application.UniqueUserServices.EEProvider.Assemblers;
using Sistran.Core.Application.UniqueUserServices.EEProvider.Entities.Views;
using Sistran.Core.Application.Utilities.DataFacade;
using Sistran.Core.Framework.DAF.Engine;
using Sistran.Core.Framework.Queries;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using COMMEN = Sistran.Core.Application.Common.Entities;
using COMML = Sistran.Core.Application.CommonService.Models;
using UUEN = Sistran.Core.Application.UniqueUser.Entities;

namespace Sistran.Core.Application.UniqueUserServices.EEProvider.DAOs
{
    public class UserBranchDAO
    {
        /// <summary>
        /// Delete UseBranch by userId
        /// </summary>
        /// <param name="userId">userId</param>
        public void DeleteUserBranchByUserId(int userId, int branchId)
        {
            DeleteQuery delete = new DeleteQuery();
            delete.Table = new ClassNameTable(typeof(UUEN.UserBranch));
            ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();
            filter.Property(UUEN.UserBranch.Properties.UserId);
            filter.Equal();
            filter.Constant(userId);
            if (branchId > 0)
            {
                filter.And();
                filter.Property(UUEN.UserBranch.Properties.BranchCode);
                filter.Equal();
                filter.Constant(branchId);
            }
            delete.Where = filter.GetPredicate();
            DataFacadeManager.Instance.GetDataFacade().Execute(delete);
        }
        /// <summary>
        /// Obtener lista de sucursales asociadas a un usuario
        /// </summary>
        /// <param name="userId">Identificador usuario</param>
        /// <returns>Lista de sucursales</returns>
        public List<COMML.Branch> GetBranchesByUserId(int userId)
        {
            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();
            UserBranchView view = new UserBranchView();
            ViewBuilder builder = new ViewBuilder("UserBranchView");

            ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();
            filter.Property(UUEN.UserBranch.Properties.UserId, typeof(UUEN.UserBranch).Name);
            filter.Equal();
            filter.Constant(userId);
            filter.And();
            filter.Property(COMMEN.CoBranch.Properties.IsIssue, typeof(COMMEN.CoBranch).Name);
            filter.Equal();
            filter.Constant(1);

            builder.Filter = filter.GetPredicate();
            
            DataFacadeManager.Instance.GetDataFacade().FillView(builder, view);
            stopWatch.Stop();
            Debug.WriteLine(stopWatch.Elapsed.ToString(), "Sistran.Core.Application.CommonService.Providers.GetBranchesByUserId");
            return ModelAssembler.CreateBranches(view.Branches, view.UserBranches);
        }

        public COMML.Branch GetDefaultBranchesByUserId(int userId)
        {
            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();
            UserBranchView view = new UserBranchView();
            ViewBuilder builder = new ViewBuilder("UserBranchView");

            ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();
            filter.Property(UUEN.UserBranch.Properties.UserId, typeof(UUEN.UserBranch).Name);
            filter.Equal();
            filter.Constant(userId);
            filter.And();
            filter.Property(UUEN.UserBranch.Properties.DefaultBranch, typeof(UUEN.UserBranch).Name);
            filter.Equal();
            filter.Constant(true);
            builder.Filter = filter.GetPredicate();

            DataFacadeManager.Instance.GetDataFacade().FillView(builder, view);
            stopWatch.Stop();
            Debug.WriteLine(stopWatch.Elapsed.ToString(), "Sistran.Core.Application.CommonService.Providers.GetDefaultBranchesByUserId");
            return ModelAssembler.CreateBranches(view.Branches, view.UserBranches).FirstOrDefault();
        }
    }
}
