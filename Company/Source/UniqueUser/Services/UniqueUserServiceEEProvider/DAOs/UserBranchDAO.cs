using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sistran.Company.Application.UniqueUserServices.Models;
using System.Diagnostics;
using Sistran.Company.Application.UniqueUserServices.EEProvider.Entities.Views;
using Sistran.Core.Framework.DAF.Engine;
using Sistran.Core.Framework.Queries;
using UUEN = Sistran.Core.Application.UniqueUser.Entities;
using COMMEN = Sistran.Core.Application.Common.Entities;
using Sistran.Core.Application.Utilities.DataFacade;

namespace Sistran.Company.Application.UniqueUserServices.EEProvider.DAOs
{
    public class UserBranchDAO
    {
        /// <summary>
        /// Obtener lista de sucursales asociadas a un usuario
        /// </summary>
        /// <param name="userId">Identificador usuario</param>
        /// <returns>Lista de sucursales</returns>
        public List<CompanyUserBranch> GetCompanyBranchesByUserId(int userId, int isIssue)
        {
            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();
            CompanyUserBranchView view = new CompanyUserBranchView();
            ViewBuilder builder = new ViewBuilder("CompanyUserBranchView");

            ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();
            filter.Property(UUEN.UserBranch.Properties.UserId, typeof(UUEN.UserBranch).Name);
            filter.Equal();
            filter.Constant(userId);
            filter.And();
            filter.Property(COMMEN.CoBranch.Properties.IsIssue, typeof(COMMEN.CoBranch).Name);
            filter.Equal();
            filter.Constant(isIssue);

            builder.Filter = filter.GetPredicate();

            DataFacadeManager.Instance.GetDataFacade().FillView(builder, view);
            stopWatch.Stop();
            Debug.WriteLine(stopWatch.Elapsed.ToString(), "Sistran.Core.Application.CommonService.Providers.GetBranchesByUserId");
            return Assemblers.ModelAssembler.CreateBranches(view.Branches, view.UserBranches);
        }
    }
}
