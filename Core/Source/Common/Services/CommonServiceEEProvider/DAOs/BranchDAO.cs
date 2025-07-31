using Sistran.Core.Application.CommonService.Models;
using Sistran.Core.Application.CommonServices.EEProvider.Assemblers;
using Sistran.Core.Application.Utilities.DataFacade;
using Sistran.Core.Application.UtilitiesServicesEEProvider.DAOs;
using Sistran.Core.Framework.Contexts;
using Sistran.Core.Framework.DAF;
using Sistran.Core.Framework.Queries;
using Sistran.Core.Framework.Transactions;
using Sistran.Core.Services.UtilitiesServices.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using COMMEN = Sistran.Core.Application.Common.Entities;
using COMMML = Sistran.Core.Application.CommonService.Models;
using Sistran.Core.Services.UtilitiesServices.Enums;

namespace Sistran.Core.Application.CommonServices.EEProvider.DAOs
{
    public class BranchDAO
    {
        FileDAO fileDAO = new FileDAO();

        /// <summary>
        /// Obtiene la lista de sucursales
        /// </summary>
        /// <returns>Lista de sucursales consultadas</returns>
        public List<Branch> GetBranches()
        {
            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();
            BusinessCollection businessCollection = new BusinessCollection(DataFacadeManager.Instance.GetDataFacade().SelectObjects(typeof(COMMEN.Branch)));
            List<Branch> branches = ModelAssembler.CreateBranches(businessCollection);

            //Providers.SalePoint salePointProvider = new Providers.SalePoint();
            //List<COMMML.SalePoint> salePoints = salePointProvider.GetSalePoints();

            //foreach (Branch item in branches)
            //{
            //    item.SalePoints = salePoints.Where(x => x.Branch.Id == item.Id).ToList();
            //}
            stopWatch.Stop();
            Debug.WriteLine(stopWatch.Elapsed.ToString(), "Sistran.Core.Application.CommonService.Providers.GetBranches");
            return branches;
        }

        /// <summary>
        /// Obtener Sucursal por ID
        /// </summary>
        /// <param name="id">Id sucursal</param>
        /// <returns>Sucursal</returns>
        public Branch GetBranchById(int id)
        {
            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();
            PrimaryKey key = COMMEN.Branch.CreatePrimaryKey(id);
            COMMEN.Branch branch = (COMMEN.Branch)DataFacadeManager.Instance.GetDataFacade().GetObjectByPrimaryKey(key);
            stopWatch.Stop();
            Debug.WriteLine(stopWatch.Elapsed.ToString(), "Sistran.Core.Application.CommonService.Providers.GetBranchById");
            return ModelAssembler.CreateBranch(branch);
        }

        /// <summary>
        /// Obtiene el ID maximo para realizar el ingreso de nuevas sucursales
        /// </summary>
        /// <returns>ID maximo</returns>
        public int GetIdBranch()
        {
            int maxBranchCode = new BusinessCollection(DataFacadeManager.Instance.GetDataFacade()
                            .SelectObjects(typeof(COMMEN.Branch)))
                            .Cast<COMMEN.Branch>().Max(x => x.BranchCode);
            maxBranchCode++;
            return maxBranchCode;
        }

        /// <summary>
        /// Realiza los procesos del CRUD para las Sucursales
        /// </summary>
        /// <param name="ListAdded"> Lista de branchs(sucursales) para ser agregados</param>
        /// <returns>El modelo de respuesta con el total de procesos realizados </returns>
        public List<Branch> CreateBranchs(List<Branch> branchsAdded)
        {
            List<Branch> returnedList;
            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();
            using (Context.Current)
            {
                #region Agregar Sucursales
                if (branchsAdded != null)
                {

                    using (Transaction transaction = new Transaction())
                    {

                        try
                        {

                            foreach (COMMML.Branch item in branchsAdded)
                            {
                                item.Id = GetIdBranch();
                                COMMEN.Branch entityBranch = EntityAssembler.CreateBranch(item);
                                DataFacadeManager.Instance.GetDataFacade().InsertObject(entityBranch);
                            }
                            transaction.Complete();
                            
                        }
                        catch (System.Exception ex)
                        {
                            transaction.Dispose();
                            throw (ex);

                        }
                    }
                }
                #endregion

               
                returnedList = GetBranches();
            }
            stopWatch.Stop();
            Debug.WriteLine(stopWatch.Elapsed.ToString(), "Sistran.Core.Application.CommonService.Providers.SaveBranchs");
            return returnedList;
        }

        /// <summary>
        /// Realiza los procesos del CRUD para las Sucursales
        /// </summary>
        /// <param name="ListEdited">Lista de branchs(sucursales) para ser modificados</param>
        /// <returns>El modelo de respuesta con el total de procesos realizados </returns>
        public List<Branch> UpdateBranchs(List<Branch> branchsEdited)
        {
            List<Branch> returnedList;
            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();
            using (Context.Current)
            {
                #region Modificar Sucursales
                if (branchsEdited != null)
                {
                    using (Transaction transaction = new Transaction())
                    {
                        try
                        {
                            foreach (var item in branchsEdited)
                            {
                                PrimaryKey key = COMMEN.Branch.CreatePrimaryKey(item.Id);
                                COMMEN.Branch branchEntity = new COMMEN.Branch(item.Id);
                                branchEntity = (COMMEN.Branch)DataFacadeManager.Instance.GetDataFacade().GetObjectByPrimaryKey(key);
                                branchEntity.Description = item.Description;
                                branchEntity.SmallDescription = item.SmallDescription;
                                DataFacadeManager.Instance.GetDataFacade().UpdateObject(branchEntity);
                            }
                            transaction.Complete();
                        }
                        catch (System.Exception ex)
                        {
                            transaction.Dispose();
                            throw (ex);
                        }
                    }
                }
                #endregion

               
                returnedList = GetBranches();
            }
            stopWatch.Stop();
            Debug.WriteLine(stopWatch.Elapsed.ToString(), "Sistran.Core.Application.CommonService.Providers.SaveBranchs");
            return returnedList;
        }

        /// <summary>
        /// Realiza los procesos del CRUD para las Sucursales
        /// </summary>
        /// <param name="ListDeleted">Lista de branchs(sucursales) para ser eliminados</param>
        /// <returns>El modelo de respuesta con el total de procesos realizados </returns>
        public List<Branch> DeleteBranchs(List<Branch> branchsDeleted)
        {
            List<Branch> returnedList;
            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();
            using (Context.Current)
            {
               

                #region Borrar Sucursales
                if (branchsDeleted != null)
                {
                    using (Transaction transaction = new Transaction())
                    {
                        try
                        {
                            ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();
                            filter.Property(COMMEN.Branch.Properties.BranchCode).In().ListValue();
                            branchsDeleted.ForEach(x => filter.Constant(x.Id));
                            filter.EndList();
                            DataFacadeManager.Instance.GetDataFacade().DeleteObjects(typeof(COMMEN.Branch), filter.GetPredicate());
                            transaction.Complete();
                        }
                        catch (ForeignKeyException ex)
                        {
                            transaction.Dispose();
                            throw (ex);
                        }
                        catch (RelatedObjectException ex)
                        {
                            transaction.Dispose();
                            throw (ex);
                        }
                        catch (System.Exception ex)
                        {
                            transaction.Dispose();
                            throw (ex);
                        }
                    }
                }
                #endregion
                returnedList = GetBranches();
            }
            stopWatch.Stop();
            Debug.WriteLine(stopWatch.Elapsed.ToString(), "Sistran.Core.Application.CommonService.Providers.SaveBranchs");
            return returnedList;
        }
        /// <summary>
        /// Genera archivo excel sucursales
        /// </summary>
        /// <param name="branch"></param>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public string GenerateFileToBranch(List<Branch> branch, string fileName)
        {
            FileProcessValue fileProcessValue = new FileProcessValue();
            fileProcessValue.Key1 = (int)FileProcessType.ParametrizationBranch;

            File file = fileDAO.GetFileByFileProcessValue(fileProcessValue);

            if (file != null && file.IsEnabled)
            {
                file.Name = fileName;
                List<Row> rows = new List<Row>();

                foreach (Branch branchs in branch)
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

                    fields[0].Value = branchs.Id.ToString();
                    fields[1].Value = branchs.SmallDescription;
                    fields[2].Value = branchs.Description;

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
    }
}
