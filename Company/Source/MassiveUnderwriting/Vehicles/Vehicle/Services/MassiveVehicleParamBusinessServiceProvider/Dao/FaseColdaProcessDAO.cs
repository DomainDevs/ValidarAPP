using System.Linq;
using Newtonsoft.Json;
using Sistran.Core.Framework.DAF;
using System.Collections.Generic;
using Sistran.Co.Application.Data;
using Sistran.Core.Framework.Queries;
using Sistran.Core.Application.CommonService.Enums;
using Sistran.Core.Application.Utilities.Constants;
using Sistran.Core.Application.Utilities.DataFacade;
using Sistran.Core.Services.UtilitiesServices.Enums;
using Sistran.Core.Services.UtilitiesServices.Models;
using COMMEN = Sistran.Core.Application.Common.Entities;
using Sistran.Company.Application.MassiveVehicleParamBusinessService.Model;
using Sistran.Company.Application.Vehicles.MassiveVehicleParamApplicationService.Enum;
using Sistran.Company.Application.MassiveVehicleParamBusinessServiceProvider.Assembler;

namespace Sistran.Company.Application.MassiveVehicleParamBusinessServiceProvider.Dao
{
    public class FaseColdaProcessDAO
    {
        public string GetErrorExcelProcessVehicleFasecolda(int loadProcessId)
        {

            CompanyProcessFasecolda faseColda = new CompanyProcessFasecolda();
            faseColda = GetMassiveFasecoldaByProcessId(loadProcessId, true);

            FileProcessValue fileProcessValue = new FileProcessValue();

            fileProcessValue.Key1 = (int)FileProcessType.ErroresFaseColda;
            fileProcessValue.Key4 = 7;
            fileProcessValue.Key5 = (int)SubCoveredRiskType.Vehicle;

            File file = DelegateService.utilitiesService.GetFileByFileProcessValue(fileProcessValue);

            int headersCount = file.Templates.First(x => x.IsPrincipal).Rows.Count;

            file.Templates[0].Rows.Last().Fields.Add(new Field
            {
                Order = file.Templates[0].Rows.Last().Fields.Max(y => y.Order) + 1,
                ColumnSpan = 1,
                RowPosition = file.Templates[0].Rows.Last().Fields.First().RowPosition,
                FieldType = FieldType.String,
                Description = Resources.Errors.LabelError
            });

            file.Templates[1].Rows.Last().Fields.Add(new Field
            {
                Order = file.Templates[1].Rows.Last().Fields.Max(y => y.Order) + 1,
                ColumnSpan = 1,
                RowPosition = file.Templates[1].Rows.Last().Fields.First().RowPosition,
                FieldType = FieldType.String,
                Description = Resources.Errors.LabelError
            });

            foreach (CompanyMassiveVehicleFasecoldaRow faseColdaRow in faseColda.Row)
            {
                File fileSerialized = JsonConvert.DeserializeObject<File>(faseColdaRow.SerializedRow);

                foreach (Template template in fileSerialized.Templates)
                {
                    if (template.IsPrincipal)
                    {
                        template.Rows.Last().Fields.Add(new Field
                        {
                            Order = template.Rows.Last().Fields.Max(y => y.Order) + 1,
                            ColumnSpan = 1,
                            RowPosition = template.Rows.Last().Fields.First().RowPosition,
                            FieldType = FieldType.String,
                            Value = faseColdaRow.Error_Description
                        });

                        if (file.Templates.First(x => x.Order == 1).Rows.Count == headersCount || template.Order != 1)
                        {
                            file.Templates.First(x => x.PropertyName == template.PropertyName).Rows.AddRange(template.Rows);
                        }
                    }
                }
            }

            faseColda = GetMassiveFasecoldaByProcessId(loadProcessId, true);

            if (faseColda != null)
            {
                foreach (CompanyMassiveVehicleFasecoldaRow faseColdaRow in faseColda.Row)
                {
                    File fileSerialized = JsonConvert.DeserializeObject<File>(faseColdaRow.SerializedRow);

                    foreach (Template template in fileSerialized.Templates)
                    {
                        if (template.PropertyName == TemplatePropertyName.LoadCodesFaseColda)
                        {
                            template.Rows.Last().Fields.Add(new Field
                            {
                                Order = template.Rows.Last().Fields.Max(y => y.Order) + 1,
                                ColumnSpan = 1,
                                RowPosition = template.Rows.Last().Fields.First().RowPosition,
                                FieldType = FieldType.String,
                                Value = faseColdaRow.Error_Description
                            });

                            if (file.Templates.First(x => x.Order == 1).Rows.Count == headersCount || template.Order != 1)
                            {
                                file.Templates.First(x => x.PropertyName == template.PropertyName).Rows.AddRange(template.Rows);
                            }
                        }
                    }
                }
            }

            file.Name = "Errores_Fasecolda_" + loadProcessId;
            return DelegateService.utilitiesService.GenerateFile(file);
        }
        
        public void CreateProcessFasecoldaRow(CompanyMassiveVehicleFasecoldaRow vehicleFasecoldaRow)
        {
            COMMEN.CiaAsynchronousProcessFasecoldaRow entityCiaAsynchronousProcessFasecoldaRow = EntityAssembler.CreateProcessFasecoldaRow(vehicleFasecoldaRow);

            DataFacadeManager.Instance.GetDataFacade().InsertObject(entityCiaAsynchronousProcessFasecoldaRow);
        }

        public CompanyProcessFasecolda GenerateProcessMassiveVehicleFasecolda(int processId)
        {
            FaseColdaLoadDAO faseColdaLoadDAO = new FaseColdaLoadDAO();
            CompanyProcessFasecolda processFasecolda = GetMassiveFasecoldaByProcessId(processId, false);
             using (DynamicDataAccess dynamicDataAccess = new DynamicDataAccess())
             {
                 dynamicDataAccess.ExecuteSPNonQuery("TMP.LOAD_FASECOLDA_CODE");
                 dynamicDataAccess.ExecuteSPNonQuery("COMM.LOAD_FASECOLDA_VALUES");
                 dynamicDataAccess.ExecuteSPNonQuery("TMP.CO_UPD_NEW_VEHICLE_PRICE");
             }

            processFasecolda.ProcessStatusType.StatusType = VehicleFasecoldaProcessStatusEnum.Procesando;
              
            faseColdaLoadDAO.UpdateProcessFasecolda(processFasecolda);

            return processFasecolda;
        }

        public CompanyProcessFasecolda GetMassiveFasecoldaByProcessId(int processId, bool? withErrors)
        {
            CompanyProcessFasecolda companyProcessFasecolda = new CompanyProcessFasecolda();
            FaseColdaLoadDAO faseColdaLoadDAO = new FaseColdaLoadDAO();

            companyProcessFasecolda = faseColdaLoadDAO.GetLoadMassiveVehiclefasecolda(processId);

            if (companyProcessFasecolda != null)
            {
                ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();
                filter.Property(COMMEN.CiaAsynchronousProcessFasecoldaRow.Properties.ProcessId, typeof(COMMEN.CiaAsynchronousProcessFasecoldaRow).Name);
                filter.Equal();
                filter.Constant(processId);

                filter.And();
                filter.Property(COMMEN.CiaAsynchronousProcessFasecoldaRow.Properties.HasError, typeof(COMMEN.CiaAsynchronousProcessFasecoldaRow).Name);
                filter.Equal();
                filter.Constant(withErrors.Value);

                BusinessCollection businessCollection = new BusinessCollection(DataFacadeManager.Instance.GetDataFacade().SelectObjects(typeof(COMMEN.CiaAsynchronousProcessFasecoldaRow), filter.GetPredicate()));
                List<CompanyMassiveVehicleFasecoldaRow> collectiveLoadProcesses = ModelAssembler.CreateMassiveFasecoldaRow(businessCollection);
                companyProcessFasecolda.Row = collectiveLoadProcesses;
            }

            return companyProcessFasecolda;
        }
    }
}
