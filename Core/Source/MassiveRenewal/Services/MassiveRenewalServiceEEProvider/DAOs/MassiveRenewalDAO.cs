using AutoMapper;
using Newtonsoft.Json;
using Sistran.Core.Application.CommonService.Enums;
using Sistran.Core.Application.CommonService.Models;
using Sistran.Core.Application.MassiveRenewalServices.EEProvider.Assemblers;
using Sistran.Core.Application.MassiveRenewalServices.EEProvider.Entities.Views;
using Sistran.Core.Application.MassiveRenewalServices.Entities.Views;
using Sistran.Core.Application.MassiveRenewalServices.Enums;
using Sistran.Core.Application.MassiveRenewalServices.Models;
using Sistran.Core.Application.MassiveServices.Models;
using Sistran.Core.Application.UnderwritingServices.Models;
using Sistran.Core.Application.Utilities.Constants;
using Sistran.Core.Application.Utilities.DataFacade;
using Sistran.Core.Framework.DAF;
using Sistran.Core.Framework.DAF.Engine;
using Sistran.Core.Framework.Queries;
using Sistran.Core.Services.UtilitiesServices.Enums;
using Sistran.Core.Services.UtilitiesServices.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using COMMEN = Sistran.Core.Application.Common.Entities;
using ENT = Sistran.Core.Application.Massive.Entities;
using ISSEN = Sistran.Core.Application.Issuance.Entities;

namespace Sistran.Core.Application.MassiveRenewalServices.EEProvider.DAOs
{
    public class MassiveRenewalDAO
    {
         #region Massive Renewal
        public MassiveRenewal CreateMassiveRenewal(MassiveRenewal massiveRenewal)
        {
            MassiveLoad massiveLoad = DelegateService.massiveServiceCore.CreateMassiveLoad(massiveRenewal);
            if (massiveLoad == null)
            {
                return null;
            }
            massiveRenewal.Id = massiveLoad.Id;
            ENT.MassiveRenewal massiveRenewalEntity = EntityAssembler.CreateMassiveRenewal(massiveRenewal);
            DataFacadeManager.Instance.GetDataFacade().InsertObject(massiveRenewalEntity);
            if (massiveRenewalEntity == null)
            {
                return null;
            }
            return massiveRenewal;
        }

        public MassiveRenewal UpdateMassiveRenewal(MassiveRenewal massiveRenewal)
        {
            MassiveLoad massiveLoad = DelegateService.massiveServiceCore.UpdateMassiveLoad(massiveRenewal);
            if (massiveLoad == null)
            {
                return null;
            }
            PrimaryKey primaryKey = ENT.MassiveRenewal.CreatePrimaryKey(massiveRenewal.Id);
            ENT.MassiveRenewal massiveRenewalEntity = (ENT.MassiveRenewal)DataFacadeManager.Instance.GetDataFacade().GetObjectByPrimaryKey(primaryKey);
            if (massiveRenewalEntity == null)
            {
                return null;
            }
            massiveRenewalEntity.CurrentFrom = massiveRenewal.CurrentFrom;
            massiveRenewalEntity.CurrentTo = massiveRenewal.CurrentTo;
            if (massiveRenewal.Agency != null)
            {
                massiveRenewalEntity.AgencyId = massiveRenewal.Agency.Id;
            }
            if (massiveRenewal.Prefix != null)
            {
                massiveRenewalEntity.PrefixId = massiveRenewal.Prefix.Id;
            }
            massiveRenewalEntity.RequestId = massiveRenewal.RequestId;
            if (massiveRenewal.Product != null)
            {
                massiveRenewalEntity.ProductId = massiveRenewal.Product.Id;
            }
            if (massiveRenewal.Hoder != null)
            {
                massiveRenewalEntity.HolderId = massiveRenewal.Hoder.InsuredId;
            }

            DataFacadeManager.Instance.GetDataFacade().UpdateObject(massiveRenewalEntity);
            return massiveRenewal;
        }
        
        public MassiveRenewal GetMassiveRenewalByMassiveLoadIdWithRowsErrors(int massiveLoadId, bool withRows, bool? withError, bool? withEvent)
        {
            MassiveRenewalView view = new MassiveRenewalView();
            ViewBuilder builder = new ViewBuilder("MassiveRenewalView");

            ObjectCriteriaBuilder filterView = new ObjectCriteriaBuilder();
            filterView.Property(ENT.MassiveLoad.Properties.Id, typeof(ENT.MassiveLoad).Name);
            filterView.Equal();
            filterView.Constant(massiveLoadId);
            builder.Filter = filterView.GetPredicate();
            DataFacadeManager.Instance.GetDataFacade().FillView(builder, view);

            ENT.MassiveLoad massiveLoadView = view.MassiveLoad.Cast<ENT.MassiveLoad>().FirstOrDefault();
            ENT.MassiveRenewal massiveRenealView = view.MassiveRenewal.Cast<ENT.MassiveRenewal>().FirstOrDefault();
            if (massiveLoadView == null || massiveRenealView == null)
                return null;
            MassiveRenewal massiveRenewal = ModelAssembler.CreateMassiveRenewal(massiveLoadView, massiveRenealView);

            if (withRows)
            {
                ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();
                filter.Property(ENT.MassiveRenewalRow.Properties.MassiveLoadId, typeof(ENT.MassiveRenewalRow).Name);
                filter.Equal();
                filter.Constant(massiveLoadId);
                if (withError.HasValue && withError.Value && withEvent.HasValue && withEvent.Value)
                {
                    filter.And();
                    filter.OpenParenthesis();
                    filter.Property(ENT.MassiveRenewalRow.Properties.HasError, typeof(ENT.MassiveRenewalRow).Name);
                    filter.Equal();
                    filter.Constant(withError.Value);
                    filter.Or();
                    filter.Property(ENT.MassiveRenewalRow.Properties.HasEvents, typeof(ENT.MassiveRenewalRow).Name);
                    filter.Equal();
                    filter.Constant(withEvent.Value);
                    filter.CloseParenthesis();
                }
                else
                {
                    if (withError.HasValue)
                    {
                        filter.And();
                        filter.Property(ENT.MassiveRenewalRow.Properties.HasError, typeof(ENT.MassiveRenewalRow).Name);
                        filter.Equal();
                        filter.Constant(withError.Value);
                    }

                    if (withEvent.HasValue)
                    {
                        filter.And();
                        filter.Property(ENT.MassiveRenewalRow.Properties.HasEvents, typeof(ENT.MassiveRenewalRow).Name);
                        filter.Equal();
                        filter.Constant(withEvent.Value);
                    }
                }


                BusinessCollection businessCollection = new BusinessCollection(DataFacadeManager.Instance.GetDataFacade().SelectObjects(typeof(ENT.MassiveRenewalRow), filter.GetPredicate()));
                List<MassiveRenewalRow> massiveRenewalRows = ModelAssembler.CreateMassiveRenewalRows(businessCollection);
                massiveRenewal.Rows = massiveRenewalRows;
            }
            return massiveRenewal;
        }


        /// <summary>
        /// Genera el archivo de error del proceso de emisión masiva
        /// </summary>
        /// <param name="massiveLoadProccessId"></param>
        /// <returns></returns>
        public string GenerateFileErrorsMassiveRenewal(int massiveLoadId)
        {
            MassiveRenewal massiveRenewal = new MassiveRenewal();
            massiveRenewal = GetMassiveRenewalByMassiveLoadIdWithRowsErrors(massiveLoadId, true, true,null);
            SubCoveredRiskType subCoveredRiskType = DelegateService.commonServiceCore.GetSubCoveredRiskTypeByPrefixIdCoveredRiskTypeId(massiveRenewal.Prefix.Id, (int)massiveRenewal.CoveredRiskType);

            FileProcessValue fileProcessValue = new FileProcessValue();

            switch (subCoveredRiskType)
            {

                case SubCoveredRiskType.Vehicle:

                    fileProcessValue.Key1 = (int)FileProcessType.MassiveRenewal;
                    fileProcessValue.Key4 = massiveRenewal.Prefix.Id;
                    fileProcessValue.Key5 = (int)SubCoveredRiskType.Vehicle;

                    break;
                case SubCoveredRiskType.ThirdPartyLiability:

                    fileProcessValue.Key1 = (int)FileProcessType.MassiveRenewal;
                    fileProcessValue.Key4 = massiveRenewal.Prefix.Id;
                    fileProcessValue.Key5 = (int)SubCoveredRiskType.ThirdPartyLiability;

                    break;
                case SubCoveredRiskType.Property:

                    fileProcessValue.Key1 = (int)FileProcessType.MassiveRenewal;
                    fileProcessValue.Key4 = massiveRenewal.Prefix.Id;
                    fileProcessValue.Key5 = (int)SubCoveredRiskType.Property;

                    break;
                case SubCoveredRiskType.Liability:

                    fileProcessValue.Key1 = (int)FileProcessType.MassiveRenewal;
                    fileProcessValue.Key4 = massiveRenewal.Prefix.Id;
                    fileProcessValue.Key5 = (int)SubCoveredRiskType.Liability;

                    break;

                default:
                    break;
            }

            File file = DelegateService.utilitiesServiceCore.GetFileByFileProcessValue(fileProcessValue);

            file.Templates[0].Rows.Last().Fields.Add(new Field()
            {
                ColumnSpan = 1,
                FieldType = FieldType.String,
                Description = "Errores",
                IsEnabled = true,
                IsMandatory = false,
                Id = 0,
                Order = file.Templates[0].Rows.Last().Fields.Count(),
                RowPosition = file.Templates[0].Rows.Last().Fields.First().RowPosition
            });

            foreach (MassiveRenewalRow proccess in massiveRenewal.Rows)
            {

                if (proccess.HasError.GetValueOrDefault() && proccess.SerializedRow != null)
                {
                    File fileSerialized = JsonConvert.DeserializeObject<File>(proccess.SerializedRow);

                    foreach (Template t in fileSerialized.Templates)
                    {
                        file.Templates.Find(x => x.PropertyName == t.PropertyName).Rows.AddRange(t.Rows);
                    }

                    file.Templates[0].Rows.Last().Fields.Add(new Field()
                    {
                        ColumnSpan = 1,
                        FieldType = FieldType.String,
                        Value = proccess.Observations,
                        IsEnabled = true,
                        IsMandatory = false,
                        Id = 0,
                        Order = file.Templates[0].Rows.Last().Fields.Count(),
                        RowPosition = file.Templates[0].Rows.Last().Fields.First().RowPosition
                    });
                }
            }

            file.Name = "Errores_" + DateTime.Now.ToString("dd_MM_yyyy_ssms");
            return DelegateService.utilitiesServiceCore.GenerateFile(file);
        }

        /// <summary>
        /// Generar Archivo Proceso Renovación
        /// </summary>
        /// <param name="renewalProcesses">Procesos</param>
        /// <param name="renewalStatus">Estado Proceso</param>
        /// <param name="fileName">Nombre Archivo</param>
        /// <returns>Ruta Archivo</returns>
        public string GenerateFileToMassiveRenewal(List<MassiveRenewalRow> massiveRenewalRows, ProcessRenewalStatus renewalStatus, string fileName)
        {
            if (renewalStatus == ProcessRenewalStatus.Temporals)
            {
                return GenerateFileToMassiveRenewalTemporals(massiveRenewalRows, fileName);
            }
            else if (renewalStatus == ProcessRenewalStatus.Finalized)
            {
                return GenerateFileToMassiveRenewalPolicies(massiveRenewalRows, fileName);
            }
            else
            {
                return "";
            }
        }

        /// <summary>
        /// Generar Archivo Proceso Renovación Temporales
        /// </summary>
        /// <param name="renewalProcesses">Procesos</param>
        /// <param name="fileName">Nombre Archivo</param>
        /// <returns>Ruta Archivo</returns>
        private string GenerateFileToMassiveRenewalTemporals(List<MassiveRenewalRow> massiveRenewalRows, string fileName)
        {
            FileProcessValue fileProcessValue = new FileProcessValue();
            fileProcessValue.Key1 = (int)FileProcessType.MassiveRenewal;

            File file = DelegateService.utilitiesServiceCore.GetFileByFileProcessValue(fileProcessValue);

            if (file != null && file.IsEnabled)
            {
                file.Name = fileName;
                List<Row> rows = new List<Row>();

                foreach (MassiveRenewalRow massiveRenewalRow in massiveRenewalRows)
                {
                    Row row = new Row();
                    row.Fields = new List<Field>();


                    row.Fields.Add(NewField(file.Templates[0].Rows[0].Fields, FieldPropertyName.Estado, massiveRenewalRow.Status.ToString()));
                    row.Fields.Add(NewField(file.Templates[0].Rows[0].Fields, FieldPropertyName.Temporal, massiveRenewalRow.Risk.Policy.Id.ToString()));
                    row.Fields.Add(NewField(file.Templates[0].Rows[0].Fields, FieldPropertyName.Riesgos, massiveRenewalRow.Risk.Policy.Summary.Description));
                    row.Fields.Add(NewField(file.Templates[0].Rows[0].Fields, FieldPropertyName.Suma, massiveRenewalRow.Risk.Policy.Summary.AmountInsured.ToString()));
                    row.Fields.Add(NewField(file.Templates[0].Rows[0].Fields, FieldPropertyName.Prima, massiveRenewalRow.Risk.Policy.Summary.FullPremium.ToString()));
                    row.Fields.Add(NewField(file.Templates[0].Rows[0].Fields, FieldPropertyName.Producto, massiveRenewalRow.Risk.Policy.Product.Description));
                    row.Fields.Add(NewField(file.Templates[0].Rows[0].Fields, FieldPropertyName.Sucursal, massiveRenewalRow.Risk.Policy.Branch.Description));
                    row.Fields.Add(NewField(file.Templates[0].Rows[0].Fields, FieldPropertyName.Poliza, massiveRenewalRow.Risk.Policy.DocumentNumber.ToString()));
                    row.Fields.Add(NewField(file.Templates[0].Rows[0].Fields, FieldPropertyName.Tomador, massiveRenewalRow.Risk.Policy.Holder.Name));
                    row.Fields.Add(NewField(file.Templates[0].Rows[0].Fields, FieldPropertyName.Intermediario, massiveRenewalRow.Risk.Policy.Agencies[0].FullName));
                    row.Fields.Add(NewField(file.Templates[0].Rows[0].Fields, FieldPropertyName.Observaciones, string.IsNullOrEmpty(massiveRenewalRow.Observations) ? "" : massiveRenewalRow.Observations));

                    row.Fields = row.Fields.Where(x => x != null).OrderBy(x => x.Order).ToList();
                    rows.Add(row);
                }

                file.Templates[0].Rows = rows;

                return DelegateService.utilitiesServiceCore.GenerateFile(file);
            }
            else
            {
                return "";
            }
        }

        /// <summary>
        /// Generar Archivo Proceso Renovación Pólizas
        /// </summary>
        /// <param name="renewalProcesses">Procesos</param>
        /// <param name="fileName">Nombre Archivo</param>
        /// <returns>Ruta Archivo</returns>
        private string GenerateFileToMassiveRenewalPolicies(List<MassiveRenewalRow> massiveRenewalRows, string fileName)
        {
            FileProcessValue fileProcessValue = new FileProcessValue();
            fileProcessValue.Key1 = (int)FileProcessType.MassiveRenewal;

            File file = DelegateService.utilitiesServiceCore.GetFileByFileProcessValue(fileProcessValue);

            if (file != null && file.IsEnabled)
            {
                file.Name = fileName;
                List<Row> rows = new List<Row>();

                foreach (MassiveRenewalRow massiveRenewalRow in massiveRenewalRows)
                {
                    Row row = new Row();
                    row.Fields = new List<Field>();

                    row.Fields.Add(NewField(file.Templates[0].Rows[0].Fields, FieldPropertyName.Observaciones, string.IsNullOrEmpty(massiveRenewalRow.Observations) ? "" : massiveRenewalRow.Observations));
                    row.Fields.Add(NewField(file.Templates[0].Rows[0].Fields, FieldPropertyName.Sucursal, massiveRenewalRow.Risk.Policy.Branch.Description));
                    row.Fields.Add(NewField(file.Templates[0].Rows[0].Fields, FieldPropertyName.Poliza, massiveRenewalRow.Risk.Policy.DocumentNumber.ToString()));
                    row.Fields.Add(NewField(file.Templates[0].Rows[0].Fields, FieldPropertyName.Endoso, massiveRenewalRow.Risk.Policy.Endorsement.Number.ToString()));
                    row.Fields.Add(NewField(file.Templates[0].Rows[0].Fields, FieldPropertyName.Tomador, massiveRenewalRow.Risk.Policy.Holder.Name));
                    row.Fields.Add(NewField(file.Templates[0].Rows[0].Fields, FieldPropertyName.Intermediario, massiveRenewalRow.Risk.Policy.Agencies[0].FullName));
                    row.Fields.Add(NewField(file.Templates[0].Rows[0].Fields, FieldPropertyName.Producto, massiveRenewalRow.Risk.Policy.Product.Description));
                    row.Fields.Add(NewField(file.Templates[0].Rows[0].Fields, FieldPropertyName.Riesgos, massiveRenewalRow.Risk.Policy.Summary.Description));
                    row.Fields.Add(NewField(file.Templates[0].Rows[0].Fields, FieldPropertyName.Suma, massiveRenewalRow.Risk.Policy.Summary.AmountInsured.ToString()));
                    row.Fields.Add(NewField(file.Templates[0].Rows[0].Fields, FieldPropertyName.Prima, massiveRenewalRow.Risk.Policy.Summary.FullPremium.ToString()));


                    row.Fields = row.Fields.Where(x => x != null).OrderBy(x => x.Order).ToList();
                    rows.Add(row);
                }

                file.Templates[0].Rows = rows;

                return DelegateService.utilitiesServiceCore.GenerateFile(file);
            }
            else
            {
                return "";
            }
        }

        /// <summary>
        /// Generar Planilla Intermediario Ubicación
        /// </summary>
        /// <param name="renewalProcesses">Procesos</param>
        /// <param name="fileName">Nombre Archivo</param>
        /// <returns>Ruta Archivo</returns>
        public string GenerateFileToPayrollByAgentLocation(List<MassiveRenewalRow> massiveRenewalRows, string fileName)
        {
            FileProcessValue fileProcessValue = new FileProcessValue();
            fileProcessValue.Key1 = (int)FileProcessType.PayrollAgent;

            File file = DelegateService.utilitiesServiceCore.GetFileByFileProcessValue(fileProcessValue);

            if (file != null && file.IsEnabled)
            {
                file.Name = fileName;
                List<Row> rows = new List<Row>();

                foreach (MassiveRenewalRow massiveRenewalRow in massiveRenewalRows)
                {
                    foreach (Sistran.Core.Application.UnderwritingServices.Models.Risk risk in massiveRenewalRow.Risk.Policy.Summary.Risks)
                    {
                        Row row = new Row();
                        row.Fields = new List<Field>();

                        //row.Fields.Add(NewField(file.Templates[0].Rows[0].Fields, FieldPropertyName.Temporal, ""));
                        row.Fields.Add(NewField(file.Templates[0].Rows[0].Fields, FieldPropertyName.Poliza, massiveRenewalRow.Risk.Policy.DocumentNumber.ToString()));
                        row.Fields.Add(NewField(file.Templates[0].Rows[0].Fields, FieldPropertyName.FecDesde, massiveRenewalRow.Risk.Policy.CurrentFrom.ToString()));
                        row.Fields.Add(NewField(file.Templates[0].Rows[0].Fields, FieldPropertyName.FecHasta, massiveRenewalRow.Risk.Policy.CurrentTo.ToString()));
                        row.Fields.Add(NewField(file.Templates[0].Rows[0].Fields, FieldPropertyName.Tomador, massiveRenewalRow.Risk.Policy.Holder.Name));
                        row.Fields.Add(NewField(file.Templates[0].Rows[0].Fields, FieldPropertyName.Documento, massiveRenewalRow.Risk.Policy.Holder.IdentificationDocument.Number));

                        //List<Phone> phones = DelegateService.uniquePersonServiceCore.GetPhonesByIndividualId(massiveRenewalRow.Risk.Policy.Holder.IndividualId);

                        //row.Fields.Add(NewField(file.Templates[0].Rows[0].Fields, FieldPropertyName.TelefonoCliente, phones.Count > 0 ? phones.First().Description : ""));


                        ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();
                        filter.Property(ISSEN.RiskLocation.Properties.RiskId, typeof(ISSEN.RiskLocation).Name).Equal().Constant(risk.RiskId);

                        RiskLocationDataPayrollView riskLocationDataPayrollView = new RiskLocationDataPayrollView();
                        ViewBuilder builder = new ViewBuilder("RiskLocationDataPayrollView");
                        builder.Filter = filter.GetPredicate();

                        DataFacadeManager.Instance.GetDataFacade().FillView(builder, riskLocationDataPayrollView);

                        if (riskLocationDataPayrollView.RiskLocations.Count > 0)
                        {
                            row.Fields.Add(NewField(file.Templates[0].Rows[0].Fields, FieldPropertyName.Riesgos,
                                riskLocationDataPayrollView.RiskLocations.Cast<ISSEN.RiskLocation>().First().Street +
                                " - " + riskLocationDataPayrollView.States.Cast<COMMEN.State>().First().Description +
                                " - " + riskLocationDataPayrollView.Cities.Cast<COMMEN.City>().First().Description +
                                " - " + riskLocationDataPayrollView.RiskCommercialClasses
                                    .Cast<RiskCommercialClass>().First().Description));
                        }
                        else
                        {
                            row.Fields.Add(NewField(file.Templates[0].Rows[0].Fields, FieldPropertyName.Riesgos, ""));
                        }

                        row.Fields.Add(NewField(file.Templates[0].Rows[0].Fields, FieldPropertyName.ValorAsegurado, massiveRenewalRow.Risk.Policy.Summary.AmountInsured.ToString()));
                        row.Fields.Add(NewField(file.Templates[0].Rows[0].Fields, FieldPropertyName.Tasa, ""));
                        row.Fields.Add(NewField(file.Templates[0].Rows[0].Fields, FieldPropertyName.PrimaNeta, massiveRenewalRow.Risk.Policy.Summary.Premium.ToString()));
                        row.Fields.Add(NewField(file.Templates[0].Rows[0].Fields, FieldPropertyName.GastosEmision, massiveRenewalRow.Risk.Policy.Summary.Expenses.ToString()));
                        row.Fields.Add(NewField(file.Templates[0].Rows[0].Fields, FieldPropertyName.Prima, massiveRenewalRow.Risk.Policy.Summary.FullPremium.ToString()));

                        row.Fields.Add(NewField(file.Templates[0].Rows[0].Fields, FieldPropertyName.SarlaftTomador, ""));
                        row.Fields.Add(NewField(file.Templates[0].Rows[0].Fields, FieldPropertyName.SarlaftAsegurado, ""));

                        row.Fields = row.Fields.Where(x => x != null).OrderBy(x => x.Order).ToList();
                        rows.Add(row);
                    }
                }

                file.Templates[0].Rows = rows;

                return DelegateService.utilitiesServiceCore.GenerateFile(file);
            }
            else
            {
                return "";
            }
        }

        /// <summary>
        /// Generar Planilla Intermediario Vehiculos
        /// </summary>
        /// <param name="renewalProcesses">Procesos</param>
        /// <param name="fileName">Nombre Archivo</param>
        /// <returns>Ruta Archivo</returns>
        public File GenerateFileToPayrollByAgentVehicle(List<MassiveRenewalRow> massiveRenewalRows, string fileName)
        {
            FileProcessValue fileProcessValue = new FileProcessValue();
            fileProcessValue.Key1 = (int)FileProcessType.PayrollAgent;

            File file = DelegateService.utilitiesServiceCore.GetFileByFileProcessValue(fileProcessValue);

            if (file != null && file.IsEnabled)
            {
                file.Name = fileName;
                List<Row> rows = new List<Row>();

                foreach (MassiveRenewalRow massiveRenewalRow in massiveRenewalRows)
                {
                    foreach (Risk risk in massiveRenewalRow.Risk.Policy.Summary.Risks)
                    {
                        Row row = new Row();
                        row.Fields = new List<Field>();

                        row.Fields.Add(NewField(file.Templates[0].Rows[0].Fields, FieldPropertyName.Poliza, massiveRenewalRow.Risk.Policy.DocumentNumber.ToString()));
                        row.Fields.Add(NewField(file.Templates[0].Rows[0].Fields, FieldPropertyName.FecDesde, massiveRenewalRow.Risk.Policy.CurrentFrom.ToString()));
                        row.Fields.Add(NewField(file.Templates[0].Rows[0].Fields, FieldPropertyName.FecHasta, massiveRenewalRow.Risk.Policy.CurrentTo.ToString()));
                        row.Fields.Add(NewField(file.Templates[0].Rows[0].Fields, FieldPropertyName.Tomador, massiveRenewalRow.Risk.Policy.Holder.Name));
                        row.Fields.Add(NewField(file.Templates[0].Rows[0].Fields, FieldPropertyName.Documento, massiveRenewalRow.Risk.Policy.Holder.IdentificationDocument.Number));

                        //List<Phone> phones = DelegateService.uniquePersonServiceCore.GetPhonesByIndividualId(massiveRenewalRow.Risk.Policy.Holder.IndividualId);

                        //row.Fields.Add(NewField(file.Templates[0].Rows[0].Fields, FieldPropertyName.TelefonoCliente, phones.Count > 0 ? phones.First().Description : ""));


                        ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();
                        filter.Property(ISSEN.RiskVehicle.Properties.RiskId, typeof(ISSEN.RiskVehicle).Name).Equal().Constant(risk.RiskId);

                        RiskVehicleDataPayrollView riskVehicleDataPayrollView = new RiskVehicleDataPayrollView();
                        ViewBuilder builder = new ViewBuilder("RiskVehicleDataPayrollView");
                        builder.Filter = filter.GetPredicate();

                        DataFacadeManager.Instance.GetDataFacade().FillView(builder, riskVehicleDataPayrollView);

                        if (riskVehicleDataPayrollView.RiskVehicles.Count > 0)
                        {
                            row.Fields.Add(NewField(file.Templates[0].Rows[0].Fields, FieldPropertyName.Riesgos, riskVehicleDataPayrollView.RiskVehicles.Cast<ISSEN.RiskVehicle>().First().LicensePlate + " - " + riskVehicleDataPayrollView.VehicleTypes.Cast<COMMEN.VehicleType>().First().Description + " - " + riskVehicleDataPayrollView.VehicleMakes.Cast<COMMEN.VehicleMake>().First().SmallDescription + " - " + riskVehicleDataPayrollView.VehicleModels.Cast<COMMEN.VehicleModel>().First().Description));
                            row.Fields.Add(NewField(file.Templates[0].Rows[0].Fields, FieldPropertyName.ValorAsegurado, riskVehicleDataPayrollView.RiskVehicles.Cast<ISSEN.RiskVehicle>().First().VehiclePrice.ToString()));
                            row.Fields.Add(NewField(file.Templates[0].Rows[0].Fields, FieldPropertyName.Tasa, riskVehicleDataPayrollView.CoRiskVehicles.Cast<ISSEN.CoRiskVehicle>().First().FlatRatePercentage.GetValueOrDefault().ToString()));
                        }
                        else
                        {
                            row.Fields.Add(NewField(file.Templates[0].Rows[0].Fields, FieldPropertyName.Riesgos, ""));
                            row.Fields.Add(NewField(file.Templates[0].Rows[0].Fields, FieldPropertyName.ValorAsegurado, ""));
                            row.Fields.Add(NewField(file.Templates[0].Rows[0].Fields, FieldPropertyName.Tasa, ""));
                        }

                        row.Fields.Add(NewField(file.Templates[0].Rows[0].Fields, FieldPropertyName.PrimaNeta, massiveRenewalRow.Risk.Policy.Summary.Premium.ToString()));
                        row.Fields.Add(NewField(file.Templates[0].Rows[0].Fields, FieldPropertyName.GastosEmision, massiveRenewalRow.Risk.Policy.Summary.Expenses.ToString()));
                        row.Fields.Add(NewField(file.Templates[0].Rows[0].Fields, FieldPropertyName.Prima, massiveRenewalRow.Risk.Policy.Summary.FullPremium.ToString()));


                        row.Fields.Add(NewField(file.Templates[0].Rows[0].Fields, FieldPropertyName.SarlaftTomador, ""));
                        row.Fields.Add(NewField(file.Templates[0].Rows[0].Fields, FieldPropertyName.SarlaftAsegurado, ""));

                        row.Fields = row.Fields.Where(x => x != null).OrderBy(x => x.Order).ToList();
                        rows.Add(row);
                    }
                }

                file.Templates[0].Rows = rows;

                return file;
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// Crear Nueva Instancia de Field
        /// </summary>
        /// <param name="oldField">Field Origen</param>
        /// <returns>Nuevo Field</returns>
        private Field NewField(List<Field> fields, string fieldPropertyName, string value)
        {
            Field oldField = fields.FirstOrDefault(x => x.PropertyName == fieldPropertyName);
            Field field = new Field();
            
            return null;
        }
        #endregion
    }
}