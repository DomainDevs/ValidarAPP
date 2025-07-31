using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sistran.Company.Application.UnderwritingServices.Models;
using COMPENT = Sistran.Company.Application.Common.Entities;
using Sistran.Company.Application.UnderwritingServices.EEProvider.Assemblers;
using Sistran.Core.Application.Utilities.DataFacade;
using Sistran.Core.Framework.DAF;
using Sistran.Core.Framework.Queries;
using COMENT = Sistran.Core.Application.Common.Entities;
using Sistran.Company.Application.UnderwritingServices.EEProvider.Entities.Views;
using Sistran.Core.Framework.DAF.Engine;
using UTILMO = Sistran.Core.Services.UtilitiesServices.Models;
using UTILEN = Sistran.Core.Services.UtilitiesServices.Enums;

namespace Sistran.Company.Application.UnderwritingServices.EEProvider.Business
{
    internal class CiaRatingZoneBranchBusiness
    {
        /// <summary>
        /// Crea una nueva zona asociada
        /// </summary>
        /// <param name="ciaRatingZoneBranch"></param>
        /// <returns></returns>
        internal CiaRatingZoneBranch CreateCiaRatingZoneBranch(CiaRatingZoneBranch ciaRatingZoneBranch)
        {
            //COMPENT.CiaRatingZoneBranch entityCiaRatingZoneBranch = EntityAssembler.CreateCiaRatingZoneBranch(ciaRatingZoneBranch);
            //DataFacadeManager.Insert(entityCiaRatingZoneBranch);

            return null; // ModelAssembler.CreateCiaRatingZoneBranch(entityCiaRatingZoneBranch);
        }

        /// <summary>
        /// Elimina zona asociada
        /// </summary>
        /// <param name="ratingZoneCode"></param>
        /// <param name="branchCode"></param>
        internal void DeleteCiaBranchRatingZone(int ratingZoneCode, int branchCode)
        {
            PrimaryKey primaryKey = COMPENT.CiaRatingZoneBranch.CreatePrimaryKey(ratingZoneCode, branchCode);

            DataFacadeManager.Delete(primaryKey);
        }
        /// <summary>
        /// Obtiene Zonas asociadas por ratingZoneCode y branchCode
        /// </summary>
        /// <param name="ratingZoneCode"></param>
        /// <param name="branchCode"></param>
        /// <returns></returns>
        internal CiaRatingZoneBranch GetRatingZoneBranch(int ratingZoneCode, int branchCode)
        {
            PrimaryKey primaryKey = COMENT.CiaRatingZoneBranch.CreatePrimaryKey(ratingZoneCode, branchCode);
            COMENT.CiaRatingZoneBranch entityCiaRatingZoneBranch = (COMENT.CiaRatingZoneBranch)DataFacadeManager.GetObject(primaryKey);

            return ModelAssembler.CreateCiaRatingZoneBranch(entityCiaRatingZoneBranch);
        }

        /// <summary>
        /// Obtiene todas las zonas asociadas
        /// </summary>
        /// <returns></returns>
        internal List<CiaRatingZoneBranch> GetRatingZonesBranchs()
        {
            return ModelAssembler.CreateCiaRatingZoneBranchs(DataFacadeManager.GetObjects(typeof(COMPENT.CiaRatingZoneBranch)));
        }

        /// <summary>
        /// Obtiene las zonas asociadas por ramo y sucursal
        /// </summary>
        /// <param name="prefixId"></param>
        /// <param name="branchId"></param>
        /// <returns></returns>
        internal List<CompanyRatingZone> GetRatingZonesByPrefixIdAndBranchId(int prefixId, int branchId)
        {
            List<CompanyRatingZone> companyRatingZones = new List<CompanyRatingZone>();

            ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();
            filter.PropertyEquals(COMENT.RatingZone.Properties.PrefixCode, typeof(COMENT.RatingZone).Name, prefixId);
            filter.And();
            filter.PropertyEquals(COMPENT.CiaRatingZoneBranch.Properties.BranchCode, typeof(COMPENT.CiaRatingZoneBranch).Name, branchId);

            CompanyCiaRatingZoneBranchView ciaRatingZoneBranchView = new CompanyCiaRatingZoneBranchView();
            ViewBuilder viewBuilder = new ViewBuilder("CompanyCiaRatingZoneBranchView");
            viewBuilder.Filter = filter.GetPredicate();

            DataFacadeManager.Instance.GetDataFacade().FillView(viewBuilder, ciaRatingZoneBranchView);

            if (ciaRatingZoneBranchView.RatingZones.Count > 0)
            {
                List<COMENT.RatingZone> entityRatingZone = ciaRatingZoneBranchView.RatingZones.Cast<COMENT.RatingZone>().ToList();
                List<COMPENT.CiaRatingZoneBranch> entityCiaRatingZoneBranche = ciaRatingZoneBranchView.CiaRatingZoneBranchs.Cast<COMPENT.CiaRatingZoneBranch>().ToList();

                entityRatingZone = entityRatingZone.Where(x => entityCiaRatingZoneBranche.Any(y => y.RatingZoneCode == x.RatingZoneCode)).ToList();

                companyRatingZones = null;//ModelAssembler.CreateCompanyRatingZoneEntitys(entityRatingZone);
            }


            return companyRatingZones;
        }

        /// <summary>
        /// Obtiene las sucursales asociadas
        /// </summary>
        /// <param name="branchCode"></param>
        /// <returns></returns>
        internal List<CiaRatingZoneBranch> GetRatingZoneBranchByBranchId(int branchCode)
        {
            ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();
            filter.PropertyEquals(COMPENT.CiaRatingZoneBranch.Properties.BranchCode, typeof(COMPENT.CiaRatingZoneBranch).Name, branchCode);

            return ModelAssembler.CreateCiaRatingZoneBranchs(DataFacadeManager.GetObjects(typeof(COMPENT.CiaRatingZoneBranch), filter.GetPredicate()));
        }

        /// <summary>
        /// Guarda las nuevas zonas asociadas
        /// </summary>
        /// <param name="companyRatingZones"></param>
        /// <param name="branchId"></param>
        /// <returns></returns>
        internal List<CiaRatingZoneBranch> SaveCiaRatingZoneBranch(List<CompanyRatingZone> companyRatingZones, int branchId)
        {
            //Lista que trae los datos de la BD
            List<CiaRatingZoneBranch> RatingZones = GetRatingZoneBranchByBranchId(branchId);
            //Si viene datos del front los recorre
            if (companyRatingZones != null)
            {
                //Itero los datos que vienen del front 
                foreach (CompanyRatingZone CompanyRatingZone in companyRatingZones)
                {
                    // Si existe un dato nuevo lo agrega
                    if (!(RatingZones.Exists(x => (x.RatingZone.Id == CompanyRatingZone.Id) && (x.Branch.Id == branchId))))
                    {
                        CiaRatingZoneBranch ciaRatingZoneBranch = new CiaRatingZoneBranch
                        {
                            RatingZone = new CompanyRatingZone { Id = CompanyRatingZone.Id },
                            Branch = new CommonServices.Models.CompanyBranch { Id = branchId }
                        };
                        CreateCiaRatingZoneBranch(ciaRatingZoneBranch);
                    }
                }
            }
            //Si un dato que esta en BD no viene del front lo elimina de BD 
            List<CiaRatingZoneBranch> ciaRatingZoneBranchDelete = (from t in RatingZones where !companyRatingZones.Any(x => x.Id == t.RatingZone.Id && branchId == t.Branch.Id) select t).ToList();

            foreach (CiaRatingZoneBranch ratingZones in ciaRatingZoneBranchDelete)
            {
                DeleteCiaBranchRatingZone(ratingZones.RatingZone.Id, ratingZones.Branch.Id);
            }

            return RatingZones;
        }

        /// <summary>
        /// Obtiene ramo , sucursal y zona de tarifacion 
        /// </summary>
        /// <returns></returns>
        internal List<CompanyRatingZone> GetRatingZonesAndPrefixAndBranch()
        {
            List<CompanyRatingZone> companyRatingZones = new List<CompanyRatingZone>();

            ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();

            CiaZonesCirculationByBranchView ciaZonesCirculationByBranchView = new CiaZonesCirculationByBranchView();
            ViewBuilder viewBuilder = new ViewBuilder("CiaZonesCirculationByBranchView");
            viewBuilder.Filter = filter.GetPredicate();

            DataFacadeManager.Instance.GetDataFacade().FillView(viewBuilder, ciaZonesCirculationByBranchView);

            if (ciaZonesCirculationByBranchView.CiaRatingZoneBranchs.Count > 0)
            {
                List<COMENT.Prefix> entityPrefixes = ciaZonesCirculationByBranchView.Prefixes.Cast<COMENT.Prefix>().ToList();
                List<COMENT.Branch> entityBranches = ciaZonesCirculationByBranchView.Branchs.Cast<COMENT.Branch>().ToList();

                companyRatingZones = null; //ModelAssembler.CreateCompanyRatingZones(ciaZonesCirculationByBranchView.RatingZones, ciaZonesCirculationByBranchView.CiaRatingZoneBranchs);

                foreach (CompanyRatingZone companyRatingZone in companyRatingZones)
                {
                    companyRatingZone.Prefix.Description = entityPrefixes.First(x => x.PrefixCode == companyRatingZone.Prefix.Id).Description;
                    companyRatingZone.Branch.Description = entityBranches.First(x => x.BranchCode == companyRatingZone.Branch.Id).Description;
                }
            }

            return companyRatingZones;
        }

        /// <summary>
        /// Genera el Archivo excel
        /// </summary>
        /// <param name="companyRatingZones"></param>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public string GenerateFileToCiaRatingZone(List<CompanyRatingZone> companyRatingZones, string fileName)
        {
            try
            {
                UTILMO.FileProcessValue fileProcessValue = new UTILMO.FileProcessValue();
                fileProcessValue.Key1 = (int)UTILEN.FileProcessType.ParametrizationRatingZoneBranch;

                UTILMO.File file = DelegateService.utilitiesServiceCore.GetFileByFileProcessValue(fileProcessValue);

                if (file != null && file.IsEnabled)
                {
                    file.Name = fileName;
                    List<UTILMO.Row> rows = new List<UTILMO.Row>();

                    foreach (CompanyRatingZone ratingzone in companyRatingZones)
                    {
                        List<UTILMO.Field> fields = file.Templates[0].Rows[0].Fields.OrderBy(x => x.Order).Select(p => new UTILMO.Field()
                        {
                            ColumnSpan = p.ColumnSpan,
                            Description = p.Description,
                            FieldType = p.FieldType,
                            Id = p.Id,
                            IsEnabled = p.IsEnabled,
                            IsMandatory = p.IsMandatory,
                            Order = p.Order,
                            RowPosition = p.RowPosition,
                            SmallDescription = p.SmallDescription
                        }).ToList();

                        fields[0].Value = ratingzone.Prefix.Id.ToString();
                        fields[1].Value = ratingzone.Prefix.Description;
                        fields[2].Value = ratingzone.Branch.Id.ToString();
                        fields[3].Value = ratingzone.Branch.Description;
                        fields[4].Value = ratingzone.Id.ToString();
                        fields[5].Value = ratingzone.Description;

                        rows.Add(new UTILMO.Row
                        {
                            Fields = fields
                        });
                    }

                    file.Templates[0].Rows = rows;
                    file.Name = fileName + "_" + DateTime.Now.ToString("dd_MM_yyyy");

                    string generateFile = DelegateService.utilitiesServiceCore.GenerateFile(file);
                    return generateFile;

                }
                else
                {
                    return string.Empty;
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error in GenerateFileToCiaRatingZONE" + ex);
            }
        }


    }
}
