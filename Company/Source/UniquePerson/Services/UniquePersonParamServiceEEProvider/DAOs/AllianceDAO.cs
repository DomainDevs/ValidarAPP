// -----------------------------------------------------------------------
// <copyright file="AllianceDAO.cs" company="SISTRAN">
// Copyright (c) SISTRAN ANDINA. All rights reserved.
// </copyright>
// <author>Juan Sebastián Cárdenas Leiva</author>
// -----------------------------------------------------------------------

namespace Sistran.Company.Application.UniquePersonParamService.EEProvider.DAOs
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Sistran.Company.Application.UniquePerson.Entities;    
    using Sistran.Core.Application.Utilities.DataFacade;
    using Sistran.Core.Framework.DAF;    
    using Sistran.Core.Application.Common.Entities;
    using System.Data;
    using Sistran.Core.Framework.Transactions;
    using Sistran.Company.Application.UniquePersonParamService.Models;
    using Sistran.Company.Application.UniquePersonParamService.EEProvider.Assemblers;
    using Sistran.Core.Framework.Queries;
    using Sistran.Company.Application.UniquePersonParamService.EEProvider.Resources;

    /// <summary>
    /// Acceso a datos de aliados
    /// </summary>
    public class AllianceDAO
    {
        /// <summary>
        /// Obtiene todos los aliados
        /// </summary>
        /// <returns>Listado de aliados</returns>
        public List<Alliance> GetAllAlliances()
        {
            BusinessCollection businessCollection = new BusinessCollection(DataFacadeManager.Instance.GetDataFacade().SelectObjects(typeof(CptAlliance)));
            return ModelAssembler.CreateAlliances(businessCollection);
        }
        
        /// <summary>
        /// Ejecuta las operaciones de Crear, modificar y borrar aliados
        /// </summary>
        /// <param name="alliances">Listado de aliados</param>
        /// <returns>Listado de todos los aliados</returns>
        public List<string> ExecuteOprationsAlliances(List<Alliance> alliances)
        {
            List<string> result = new List<string>();
            if (alliances.Any(a => a.Status == "create"))
            {
                result.AddRange(CreateAlliance(alliances.FindAll(a => a.Status == "create")));
            }
            if (alliances.Any(a => a.Status == "update"))
            {
                result.AddRange(UpdateAlliance(alliances.FindAll(a => a.Status == "update")));
            }
            if (alliances.Any(a => a.Status == "delete"))
            {
                result.AddRange(DeleteAlliances(alliances.FindAll(a => a.Status == "delete")));
            }
            return result;
        }

        /// <summary>
        /// Persiste en la base de datos los nuevos aliados
        /// </summary>
        /// <param name="alliancesToCreate">Listado de aliados</param>
        /// <returns>Listado de nuevos aliado</returns>
        private List<string> CreateAlliance(List<Alliance> alliancesToCreate)
        {
            List<string> result = new List<string>();
            int createCount = 0;
            if (alliancesToCreate != null)
            {
                foreach (Alliance alliance in alliancesToCreate)
                {
                    try
                    {
                        PrimaryKey key = CptAlliance.CreatePrimaryKey(alliance.AllianceId);
                        CptAlliance allianceEntity = new CptAlliance(alliance.AllianceId);
                        allianceEntity = (CptAlliance)DataFacadeManager.Instance.GetDataFacade().GetObjectByPrimaryKey(key);
                        if (allianceEntity == null)
                        {
                            alliance.AllianceId = GetIdAlliance();
                            allianceEntity = EntityAssembler.CreateAlliance(alliance);
                            DataFacadeManager.Instance.GetDataFacade().InsertObject(allianceEntity);
                            createCount++;
                        }
                    }
                    catch (Exception)
                    {
                        result.Add(Errors.ErrorCreateAlliance + " " + alliance.Description + ". ");
                    }
                }
            }
            result.Add(String.Format("Se crearon {0} registros", createCount));
            return result;
        }

        /// <summary>
        /// Actualiza en la base de datos los aliados
        /// </summary>
        /// <param name="alliancesToUpdate">Listado de aliados</param>
        /// <returns>Nuevo listado de aliados</returns>
        private List<string> UpdateAlliance(List<Alliance> alliancesToUpdate)
        {
            List<string> result = new List<string>();
            int updateCount = 0;
            foreach (Alliance alliance in alliancesToUpdate)
            {
                try
                {
                    PrimaryKey key = CptAlliance.CreatePrimaryKey(alliance.AllianceId);
                    CptAlliance allianceEntity = new CptAlliance(alliance.AllianceId);
                    allianceEntity = (CptAlliance)DataFacadeManager.Instance.GetDataFacade().GetObjectByPrimaryKey(key);
                    allianceEntity.Description = alliance.Description;
                    allianceEntity.IsFine = alliance.IsFine;
                    allianceEntity.IsScore = alliance.IsScore;
                    DataFacadeManager.Instance.GetDataFacade().UpdateObject(allianceEntity);
                    updateCount++;
                }
                catch (Exception ex)
                {
                    result.Add(Errors.ErrorUpdateAlliance + " " + alliance.Description + ". " + ex.Message);
                }
            }
            result.Add(String.Format("Se actualizaron {0} registros", updateCount));
            return result;
        }

        /// <summary>
        /// Borra los aliados de la base de datos
        /// </summary>
        /// <param name="alliancesToDelete">Listado de aliados</param>
        /// <returns>Nuevo listado de aliados</returns>
        private List<string> DeleteAlliances(List<Alliance> alliancesToDelete)
        {
            List<string> result = new List<string>();
            int deleteCount = 0;
            foreach (Alliance alliance in alliancesToDelete)
            {
                try
                {
                    PrimaryKey key = CptAlliance.CreatePrimaryKey(alliance.AllianceId);
                    CptAlliance perilEntity = new CptAlliance(alliance.AllianceId);
                    perilEntity = (CptAlliance)DataFacadeManager.Instance.GetDataFacade().GetObjectByPrimaryKey(key);
                    DataFacadeManager.Instance.GetDataFacade().DeleteObject(perilEntity);
                    deleteCount++;
                }
                catch (Exception)
                {
                    result.Add(Errors.ErrorDeleteAlliance + " " + alliance.Description + ". " + Errors.ErrorRelatedBranchAlliance);
                }
            }
            result.Add(String.Format("Se borraron {0} registros", deleteCount));
            return result;
        }

        /// <summary>
        /// Obtiene el Id del Peril
        /// </summary>
        /// <returns></returns>
        public int GetIdAlliance()
        {
            return GetAllAlliances().Max(p => p.AllianceId) + 1;
        }

        /// <summary>
        /// Obtiene los aliados por descripcion
        /// </summary>
        /// <param name="description"></param>
        /// <returns>Aliado</returns>
        public List<Alliance> GetAllianceByDescription(string description)
        {
            ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();
            if (!string.IsNullOrEmpty(description))
            {
                filter.Property(CptAlliance.Properties.Description);
                filter.Like();
                filter.Constant("%" + description + "%");
            }
            BusinessCollection businessCollection = new BusinessCollection(DataFacadeManager.Instance.GetDataFacade().SelectObjects(typeof(CptAlliance), filter.GetPredicate()));
            return businessCollection.Select(x => ModelAssembler.CreateAlliance((CptAlliance)x)).OrderBy(p => p.Description).ToList();
        }

        /// <summary>
        /// Obtiene la lista de sucursales de un aliado
        /// </summary>
        /// <param name="allianceId">Identificdor del aliado</param>
        /// <returns>Listado de sucursales</returns>
        public List<BranchAlliance> GetAllBranchAlliancesByAlliancedId(int allianceId)
        {
            SelectQuery select = new SelectQuery();

            select.AddSelectValue(new SelectValue(new Column(CptBranchAlliance.Properties.AllianceId, "ba"), "AllianceId"));
            select.AddSelectValue(new SelectValue(new Column(CptBranchAlliance.Properties.BranchDescription, "ba"), "BranchDescription"));
            select.AddSelectValue(new SelectValue(new Column(CptBranchAlliance.Properties.BranchId, "ba"), "BranchId"));
            select.AddSelectValue(new SelectValue(new Column(CptBranchAlliance.Properties.CountryCD, "ba"), "CountryCD"));
            select.AddSelectValue(new SelectValue(new Column(CptBranchAlliance.Properties.StateCD, "ba"), "StateCD"));
            select.AddSelectValue(new SelectValue(new Column(CptBranchAlliance.Properties.DivipolaID, "ba"), "DivipolaID"));

            select.AddSelectValue(new SelectValue(new Column(City.Properties.Description, "c"), "CityDescription"));
            select.AddSelectValue(new SelectValue(new Column(State.Properties.Description, "s"), "StateDescription"));
            select.AddSelectValue(new SelectValue(new Column(Country.Properties.Description, "ct"), "CountryDescription"));

            Join join = new Join(new ClassNameTable(typeof(CptBranchAlliance), "ba"), new ClassNameTable(typeof(City), "c"), JoinType.Inner);
            join.Criteria = (new ObjectCriteriaBuilder()
                .Property(CptBranchAlliance.Properties.DivipolaID, "ba")
                .Equal()
                .Property(City.Properties.CityCode, "c")
                .And()
                .Property(CptBranchAlliance.Properties.StateCD, "ba")
                .Equal()
                .Property(City.Properties.StateCode, "c")
                .And()
                .Property(CptBranchAlliance.Properties.CountryCD, "ba")
                .Equal()
                .Property(City.Properties.CountryCode, "c")
                .GetPredicate());

            join = new Join(join, new ClassNameTable(typeof(State), "s"), JoinType.Inner);
            join.Criteria = (new ObjectCriteriaBuilder()
                .Property(State.Properties.StateCode, "s")
                .Equal()
                .Property(CptBranchAlliance.Properties.StateCD, "ba")
                .And()
                .Property(State.Properties.CountryCode, "s")
                .Equal()
                .Property(CptBranchAlliance.Properties.CountryCD, "ba")
                .GetPredicate());

            join = new Join(join, new ClassNameTable(typeof(Country), "ct"), JoinType.Inner);
            join.Criteria = (new ObjectCriteriaBuilder()
                .Property(Country.Properties.CountryCode, "ct")
                .Equal()
                .Property(CptBranchAlliance.Properties.StateCD, "ba")
                .GetPredicate());

            ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();
            filter.Property(CptBranchAlliance.Properties.AllianceId, "ba");
            filter.Equal();
            filter.Constant(allianceId);

            select.Table = join;
            select.Where = filter.GetPredicate();

            List<BranchAlliance> branchAlliances = new List<BranchAlliance>();
            using (IDataReader reader = DataFacadeManager.Instance.GetDataFacade().Select(select))
            {
                while (reader.Read())
                {
                    branchAlliances.Add(ModelAssembler.CreateBrachAlliances(reader));
                }
            }
            return branchAlliances;
        }

        /// <summary>
        /// Obtiene la lista de sucursales (aliados)
        /// </summary>
        /// <returns>Listado de sucursales</returns>
        public List<BranchAlliance> GetAllBranchAlliances()
        {
            SelectQuery select = new SelectQuery();

            select.AddSelectValue(new SelectValue(new Column(CptBranchAlliance.Properties.AllianceId, "ba"), "AllianceId"));
            select.AddSelectValue(new SelectValue(new Column(CptBranchAlliance.Properties.BranchDescription, "ba"), "BranchDescription"));
            select.AddSelectValue(new SelectValue(new Column(CptBranchAlliance.Properties.BranchId, "ba"), "BranchId"));
            select.AddSelectValue(new SelectValue(new Column(CptBranchAlliance.Properties.CountryCD, "ba"), "CountryCD"));
            select.AddSelectValue(new SelectValue(new Column(CptBranchAlliance.Properties.StateCD, "ba"), "StateCD"));
            select.AddSelectValue(new SelectValue(new Column(CptBranchAlliance.Properties.DivipolaID, "ba"), "DivipolaID"));

            select.AddSelectValue(new SelectValue(new Column(City.Properties.Description, "c"), "CityDescription"));
            select.AddSelectValue(new SelectValue(new Column(State.Properties.Description, "s"), "StateDescription"));
            select.AddSelectValue(new SelectValue(new Column(Country.Properties.Description, "ct"), "CountryDescription"));

            Join join = new Join(new ClassNameTable(typeof(CptBranchAlliance), "ba"), new ClassNameTable(typeof(City), "c"), JoinType.Inner);
            join.Criteria = (new ObjectCriteriaBuilder()
                .Property(CptBranchAlliance.Properties.DivipolaID, "ba")
                .Equal()
                .Property(City.Properties.CityCode, "c")
                .And()
                .Property(CptBranchAlliance.Properties.StateCD, "ba")
                .Equal()
                .Property(City.Properties.StateCode, "c")
                .And()
                .Property(CptBranchAlliance.Properties.CountryCD, "ba")
                .Equal()
                .Property(City.Properties.CountryCode, "c")
                .GetPredicate());

            join = new Join(join, new ClassNameTable(typeof(State), "s"), JoinType.Inner);
            join.Criteria = (new ObjectCriteriaBuilder()
                .Property(State.Properties.StateCode, "s")
                .Equal()
                .Property(CptBranchAlliance.Properties.StateCD, "ba")
                .And()
                .Property(State.Properties.CountryCode, "s")
                .Equal()
                .Property(CptBranchAlliance.Properties.CountryCD, "ba")
                .GetPredicate());

            join = new Join(join, new ClassNameTable(typeof(Country), "ct"), JoinType.Inner);
            join.Criteria = (new ObjectCriteriaBuilder()
                .Property(Country.Properties.CountryCode, "ct")
                .Equal()
                .Property(CptBranchAlliance.Properties.StateCD, "ba")
                .GetPredicate());

            select.Table = join;

            List<BranchAlliance> alliances = new List<BranchAlliance>();
            using (IDataReader reader = DataFacadeManager.Instance.GetDataFacade().Select(select))
            {
                while (reader.Read())
                {
                    alliances.Add(ModelAssembler.CreateBrachAlliances(reader));
                }
            }
            return alliances;
        }

        /// <summary>
        /// Obtiene la lista de puntos de venta de sucursales de un aliado
        /// </summary>
        /// <param name="branchId">Identificador de la sucursal</param>
        /// <returns>Listado de puntos de venta</returns>
        public List<AllianceBranchSalePonit> GetAllSalesPointsByBranchId(int branchId, int allianceId)
        {
            ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();
            filter.Property(CptBranchAlliance.Properties.BranchId, typeof(CptBranchAlliance).Name);
            filter.Equal();
            filter.Constant(branchId);
            filter.And();
            filter.Property(CptBranchAlliance.Properties.AllianceId, typeof(CptBranchAlliance).Name);
            filter.Equal();
            filter.Constant(allianceId);
            BusinessCollection<CptAllianceBranchSalePoint> businessCollection = DataFacadeManager.Instance.GetDataFacade().List<CptAllianceBranchSalePoint>(filter.GetPredicate());
            return ModelAssembler.CreateSalesPointsAlliances(businessCollection);
        }

        /// <summary>
        /// Obtiene la lista de todos los puntos de venta aliados
        /// </summary>
        /// <returns>Listado de puntos de venta</returns>
        public List<AllianceBranchSalePonit> GetAllSalesPointsAlliance()
        {
            SelectQuery select = new SelectQuery();

            select.AddSelectValue(new SelectValue(new Column(CptAllianceBranchSalePoint.Properties.AllianceId, "sp"), "AllianceId"));
            select.AddSelectValue(new SelectValue(new Column(CptAllianceBranchSalePoint.Properties.SalePointDescription, "sp"), "SalePointDescription"));
            select.AddSelectValue(new SelectValue(new Column(CptAllianceBranchSalePoint.Properties.BranchId, "sp"), "BranchId"));
            select.AddSelectValue(new SelectValue(new Column(CptAllianceBranchSalePoint.Properties.SalePointId, "sp"), "SalePointId"));
            select.AddSelectValue(new SelectValue(new Column(CptAllianceBranchSalePoint.Properties.SalePointDescription, "sp"), "SalePointDescription"));

            select.AddSelectValue(new SelectValue(new Column(CptBranchAlliance.Properties.BranchDescription, "ba"), "BranchDescription"));
            select.AddSelectValue(new SelectValue(new Column(CptAlliance.Properties.Description, "a"), "Description"));

            Join join = new Join(new ClassNameTable(typeof(CptAllianceBranchSalePoint), "sp"), new ClassNameTable(typeof(CptBranchAlliance), "ba"), JoinType.Inner);
            join.Criteria = (new ObjectCriteriaBuilder()
                .Property(CptAllianceBranchSalePoint.Properties.AllianceId, "sp")
                .Equal()
                .Property(CptBranchAlliance.Properties.AllianceId, "ba")
                .And()
                .Property(CptAllianceBranchSalePoint.Properties.BranchId, "sp")
                .Equal()
                .Property(CptBranchAlliance.Properties.BranchId, "ba")
                .GetPredicate());

            join = new Join(join, new ClassNameTable(typeof(CptAlliance), "a"), JoinType.Inner);
            join.Criteria = (new ObjectCriteriaBuilder()
                .Property(CptAllianceBranchSalePoint.Properties.AllianceId, "sp")
                .Equal()
                .Property(CptAlliance.Properties.AllianceId, "a")
                .GetPredicate());

            select.Table = join;

            List<AllianceBranchSalePonit> salesPoints = new List<AllianceBranchSalePonit>();
            using (IDataReader reader = DataFacadeManager.Instance.GetDataFacade().Select(select))
            {
                while (reader.Read())
                {
                    salesPoints.Add(ModelAssembler.CreateSalesPointsAlliances(reader));
                }
            }
            return salesPoints;
        }

        /// <summary>
        /// Ejecuta las operaciones de Crear, modificar y borrar Puntos de venta para aliados
        /// </summary>
        /// <param name="alliances">Listado de aliados</param>
        /// <returns>Listado de todos los aliados</returns>
        public List<BranchAlliance> ExecuteOprationsBranchAlliances(List<BranchAlliance> branchAlliance)
        {
            List<BranchAlliance> lstbranchAlliance = new List<BranchAlliance>();
            if (branchAlliance.Any(a => a.Status == "create"))
            {
                lstbranchAlliance.AddRange(CreateBranchAlliance(branchAlliance.FindAll(a => a.Status == "create")));
            }
            if (branchAlliance.Any(a => a.Status == "update"))
            {
                lstbranchAlliance.AddRange(UpdateBranchAlliance(branchAlliance.FindAll(a => a.Status == "update")));
            }
            if (branchAlliance.Any(a => a.Status == "delete"))
            {
                lstbranchAlliance.AddRange(DeleteBranchAlliances(branchAlliance.FindAll(a => a.Status == "delete")));
            }

            return lstbranchAlliance;
        }

        /// <summary>
        /// Ejecuta las operaciones de Crear, modificar y borrar Puntos de venta para aliados
        /// </summary>
        /// <param name="salesPonitsAlliance">Listado de puntos de venta</param>
        /// <returns>Listado de todos los puntos de venta</returns>
        public List<AllianceBranchSalePonit> ExecuteOprationsSalesPointsAlliances(List<AllianceBranchSalePonit> salesPonitsAlliance)
        {
            List<AllianceBranchSalePonit> lstbranchAlliance = new List<AllianceBranchSalePonit>();
            if (salesPonitsAlliance.Any(a => a.Status == "create"))
            {
                lstbranchAlliance.AddRange(CreateSalePointAlliance(salesPonitsAlliance.FindAll(a => a.Status == "create")));
            }
            if (salesPonitsAlliance.Any(a => a.Status == "update"))
            {
                lstbranchAlliance.AddRange(UpdateSalePointAlliance(salesPonitsAlliance.FindAll(a => a.Status == "update")));
            }
            if (salesPonitsAlliance.Any(a => a.Status == "delete"))
            {
                lstbranchAlliance.AddRange(DeleteSalePointAlliances(salesPonitsAlliance.FindAll(a => a.Status == "delete"), new Transaction()));
            }
            return lstbranchAlliance;
        }

        /// <summary>
        /// Obtiene sucursal por descripcion
        /// </summary>
        /// <param name="description"></param>
        /// <returns>Sucursal de aliado</returns>
        public List<BranchAlliance> GetBranchAllianceByDescription(string description)
        {            
            SelectQuery select = new SelectQuery();

            select.AddSelectValue(new SelectValue(new Column(CptBranchAlliance.Properties.AllianceId, "ba"), "AllianceId"));
            select.AddSelectValue(new SelectValue(new Column(CptBranchAlliance.Properties.BranchDescription, "ba"), "BranchDescription"));
            select.AddSelectValue(new SelectValue(new Column(CptBranchAlliance.Properties.BranchId, "ba"), "BranchId"));
            select.AddSelectValue(new SelectValue(new Column(CptBranchAlliance.Properties.CountryCD, "ba"), "CountryCD"));
            select.AddSelectValue(new SelectValue(new Column(CptBranchAlliance.Properties.StateCD, "ba"), "StateCD"));
            select.AddSelectValue(new SelectValue(new Column(CptBranchAlliance.Properties.DivipolaID, "ba"), "DivipolaID"));

            select.AddSelectValue(new SelectValue(new Column(City.Properties.Description, "c"), "CityDescription"));
            select.AddSelectValue(new SelectValue(new Column(State.Properties.Description, "s"), "StateDescription"));
            select.AddSelectValue(new SelectValue(new Column(Country.Properties.Description, "ct"), "CountryDescription"));
            select.AddSelectValue(new SelectValue(new Column(CptAlliance.Properties.Description, "a"), "Description"));

            Join join = new Join(new ClassNameTable(typeof(CptBranchAlliance), "ba"), new ClassNameTable(typeof(City), "c"), JoinType.Inner);
            join.Criteria = (new ObjectCriteriaBuilder()
                .Property(CptBranchAlliance.Properties.DivipolaID, "ba")
                .Equal()
                .Property(City.Properties.CityCode, "c")
                .And()
                .Property(CptBranchAlliance.Properties.StateCD, "ba")
                .Equal()
                .Property(City.Properties.StateCode, "c")
                .And()
                .Property(CptBranchAlliance.Properties.CountryCD, "ba")
                .Equal()
                .Property(City.Properties.CountryCode, "c")
                .GetPredicate());

            join = new Join(join, new ClassNameTable(typeof(State), "s"), JoinType.Inner);
            join.Criteria = (new ObjectCriteriaBuilder()
                .Property(State.Properties.StateCode, "s")
                .Equal()
                .Property(CptBranchAlliance.Properties.StateCD, "ba")
                .And()
                .Property(State.Properties.CountryCode, "s")
                .Equal()
                .Property(CptBranchAlliance.Properties.CountryCD, "ba")
                .GetPredicate());

            join = new Join(join, new ClassNameTable(typeof(Country), "ct"), JoinType.Inner);
            join.Criteria = (new ObjectCriteriaBuilder()
                .Property(Country.Properties.CountryCode, "ct")
                .Equal()
                .Property(CptBranchAlliance.Properties.StateCD, "ba")
                .GetPredicate());

            join = new Join(join, new ClassNameTable(typeof(CptAlliance), "a"), JoinType.Inner);
            join.Criteria = (new ObjectCriteriaBuilder()
                .Property(CptAlliance.Properties.AllianceId, "a")
                .Equal()
                .Property(CptBranchAlliance.Properties.AllianceId, "ba")
                .GetPredicate());

            ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();
            filter.Property(CptBranchAlliance.Properties.BranchDescription, "ba");
            filter.Like();
            filter.Constant("%" + description + "%");

            select.Table = join;
            select.Where = filter.GetPredicate();

            List<BranchAlliance> branches = new List<BranchAlliance>();
            using (IDataReader reader = DataFacadeManager.Instance.GetDataFacade().Select(select))
            {
                while (reader.Read())
                {
                    BranchAlliance model = ModelAssembler.CreateBrachAlliances(reader);
                    model.AllianceDescription = reader["Description"].ToString();
                    branches.Add(model);
                }
            }
            return branches;
        }

        /// <summary>
        /// Persiste en la base de datos los nuevos puntos de venta
        /// </summary>
        /// <param name="salesPointsToCreate">Listado de puntos de venta</param>
        /// <returns>Puntos de venta</returns>
        private List<AllianceBranchSalePonit> CreateSalePointAlliance(List<AllianceBranchSalePonit> salesPointsToCreate)
        {
            int branchId = 0;
            int allianceId = 0;
            if (salesPointsToCreate != null)
            {
                branchId = salesPointsToCreate.First().BranchId;
                allianceId = salesPointsToCreate.First().AllianceId;
                using (Transaction transaction = new Transaction())
                {
                    try
                    {
                        foreach (AllianceBranchSalePonit salePoint in salesPointsToCreate)
                        {
                            PrimaryKey key = CptAllianceBranchSalePoint.CreatePrimaryKey(salePoint.AllianceId, salePoint.BranchId, salePoint.SalePointId);
                            CptAllianceBranchSalePoint salePointEntity = new CptAllianceBranchSalePoint(salePoint.AllianceId, salePoint.BranchId, salePoint.SalePointId);
                            salePointEntity = (CptAllianceBranchSalePoint)DataFacadeManager.Instance.GetDataFacade().GetObjectByPrimaryKey(key);
                            if (salePointEntity == null)
                            {
                                salePointEntity = EntityAssembler.CreateSalePointAlliance(salePoint);
                                DataFacadeManager.Instance.GetDataFacade().InsertObject(salePointEntity);
                            }
                        }
                        transaction.Complete();
                    }
                    catch (Exception)
                    {
                        transaction.Dispose();
                    }
                }
            }
            return GetAllSalesPointsByBranchId(branchId, allianceId);
        }

        /// <summary>
        /// Actualiza en la base de datos las sucirsales
        /// </summary>
        /// <param name="salesPointsToUpdate">Listado de sucursales</param>
        /// <returns>Nuevo listado de sucursales</returns>
        private List<AllianceBranchSalePonit> UpdateSalePointAlliance(List<AllianceBranchSalePonit> salesPointsToUpdate)
        {
            int branchId = 0;
            int allianceId = 0;
            using (Transaction transaction = new Transaction())
            {
                branchId = salesPointsToUpdate.First().SalePointId;
                allianceId = salesPointsToUpdate.First().AllianceId;
                try
                {
                    foreach (AllianceBranchSalePonit salePoint in salesPointsToUpdate)
                    {
                        PrimaryKey key = CptAllianceBranchSalePoint.CreatePrimaryKey(salePoint.AllianceId, salePoint.BranchId, salePoint.SalePointId);
                        CptAllianceBranchSalePoint branchAllianceEntity = new CptAllianceBranchSalePoint(salePoint.AllianceId, salePoint.BranchId, salePoint.SalePointId);
                        branchAllianceEntity = (CptAllianceBranchSalePoint)DataFacadeManager.Instance.GetDataFacade().GetObjectByPrimaryKey(key);
                        branchAllianceEntity.SalePointDescription = salePoint.SalePointDescription;
                        DataFacadeManager.Instance.GetDataFacade().UpdateObject(branchAllianceEntity);
                    }
                    transaction.Complete();
                }
                catch (Exception)
                {
                    transaction.Dispose();
                }
            }
            return GetAllSalesPointsByBranchId(branchId, allianceId);
        }

        /// <summary>
        /// Borra los aliados de la base de datos
        /// </summary>
        /// <param name="salesPointsToDelete">Listado de aliados</param>
        /// <returns>Nuevo listado de aliados</returns>
        private List<AllianceBranchSalePonit> DeleteSalePointAlliances(List<AllianceBranchSalePonit> salesPointsToDelete, Transaction transaction)
        {
            int branchId = 0;
            int allianceId = 0;
            using (Transaction transact = transaction)
            {
                branchId = salesPointsToDelete.First().BranchId;
                allianceId = salesPointsToDelete.First().AllianceId;
                try
                {
                    foreach (AllianceBranchSalePonit salePoint in salesPointsToDelete)
                    {
                        PrimaryKey key = CptAllianceBranchSalePoint.CreatePrimaryKey(salePoint.AllianceId, salePoint.BranchId, salePoint.SalePointId);
                        CptAllianceBranchSalePoint perilEntity = new CptAllianceBranchSalePoint(salePoint.AllianceId, salePoint.BranchId, salePoint.SalePointId);
                        perilEntity = (CptAllianceBranchSalePoint)DataFacadeManager.Instance.GetDataFacade().GetObjectByPrimaryKey(key);
                        DataFacadeManager.Instance.GetDataFacade().DeleteObject(perilEntity);
                    }
                    transact.Complete();
                }
                catch (Exception)
                {
                    transact.Dispose();
                }
            }
            return GetAllSalesPointsByBranchId(branchId, allianceId);
        }

        /// <summary>
        /// Obtiene sucursal por descripcion
        /// </summary>
        /// <param name="description"></param>
        /// <returns>Sucursal de aliado</returns>
        private List<AllianceBranchSalePonit> GetSalePointByDescription(string description)
        {
            ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();
            if (!string.IsNullOrEmpty(description))
            {
                filter.Property(CptAllianceBranchSalePoint.Properties.SalePointDescription);
                filter.Equal();
                filter.Constant(description);
            }
            BusinessCollection businessCollection = new BusinessCollection(DataFacadeManager.Instance.GetDataFacade().SelectObjects(typeof(CptAllianceBranchSalePoint), filter.GetPredicate()));
            return businessCollection.Select(x => ModelAssembler.CreateSalePointAlliance((CptAllianceBranchSalePoint)x)).OrderBy(p => p.SalePointDescription).ToList();
        }

        /// <summary>
        /// Persiste en la base de datos las nuevas sucursales
        /// </summary>
        /// <param name="branchAlliancesToCreate">Listodo de sucursales</param>
        /// <returns></returns>
        private List<BranchAlliance> CreateBranchAlliance(List<BranchAlliance> branchAlliancesToCreate)
        {
            int allianceId = 0;
            if (branchAlliancesToCreate != null)
            {
                allianceId = branchAlliancesToCreate.First().AllianceId;
                using (Transaction transaction = new Transaction())
                {
                    try
                    {
                        foreach (BranchAlliance branchAlliance in branchAlliancesToCreate)
                        {
                            PrimaryKey key = CptBranchAlliance.CreatePrimaryKey(branchAlliance.BranchId, branchAlliance.AllianceId);
                            CptBranchAlliance branchAllianceEntity = new CptBranchAlliance(branchAlliance.BranchId, branchAlliance.AllianceId);
                            branchAllianceEntity = (CptBranchAlliance)DataFacadeManager.Instance.GetDataFacade().GetObjectByPrimaryKey(key);
                            if (branchAllianceEntity == null)
                            {
                                branchAllianceEntity = EntityAssembler.CreateBranchAlliance(branchAlliance);
                                DataFacadeManager.Instance.GetDataFacade().InsertObject(branchAllianceEntity);
                            }
                        }
                        transaction.Complete();
                    }
                    catch (Exception)
                    {
                        transaction.Dispose();
                    }
                }
            }
            return GetAllBranchAlliancesByAlliancedId(allianceId);
        }

        /// <summary>
        /// Actualiza en la base de datos las sucirsales
        /// </summary>
        /// <param name="branchAlliancesToUpdate">Listado de sucursales</param>
        /// <returns>Nuevo listado de sucursales</returns>
        private List<BranchAlliance> UpdateBranchAlliance(List<BranchAlliance> branchAlliancesToUpdate)
        {
            int allianceId = 0;
            using (Transaction transaction = new Transaction())
            {
                allianceId = branchAlliancesToUpdate.First().AllianceId;
                try
                {
                    foreach (BranchAlliance branchAlliance in branchAlliancesToUpdate)
                    {
                        PrimaryKey key = CptBranchAlliance.CreatePrimaryKey(branchAlliance.BranchId, branchAlliance.AllianceId);
                        CptBranchAlliance branchAllianceEntity = new CptBranchAlliance(branchAlliance.BranchId, branchAlliance.AllianceId);
                        branchAllianceEntity = (CptBranchAlliance)DataFacadeManager.Instance.GetDataFacade().GetObjectByPrimaryKey(key);
                        branchAllianceEntity.BranchDescription = branchAlliance.BranchDescription;
                        branchAllianceEntity.CountryCD = branchAlliance.CountryCD;
                        branchAllianceEntity.StateCD = branchAlliance.StateCD;
                        branchAllianceEntity.CityCD = branchAlliance.CityCD;
                        DataFacadeManager.Instance.GetDataFacade().UpdateObject(branchAllianceEntity);
                    }
                    transaction.Complete();
                }
                catch (Exception)
                {
                    transaction.Dispose();
                }
            }
            return GetAllBranchAlliancesByAlliancedId(allianceId);
        }

        /// <summary>
        /// Borra los aliados de la base de datos
        /// </summary>
        /// <param name="branchAlliancesToDelete">Listado de aliados</param>
        /// <returns>Nuevo listado de aliados</returns>
        private List<BranchAlliance> DeleteBranchAlliances(List<BranchAlliance> branchAlliancesToDelete)
        {
            int allianceId = 0;
            using (Transaction transact = new Transaction())
            {
                allianceId = branchAlliancesToDelete.First().AllianceId;
                try
                {
                    foreach (BranchAlliance branchAlliance in branchAlliancesToDelete)
                    {
                        branchAlliance.SalesPointsAlliance.ForEach(s => s.Status = "delete");
                        DeleteSalePointAlliances(branchAlliance.SalesPointsAlliance, transact);
                        PrimaryKey key = CptBranchAlliance.CreatePrimaryKey(branchAlliance.BranchId, branchAlliance.AllianceId);
                        CptBranchAlliance perilEntity = new CptBranchAlliance(branchAlliance.BranchId, branchAlliance.AllianceId);
                        perilEntity = (CptBranchAlliance)DataFacadeManager.Instance.GetDataFacade().GetObjectByPrimaryKey(key);
                        DataFacadeManager.Instance.GetDataFacade().DeleteObject(perilEntity);
                    }
                    transact.Complete();
                }
                catch (Exception)
                {
                    transact.Dispose();
                }
            }
            return GetAllBranchAlliancesByAlliancedId(allianceId);
        }

    }
}