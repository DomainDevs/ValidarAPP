using System.Linq;
using System.Collections.Generic;
using Sistran.Core.Application.Vehicles.EEProvider.Assemblers;
using Sistran.Core.Framework.DAF;
using Sistran.Core.Framework.Queries;
using System.Diagnostics;
using Sistran.Core.Application.Common.Entities;
using Sistran.Core.Application.Utilities.DataFacade;
using ISSEN = Sistran.Core.Application.Issuance.Entities;
using Sistran.Core.Application.UnderwritingServices.Enums;
using Sistran.Core.Application.Vehicles.EEProvider.Entities.Views;
using Sistran.Core.Framework.DAF.Engine;
using UNDTO = Sistran.Core.Application.UnderwritingServices.DTOs;
using COMMEN = Sistran.Core.Application.Common.Entities;
using System.Data;
using System;
using Sistran.Core.Framework.BAF;
using Sistran.Core.Application.Vehicles.Models;
using PRODEN = Sistran.Core.Application.Product.Entities;

namespace Sistran.Core.Application.Vehicles.EEProvider.DAOs
{
    /// <summary>
    /// Auto
    /// </summary>
    public class VehicleDAO
    {
        /// <summary>
        /// Obtener lista de placas
        /// </summary>
        /// <returns>Lista de marcas</returns>
        public List<string> GetAllLicencePlates()
        {
            BusinessCollection businessCollection = null;
            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();
            using (var daf = DataFacadeManager.Instance.GetDataFacade())
            {
                businessCollection = new BusinessCollection(daf.SelectObjects(typeof(ISSEN.RiskVehicle)));
            }
            
            List<string> licencePlates = businessCollection.Select<ISSEN.RiskVehicle>(x => (ISSEN.RiskVehicle)x).Select(x => x.LicensePlate).ToList();
            //foreach (ISSEN.RiskVehicle field in businessCollection)
            //{
            //    licencePlates.Add(field.LicensePlate);
            //}

            stopWatch.Stop();
            Debug.WriteLine(stopWatch.Elapsed.ToString(), "Sistran.Core.Application.Vehicles.EEProvider.DAOs.GetMakes");
            return licencePlates;
        }
        /// <summary>
        /// Obtener lista de marcas
        /// </summary>
        /// <returns>Lista de marcas</returns>
        public List<Models.Make> GetMakes()
        {
            BusinessCollection businessCollection = null;
            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();
            using (var daf = DataFacadeManager.Instance.GetDataFacade())
            {
                businessCollection = new BusinessCollection(daf.SelectObjects(typeof(VehicleMake)));
            }
            stopWatch.Stop();
            Debug.WriteLine(stopWatch.Elapsed.ToString(), "Sistran.Core.Application.Vehicles.EEProvider.DAOs.GetMakes");
            return ModelAssembler.CreateMakes(businessCollection);
        }
		public List<Models.Make> GetVehicleMakesByDescription(string description)
        {
            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();
            ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();
            filter.Property(VehicleMake.Properties.SmallDescription, typeof(VehicleMake).Name);
            filter.Equal();
            filter.Constant(description);
            BusinessCollection businessCollection = DataFacadeManager.GetObjects(typeof(VehicleMake), filter.GetPredicate());
            return ModelAssembler.CreateMakes(businessCollection);
        }

        public List<Models.Model> GetVehicleModelsByDescription(string description)
        {
            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();
            ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();
            filter.Property(VehicleModel.Properties.SmallDescription, typeof(VehicleModel).Name);
            filter.Equal();
            filter.Constant(description);
            BusinessCollection businessCollection = DataFacadeManager.GetObjects(typeof(VehicleModel), filter.GetPredicate());
            return ModelAssembler.CreateModels(businessCollection);
        }
        /// <summary>
        /// Obtener lista de tipos
        /// </summary>
        /// <returns>Lista de tipos</returns>
        public List<Models.Type> GetTypes()
        {
            BusinessCollection businessCollection = null;
            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();
            using (var daf = DataFacadeManager.Instance.GetDataFacade())
            {
                businessCollection = new BusinessCollection(daf.SelectObjects(typeof(VehicleType)));

            }
            stopWatch.Stop();
            Debug.WriteLine(stopWatch.Elapsed.ToString(), "Sistran.Core.Application.Vehicles.EEProvider.DAOs.GetTypes");
            return ModelAssembler.CreateTypes(businessCollection);
        }

        /// <summary>
        /// Obtener lista de colores
        /// </summary>
        /// <returns>Lista de colores</returns>
        public List<Models.Color> GetColors()
        {
            BusinessCollection businessCollection = null;
            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();
            using (var daf = DataFacadeManager.Instance.GetDataFacade())
            {
                businessCollection = new BusinessCollection(daf.SelectObjects(typeof(VehicleColor)));
            }
            stopWatch.Stop();
            Debug.WriteLine(stopWatch.Elapsed.ToString(), "Sistran.Core.Application.Vehicles.EEProvider.DAOs.GetColors");
            return ModelAssembler.CreateColors(businessCollection);
        }

        /// <summary>
        /// Obtener color por identificador 
        /// </summary>
        /// <returns>Lista de colores</returns>
        public Models.Color GetColorById(int id)
        {
            VehicleColor vehicleColor = null;
            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();
            PrimaryKey key = VehicleColor.CreatePrimaryKey(id);
            using (var daf = DataFacadeManager.Instance.GetDataFacade())
            {
                vehicleColor = (VehicleColor)daf.GetObjectByPrimaryKey(key);
            }
            stopWatch.Stop();
            Debug.WriteLine(stopWatch.Elapsed.ToString(), "Sistran.Core.Application.Vehicles.EEProvider.DAOs.GetColorById");
            return ModelAssembler.CreateColor(vehicleColor);
        }


        /// <summary>
        /// Obtener lista de modelos por marca
        /// </summary>
        /// <param name="makeId">Id marca</param>
        /// <returns>Lista de modelos</returns>
        public List<Models.Model> GetModelsByMakeId(int makeId)
        {
            BusinessCollection businessCollection = null;
            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();
            ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();
            filter.Property(VehicleModel.Properties.VehicleMakeCode, typeof(VehicleModel).Name);
            filter.Equal();
            filter.Constant(makeId);
            using (var daf = DataFacadeManager.Instance.GetDataFacade())
            {
                businessCollection = new BusinessCollection(daf.SelectObjects(typeof(VehicleModel), filter.GetPredicate()));

            }
            stopWatch.Stop();
            Debug.WriteLine(stopWatch.Elapsed.ToString(), "Sistran.Core.Application.Vehicles.EEProvider.DAOs.GetModelsByMakeId");
            return ModelAssembler.CreateModels(businessCollection);
        }

        /// <summary>
        /// Obtener lista de modelos por marca y descripción
        /// </summary>
        /// <param name="makeId">Id marca</param>
        /// <returns>Lista de modelos</returns>
        public Models.Model GetModelByMakeIdModelId(int makeId, int modelId)
        {
            BusinessCollection businessCollection = null;
            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();
            ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();
            filter.Property(VehicleModel.Properties.VehicleMakeCode, typeof(VehicleModel).Name);
            filter.Equal();
            filter.Constant(makeId);
            filter.And();
            filter.Property(VehicleModel.Properties.VehicleModelCode, typeof(VehicleModel).Name);
            filter.Equal();
            filter.Constant(modelId);

            using (var daf = DataFacadeManager.Instance.GetDataFacade())
            {
                businessCollection = new BusinessCollection(daf.SelectObjects(typeof(VehicleModel), filter.GetPredicate()));
            }

            Models.Model model = new Models.Model();
            if (businessCollection.Count > 0)
            {
                VehicleModel vehicleModel = (VehicleModel)businessCollection[0];
                model = ModelAssembler.CreateModel(vehicleModel);
            }
            stopWatch.Stop();
            Debug.WriteLine(stopWatch.Elapsed.ToString(), "Sistran.Core.Application.Vehicles.EEProvider.DAOs.GetModelByMakeIdModelId");
            return model;
        }

        /// <summary>
        /// Obtener version por marca, modelo y version
        /// </summary>
        /// <param name="makeId">Id marca</param>
        /// <param name="modelId">Id modelo</param>
        /// <param name="versionId">Id version</param>
        /// <returns>Version</returns>
        public Models.Version GetVersionByVersionIdModelIdMakeId(int versionId, int modelId, int makeId)
        {
            VehicleVersion version = null;
            PrimaryKey key = VehicleVersion.CreatePrimaryKey(versionId, modelId, makeId);
            using (var daf = DataFacadeManager.Instance.GetDataFacade())
            {
                version = (VehicleVersion)daf.GetObjectByPrimaryKey(key);
            }

            return ModelAssembler.CreateVersion(version);
        }

        /// <summary>
        /// Obtener lista de versiones por marca y modelo
        /// </summary>
        /// <param name="makeId">Id marca</param>
        /// <param name="modelId">Id modelo</param>
        /// <returns>Lista de versiones</returns>
        public List<Models.Version> GetVersionsByMakeIdModelId(int makeId, int modelId)
        {
            BusinessCollection businessCollection = null;
            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();
            ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();
            filter.Property(VehicleVersion.Properties.VehicleMakeCode, typeof(VehicleVersion).Name);
            filter.Equal();
            filter.Constant(makeId);
            filter.And();
            filter.Property(VehicleVersion.Properties.VehicleModelCode, typeof(VehicleVersion).Name);
            filter.Equal();
            filter.Constant(modelId);
            using (var daf = DataFacadeManager.Instance.GetDataFacade())
            {
                businessCollection = new BusinessCollection(daf.SelectObjects(typeof(VehicleVersion), filter.GetPredicate()));
            }
            stopWatch.Stop();
            Debug.WriteLine(stopWatch.Elapsed.ToString(), "Sistran.Core.Application.Vehicles.EEProvider.DAOs.GetVersionsByMakeIdModelId");
            return ModelAssembler.CreateVersions(businessCollection);
        }



        /// <summary>
        /// Obtener lista de años por marca, modelo y versión
        /// </summary>
        /// <param name="makeId">Id marca</param>
        /// <param name="modelId">Id modelo</param>
        /// <param name="versionId">Id version</param>
        /// <returns>Lista de años</returns>
        public List<Models.Year> GetYearsByMakeIdModelIdVersionId(int makeId, int modelId, int versionId)
        {
            BusinessCollection businessCollection = null;
            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();
            ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();
            filter.Property(VehicleVersionYear.Properties.VehicleMakeCode, typeof(VehicleVersionYear).Name);
            filter.Equal();
            filter.Constant(makeId);
            filter.And();
            filter.Property(VehicleVersionYear.Properties.VehicleModelCode, typeof(VehicleVersionYear).Name);
            filter.Equal();
            filter.Constant(modelId);
            filter.And();
            filter.Property(VehicleVersionYear.Properties.VehicleVersionCode, typeof(VehicleVersionYear).Name);
            filter.Equal();
            filter.Constant(versionId);
            using (var daf = DataFacadeManager.Instance.GetDataFacade())
            {
                businessCollection = new BusinessCollection(daf.SelectObjects(typeof(VehicleVersionYear), filter.GetPredicate()));
            }
            stopWatch.Stop();
            Debug.WriteLine(stopWatch.Elapsed.ToString(), "Sistran.Core.Application.Vehicles.EEProvider.DAOs.GetYearsByMakeIdModelIdVersionId");
            return ModelAssembler.CreateYears(businessCollection);
        }



        /// <summary>
        /// Obtener lista marca por IdMarke
        /// </summary>
        /// <returns>Lista de marcas</returns>
        public Models.Make GetMakeByVehicleMakeCd(int vehicleMakeCd)
        {
            BusinessCollection businessCollection = null;
            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();
            ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();
            filter.Property(VehicleMake.Properties.VehicleMakeCode, typeof(VehicleMake).Name);
            filter.Equal();
            filter.Constant(vehicleMakeCd);
            using (var daf = DataFacadeManager.Instance.GetDataFacade())
            {
                businessCollection = new BusinessCollection(daf.SelectObjects(typeof(VehicleMake), filter.GetPredicate()));
            }

            Models.Make make = new Models.Make();
            if (businessCollection.Count > 0)
            {
                VehicleMake vehicleMake = (VehicleMake)businessCollection[0];
                make = ModelAssembler.CreateMake(vehicleMake);
            }

            stopWatch.Stop();
            Debug.WriteLine(stopWatch.Elapsed.ToString(), "Sistran.Core.Application.Vehicles.EEProvider.DAOs.GetMakeByVehicleMakeCd");
            return make;
        }

        /// <summary>
        /// Obtener tipo por IdType
        /// </summary>
        /// <returns>Tipo de acuerdo al id</returns>
        public Models.Type GetTypesByVehicleTypeCd(int vehicleTypeCd)
        {
            BusinessCollection businessCollection = null;
            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();
            ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();
            filter.Property(VehicleType.Properties.VehicleTypeCode, typeof(VehicleType).Name);
            filter.Equal();
            filter.Constant(vehicleTypeCd);
            using (var daf = DataFacadeManager.Instance.GetDataFacade())
            {
                businessCollection = new BusinessCollection(daf.SelectObjects(typeof(VehicleType), filter.GetPredicate()));
            }

            Models.Type type = new Models.Type();
            if (businessCollection.Count > 0)
            {
                VehicleType vehicleType = (VehicleType)businessCollection[0];
                type = ModelAssembler.CreateType(vehicleType);

            }

            stopWatch.Stop();
            Debug.WriteLine(stopWatch.Elapsed.ToString(), "Sistran.Core.Application.Vehicles.EEProvider.DAOs.GetTypesByVehicleTypeCd");
            return type;
        }

        /// <summary>
        /// Consulta si la placa, motor o chasis ya existe en una póliza
        /// </summary>
        /// <param name="licensePlate">Placa</param>
        /// <param name="engineNumber">Motor</param>
        /// <param name="chassisNumber">Chasis</param>
        /// <param name="productId">Id Producto</param>
        /// <returns>Mensaje</returns>
        public string ExistsRiskByLicensePlateEngineNumberChassisNumberProductId(string licensePlate, string engineNumber, string chassisNumber, int productId, int endorsementId, DateTime currentFrom)
        {
            ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();

            if (licensePlate != "TL")
            {
                filter.Property(ISSEN.RiskVehicle.Properties.LicensePlate, typeof(ISSEN.RiskVehicle).Name).Equal().Constant(licensePlate);
                filter.Or();
            }

            filter.Property(ISSEN.RiskVehicle.Properties.EngineSerNo, typeof(ISSEN.RiskVehicle).Name).Equal().Constant(engineNumber);
            filter.Or();
            filter.Property(ISSEN.RiskVehicle.Properties.ChassisSerNo, typeof(ISSEN.RiskVehicle).Name).Equal().Constant(chassisNumber);
            filter.And();
            filter.Property(ISSEN.EndorsementRisk.Properties.IsCurrent, typeof(ISSEN.EndorsementRisk).Name).Equal().Constant(1);
            filter.And();
            filter.Not();
            filter.Property(ISSEN.EndorsementRisk.Properties.RiskStatusCode, typeof(ISSEN.EndorsementRisk).Name);
            filter.In();
            filter.ListValue();
            filter.Constant((int)RiskStatusType.Excluded);
            filter.Constant((int)RiskStatusType.Cancelled);
            filter.EndList();
            filter.And();
            filter.Property(ISSEN.Policy.Properties.CurrentTo, typeof(ISSEN.Policy).Name).Greater().Constant(currentFrom);

            //if (productId > 0)
            //{
            //    filter.And();
            //    filter.Property(ISSEN.Policy.Properties.ProductId, typeof(ISSEN.Policy).Name);
            //    filter.Equal();
            //    filter.Constant(productId);
            //}

            if (endorsementId > 0)
            {
                filter.And();
                filter.Property(ISSEN.EndorsementRisk.Properties.EndorsementId, typeof(ISSEN.EndorsementRisk).Name);
                filter.Distinct();
                filter.Constant(endorsementId);
            }

            ExistsVehicleView view = new ExistsVehicleView();
            ViewBuilder builder = new ViewBuilder("ExistsVehicleView");

            builder.Filter = filter.GetPredicate();
            DataFacadeManager.Instance.GetDataFacade().FillView(builder, view);
            DataFacadeManager.Dispose();

            if (view.RiskVehicles.Count > 0)
            {
                var risksVehicles = view.RiskVehicles.Cast<ISSEN.RiskVehicle>();
                var message = "";

                var vehicleEngine = risksVehicles.FirstOrDefault(x => x.EngineSerNo == engineNumber);
                if (vehicleEngine != null)
                {
                    ISSEN.Policy policy = view.Policies.Cast<ISSEN.Policy>().SingleOrDefault(X => X.PolicyId ==
                    (view.EndorsementRisks.Cast<ISSEN.EndorsementRisk>().SingleOrDefault(y => y.RiskId == vehicleEngine.RiskId).PolicyId));
                    return CreateMessageExist(vehicleEngine, policy);
                }

                var vehiclePlate = risksVehicles.FirstOrDefault(x => x.LicensePlate == licensePlate);
                if (vehiclePlate != null)
                {
                    ISSEN.Policy policy = view.Policies.Cast<ISSEN.Policy>().FirstOrDefault(X => X.PolicyId ==
                   (view.EndorsementRisks.Cast<ISSEN.EndorsementRisk>().FirstOrDefault(y => y.RiskId == vehiclePlate.RiskId).PolicyId));
                    return CreateMessageExist(vehiclePlate, policy);
                }

                var vehicleChasis = risksVehicles.FirstOrDefault(x => x.ChassisSerNo == chassisNumber);
                if (vehicleChasis != null)
                {
                    ISSEN.Policy policy = view.Policies.Cast<ISSEN.Policy>().FirstOrDefault(X => X.PolicyId ==
                   (view.EndorsementRisks.Cast<ISSEN.EndorsementRisk>().FirstOrDefault(y => y.RiskId == vehicleChasis.RiskId).PolicyId));
                    return CreateMessageExist(vehicleChasis, policy);
                }

                return message;
            }
            else
            {
                return "";
            }
        }

        public string CreateMessageExist(ISSEN.RiskVehicle vehicle, ISSEN.Policy policy)
        {
            ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();
            filter.Property(PRODEN.Product.Properties.ProductId, typeof(PRODEN.Product).Name)
            .Equal()
            .Constant(policy.ProductId);
            PRODEN.Product ProductEntity = DataFacadeManager.Instance.GetDataFacade().List(typeof(PRODEN.Product), filter.GetPredicate()).Cast<PRODEN.Product>().FirstOrDefault();
            DataFacadeManager.Dispose();

            return $@"**El vehiculo ya posee una poliza vigente. 

                       Vehiculo:
                       Placa = {vehicle.LicensePlate} 
                       Número de chasis = {vehicle.ChassisSerNo} 
                       Número de motor = {vehicle.EngineSerNo}
                       Sucursal = {policy.BranchCode}
                       Producto = {ProductEntity.Description}
                       Nro Poliza = {policy.DocumentNumber} 
                       Fecha de inicio = {policy.CurrentFrom } 
                       Fecha de fin = {policy.CurrentTo}.";
        }


        /// <summary>
        /// Consulta los datos basicos de las polizas que esten asociadas a la placa 
        /// </summary>
        /// <param name="licensePlate"></param>
        /// <returns>poliza con datos basicos</returns>
        public List<UNDTO.PolicyRiskDTO> GetPolicyRiskDTOsByLicensePlate(string licensePlate)
        {
            List<UNDTO.PolicyRiskDTO> policyRiskDTOs = new List<UNDTO.PolicyRiskDTO>();
            List<UNDTO.PolicyRiskDTO> policyRiskDTOsVehicles = new List<UNDTO.PolicyRiskDTO>();

            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();
            ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();
            filter.Property(ISSEN.RiskVehicle.Properties.LicensePlate);
            filter.Equal();
            filter.Constant(licensePlate);
            filter.And();
            filter.Property(ISSEN.EndorsementRisk.Properties.IsCurrent);
            filter.Equal();
            filter.Constant(true);
            ExistsVehicleView view = new ExistsVehicleView();
            ViewBuilder builder = new ViewBuilder("ExistsVehicleView");
            builder.Filter = filter.GetPredicate();
            DataFacadeManager.Instance.GetDataFacade().FillView(builder, view);
            DataFacadeManager.Dispose();
            policyRiskDTOs = ModelAssembler.CreatePolicyRiskDTOs(view.Policies);
            List<COMMEN.Prefix> prefixes = view.Prefixes.Cast<COMMEN.Prefix>().ToList();
            List<COMMEN.Branch> branchs = view.Branches.Cast<COMMEN.Branch>().ToList();
            for (int i = 0; i < policyRiskDTOs.Count; i++)
            {
                policyRiskDTOs[i].PolicyId = view.EndorsementRisks.Cast<ISSEN.EndorsementRisk>().ToList()[i].PolicyId;
                policyRiskDTOs[i].EndorsementId = view.EndorsementRisks.Cast<ISSEN.EndorsementRisk>().ToList()[i].EndorsementId;
                policyRiskDTOs[i].RiskId = view.RiskVehicles.Cast<ISSEN.RiskVehicle>().ToList()[i].RiskId;
            }
            foreach (var riskVehicle in policyRiskDTOs)
            {
                riskVehicle.PrefixDescription = prefixes.Where(b => b.PrefixCode == riskVehicle.PrefixId).FirstOrDefault().Description;
                riskVehicle.BranchDescription = branchs.Where(b => b.BranchCode == riskVehicle.BranchId).FirstOrDefault().Description;
                policyRiskDTOsVehicles.Add(riskVehicle);
            }
            stopWatch.Stop();
            Debug.WriteLine(stopWatch.Elapsed.ToString(), "Sistran.Core.Application.Vehicles.EEProvider.DAOs.GetPoliciesByLicensePlate");

            if (policyRiskDTOsVehicles.Count > 0)
            {
                policyRiskDTOsVehicles = policyRiskDTOsVehicles.GroupBy(b => new { b.BranchId, b.PrefixId, b.DocumentNumber }).Select(b => b.LastOrDefault()).ToList();
            }
            return policyRiskDTOsVehicles;
        }

        /// <summary>
        /// Retorna maximo Id de vehicleVersion
        /// </summary>
        /// <returns></returns>
        public int GetMaxVehicleVersionId()
        {
            #region MyRegion
            try
            {
                int vehicleVersionId = 0;
                SelectQuery selectQuery = new SelectQuery();
                Function function = new Function(FunctionType.Max);

                function.AddParameter(new Column(VehicleVersion.Properties.VehicleVersionCode));

                selectQuery.Table = new ClassNameTable(typeof(VehicleVersion));
                selectQuery.AddSelectValue(new SelectValue(function));

                using (IDataReader reader = DataFacadeManager.Instance.GetDataFacade().Select(selectQuery))
                {
                    while (reader.Read())
                    {
                        vehicleVersionId = Convert.ToInt32(reader[0]);
                    }
                }

                return vehicleVersionId;
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
            #endregion


            //int maxVersionId = DataFacadeManager.GetObjects(typeof(VehicleVersion)).Cast<VehicleVersion>().Max(v => v.VehicleVersionCode);

            //return maxVersionId;
        }
        /// <summary>
        /// Crear VehicleVersion
        /// </summary>
        /// <param name="VehicleVersion"></param>
        /// <returns></returns>
        public Models.Version CreateVersion(Models.Version vehicleVersion)
        {
            vehicleVersion.Id = GetMaxVehicleVersionId() + 1;
            VehicleVersion vehicleVersionEntity = EntityAssembler.CreateVehicleVersion(vehicleVersion);

            DataFacadeManager.Insert(vehicleVersionEntity);
            DataFacadeManager.Dispose();

            return ModelAssembler.CreateVersion(vehicleVersionEntity);
        }


        /// <summary>
        /// Actualizar VehicleVersion
        /// </summary>
        /// <param name="VehicleVersion"></param>
        /// <returns></returns>
        public Models.Version UpdateVersion(Models.Version vehicleVersion)
        {
            VehicleVersion vehicleVersionEntity = EntityAssembler.CreateVehicleVersion(vehicleVersion);

            DataFacadeManager.Update(vehicleVersionEntity);
            DataFacadeManager.Dispose();

            return ModelAssembler.CreateVersion(vehicleVersionEntity);
        }

        public List<Models.Version> GetVehicleVersionByDescription(string description)
        {
            List<Models.Version> ListVersions = new List<Models.Version>();
            ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();
            if (!string.IsNullOrEmpty(description))
            {
                filter.Property(VehicleVersion.Properties.Description, typeof(VehicleVersion).Name);
                filter.Like();
                filter.Constant("%" + description + "%");
            }
            ClauseVehicleVersion view = new ClauseVehicleVersion();
            ViewBuilder builder = new ViewBuilder("ClauseVehicleVersion");
            builder.Filter = filter.GetPredicate();

            DataFacadeManager.Instance.GetDataFacade().FillView(builder, view);
            DataFacadeManager.Dispose();
            ListVersions = ModelAssembler.CreateVersions(view.VehicleVersion);
            return ListVersions;
        }

        internal List<Models.Version> GetVehicleVersionByMakeIdModelId(int? makeCode, int? modelCode, string description)
        {
            List<Models.Version> ListVersions = new List<Models.Version>();
            ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();
            if (!string.IsNullOrEmpty(description))
            {
                filter.Property(VehicleVersion.Properties.Description, typeof(VehicleVersion).Name);
                filter.Like();
                filter.Constant("%" + description + "%");
            }
            if (makeCode > 0)
            {
                if (!string.IsNullOrEmpty(description))
                {
                    filter.And();
                }

                filter.Property(VehicleVersion.Properties.VehicleMakeCode, typeof(VehicleVersion).Name);
                filter.Equal();
                filter.Constant(makeCode);
            }
            if (modelCode > 0)
            {
                if (makeCode > 0)
                {
                    filter.And();
                }
                filter.Property(VehicleVersion.Properties.VehicleModelCode, typeof(VehicleVersion).Name);
                filter.Equal();
                filter.Constant(modelCode);
            }

            ClauseVehicleVersion view = new ClauseVehicleVersion();
            ViewBuilder builder = new ViewBuilder("ClauseVehicleVersion");
            builder.Filter = filter.GetPredicate();
            DataFacadeManager.Instance.GetDataFacade().FillView(builder, view);
            DataFacadeManager.Dispose();

            ListVersions = ModelAssembler.CreateVersions(view.VehicleVersion);
            return ListVersions;
        }

        internal void DeleteVehicleVersion(int id, int makeId, int modelId)
        {
            PrimaryKey primaryKey = VehicleVersion.CreatePrimaryKey(id, modelId, makeId);
            DataFacadeManager.Delete(primaryKey);
            DataFacadeManager.Dispose();
        }

        /// <summary>
        /// Obtener Precio del Vehículo
        /// </summary>
        /// <param name="makeId">Id Marca</param>
        /// <param name="modelId">Id Modelo</param>
        /// <param name="versionId">Id Version</param>
        /// <param name="year">Año</param>
        /// <returns>Precio Vehículo</returns>
        public decimal GetPriceByMakeIdModelIdVersionId(int makeId, int modelId, int versionId, int year)
        {
            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();
            if (year == 0)
            {
                ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();
                filter.Property(VehicleVersionYear.Properties.VehicleMakeCode, typeof(VehicleVersionYear).Name);
                filter.Equal();
                filter.Constant(makeId);
                filter.And();
                filter.Property(VehicleVersionYear.Properties.VehicleModelCode, typeof(VehicleVersionYear).Name);
                filter.Equal();
                filter.Constant(modelId);
                filter.And();
                filter.Property(VehicleVersionYear.Properties.VehicleVersionCode, typeof(VehicleVersionYear).Name);
                filter.Equal();
                filter.Constant(versionId);

                BusinessCollection businessCollection = new BusinessCollection(DataFacadeManager.Instance.GetDataFacade().SelectObjects(typeof(VehicleVersionYear), filter.GetPredicate()));
                List<Vehicles.Models.Year> years = Vehicles.EEProvider.Assemblers.ModelAssembler.CreateYears(businessCollection);

                years = years.OrderByDescending(x => x.Description).ToList();

                stopWatch.Stop();
                Debug.WriteLine(stopWatch.Elapsed.ToString(), "Sistran.Core.Application.Vehicles.VehicleServices.EEProvider.DAOs.GetPriceByMakeIdModelIdVersionId");
                return years[0].Price;
            }
            else
            {
                PrimaryKey key = VehicleVersionYear.CreatePrimaryKey(versionId, modelId, makeId, year);
                VehicleVersionYear versionYear = (VehicleVersionYear)DataFacadeManager.Instance.GetDataFacade().GetObjectByPrimaryKey(key);

                stopWatch.Stop();
                Debug.WriteLine(stopWatch.Elapsed.ToString(), "Sistran.Core.Application.Vehicles.VehicleServices.EEProvider.DAOs.GetPriceByMakeIdModelIdVersionId");

                if (versionYear != null)
                    return Vehicles.EEProvider.Assemblers.ModelAssembler.CreateYear(versionYear).Price;

                return 0;
            }
        }
    }
}