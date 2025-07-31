using Sistran.Company.Application.CommonServices.EEProvider.Resources;
using Sistran.Core.Application.CommonService.Enums;
using Sistran.Core.Application.CommonService.Models;
using Sistran.Core.Application.CommonServices.EEProvider.Assemblers;
using Sistran.Core.Application.CommonServices.EEProvider.Entities.Views;
using Sistran.Core.Application.Utilities.DataFacade;
using Sistran.Core.Application.UtilitiesServicesEEProvider.DAOs;
using Sistran.Core.Framework.DAF;
using Sistran.Core.Framework.DAF.Engine;
using Sistran.Core.Framework.Queries;
using EnumUtilities =Sistran.Core.Services.UtilitiesServices.Enums;
using Sistran.Core.Services.UtilitiesServices.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using TP = Sistran.Core.Application.Utilities.Utility;
using COMMEN = Sistran.Core.Application.Common.Entities;
using COMMML = Sistran.Core.Application.CommonService.Models;
using PARAMEN = Sistran.Core.Application.Parameters.Entities;

namespace Sistran.Core.Application.CommonServices.EEProvider.DAOs
{
    /// <summary>
    ///  Ramo Comercial
    /// </summary>
    public class PrefixDAO
    {
        FileDAO fileDAO = new FileDAO();
        /// <summary>
        /// Obtenr Modelo Ramos Comerciales
        /// </summary>
        /// <returns>List<COMMML.Prefix></returns>
        public List<COMMML.Prefix> GetAllPrefix()
        {
            BusinessCollection listEntityprefix = new BusinessCollection(DataFacadeManager.Instance.GetDataFacade().SelectObjects(typeof(COMMEN.Prefix)));
            List<PARAMEN.PrefixType> listPrefixType = DataFacadeManager.Instance.GetDataFacade().List(typeof(PARAMEN.PrefixType), null).Cast<PARAMEN.PrefixType>().ToList();
            LineBusinessPrefixView view = new LineBusinessPrefixView();
            ViewBuilder builder = new ViewBuilder("LineBusinessPrefixView");
            DataFacadeManager.Instance.GetDataFacade().FillView(builder, view);
            List<COMMML.LineBusiness> listLineBusiness = ModelAssembler.CreateLinesBusiness(view.LineBusiness);
            if (listEntityprefix != null && listEntityprefix.Count > 0)
            {
                List<COMMML.Prefix> listPrefix = ModelAssembler.CreatePrefixes(listEntityprefix);
                TP.Parallel.ForEach(listPrefix, (currentPrefix) =>
                {
                    COMMML.Prefix prefix = currentPrefix;
                    prefix.PrefixType = new PrefixType();
                    prefix.PrefixType.Id = listPrefixType.First(m => m.PrefixTypeCode == prefix.PrefixTypeCode).PrefixTypeCode;
                    prefix.PrefixType.Description = listPrefixType.First(m => m.PrefixTypeCode == prefix.PrefixTypeCode).Description;
                    prefix.LineBusiness = new List<COMMML.LineBusiness>();
                    var prefixLineBusiness = view.PrefixLineBusiness.Cast<COMMEN.PrefixLineBusiness>().Where(y => y.PrefixCode == prefix.Id).Select(x => x.LineBusinessCode).ToList();
                    prefix.LineBusiness = listLineBusiness.Where(x => prefixLineBusiness.Contains(x.Id)).ToList();

                });
                return listPrefix;
            }
            else
            {
                return null;
            }

        }


        public List<Prefix> GetPrefixesByUserId(int userId)
        {
            ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();
            filter.PropertyEquals(COMMEN.PrefixUser.Properties.UserId, typeof(COMMEN.PrefixUser).Name, userId);
           
            return ModelAssembler.CreateUserPrefixes(DataFacadeManager.GetObjects(typeof(COMMEN.PrefixUser), filter.GetPredicate()));
        }

        /// <summary>
        /// Genera archivo excel ramo comercial
        /// </summary>
        /// <param name="Prefix"></param>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public string GenerateFileToPrefix(List<Prefix> Prefix, string fileName)
        {
            FileProcessValue fileProcessValue = new FileProcessValue();
            fileProcessValue.Key1 = (int)EnumUtilities.FileProcessType.ParametrizationPrefix;

            File file = fileDAO.GetFileByFileProcessValue(fileProcessValue);

            if (file != null && file.IsEnabled)
            {
                file.Name = fileName;
                List<Row> rows = new List<Row>();
                foreach (Prefix Prefixes in Prefix)
                {
                    if (Prefixes.LineBusiness != null && Prefixes.LineBusiness.Count > 0)
                    {
                        foreach (LineBusiness linebusiness in Prefixes.LineBusiness)
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
                            fields[0].Value = Prefixes.Id.ToString();
                            fields[1].Value = Prefixes.Description;
                            fields[2].Value = Prefixes.SmallDescription;
                            fields[3].Value = Prefixes.TinyDescription;
                            fields[4].Value = Prefixes.PrefixType.Description;
                            fields[5].Value = linebusiness.Description;

                            rows.Add(new Row
                            {
                                Fields = fields,
                            });
                        }
                    }
                    else
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
                        fields[0].Value = Prefixes.Id.ToString();
                        fields[1].Value = Prefixes.Description;
                        fields[2].Value = Prefixes.SmallDescription;
                        fields[3].Value = Prefixes.TinyDescription;
                        fields[4].Value = Prefixes.PrefixType.Description;
                        fields[5].Value = " ";

                        rows.Add(new Row
                        {
                            Fields = fields,
                        });
                    }
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
        /// Obtener lista de ramos comerciales
        /// </summary>
        /// <returns></returns>
        public List<COMMML.Prefix> GetPrefixes()
        {
            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();

            BusinessCollection prefixList = new BusinessCollection(DataFacadeManager.Instance.GetDataFacade().SelectObjects(typeof(COMMEN.Prefix)));

            stopWatch.Stop();
            Debug.WriteLine(stopWatch.Elapsed.ToString(), "Sistran.Core.Application.CommonService.Providers.GetPrefixes");
            return ModelAssembler.CreatePrefixes(prefixList);
        }

        /// <summary>
        /// Obtener Ramo comercial
        /// </summary>
        /// <param name="id">Id ramo comercial</param>
        /// <returns>Ramo comercial</returns>
        public Prefix GetPrefixById(int id)
        {
            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();
            PrimaryKey key = COMMEN.Prefix.CreatePrimaryKey(id);
            COMMEN.Prefix prefix = (COMMEN.Prefix)DataFacadeManager.Instance.GetDataFacade().GetObjectByPrimaryKey(key);
            stopWatch.Stop();
            Debug.WriteLine(stopWatch.Elapsed.ToString(), "Sistran.Core.Application.CommonService.Providers.GetPrefixById");
            return ModelAssembler.CreatePrefix(prefix);
        }

        /// <summary>
        /// Busca el Prefix segun el Identificador
        /// </summary>
        /// <param name="prefixId">The prefix identifier.</param>
        /// <returns></returns>
        public static COMMEN.Prefix Find(int prefixId)
        {
            PrimaryKey key = COMMEN.Prefix.CreatePrimaryKey(prefixId);
            COMMEN.Prefix prefix = (COMMEN.Prefix)DataFacadeManager.Instance.GetDataFacade().GetObjectByPrimaryKey(key);

            return prefix;
        }


        public List<COMMML.PrefixType> GetPrefixType()
        {
            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();
            BusinessCollection prefixListType = new BusinessCollection(DataFacadeManager.Instance.GetDataFacade().SelectObjects(typeof(PARAMEN.PrefixType)));
            stopWatch.Stop();
            return ModelAssembler.CreatePrefixType(prefixListType);
        }

        public bool UpdatePrefix(List<Prefix> ParPrefix)
        {
            foreach (Prefix item in ParPrefix)
            {
                PrimaryKey key = COMMEN.Prefix.CreatePrimaryKey(item.Id);
                COMMEN.Prefix PrefixEntity = new COMMEN.Prefix(item.Id);
                PrefixEntity = (COMMEN.Prefix)DataFacadeManager.Instance.GetDataFacade().GetObjectByPrimaryKey(key);
                if (PrefixEntity != null)
                {
                    if (item.Status == Sistran.Core.Application.CommonService.Enums.SubBranchTypes.StatusItem.Deleted.ToString())
                    {
                        DataFacadeManager.Instance.GetDataFacade().UpdateObject(PrefixEntity);
                        return false;
                    }
                }
            }

            return true;
        }

        public COMMML.PrefixType GetPrefixTypeByPrefixId(int PrefixId)
        {
            PARAMEN.PrefixType prefix = new PARAMEN.PrefixType(PrefixId);
            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();
            PrimaryKey key = PARAMEN.PrefixType.CreatePrimaryKey(PrefixId);
            prefix = (PARAMEN.PrefixType)DataFacadeManager.Instance.GetDataFacade().GetObjectByPrimaryKey(key);
            if (prefix == null)
            {
                prefix = new PARAMEN.PrefixType(0);
            }
            stopWatch.Stop();
            return ModelAssembler.CreatePrefixType(prefix);
        }

        /// <summary>
        /// Obtiene ramo comercial
        /// </summary>
        /// <returns></returns>
        public List<COMMML.Prefix> GetPrefixAll()
        {
            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();
            PrefixView view = new PrefixView();
            ViewBuilder builder = new ViewBuilder("PrefixView");
            List<COMMML.Prefix> prefixSets = new List<COMMML.Prefix>();
            DataFacadeManager.Instance.GetDataFacade().FillView(builder, view);

            if (view.Prefix.Count > 0)
            {
                List<COMMML.Prefix> prefix = ModelAssembler.CreatePrefixes(view.Prefix);
                List<PARAMEN.PrefixType> entityPrefixType = view.PrefixType.Cast<PARAMEN.PrefixType>().ToList();

                foreach (COMMML.Prefix Prefix in prefix)
                {
                    Prefix.PrefixType = new COMMML.PrefixType();
                    Prefix.PrefixType.Description = entityPrefixType.First(X => X.PrefixTypeCode == Prefix.PrefixType.Id).Description;
                    Prefix.PrefixType.Id = entityPrefixType.First(X => X.PrefixTypeCode == Prefix.PrefixType.Id).PrefixTypeCode;
                    prefixSets.Add(Prefix);
                }
            }

            return prefixSets.OrderBy(x => x.Description).ToList();
        }

        public List<string> CreatePrefix(List<COMMML.Prefix> prefix)
        {
            List<string> result = new List<string>();
            int newRegister = 0;
            int deleteRegister = 0;

            foreach (COMMML.Prefix item in prefix)
            {
                bool isError = false;
                PrimaryKey key = COMMEN.Prefix.CreatePrimaryKey(item.Id);
                COMMEN.Prefix prefixEntity = new COMMEN.Prefix(item.Id);
                prefixEntity = (COMMEN.Prefix)DataFacadeManager.Instance.GetDataFacade().GetObjectByPrimaryKey(key);
                if (prefixEntity != null)
                {
                    if (item.Status == "Delete")
                    {
                        try
                        {
                            this.DeleteLineBusinessByPrefixIdLineId(item.Id, 0);
                            DataFacadeManager.Instance.GetDataFacade().DeleteObject(prefixEntity);
                            deleteRegister += 1;
                        }
                        catch (Exception ex)
                        {
                            isError = true;
                            result.Add(Errors.ErrorDeleteItem + " " + item.Description + "(" + item.Id + ") " + ex.Message);
                        }
                    }
                    else
                    {
                        try
                        {
                            prefixEntity.Description = item.Description;
                            prefixEntity.TinyDescription = item.TinyDescription;
                            prefixEntity.SmallDescription = item.SmallDescription;
                            prefixEntity.PrefixTypeCode = item.PrefixType.Id;
                            DataFacadeManager.Instance.GetDataFacade().UpdateObject(prefixEntity);
                            newRegister += 1;
                        }
                        catch (Exception ex)
                        {
                            isError = true;
                            result.Add(Errors.ErrorUpdateItem + " " + item.Description + "(" + item.Id + ") " + ex.Message);
                        }
                    }
                }
                else
                {
                    if (item.Status != "Delete")
                    {
                        try
                        {
                            prefixEntity = Assemblers.EntityAssembler.CreatePrefix(item);
                            DataFacadeManager.Instance.GetDataFacade().InsertObject(prefixEntity);
                            newRegister += 1;
                        }
                        catch (Exception ex)
                        {
                            isError = true;
                            result.Add(Errors.ErrorInsertItem + " " + item.Description + "(" + item.Id + ") " + ex.Message);
                        }
                    }
                }

                //if (!isError && item.Status != "Delete")
                //{
                //    //PrefixLineBusiness en base
                //    List<PrefixLineBusiness> entityLines = GetPrefixLineBusinessById(0, item.Id);
                //    //si está en base y no está en los actuales, se elimina
                //    foreach (PrefixLineBusiness prefixLine in entityLines)
                //    {
                //        COMMML.LineBusiness line = item.LineBusiness.Where(x => x.Id == prefixLine.LineBusinessCode).FirstOrDefault();
                //        if (line == null)
                //        {
                //            this.DeleteLineBusinessByPrefixIdLineId(item.Id, prefixLine.LineBusinessCode);
                //        }
                //    }

                //    //Listado actual
                //    foreach (COMMML.LineBusiness line in item.LineBusiness)
                //    {
                //        //si no está en base se agrega
                //        PrefixLineBusiness entityLine = entityLines.Where(x => x.LineBusinessCode == line.Id).FirstOrDefault();
                //        if (entityLine == null)
                //        {
                //            PrefixLineBusiness prefixLine = new PrefixLineBusiness
                //            {
                //                PrefixCode = item.Id,
                //                LineBusinessCode = line.Id
                //            };
                //            this.CreatePrefixByLineBusiness(prefixLine);
                //        }
                //    }


                //}
            }


            if (newRegister > 0)
            {
                result.Add("Se actualizaron " + newRegister + " registros");
            }
            if (deleteRegister > 0)
            {
                result.Add("Se eliminaron " + deleteRegister + " registros");
            }
            return result;
        }

        /// <summary>
        /// borra las lineas de negocio asociadas al prefix
        /// </summary>
        /// <param name="prefixId">prefixId</param>
        public void DeleteLineBusinessByPrefixIdLineId(int prefixId, int lineId)
        {
            ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();
            filter.Property(COMMEN.PrefixLineBusiness.Properties.PrefixCode, typeof(COMMEN.PrefixLineBusiness).Name);
            filter.Equal();
            filter.Constant(prefixId);
            if (lineId != 0)
            {
                filter.And();
                filter.Property(COMMEN.PrefixLineBusiness.Properties.LineBusinessCode, typeof(COMMEN.PrefixLineBusiness).Name);
                filter.Equal();
                filter.Constant(lineId);
            }
            BusinessCollection businessCollection = new BusinessCollection(DataFacadeManager.Instance.GetDataFacade().SelectObjects(typeof(COMMEN.PrefixLineBusiness), filter.GetPredicate()));

            if (businessCollection.Count > 0)
            {
                List<COMMEN.PrefixLineBusiness> entities = businessCollection.Cast<COMMEN.PrefixLineBusiness>().ToList();
                foreach (COMMEN.PrefixLineBusiness item in entities)
                {
                    DataFacadeManager.Instance.GetDataFacade().DeleteObject(item);
                }
            }
        }

        public List<Prefix> GetPrefixesByCoveredRiskType(CoveredRiskType coveredRiskType)
        {
            List<COMMML.Prefix> prefixes = new List<Prefix>();
            ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();

            filter.Property(PARAMEN.HardRiskType.Properties.CoveredRiskTypeCode, typeof(PARAMEN.HardRiskType).Name);
            filter.Equal();
            filter.Constant((int)coveredRiskType);

            PrefixHardRiskTypeView view = new PrefixHardRiskTypeView();
            ViewBuilder builder = new ViewBuilder("PrefixHardRiskTypeView");
            builder.Filter = filter.GetPredicate();

            DataFacadeManager.Instance.GetDataFacade().FillView(builder, view);
            
            if (view.Prefix.Count > 0)
            {
                prefixes = ModelAssembler.CreatePrefixes(view.Prefix);
            }

            return prefixes;
        }

    }
}
