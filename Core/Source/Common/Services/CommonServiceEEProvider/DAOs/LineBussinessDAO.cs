using Sistran.Core.Application.CommonService.Enums;
using Sistran.Core.Application.CommonService.Models;
using Sistran.Core.Application.CommonServices.EEProvider.Assemblers;
using Sistran.Core.Application.CommonServices.EEProvider.Entities.Views;
using Sistran.Core.Application.Utilities.DataFacade;
using Sistran.Core.Application.UtilitiesServicesEEProvider.DAOs;
using Sistran.Core.Framework.BAF;
using Sistran.Core.Framework.Contexts;
using Sistran.Core.Framework.DAF;
using Sistran.Core.Framework.DAF.Engine;
using Sistran.Core.Framework.Queries;
using EnumUtilities =Sistran.Core.Services.UtilitiesServices.Enums;
using Sistran.Core.Services.UtilitiesServices.Models;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;
using COMMML = Sistran.Core.Application.CommonService.Models;
using EtLineBusiness = Sistran.Core.Application.Common.Entities;
using EtQuot = Sistran.Core.Application.Quotation.Entities;
using PARAMEN = Sistran.Core.Application.Parameters.Entities;

namespace Sistran.Core.Application.CommonServices.EEProvider.DAOs
{
    public class LineBussinessDAO
    {
        FileDAO fileDAO = new FileDAO();
        public LineBusiness CreateLineBussiness(LineBusiness linebussiness)
        {
            Stopwatch stopWatch = new Stopwatch();
            
            EtLineBusiness.LineBusiness lineBusinessEntity = new EtLineBusiness.LineBusiness(linebussiness.Id);
            EtLineBusiness.LineBusinessCoveredRiskType lineByRiskEntity = new EtLineBusiness.LineBusinessCoveredRiskType(linebussiness.Id, linebussiness.IdLineBusinessbyRiskType);
            EtQuot.ClauseLevel ClauseLineBusinessEntity = new EtQuot.ClauseLevel();
            EtQuot.InsObjLineBusiness InsuredObjectEntity = new EtQuot.InsObjLineBusiness(0, linebussiness.Id);
            EtQuot.PerilLineBusiness ProtectionLineBusinesEntity = new EtQuot.PerilLineBusiness(0, linebussiness.Id);

            stopWatch.Start();

            using (Context.Current)
            {
                DeleteCoveredRiskTypeByLineBusinessId(linebussiness.Id);
                if (CreateLineBusiness(linebussiness, lineBusinessEntity))
                {
                    bool riskType = CreateLineBussinessByCoveredRisktype(linebussiness, lineBusinessEntity, lineByRiskEntity);
                    bool Insured = CreateLineBusinessForInsuredObjects(linebussiness, lineBusinessEntity, InsuredObjectEntity);
                    bool Protection = CreateLineBusinessForProtections(linebussiness, lineBusinessEntity, ProtectionLineBusinesEntity);
                    //val = true;
                }
                //val = false;
            }
            stopWatch.Stop();
            Debug.WriteLine(stopWatch.Elapsed.ToString(), "Sistran.Core.Application.UnderwritingServices.EEProvider.DAOs.SaveInsurancesObject");
            return linebussiness;
            //return val;
        }

        /// <summary>
        /// funcion para crear ramos tecnicos
        /// </summary>
        /// <param name="linebussiness"></param>
        /// <param name="lineBusinessEntity"></param>
        /// <param name="returnObjects"></param>
        public bool CreateLineBusiness(LineBusiness linebussiness, EtLineBusiness.LineBusiness lineBusinessEntity)
        {
            try
            {
                if (linebussiness != null)
                {
                    lineBusinessEntity.Description = linebussiness.Description;
                    lineBusinessEntity.LineBusinessCode = linebussiness.Id;
                    lineBusinessEntity.SmallDescription = linebussiness.ShortDescription;
                    lineBusinessEntity.TinyDescription = linebussiness.TyniDescription;

                    lineBusinessEntity.ReportLineBusinessCode = linebussiness.Id.ToString();

                    if (lineBusinessEntity != null)
                    {
                        if (linebussiness.Description != "" && linebussiness.Id != 0)
                        {
                            PrimaryKey key = EtLineBusiness.LineBusiness.CreatePrimaryKey(lineBusinessEntity.LineBusinessCode);
                            lineBusinessEntity = (EtLineBusiness.LineBusiness)DataFacadeManager.Instance.GetDataFacade().GetObjectByPrimaryKey(key);
                            if (lineBusinessEntity == null)
                            {
                                //Creacion de registro Nuevo
                                lineBusinessEntity = EntityAssembler.CreateLineBusiness(linebussiness);
                                DataFacadeManager.Instance.GetDataFacade().InsertObject(lineBusinessEntity);
                                return true;
                            }
                            else
                            {
                                //Actualizacion  de registro 
                                lineBusinessEntity.Description = linebussiness.Description;
                                lineBusinessEntity.SmallDescription = linebussiness.ShortDescription;
                                lineBusinessEntity.TinyDescription = linebussiness.TyniDescription;
                                lineBusinessEntity.ReportLineBusinessCode = linebussiness.ReportLineBusiness.ToString();
                                DataFacadeManager.Instance.GetDataFacade().UpdateObject(lineBusinessEntity);
                                return true;
                            }
                        }

                        return false;

                    }
                    return false;
                }
                return false;
            }
            catch (Exception ex)
            {
                throw (ex);
            }
        }

        /// <summary>
        /// funciones para crear tipos de riesgos asociados a los ramos tecnicos
        /// </summary>
        /// <param name="lineBusinessEntity"></param>
        /// <param name="lineByRiskEntity"></param>
        /// <param name="returnObjects"></param>
        public bool CreateLineBussinessByCoveredRisktype(LineBusiness linebussiness, EtLineBusiness.LineBusiness lineBusinessEntity, EtLineBusiness.LineBusinessCoveredRiskType lineByRiskEntity)
        {
            try
            {
                PrimaryKey key = EtLineBusiness.LineBusinessCoveredRiskType.CreatePrimaryKey(lineBusinessEntity.LineBusinessCode, linebussiness.IdLineBusinessbyRiskType);
                lineByRiskEntity = (EtLineBusiness.LineBusinessCoveredRiskType)DataFacadeManager.Instance.GetDataFacade().GetObjectByPrimaryKey(key);
                
                if (lineByRiskEntity == null)
                {
                    //Creacion de registro
                    DataFacadeManager.Instance.GetDataFacade().InsertObject(lineByRiskEntity);
                }
                else
                {
                    //Actualiza el registro
                    DataFacadeManager.Instance.GetDataFacade().UpdateObject(lineByRiskEntity);
                }

                if (lineByRiskEntity == null)
                {
                    //Creacion de registro
                    DataFacadeManager.Instance.GetDataFacade().InsertObject(lineByRiskEntity);
                    return true;
                }
                else
                {
                    //Modificacion de registro
                   DataFacadeManager.Instance.GetDataFacade().UpdateObject(lineByRiskEntity);
                    return true;
                }
                
            }
            catch (Exception ex)
            {
                throw (ex);
            }
        }
        /// <summary>
        /// Funcion para crear objetos de seguro asociados a un ramo tecnico
        /// </summary>
        /// <param name="linebussiness"></param>
        /// <param name="lineBusinessEntity"></param>
        /// <param name="InsuredObjectEntity"></param>
        /// <param name="returnObjects"></param>
        public bool CreateLineBusinessForInsuredObjects(LineBusiness linebussiness, EtLineBusiness.LineBusiness lineBusinessEntity, EtQuot.InsObjLineBusiness InsuredObjectEntity)
        {
            try
            {
                if (linebussiness.ListInsurectObjects != null)
                {
                    if (linebussiness.ListInsurectObjects.Count > 0)
                    {

                        ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();
                        filter.Property(EtQuot.InsObjLineBusiness.Properties.LineBusinessCode);
                        filter.Equal();
                        filter.Constant(InsuredObjectEntity.LineBusinessCode);
                        BusinessCollection businessCollection = DataFacadeManager.Instance.GetDataFacade().List(typeof(EtQuot.InsObjLineBusiness), filter.GetPredicate());
                        foreach (var item in businessCollection)
                            DataFacadeManager.Instance.GetDataFacade().DeleteObject(item);

                        foreach (var item in linebussiness.ListInsurectObjects)
                        {

                            EtQuot.InsObjLineBusiness insuredObjectEntity = new EtQuot.InsObjLineBusiness(item, linebussiness.Id);

                            DataFacadeManager.Instance.GetDataFacade().InsertObject(insuredObjectEntity);
                        }
                    }

                }
                return true;

            }
            catch (Exception ex)
            {
                throw (ex);
            }
        }

        /// <summary>
        /// Funcion para calcular el consecutivo
        /// </summary>
        /// <param name="idClause"></param>
        /// <returns></returns>
        public int GetClauseIdConsecutive(int idClause)
        {
            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();
            int consecutive = 1;
            SelectQuery select = new SelectQuery();
            Function function = new Function(FunctionType.Max);
            function.AddParameter(new Column(EtQuot.ClauseLevel.Properties.ClauseLevelId, typeof(EtQuot.ClauseLevel).Name));
            select.AddSelectValue(new SelectValue(function, "ClauseLevelId"));

            ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();

            filter.Property(EtQuot.ClauseLevel.Properties.ClauseId, typeof(EtQuot.ClauseLevel).Name);
            filter.Equal();
            filter.Constant(idClause);
            select.Where = filter.GetPredicate();
            select.Table = new ClassNameTable(typeof(EtQuot.ClauseLevel));

            using (IDataReader reader = DataFacadeManager.Instance.GetDataFacade().Select(select))
            {
                while (reader.Read())
                {
                    if (reader[0] != null)
                    {
                        consecutive = Convert.ToInt32(reader[0].ToString()) + 1;
                    }
                }
            }
            stopWatch.Stop();
            Debug.WriteLine(stopWatch.Elapsed.ToString(), "Sistran.Core.Application.CommonService.DAOs.GetSubLinesBusinessByLineBusinessId");

            return consecutive;
        }

        public bool CreateLineBusinessForProtections(LineBusiness linebussiness, EtLineBusiness.LineBusiness lineBusinessEntity, EtQuot.PerilLineBusiness ProtectionLineBusinesEntity)
        {
            try
            {
                ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();
                filter.Property(EtQuot.PerilLineBusiness.Properties.LineBusinessCode);
                filter.Equal();
                filter.Constant(ProtectionLineBusinesEntity.LineBusinessCode);

                BusinessCollection businessCollection = DataFacadeManager.Instance.GetDataFacade().List(typeof(EtQuot.PerilLineBusiness), filter.GetPredicate());

                foreach (var item in businessCollection)

                    DataFacadeManager.Instance.GetDataFacade().DeleteObject(item);

                return true;


            }
            catch (Exception ex)
            {
                throw (ex);
            }

        }

        public LineBusiness GetLineBusinessById(string descriptionLineBusiness, int IdLineBusiness)
        {
            EtLineBusiness.LineBusiness linebusiness = new EtLineBusiness.LineBusiness(IdLineBusiness);
            linebusiness.Description = descriptionLineBusiness;

            ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();
            if (IdLineBusiness != 0)
            {

                filter.Property(EtLineBusiness.LineBusiness.Properties.LineBusinessCode, typeof(EtLineBusiness.LineBusiness).Name);
                filter.Equal();
                filter.Constant(IdLineBusiness);
            }
            else
            {
                filter.Property(EtLineBusiness.LineBusiness.Properties.Description, typeof(EtLineBusiness.LineBusiness).Name);
                filter.Like();
                filter.Constant('%' + descriptionLineBusiness + '%');
            }

            BusinessCollection businessCollection = new BusinessCollection(DataFacadeManager.Instance.GetDataFacade().SelectObjects(typeof(EtLineBusiness.LineBusiness), filter.GetPredicate()));
            return ModelAssembler.CreateLinesBusiness(businessCollection).FirstOrDefault();
        }

        public List<COMMML.LineBusiness> GetRiskTypeByLineBusinessId()
        {
            try
            {
                Stopwatch stopWatch = new Stopwatch();
                stopWatch.Start();
                Entities.Views.LineBusinessCoveredRiskType view = new Entities.Views.LineBusinessCoveredRiskType();
                ViewBuilder builder = new ViewBuilder("LineBusinessCoveredRiskType");

                DataFacadeManager.Instance.GetDataFacade().FillView(builder, view);

                var query = from t1 in ((IList)view.LineBusiness).Cast<EtLineBusiness.LineBusiness>().ToList()
                            join t2 in ((IList)view.LineBusinessByCoveredRiskType).Cast<EtLineBusiness.LineBusinessCoveredRiskType>().ToList()
                              on t1.LineBusinessCode equals t2.LineBusinessCode
                            select new
                            {
                                IdRiskType = t2.CoveredRiskTypeCode,
                                IdLineBusines = t2.LineBusinessCode,
                            };

                List<COMMML.LineBusiness> lineBusinessModel = new List<COMMML.LineBusiness>();
                //List<COMMML.LineBusinessCoveredRiskType> lineBusinessModel = new List<COMMML.LineBusinessCoveredRiskType>();
                foreach (var item in query)
                {
                    COMMML.LineBusiness lineBusinesModel = new COMMML.LineBusiness();
                    //COMMML.LineBusinessCoveredRiskType lineBusinesModel = new COMMML.LineBusinessCoveredRiskType();
                    lineBusinesModel.IdLineBusinessbyRiskType = item.IdRiskType;
                    lineBusinesModel.Id = item.IdLineBusines;

                    lineBusinessModel.Add(lineBusinesModel);
                }

                return lineBusinessModel.OrderByDescending(x => x.Id).ToList();
                //return lineBusinessModel.OrderByDescending(x => x.IdRiskType).ToList();
            }
            catch (Exception ex)
            {
                throw new BusinessException("Error Obtener GetRiskTypeByLineBusiness", ex);
            }
        }
        public SubLineBusiness GetSubLineBusinessById(int Id, int lineBusinessId)
        {
            EtLineBusiness.SubLineBusiness linebusiness = new EtLineBusiness.SubLineBusiness(Id, lineBusinessId);

            ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();

            filter.Property(EtLineBusiness.SubLineBusiness.Properties.LineBusinessCode, typeof(EtLineBusiness.SubLineBusiness).Name);
            filter.Equal();
            filter.Constant(lineBusinessId);
            filter.And();
            filter.Property(EtLineBusiness.SubLineBusiness.Properties.SubLineBusinessCode, typeof(EtLineBusiness.SubLineBusiness).Name);
            filter.Equal();
            filter.Constant(Id);

            BusinessCollection businessCollection = new BusinessCollection(DataFacadeManager.Instance.GetDataFacade().SelectObjects(typeof(EtLineBusiness.SubLineBusiness), filter.GetPredicate()));
            return ModelAssembler.CreateSubLinesBusiness(businessCollection).FirstOrDefault();
        }
        /// <summary>
        /// Obtener todos los ramos tecnicos
        /// </summary>
        /// <returns></returns>
        public List<COMMML.LineBusiness> GetLinesBusiness()
        {
            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();
            BusinessCollection listLineBusiness = new BusinessCollection(DataFacadeManager.Instance.GetDataFacade().SelectObjects(typeof(EtLineBusiness.LineBusiness)));
            stopWatch.Stop();
            Debug.WriteLine(stopWatch.Elapsed.ToString(), "Sistran.Core.Application.CommonService.Providers.GetLinesBusiness");
            return ModelAssembler.CreateLinesBusiness(listLineBusiness);
        }
        /// <summary>
        /// Eliminar los tipos de riesgo por linea
        /// </summary>
        /// <returns></returns>
        public void DeleteCoveredRiskTypeByLineBusinessId(int lineBusinessId)
        {
            HardRiskTypeCoveredRiskType view = new HardRiskTypeCoveredRiskType();
            ViewBuilder builder = new ViewBuilder("HardRiskTypeCoveredRiskType");

            ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();
            filter.Property(EtLineBusiness.LineBusinessCoveredRiskType.Properties.LineBusinessCode, typeof(EtLineBusiness.LineBusinessCoveredRiskType).Name);
            filter.Equal();
            filter.Constant(lineBusinessId);
            builder.Filter = filter.GetPredicate();
            DataFacadeManager.Instance.GetDataFacade().FillView(builder, view);

            List<EtLineBusiness.LineBusinessCoveredRiskType> coveredRiskTypes = view.LineBusinessByCoveredRiskType.Cast<EtLineBusiness.LineBusinessCoveredRiskType>().ToList();
            List<PARAMEN.HardRiskType> hardRiskTypes = view.HardRiskTypes.Cast<PARAMEN.HardRiskType>().ToList();

            foreach (EtLineBusiness.LineBusinessCoveredRiskType item in coveredRiskTypes)
            {
                //No se eliminan los registros de COMM.LB_COVERED_RISK_TYPE que tengan información en la tabla PARAM.HARD_RISK_TYPE
                PARAMEN.HardRiskType hardRiskType = hardRiskTypes.Where(x => x.CoveredRiskTypeCode == item.CoveredRiskTypeCode).FirstOrDefault();
                if (hardRiskType == null)
                {
                    DataFacadeManager.Instance.GetDataFacade().DeleteObject(item);
                }
            }
        }

        /// <summary>
        /// Obtener todos los ramos tecnicos por prefixId
        /// </summary>
        /// <returns></returns>
        public List<COMMML.LineBusiness> GetLineBusinessByPrefixId(int prefixId)
        {
            LineBusinessPrefixView view = new LineBusinessPrefixView();
            ViewBuilder builder = new ViewBuilder("LineBusinessPrefixView");
            ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();
            filter.Property(EtLineBusiness.PrefixLineBusiness.Properties.PrefixCode, typeof(EtLineBusiness.PrefixLineBusiness).Name);
            filter.Equal();
            filter.Constant(prefixId);
            builder.Filter = filter.GetPredicate();
            DataFacadeManager.Instance.GetDataFacade().FillView(builder, view);
            return ModelAssembler.CreateLinesBusiness(view.LineBusiness);
        }


        public List<LineBusiness> GetLineBusinessBySubCoveredRiskType(SubCoveredRiskType subCoveredRiskType)
        {
            List<PARAMEN.HardRiskType> hardRiskTypes = new List<PARAMEN.HardRiskType>();

            ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();
            filter.Property(PARAMEN.HardRiskType.Properties.SubCoveredRiskTypeCode, typeof(PARAMEN.HardRiskType).Name);
            filter.Equal();
            filter.Constant(subCoveredRiskType);

            BusinessCollection businessCollection = DataFacadeManager.Instance.GetDataFacade().List(typeof(PARAMEN.HardRiskType), filter.GetPredicate());
            var linebusiness = new List<LineBusiness>();

            if (businessCollection.Count > 0)
            {
                linebusiness = ModelAssembler.CreateLineBusinessByHardRiskType(businessCollection);
            }

            return linebusiness;
        }

        public SubCoveredRiskType GetSubCoveredRiskTypeByPrefixIdCoveredRiskTypeId(int prefixId, int? coveredRiskType)
        {

            List<PARAMEN.HardRiskType> hardRiskTypes = new List<PARAMEN.HardRiskType>();

            ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();
            filter.Property(PARAMEN.HardRiskType.Properties.LineBusinessCode, typeof(PARAMEN.HardRiskType).Name);
            filter.Equal();
            filter.Constant(prefixId);
            if (coveredRiskType.HasValue)
            {
                filter.And();
                filter.Property(PARAMEN.HardRiskType.Properties.CoveredRiskTypeCode, typeof(PARAMEN.HardRiskType).Name);
                filter.Equal();
                filter.Constant(coveredRiskType);
            }
            BusinessCollection businessCollection = DataFacadeManager.Instance.GetDataFacade().List(typeof(PARAMEN.HardRiskType), filter.GetPredicate());

            return (SubCoveredRiskType)businessCollection.Cast<PARAMEN.HardRiskType>().First().SubCoveredRiskTypeCode;
        }


        /// <summary>
        /// Genera archivo excel subramo técnico
        /// </summary>
        /// <param name="subLinebusiness"></param>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public string GenerateFileToSubLinebusiness(List<SubLineBusiness> subLinebusiness, string fileName)
        {
            FileProcessValue fileProcessValue = new FileProcessValue();
            fileProcessValue.Key1 = (int)EnumUtilities.FileProcessType.ParametrizationSubLineBusiness;

            File file = fileDAO.GetFileByFileProcessValue(fileProcessValue);

            if (file != null && file.IsEnabled)
            {
                file.Name = fileName;
                List<Row> rows = new List<Row>();

                foreach (SubLineBusiness sublinebusiness in subLinebusiness)
                {
                    var fields = file.Templates[0].Rows[0].Fields.Select(x => new Field
                    {
                        ColumnSpan = x.ColumnSpan,
                        Description = x.Description,
                        FieldType = x.FieldType,
                        Id = x.Id,
                        IsEnabled = x.IsEnabled,
                        IsMandatory = x.IsMandatory,
                        Order = x.Order,
                        RowPosition = x.RowPosition,
                        SmallDescription = x.SmallDescription
                    }).ToList();

                    fields[0].Value = sublinebusiness.LineBusinessId.ToString();
                    fields[1].Value = sublinebusiness.LineBusinessDescription;
                    fields[2].Value = sublinebusiness.Id.ToString();
                    fields[3].Value = sublinebusiness.Description;
                    fields[4].Value = sublinebusiness.SmallDescription;

                    rows.Add(new Row
                    {
                        Fields = fields
                    });
                }

                file.Templates[0].Rows = rows;
                file.Name = fileName + "_" + DateTime.Now.ToString("dd_MM_yyyy");
                return fileDAO.GenerateFile(file);
            }
            else
            {
                return "";
            }
        }


        /// <summary>
        /// Genera archivo excel ramo técnico
        /// </summary>
        /// <param name="linebusiness"></param>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public string GenerateFileLinebusiness(List<LineBusiness> linebusiness, string fileName)
        {
            FileProcessValue fileProcessValue = new FileProcessValue();
            fileProcessValue.Key1 = (int)EnumUtilities.FileProcessType.ParametrizationLineBusiness;

            File file =fileDAO.GetFileByFileProcessValue(fileProcessValue);

            if (file != null && file.IsEnabled)
            {
                file.Name = fileName;
                List<Row> rows = new List<Row>();

                foreach (LineBusiness LineBusiness in linebusiness)
                {
                    var fields = file.Templates[0].Rows[0].Fields.Select(x => new Field
                    {
                        ColumnSpan = x.ColumnSpan,
                        Description = x.Description,
                        FieldType = x.FieldType,
                        Id = x.Id,
                        IsEnabled = x.IsEnabled,
                        IsMandatory = x.IsMandatory,
                        Order = x.Order,
                        RowPosition = x.RowPosition,
                        SmallDescription = x.SmallDescription
                    }).ToList();

                    fields[0].Value = LineBusiness.Id.ToString();
                    fields[1].Value = LineBusiness.Description;
                    fields[2].Value = LineBusiness.ShortDescription;
                    fields[3].Value = LineBusiness.TyniDescription;
                    fields[4].Value = LineBusiness.ReportLineBusiness.ToString();
                    //if (LineBusiness.ListLineBusinessCoveredrisktype != null && LineBusiness.ListLineBusinessCoveredrisktype.Count > 0)
                    //{
                    //    fields[5].Value = LineBusiness.ListLineBusinessCoveredrisktype.First().IdRiskType.ToString();
                    //}
                    //else
                    //{
                    fields[5].Value = " ";
                    //}
                    //if (LineBusiness.ListClausesLineBusiness != null)
                    //{
                    //    foreach (var item in LineBusiness.ListClausesLineBusiness)
                    //    {
                    //        fields[6].Value = item.DescriptionClauseByLineBusiness;
                    //    }
                    //}
                    //else
                    //{
                    //    fields[6].Value = " ";
                    //}
                    //if (LineBusiness.ListInsurectObjects != null)
                    //{
                    //    foreach (var item in LineBusiness.ListInsurectObjects)
                    //    {
                    //        fields[7].Value = item.ToString();
                    //    }
                    //}
                    //else
                    //{
                    //    fields[7].Value = " ";
                    //}
                    //if (LineBusiness.ListProtections != null)
                    //{
                    //    foreach (var item in LineBusiness.ListProtections)
                    //    {
                    //        fields[8].Value = item.IdPeril.ToString();
                    //    }
                    //}
                    //else
                    //{
                    //    fields[8].Value = " ";
                    //}

                    rows.Add(new Row
                    {
                        Fields = fields
                    });
                }
                file.Templates[0].Rows = rows;
                List<Row> rowsClause = new List<Row>();

                foreach (LineBusiness LineBusiness in linebusiness)
                {
                    LineBusinessClausesView lbc = LineBusinessClausesByLineBusinessId(LineBusiness.Id);



                    foreach (var item in lbc.Clause.Cast<EtQuot.Clause>())
                    {

                        var fields = file.Templates[1].Rows[0].Fields.Select(x => new Field
                        {
                            ColumnSpan = x.ColumnSpan,
                            Description = x.Description,
                            FieldType = x.FieldType,
                            Id = x.Id,
                            IsEnabled = x.IsEnabled,
                            IsMandatory = x.IsMandatory,
                            Order = x.Order,
                            RowPosition = x.RowPosition,
                            SmallDescription = x.SmallDescription
                        }).ToList();

                        fields[0].Value = LineBusiness.Id.ToString();
                        fields[1].Value = LineBusiness.Description;
                        fields[2].Value = item.ClauseId.ToString();
                        fields[3].Value = item.ClauseName;
                        fields[4].Value = item.ClauseTitle;

                        rowsClause.Add(new Row
                        {
                            Fields = fields
                        });
                    }
                }
                file.Templates[1].Rows = rowsClause;
                List<Row> rowsInsuredObject = new List<Row>();

                foreach (LineBusiness LineBusiness in linebusiness)
                {
                    LineBusinessInsuredObjectPerilView lb = LineBusinessInsuredObjectPerilByLineBusinessId(LineBusiness.Id);



                    foreach (var io in lb.InsuredObject.Cast<EtQuot.InsuredObject>())
                    {
                        foreach (var p in lb.Peril.Cast<EtQuot.Peril>())
                        {

                            var fields = file.Templates[2].Rows[0].Fields.Select(x => new Field
                            {
                                ColumnSpan = x.ColumnSpan,
                                Description = x.Description,
                                FieldType = x.FieldType,
                                Id = x.Id,
                                IsEnabled = x.IsEnabled,
                                IsMandatory = x.IsMandatory,
                                Order = x.Order,
                                RowPosition = x.RowPosition,
                                SmallDescription = x.SmallDescription
                            }).ToList();


                            fields[0].Value = LineBusiness.Id.ToString();
                            fields[1].Value = LineBusiness.Description;
                            fields[2].Value = io.InsuredObjectId.ToString();
                            fields[3].Value = io.Description;
                            fields[4].Value = p.PerilCode.ToString();
                            fields[5].Value = p.Description;

                            rowsInsuredObject.Add(new Row
                            {
                                Fields = fields
                            });
                        }
                    }
                }
                file.Templates[2].Rows = rowsInsuredObject;

                file.Name = string.Format(fileName + "_" + DateTime.Now.ToString("dd_MM_yyyy"));
                return fileDAO.GenerateFile(file);
            }
            else
            {
                return "";
            }
        }

        private LineBusinessClausesView LineBusinessClausesByLineBusinessId(int lineBusinessId)
        {
            ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();

            filter.Property(EtLineBusiness.LineBusiness.Properties.LineBusinessCode, typeof(LineBusiness).Name);
            filter.Equal();
            filter.Constant(lineBusinessId);
            filter.And();
            filter.Property(EtQuot.Clause.Properties.ConditionLevelCode, typeof(EtQuot.Clause).Name);
            filter.Equal();
            filter.Constant(4);//LineBusiness

            LineBusinessClausesView view = new LineBusinessClausesView();
            ViewBuilder builder = new ViewBuilder("LineBusinessClausesView");
            builder.Filter = filter.GetPredicate();

            DataFacadeManager.Instance.GetDataFacade().FillView(builder, view);
            return view;
        }

        private LineBusinessInsuredObjectPerilView LineBusinessInsuredObjectPerilByLineBusinessId(int lineBusinessId)
        {
            ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();

            filter.Property(EtLineBusiness.LineBusiness.Properties.LineBusinessCode, typeof(EtLineBusiness.LineBusiness).Name);
            filter.Equal();
            filter.Constant(lineBusinessId);

            LineBusinessInsuredObjectPerilView view = new LineBusinessInsuredObjectPerilView();
            ViewBuilder builder = new ViewBuilder("LineBusinessInsuredObjectPerilView");
            builder.Filter = filter.GetPredicate();

            DataFacadeManager.Instance.GetDataFacade().FillView(builder, view);
            return view;
        }

        //para analizar 

        //public List<ModCom.LineBusinessCoveredRiskType> GetLineBusinessCoveredRiskTypeByLineBusinessId(int lineBusinessId)
        //{
        //    ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();
        //    filter.Property(EtLineBusiness.LineBusinessCoveredRiskType.Properties.LineBusinessCode, typeof(EtLineBusiness.LineBusinessCoveredRiskType).Name);
        //    filter.Equal();
        //    filter.Constant(lineBusinessId);

        //    BusinessCollection businessCollection = DataFacadeManager.Instance.GetDataFacade().List(typeof(EtLineBusiness.LineBusinessCoveredRiskType), filter.GetPredicate());

        //    return ModelAssembler.CreateLineBusinessCoveredRiskTypes(businessCollection);
        //}


    }
}
