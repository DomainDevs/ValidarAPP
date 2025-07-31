using Sistran.Core.Application.CommonService.Models;
using Sistran.Core.Application.UniqueUserServices.EEProvider.Assemblers;
using Sistran.Core.Application.UniqueUserServices.EEProvider.Entities.Views;
using Sistran.Core.Application.Utilities.DataFacade;
using Sistran.Core.Framework.DAF;
using Sistran.Core.Framework.DAF.Engine;
using Sistran.Core.Framework.Queries;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;
using etcommon = Sistran.Core.Application.Common.Entities;
using Model = Sistran.Core.Application.CommonService.Models;
using UUEN = Sistran.Core.Application.UniqueUser.Entities;

namespace Sistran.Core.Application.UniqueUserServices.EEProvider.DAOs
{
    /// <summary>
    /// Dao UserSalePoint
    /// </summary>
    public class UserSalePointDAO
    {

        /// <summary>
        /// Delete UserSalesPoint by userId
        /// </summary>
        /// <param name="userId">userId</param>
        public void DeleteUserSalesPointByUserId(int userId, int branchId)
        {
            DeleteQuery delete = new DeleteQuery();
            delete.Table = new ClassNameTable(typeof(UUEN.UserSalePoint));
            ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();
            filter.Property(UUEN.UserSalePoint.Properties.UserId);
            filter.Equal();
            filter.Constant(userId);
            if (branchId > 0)
            {
                filter.And();
                filter.Property(UUEN.UserSalePoint.Properties.BranchCode);
                filter.Equal();
                filter.Constant(branchId);
            }
            delete.Where = filter.GetPredicate();
            DataFacadeManager.Instance.GetDataFacade().Execute(delete);
        }
        public void DeleteUserBranchByUserId(int userId)
        {
            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();
            DeleteQuery delete = new DeleteQuery();
            delete.Table = new ClassNameTable(typeof(UUEN.UserBranch));
            ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();
            filter.Property(UUEN.UserBranch.Properties.UserId);
            filter.Equal();
            filter.Constant(userId);
            delete.Where = filter.GetPredicate();
            DataFacadeManager.Instance.GetDataFacade().Execute(delete);
            stopWatch.Stop();
            Debug.WriteLine(stopWatch.Elapsed.ToString(), "Sistran.Core.Application.UniqueUserServices.EEProvider.DAOs.DeleteUserBranchByUserId");
        }
        /// <summary>
        /// Save UserSalesPoint
        /// </summary>
        /// <param name="salesPoint">List of salesPoint</param>
        /// <param name="userId">userId</param>

        public void CreateUserSalePoint(SalePoint salePoint, int branchId, int userId)
        {
            PrimaryKey key = UUEN.UserSalePoint.CreatePrimaryKey(userId, branchId, salePoint.Id);
            UUEN.UserSalePoint entity = (UUEN.UserSalePoint)DataFacadeManager.Instance.GetDataFacade().GetObjectByPrimaryKey(key);
            entity = Assemblers.EntityAssembler.CreateUserSalePoint(salePoint, branchId, userId);
            DataFacadeManager.Instance.GetDataFacade().InsertObject(entity);
        }

        public void CreatSalesPoint(Branch branch, int userId)
        {
            this.DeleteUserSalesPointByUserId(userId, branch.Id);
            foreach (var item in branch.SalePoints)
            {
                if (item.Id != 0)
                {
                    CreateUserSalePoint(item, branch.Id, userId);
                }
            }
        }

        /// <summary>
        /// CreateUserBranch
        /// </summary>
        /// <param name="branch">list of branch</param>
        /// <param name="userId">userId</param>
        public void CreateBranchs(List<Branch> branch, int userId)
        {

            foreach (Branch item in branch)
            {
                PrimaryKey key = UUEN.UserBranch.CreatePrimaryKey(userId, item.Id);
                UUEN.UserBranch entity = (UUEN.UserBranch)DataFacadeManager.Instance.GetDataFacade().GetObjectByPrimaryKey(key);

                if (entity == null)
                {
                    entity = Assemblers.EntityAssembler.CreateUserBranch(item, userId);
                    entity.DefaultClaimBranch = entity.DefaultClaimBranch == null ? false : entity.DefaultClaimBranch;
                    entity.DefaultPaymentRequestBranch = entity.DefaultPaymentRequestBranch == null ? false : entity.DefaultPaymentRequestBranch;
                    DataFacadeManager.Instance.GetDataFacade().InsertObject(entity);
                }

                if (item.SalePoints != null)
                {
                    CreatSalesPoint(item, userId);
                }
            }

        }

        /// <summary>
        /// Get UserSalePoint ByUserId
        /// </summary>
        /// <param name="userId">userId</param>
        /// <returns> List of UserSalePoint</returns>
        public List<UUEN.UserSalePoint> GetUserSalePointByUserId(int userId)
        {
            List<UUEN.UserSalePoint> entities = new List<UUEN.UserSalePoint>();
            ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();
            filter.Property(UUEN.UserSalePoint.Properties.UserId);
            filter.Equal();
            filter.Constant(userId);

            BusinessCollection businessCollection = new BusinessCollection(DataFacadeManager.Instance.GetDataFacade().SelectObjects(typeof(UUEN.UserSalePoint), filter.GetPredicate()));

            if (businessCollection.Count > 0)
            {
                entities = businessCollection.Cast<UUEN.UserSalePoint>().ToList();
            }
            return entities;
        }

        /// <summary>
        /// Obtener puntos de venta por sucursal
        /// </summary>
        /// <param name="branchId">Id Sucursal</param>
        /// <param name="userId">Id Usuario</param>
        /// <returns>Lista de puntos de venta</returns>
        public List<Model.SalePoint> GetSalePointsByBranchIdUserId(int branchId, int userId)
        {
            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();
            SelectQuery select = new SelectQuery();

            select.Distinct = true;
            select.AddSelectValue(new SelectValue(new Column(etcommon.SalePoint.Properties.SalePointCode, "sp"), "SalePointCode"));
            select.AddSelectValue(new SelectValue(new Column(etcommon.SalePoint.Properties.Description, "sp"), "Description"));
            select.AddSelectValue(new SelectValue(new Column(UUEN.UserSalePoint.Properties.DefaultSalePoint, "usp"), "DefaultSalePoint"));

            Join join;
            if (userId == 0)
                join = new Join(new ClassNameTable(typeof(etcommon.SalePoint), "sp"), new ClassNameTable(typeof(UUEN.UserSalePoint), "usp"), JoinType.Left);
            else
                join = new Join(new ClassNameTable(typeof(etcommon.SalePoint), "sp"), new ClassNameTable(typeof(UUEN.UserSalePoint), "usp"), JoinType.Inner);
            
            join.Criteria = (new ObjectCriteriaBuilder()
                .Property(etcommon.SalePoint.Properties.SalePointCode, "sp")
                .Equal()
                .Property(UUEN.UserSalePoint.Properties.SalePointCode, "usp")
                .And()
                .Property(etcommon.SalePoint.Properties.BranchCode, "sp")
                .Equal()
                .Property(UUEN.UserSalePoint.Properties.BranchCode, "usp")
                .GetPredicate());

            select.Table = join;

            ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();
            if (branchId != 0)
            {
                if (userId == 0)
                    filter.Property(etcommon.SalePoint.Properties.BranchCode, "sp");
                else
                    filter.Property(UUEN.UserSalePoint.Properties.BranchCode, "usp");
                filter.Equal();
                filter.Constant(branchId);
            }
            if (userId != 0)
            {
                filter.And();
                filter.Property(UUEN.UserSalePoint.Properties.UserId, "usp");
                filter.Equal();
                filter.Constant(userId);
            }

            filter.And();
            filter.OpenParenthesis();
            filter.Property(etcommon.SalePoint.Properties.Enabled, "sp");
            filter.Equal();
            filter.Constant(1);
            filter.Or();
            filter.Property(etcommon.SalePoint.Properties.Enabled, "sp");
            filter.IsNull();
            filter.CloseParenthesis();

            select.Where = filter.GetPredicate();
            var salePoints = new List<Model.SalePoint>();
            using (IDataReader reader = Sistran.Core.Application.Utilities.DataFacade.DataFacadeManager.Instance.GetDataFacade().Select(select))
            {
                while (reader.Read())
                {
                    var model = new Model.SalePoint();
                    model.Id = Convert.ToInt32(reader["SalePointCode"]);
                    model.Description = (string)reader["Description"];
                    model.IsDefault = (int?)reader["DefaultSalePoint"] != null ? ((int)reader["DefaultSalePoint"] == 0 ? false : true) : true;
                    salePoints.Add(model);
                }
            }

            return salePoints.GroupBy(x => x.Id).Select(g => g.First()).ToList();
        }


        /// <summary>
        /// Obtener Puntos de Venta por Usuario
        /// </summary>
        /// <param name="userId">Id Usuario</param>
        /// <returns>Puntos de Venta</returns>
        public List<Model.SalePoint> GetSalePointsByUserId(int userId)
        {
            UserSalePointView view = new UserSalePointView();
            ViewBuilder builder = new ViewBuilder("UserSalePointView");

            ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();
            filter.Property(UUEN.UserSalePoint.Properties.UserId, typeof(UUEN.UserSalePoint).Name);
            filter.Equal();
            filter.Constant(userId);
            filter.And();
            filter.Property(etcommon.SalePoint.Properties.Enabled, typeof(etcommon.SalePoint).Name);
            filter.Equal();
            filter.Constant(1);

            builder.Filter = filter.GetPredicate();

            DataFacadeManager.Instance.GetDataFacade().FillView(builder, view);
            List<Model.SalePoint> salePoints = ModelAssembler.CreateSalePoints(view.SalesPoint, view.UserSalesPoint);

            return salePoints;
        }

    }
}
